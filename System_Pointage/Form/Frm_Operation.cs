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
        private Frm_MVM_Operation otherForm;
        // حدث لتمرير البيانات إلى الفورم الأصلية
        public event EventHandler<BindingList<Models.AgentStatus>> AgentsTransferred;
        public Frm_Operation(Master.MVMType _Type, Frm_MVM_Operation formPrevu)
        {
            InitializeComponent();
            Type = _Type;
            SetFormType();
            otherForm = formPrevu;
        }
        void SetFormType()
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    this.Text = "Liste des agents  en congé";
                    layoutControlGroup1.Text = "Liste des agents  en congé";
                    btn_envoyer.Caption = "Envoyer sur liste rentrant";
                    break;
                case Master.MVMType.A:
                    this.Text = "Liste des agents  en Présent";
                    layoutControlGroup1.Text = "Liste des agents";
                    btn_envoyer.Caption = "Envoyer sur liste absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "Liste des agents  en Présent";
                    layoutControlGroup1.Text = "Liste des agents   Présent";
                    btn_envoyer.Caption = "Envoyer sur liste partant";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Frm_Operation_Load(object sender, EventArgs e)
        {
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " ";
            #endregion
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
                    SomeMethod();
                }
            }
        }
        private void SetupGridColumns()
        {
            gridView2.Columns.Clear();

            gridView2.Columns.AddVisible("Name", "Nom");
            gridView2.Columns.AddVisible("FirstName", "Prénom");
            gridView2.Columns.AddVisible("Poste", "Poste");
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as Models.AgentStatus;

            if (selectedRow != null)
            {
                // نقل السطر إلى القائمة الثانية
                transferredAgentsList.Add(selectedRow);

                // إزالة السطر من القائمة الأولى
                activeAgentsList.Remove(selectedRow);
                SomeMethod();
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
            activeAgentsList = agentDataService.GetAgentStatuses(userAccessPosteID, Type, isAdmin,null,true,"frm_op");


            SomeMethod();

            GridName();
        }
      
        private void SomeMethod()
        {
            if (otherForm.ActiveAgentsList != null)
            {
                DateTime today = DateTime.Today;

                //var filteredList = new BindingList<Models.AgentStatus>(
                //    activeAgentsList.Where(agent =>
                //        !(otherForm.ActiveAgentsList.Any(existingAgent => existingAgent.Name == agent.Name && existingAgent.Date != today) ||
                //          otherForm.TransferredAgentsList.Any(existingAgent => existingAgent.Name == agent.Name))
                //    )
                //    .Where(agent => agent.Date.Date != today) 
                //    .ToList()
                //);

                var filteredList = new BindingList<Models.AgentStatus>(
                     activeAgentsList.Where(agent =>
                     !(otherForm.ActiveAgentsList.Any(existingAgent => existingAgent.Name == agent.Name) ||
                       otherForm.TransferredAgentsList.Any(existingAgent => existingAgent.Name == agent.Name))
                  ).ToList()
                );
                gridControl1.DataSource = filteredList;
            }
            else
            {
                gridControl1.DataSource = activeAgentsList;
            }
        }
       
        public void GridName()
        {
            gridView1.GroupPanelText = " ";
            gridView1.Columns["Name"].Caption = "Nom";
            gridView1.Columns["FirstName"].Caption = "prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Difference"].Caption = "Ecart";
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["FirstName"].VisibleIndex =2;
            gridView1.Columns["Poste"].VisibleIndex = 3;
            gridView1.Columns["Affecter"].VisibleIndex =4;
            gridView1.Columns["Date"].VisibleIndex = 5;
            gridView1.Columns["Jour"].VisibleIndex = 6;
            gridView1.Columns["CalculatedDate"].VisibleIndex =7;
            gridView1.Columns["DaysCount"].VisibleIndex = 8;
            gridView1.Columns["Difference"].VisibleIndex =9;
            gridView1.Columns["Statut"].VisibleIndex =10;
            gridView1.Columns["screenPosteD"].Visible =false;
            gridView1.Columns["Statutmvm"].Visible = false;
            gridView1.Columns["Jour"].Visible = false;

            switch (Type)
            {
                case Master.MVMType.P:
                    gridView1.Columns["Date"].Caption = "Date de début Congé";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue rentrée";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de congé";
                    gridView1.Columns["DaysCount"].Caption = "Les jours de congé";
                    break;
                case Master.MVMType.A:
                    gridView1.Columns["Date"].Caption = "Date de rentrée";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
                    break;
                case Master.MVMType.CR:
                    gridView1.Columns["Date"].Caption = "Date de rentrée";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void btn_envoyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (transferredAgentsList.Count > 0)
            {
                var mainForm = this.Owner as Frm_MVM_Operation;
                if (mainForm != null)
                {
                    // التحقق من التكرارات
                    //var duplicateAgents = transferredAgentsList
                    //    .Where(agent => mainForm.IsAgentExists(agent))
                    //    .ToList();

                    //if (duplicateAgents.Any())
                    //{
                    //    string duplicateNames = string.Join(", ", duplicateAgents.Select(a => a.Name));
                    //    MessageBox.Show(
                    //        $"Les Agent suivants sont déjà dupliqués dans la liste: {duplicateNames}",
                    //        "avertissement",
                    //        MessageBoxButtons.OK,
                    //        MessageBoxIcon.Warning
                    //    );
                    //}
                    //else
                    //{
                        mainForm.AddTransferredAgents(transferredAgentsList);
                        this.Close();
                 //   }
                }
            }
            else
            {
                MessageBox.Show("Il n'y a aucune donnée à envoyer.", "avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}