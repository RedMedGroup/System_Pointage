using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Prevu : DevExpress.XtraEditors.XtraForm
    {
        Master.MVMType Type;
        private BindingList<Models.AgentStatus> transferredAgentsList;       

        public Frm_Prevu(Master.MVMType _Type)
        {
            InitializeComponent();
            Type = _Type;
        }
 
        private void Frm_Prevu_Load(object sender, EventArgs e)
        {
            dateEdit1.EditValueChanged += DateEdit1_EditValueChanged;

            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            transferredAgentsList = new BindingList<Models.AgentStatus>();
        }

        private void DateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue is DateTime selectedDate)
            {
                LoadEligibleEmployees(selectedDate);
            }
        }
        private void LoadEligibleEmployees(DateTime selectedDate)
        {
            using (var context = new DAL.DataClasses1DataContext())
            {
                // استرجاع البيانات من الجداول
                var eligibleEmployees = context.MVMAgentDetails
                    .Where(m => m.Statut == "P") // التأكد من أن حالة العامل هي "P"
                    .Join(context.Fiche_Agents, // الربط بين الجداول Fiche_Agents و MVMAgentDetails
                          m => m.ItemID,
                          agent => agent.ID,
                          (m, agent) => new { agent.Name, m.Date, m.Statut, agent.Jour }) // اختيار Name من Fiche_Agents
                    .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
                    .ToList();

                // إنشاء BindingList جديدة
                transferredAgentsList = new BindingList<Models.AgentStatus>(
                    eligibleEmployees.Select(x => new Models.AgentStatus
                    {
                        Name = x.Name,
                        Date = x.Date,
                        Statut = x.Statut
                    }).ToList()
                );
                gridControl1.DataSource = transferredAgentsList;
            }

            //using (var context = new DAL.DataClasses1DataContext())
            //{
            //    // استرجاع البيانات من الجداول
            //    var eligibleEmployees = context.MVMAgentDetails
            //        .Where(m => m.Statut == "P") // التأكد من أن حالة العامل هي "P"
            //        .Join(context.Fiche_Agents, // الربط بين الجداول Fiche_Agents و MVMAgentDetails
            //              m => m.ItemID,
            //              agent => agent.ID,
            //              (m, agent) => new { agent.Name, m.Date, m.Statut, agent.Jour })
            //        .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
            //        .ToList();

            //    // ربط البيانات بالكريد
            //    gridControl1.DataSource = eligibleEmployees;
            //}
        }
        private void btn_prevu_Click(object sender, EventArgs e)
        {

            //using (var context = new DAL.DataClasses1DataContext())
            //{
            //    // استرجاع البيانات من الجداول
            //    var eligibleEmployees = context.Fiche_Agents
            //        .Where(agent => agent.Jour > 0) // تأكد من أن Jour أكبر من 0
            //        .Select(agent => new
            //        {
            //            agent.ID,
            //            agent.Name,
            //            agent.Jour,
            //            LastRecord = context.MVMAgentDetails
            //                .Where(m => m.ItemID == agent.ID)
            //                .OrderByDescending(m => m.Date)
            //                .FirstOrDefault()
            //        })
            //        .Where(x => x.LastRecord != null) // تأكد من وجود سجل
            //        .Where(x => x.LastRecord.Statut == "P" // تأكد أن آخر حالة كانت "P"
            //                    && (selectedDate - x.LastRecord.Date).TotalDays >= x.Jour) // تحقق من الأيام
            //        .ToList();

            //    // ربط البيانات بالكريد
            //    gridControl1.DataSource = eligibleEmployees;
            //}
        }


        //public class AgentStatus
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
                        // استدعاء الطريقة لإضافة البيانات إلى Frm_MVM_Operation
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