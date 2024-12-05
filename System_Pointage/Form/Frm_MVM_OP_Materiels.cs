using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_MVM_OP_Materiels : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.MaterielsStatus> activeAgentsList;
        public BindingList<Models.MaterielsStatus> ActiveAgentsList
        {
            get { return activeAgentsList; }
            set { activeAgentsList = value; }
        }
        private BindingList<Models.MaterielsStatus> transferredAgentsList;
        public BindingList<Models.MaterielsStatus> TransferredAgentsList
        {
            get { return transferredAgentsList; }
            set { transferredAgentsList = value; }
        }
        public Frm_MVM_OP_Materiels()
        {
            InitializeComponent();
        }

        private void Frm_MVM_OP_Materiels_Load(object sender, EventArgs e)
        {
            activeAgentsList = new BindingList<Models.MaterielsStatus>();
            transferredAgentsList = new BindingList<Models.MaterielsStatus>();


            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
           // GridName();
        }
        //public void AddTransferredAgentse(BindingList<Models.MaterielsStatus> transferredAgents)
        //{
        //    // يمكن هنا إضافة البيانات إلى القائمة في Frm_MVM_Operation
        //    foreach (var agent in transferredAgents)
        //    {
        //        transferredAgentsList.Add(agent);
        //    }

        //    // تأكد من تحديث الواجهة إذا لزم الأمر
        //    gridControl1_mvm.DataSource = transferredAgentsList;
        //}
        public void AddTransferredAgents(BindingList<Models.MaterielsStatus> transferredAgents)
        {
            foreach (var agent in transferredAgents)
            {
                activeAgentsList.Add(agent);
            }
             FilterGrid();
            gridControl1_mvm.RefreshDataSource();
        }
        private void btn_ovrirEn_Click(object sender, EventArgs e)
        {
            Frm_OP_Materiels frmType1 = new Frm_OP_Materiels(this);
            frmType1.Owner = this;
            frmType1.Show();
        }
        void New()
        {
            activeAgentsList = new BindingList<Models.MaterielsStatus>();
            transferredAgentsList = new BindingList<Models.MaterielsStatus>();
           // GridColumnSup();
        }
        void FilterGrid()
        {// التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            // دمج القائمتين مع الاحتفاظ بعنصر واحد فقط من العناصر المتكررة
            var mergedList = new BindingList<Models.MaterielsStatus>(
                transferredAgentsList
                    .Concat(activeAgentsList)
                     .Where(agent => isAdmin || agent.screenPosteD == userAccessPosteID)
                    .GroupBy(agent => agent.Name)
                    .Select(group => group.First()) // أخذ العنصر الأول من كل مجموعة
                    .ToList()
            );

            gridControl1_mvm.DataSource = new BindingList<Models.MaterielsStatus>(mergedList);
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner au moins une ligne à enregistrer.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dateEdit1.DateTime > DateTime.Now)
            {
                XtraMessageBox.Show("Veuillez choisir une date qui ne dépasse pas la date actuelle.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            var agentService = new AgentDataService();

            foreach (var rowIndex in selectedRows)
            {
                var agent = gridView1.GetRow(rowIndex) as Models.MaterielsStatus;
                if (agent != null)
                {
                    // التحقق من وجود سجل بنفس التاريخ
                    if (agentService.DoesMaterielRecordExist(agent.Matricule, dateEdit1.DateTime))
                    {
                        XtraMessageBox.Show($"L'agent avec le matricule {agent.Matricule} possède déjà une entrée pour la date {dateEdit1.DateTime:dd/MM/yyyy}.",
                            "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            //
            List<Models.MaterielsStatus> selectedAgents = new List<Models.MaterielsStatus>();
            foreach (var rowIndex in selectedRows)
            {
                var agent = gridView1.GetRow(rowIndex) as Models.MaterielsStatus;
                if (agent != null)
                {
                    selectedAgents.Add(agent);
                }
            }       
                    if (comboBoxEdit1.Text == "")
                    {
                        comboBoxEdit1.ErrorText = Classe.Master.ErrorText;
                        return;
                    }
                           
            DateTime date = dateEdit1.DateTime;
            using (var context = new DAL.DataClasses1DataContext())
            {
                foreach (var agent in selectedAgents)
                {
                    var newAgentDetail = new DAL.MVMmaterielsDetail
                    {
                        ItemID = context.Fiche_Matricules.FirstOrDefault(f => f.Matricule == agent.Matricule)?.ID ?? 0,
                        Date = date,
                        Statut = comboBoxEdit1.Text,
                    };

                    context.MVMmaterielsDetails.InsertOnSubmit(newAgentDetail);
                    UserLogAction userLogAction = null;
                    UserLogAction.ActionType actionType = UserLogAction.ActionType.Sortie;

                            if (comboBoxEdit1.Text == "A")
                            {
                                actionType = UserLogAction.ActionType.Absent;
                                userLogAction = new UserLogAction
                                {
                                    PartID = newAgentDetail.ItemID,
                                    PartName = $"Absence la Agent: {agent.Name} A-",
                                    Name = "Frm_MVM_Operation_A",
                                };
                            }
                            else if (comboBoxEdit1.Text == "P")
                            {
                                actionType = UserLogAction.ActionType.Absent;
                                userLogAction = new UserLogAction
                                {
                                    PartID = newAgentDetail.ItemID,
                                    PartName = $"Maladie la Agent: {agent.Name} M-",
                                    Name = "Frm_MVM_Operation_A",
                                };
                            }
                           
                    userLogAction.SaveAction(() => { context.SubmitChanges(); }, actionType);
                }

                context.SubmitChanges();
                XtraMessageBox.Show("Enregistrer succés");
              
                activeAgentsList.Clear();
                gridControl1_mvm.DataSource = new BindingList<Models.MaterielsStatus>(activeAgentsList);
                gridView1.ClearSelection();
            }
        }
    }
}