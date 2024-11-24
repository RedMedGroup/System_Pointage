using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
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
            gridView2.OptionsBehavior.Editable = true;
            transferredAgentsList = new BindingList<Models.AgentStatus>();
            gridControl2.DataSource = transferredAgentsList;

            RefrecheData();
            SetupGridColumns();
            #region إضافة زر الحذف
            RepositoryItemButtonEdit buttonEdit = new RepositoryItemButtonEdit
            {
                TextEditStyle = TextEditStyles.HideTextEditor
            };
            buttonEdit.Buttons.Clear();
            buttonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Minus));
            buttonEdit.ButtonClick += ButtonEdit_ButtonClick;

            gridControl2.RepositoryItems.Add(buttonEdit);

            GridColumn clmnDelete = new GridColumn
            {
                Name = "clmnDelete",
                Caption = "Sup",
                FieldName = "Delete", 
                UnboundType = DevExpress.Data.UnboundColumnType.Object, 
                ColumnEdit = buttonEdit,
                VisibleIndex = gridView2.Columns.Count, 
                Width = 50,
                MaxWidth = 50,
                Visible = true
            };

            gridView2.Columns.Add(clmnDelete);
            gridView2.RefreshData(); 
            #endregion
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = gridControl2.FocusedView as GridView;

            if (view != null && view.FocusedRowHandle >= 0)
            {
                // الحصول على السطر المحدد من GridView2
                var selectedRow = view.GetFocusedRow() as Models.AgentStatus;

                if (selectedRow != null)
                {
                    // إرجاع السطر إلى القائمة الأولى (activeAgentsList)
                    activeAgentsList.Add(selectedRow);

                    // إزالة السطر من القائمة الثانية (transferredAgentsList)
                    transferredAgentsList.Remove(selectedRow);
                }
            }
        }

        private void SetupGridColumns()
        {
            gridView2.Columns.Clear();

            gridView2.Columns.AddVisible("Name", "Nom et prénom");
        //    gridView2.Columns.AddVisible("Date", "Date");
            gridView2.Columns.AddVisible("Poste", "Poste");

            // إعداد تاريخ التنسيق
            //gridView2.Columns["Date"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            //gridView2.Columns["Date"].DisplayFormat.FormatString = "dd/MM/yyyy";
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
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            // جلب البيانات
            activeAgentsList = agentDataService.GetAgentStatuses(userAccessPosteID, Type, isAdmin);

            // تعيين البيانات للجدول
            gridControl1.DataSource = activeAgentsList;

            GridName();
        }

        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Date"].Caption = "Date de début";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue";
            gridView1.Columns["Name"].VisibleIndex = 0;
            gridView1.Columns["Poste"].VisibleIndex = 1;
            gridView1.Columns["Date"].VisibleIndex = 3;
            gridView1.Columns["Jour"].VisibleIndex = 4;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 5;
            gridView1.Columns["Statut"].VisibleIndex = 6;
        }
        private void btn_envoyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (transferredAgentsList.Count > 0)
            {
                var mainForm = this.Owner as Frm_MVM_Operation;
                if (mainForm != null)
                {
                    // التحقق من التكرارات
                    var duplicateAgents = transferredAgentsList
                        .Where(agent => mainForm.IsAgentExists(agent))
                        .ToList();

                    if (duplicateAgents.Any())
                    {
                        string duplicateNames = string.Join(", ", duplicateAgents.Select(a => a.Name));
                        MessageBox.Show(
                            $"Les Agent suivants sont déjà dupliqués dans la liste: {duplicateNames}",
                            "avertissement",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                    else
                    {
                        mainForm.AddTransferredAgents(transferredAgentsList);
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Il n'y a aucune donnée à envoyer.", "avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}