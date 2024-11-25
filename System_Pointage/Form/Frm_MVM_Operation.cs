using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Form;
using static System_Pointage.Form.Frm_Operation;

namespace System_Pointage.Classe
{
    public partial class Frm_MVM_Operation : DevExpress.XtraEditors.XtraForm
    {
        Master.MVMType Type;
        private BindingList<Models.AgentStatus> agentList;
        private BindingList<Models.AgentStatus> activeAgentsList;

        public Frm_MVM_Operation(Master.MVMType _Type)
        {
            InitializeComponent();
            Type = _Type;
            SetFormType();
            agentList = new BindingList<Models.AgentStatus>();
        }
        void SetFormType()
        {
            switch (Type)
            {
    
                case Master.MVMType.P:
                    this.Text = "Ajouter des Rentrants";//
                    btn_list_prevu.Text = "Rentrant prévue";
                    layoutControlItem2.Text= "Date de Rentrant";
                    btn_ovrirEn.Text = "Séléction Agent Rentrant";
                    break;
                case Master.MVMType.A:
                    this.Text = "Ajouter des Absent";
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem2.Text = "Date de Absent";
                    btn_ovrirEn.Text = "Séléction Agent Absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "Ajouter des Partants";
                    btn_list_prevu.Text = "Partant prévue";
                    layoutControlItem2.Text = "Date de Partant";
                    btn_ovrirEn.Text = "Séléction Agent Partant";

                    //this.Name = Class.Screens.Trqnsfertchambre.ScreenName;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Frm_MVM_Operation_Load(object sender, EventArgs e)
        {
            activeAgentsList = new BindingList<Models.AgentStatus>();
            gridControl1.DataSource = activeAgentsList;
            dateEdit11.DateTime = DateTime.Now;

            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            GridName();
        }
      
        private BindingList<Models.AgentStatus> transferredAgentsList = new BindingList<Models.AgentStatus>();
        public void AddTransferredAgentse(BindingList<Models.AgentStatus> transferredAgents)
        {
            // يمكن هنا إضافة البيانات إلى القائمة في Frm_MVM_Operation
            foreach (var agent in transferredAgents)
            {
                transferredAgentsList.Add(agent);
            }

            // تأكد من تحديث الواجهة إذا لزم الأمر
            gridControl1.DataSource = transferredAgentsList;
        }

        // يمكنك إضافة دالة للتحقق من وجود العامل في القائمة
        public bool IsAgentExistse(Models.AgentStatus agent)
        {
            return transferredAgentsList.Any(a => a.Name == agent.Name);
        }
    
        ///

    public void AddTransferredAgents(BindingList<Models.AgentStatus> transferredAgents)
        {
            foreach (var agent in transferredAgents)
            {
                if (!IsAgentExists(agent))
                {
                    activeAgentsList.Add(agent);
                }
            }

            // تحديث البيانات في واجهة المستخدم
            gridControl1.RefreshDataSource();
        }
        public bool IsAgentExists(Models.AgentStatus agent)
        {
            return activeAgentsList != null && activeAgentsList.Any(existingAgent =>
                existingAgent.Name == agent.Name &&
                existingAgent.Date.Date == agent.Date.Date &&
                existingAgent.Statut == agent.Statut);
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner au moins une ligne à enregistrer.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            List<Models.AgentStatus> selectedAgents = new List<Models.AgentStatus>();

            foreach (var rowIndex in selectedRows)
            {
                var agent = gridView1.GetRow(rowIndex) as Models.AgentStatus;
                if (agent != null)
                {
                    selectedAgents.Add(agent);
                }
            }
            string statut = Type == Master.MVMType.P ? "P" : (Type == Master.MVMType.A ? "A" : "CR");

            DateTime date = dateEdit11.DateTime;
            using (var context = new DAL.DataClasses1DataContext())
            {
                foreach (var agent in selectedAgents)
                {
                    var newAgentDetail = new DAL.MVMAgentDetail
                    {
                        ItemID = context.Fiche_Agents.FirstOrDefault(f => f.Name == agent.Name)?.ID ?? 0,
                        Date = date, 
                        Statut = statut
                    };

                    context.MVMAgentDetails.InsertOnSubmit(newAgentDetail);
                }

                context.SubmitChanges();
                XtraMessageBox.Show("Enregistrer succés");
                activeAgentsList.Clear(); // إفراغ القائمة
                gridControl1.DataSource = new BindingList<Models.AgentStatus>(activeAgentsList); 
                gridView1.ClearSelection();
            }
        }

        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["Poste"].VisibleIndex = 2;
            gridView1.Columns["Affecter"].VisibleIndex = 3;
            gridView1.Columns["Date"].VisibleIndex = 4;
            gridView1.Columns["Jour"].VisibleIndex = 5;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 6;
            gridView1.Columns["DaysCount"].VisibleIndex = 7;
            gridView1.Columns["Statut"].VisibleIndex = 8;
            switch (Type)
            {
                case Master.MVMType.P:
                    gridView1.Columns["Date"].Caption = "Date de début Congé";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue rentrée";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de congé";
                    break;
                case Master.MVMType.A:
                    gridView1.Columns["Date"].Caption = "Date de rentrée";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
                    break;
                case Master.MVMType.CR:
                    gridView1.Columns["Date"].Caption = "Date de rentrée";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void btn_list_prevu_Click_1(object sender, EventArgs e)
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    Frm_Prevu frmType1 = new Frm_Prevu(Type);
                    frmType1.Owner = this;
                    frmType1.Show();
                    break;

                case Master.MVMType.A:
                    Frm_Prevu frmType2 = new Frm_Prevu(Type);
                    frmType2.Owner = this;
                    frmType2.Show();
                    break;
                case Master.MVMType.CR:
                    Frm_Prevu frmType3 = new Frm_Prevu(Type);
                    frmType3.Owner = this;
                    frmType3.Show();
                    break;
                default:
                    MessageBox.Show("نوع الاستقبال غير معروف.");
                    break;
            }
        }

        private void btn_ovrirEn_Click_1(object sender, EventArgs e)
        {
            switch (Type)
            {
             
                case Master.MVMType.P:
                    Frm_Operation frmType1 = new Frm_Operation(Type);
                    frmType1.Owner = this;
                    frmType1.Show();
                    break;

                case Master.MVMType.A:
                    Frm_Operation frmType2 = new Frm_Operation(Type);
                    frmType2.Owner = this;
                    frmType2.Show();
                    break;
                case Master.MVMType.CR:
                    Frm_Operation frmType3 = new Frm_Operation(Type);
                    frmType3.Owner = this;
                    frmType3.Show();
                    break;
                default:
                    MessageBox.Show("نوع الاستقبال غير معروف.");
                    break;
            }
        }
    }
}