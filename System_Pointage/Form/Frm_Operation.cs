using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Operation : DevExpress.XtraEditors.XtraForm
    {
        Master.MVMType Type;

        private BindingList<Models.AgentStatus> activeAgentsList;
        private BindingList<Models.AgentStatus> transferredAgentsList;

        // حدث لتمرير البيانات إلى الفورم الأصلية
        public event EventHandler<BindingList<Models.AgentStatus>> AgentsTransferred;
        public Frm_Operation(Master.MVMType _Type)
        {
            InitializeComponent();
            Type = _Type;
            SetFormType();
        }
        void SetFormType()
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    this.Text = "Présent";
                    break;
                case Master.MVMType.A:
                    this.Text = "Absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "cr";
                    //this.Name = Class.Screens.Trqnsfertchambre.ScreenName;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Frm_Operation_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            gridView2.OptionsBehavior.Editable = false;
            transferredAgentsList = new BindingList<Models.AgentStatus>();
            gridControl2.DataSource = transferredAgentsList;

            RefrecheData();
            SetupGridColumns();
            gridView1.DoubleClick += GridView1_DoubleClick;
        }
        private void SetupGridColumns()
        {
            gridView2.Columns.Clear();

            gridView2.Columns.AddVisible("Name", "Name");
            gridView2.Columns.AddVisible("Date", "Date");
            gridView2.Columns.AddVisible("Statut", "Statut");

            // إعداد تاريخ التنسيق
            gridView2.Columns["Date"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            // الحصول على السطر المحدد
            var selectedRow = gridView1.GetFocusedRow() as Models.AgentStatus;

            if (selectedRow != null)
            {
                // نقل السطر إلى القائمة الثانية
                transferredAgentsList.Add(selectedRow);

                // إزالة السطر من القائمة الأولى
                activeAgentsList.Remove(selectedRow);
            }
        }
        void RefrecheData()
        {
            using (var context = new DAL.DataClasses1DataContext())
            {
                switch (Type)
                {
                    case Master.MVMType.P:
                        activeAgentsList = new BindingList<Models.AgentStatus>(
                         context.MVMAgentDetails
                             .GroupBy(agent => agent.ItemID)
                             .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                             .Where(lastRecord => lastRecord.Statut == "A" || lastRecord.Statut == "CR")
                             .Join(context.Fiche_Agents,
                                   agent => agent.ItemID,
                                   worker => worker.ID,
                                   (agent, worker) => new Models.AgentStatus
                                   {
                                       Name = worker.Name,
                                       Date = agent.Date,
                                       Statut = agent.Statut
                                   })
                             .ToList()
                     );
                         
                        break;

                    case Master.MVMType.A:
                        activeAgentsList = new BindingList<Models.AgentStatus>(
                                        context.MVMAgentDetails
                                            .GroupBy(agent => agent.ItemID)
                                            .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                                            .Where(lastRecord => lastRecord.Statut == "P" || lastRecord.Statut == "CR")
                                            .Join(context.Fiche_Agents,
                                                  agent => agent.ItemID,
                                                  worker => worker.ID,
                                                  (agent, worker) => new Models.AgentStatus // استخدام الكلاس الجديد
                                                  {
                                                      Name = worker.Name,
                                                      Date = agent.Date,
                                                      Statut = agent.Statut
                                                  })
                                            .ToList()
                                    );

                        break;

                    case Master.MVMType.CR:
                        activeAgentsList = new BindingList<Models.AgentStatus>(
                                             context.MVMAgentDetails
                                                 .GroupBy(agent => agent.ItemID)
                                                 .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                                                 .Where(lastRecord => lastRecord.Statut == "P" || lastRecord.Statut == "A")
                                                 .Join(context.Fiche_Agents,
                                                       agent => agent.ItemID,
                                                       worker => worker.ID,
                                                       (agent, worker) => new Models.AgentStatus // استخدام الكلاس الجديد
                                                       {
                                                           Name = worker.Name,
                                                           Date = agent.Date,
                                                           Statut = agent.Statut
                                                       })
                                                 .ToList()
                                         );

                        break;

                    default:
                        throw new NotImplementedException();
                }

                // ربط البيانات بالكريد كونترول الأول
                gridControl1.DataSource = activeAgentsList;
            }
        }
//        public class AgentStatus
//{
//    public string Name { get; set; }
//    public DateTime Date { get; set; }
//    public string Statut { get; set; }
//}

        private void btn_envoyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (transferredAgentsList.Count > 0)
            {
                // الحصول على الفورم الأصلية (Frm_MVM_Operation)
                var mainForm = this.Owner as Frm_MVM_Operation;
                if (mainForm != null)
                {
                    // التحقق من التكرارات
                    var duplicateAgents = transferredAgentsList
                        .Where(agent => mainForm.IsAgentExists(agent))
                        .ToList();

                    if (duplicateAgents.Any())
                    {
                        // عرض رسالة تحذيرية
                        string duplicateNames = string.Join(", ", duplicateAgents.Select(a => a.Name));
                        MessageBox.Show(
                            $"العناصر التالية مكررة بالفعل في القائمة: {duplicateNames}",
                            "تحذير",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                    else
                    {
                        // استدعاء الطريقة لإضافة البيانات
                        mainForm.AddTransferredAgents(transferredAgentsList);

                        // إغلاق الفورم الحالية
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("لا توجد بيانات لإرسالها.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}