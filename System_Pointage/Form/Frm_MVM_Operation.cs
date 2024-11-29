using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Form;
using static System_Pointage.Form.Frm_Operation;

namespace System_Pointage.Classe
{
    public partial class Frm_MVM_Operation : DevExpress.XtraEditors.XtraForm
    {
        bool delete=false;
        Master.MVMType Type;
        DAL.Attent_Heder ATH;
        private BindingList<Models.AgentStatus> activeAgentsList;
        public BindingList<Models.AgentStatus> ActiveAgentsList
        {
            get { return activeAgentsList; }
            set { activeAgentsList = value; }
        }
        private BindingList<Models.AgentStatus> transferredAgentsList;
        public BindingList<Models.AgentStatus> TransferredAgentsList
        {
            get { return transferredAgentsList; }
            set { transferredAgentsList = value; }
        }
        public Frm_MVM_Operation(Master.MVMType _Type)
        {
            InitializeComponent();
            ATH=new DAL.Attent_Heder();
            Type = _Type;
            SetFormType();
        }
        void SetFormType()
        {
            switch (Type)
            {
    
                case Master.MVMType.P:
                    this.Text = "Liste des Rentrants";//
                    btn_list_prevu.Text = "Rentrant prévue";
                    layoutControlItem2.Text= "Date de Rentrant";
                    btn_ovrirEn.Text = "Séléction Agent Rentrant";
               //     layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                case Master.MVMType.A:
                    this.Text = "Liste des Absents";
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem2.Text = "Date de Absent";
                    btn_ovrirEn.Text = "Séléction Agent Absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "Liste des Partants";
                    btn_list_prevu.Text = "Partant prévue";
                    layoutControlItem2.Text = "Date de Partant";
                    btn_ovrirEn.Text = "Séléction Agent Partant";
                   // layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Frm_MVM_Operation_Load(object sender, EventArgs e)
        {
            activeAgentsList = new BindingList<Models.AgentStatus>();
            transferredAgentsList = new BindingList<Models.AgentStatus>();

            RefrechAttent();
            gridView2.DoubleClick += GridView2_DoubleClick;

            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            GridName();
        }

        private void GridView2_DoubleClick(object sender, EventArgs e)
        {
            delete = true;
            btn_delet.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            var agentDataService = new AgentDataService();
            var selectedRow = gridView2.GetFocusedRow() as DAL.Attent_Heder;
            ATH.ID = selectedRow.ID;
            if (selectedRow != null)
            {
                // استخراج ID_Attent_Liste
                int selectedID = selectedRow.ID;

                bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;
                int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;
                // جلب البيانات باستخدام GetAgentStatuses مع ID_Attent_Liste
                var agentData = agentDataService.GetAgentStatuses(userAccessPosteID, Type, isAdmin, selectedID,false);

                // تعيين البيانات إلى GridControl1
                gridControl1_mvm.DataSource = agentData;
                activeAgentsList = agentData;
            }
            else
            {
                XtraMessageBox.Show("Veuillez sélectionner une ligne.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void RefrechAttent()
        {
            btn_delet.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridControl1_mvm.DataSource = activeAgentsList;
            dateEdit1.DateTime = DateTime.Now;

            var db = new DAL.DataClasses1DataContext();
            gridControl1_attent.DataSource = db.Attent_Heders;
            gridView2.Columns["ID"].Visible = false;
            gridView2.OptionsBehavior.Editable = false;
        }
        public void AddTransferredAgentse(BindingList<Models.AgentStatus> transferredAgents)
        {
            // يمكن هنا إضافة البيانات إلى القائمة في Frm_MVM_Operation
            foreach (var agent in transferredAgents)
            {
                transferredAgentsList.Add(agent);
            }

            // تأكد من تحديث الواجهة إذا لزم الأمر
            gridControl1_mvm.DataSource = transferredAgentsList;
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
                //if (!IsAgentExists(agent))
                //{
                    activeAgentsList.Add(agent);
              //  }
            }
            FilterGrid();
            // تحديث البيانات في واجهة المستخدم
            gridControl1_mvm.RefreshDataSource();
        }
        //public bool IsAgentExists(Models.AgentStatus agent)
        //{
        //    return activeAgentsList != null && activeAgentsList.Any(existingAgent =>
        //        existingAgent.Name == agent.Name &&
        //        existingAgent.Date.Date == agent.Date.Date &&
        //        existingAgent.Statut == agent.Statut);
        //}

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

            DateTime date = dateEdit1.DateTime;
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
                if (delete == true) { Delete(); }
                activeAgentsList.Clear(); // إفراغ القائمة
                gridControl1_mvm.DataSource = new BindingList<Models.AgentStatus>(activeAgentsList); 
                gridView1.ClearSelection();
            }
        }

        public void GridName()
        {
            gridView1.GroupPanelText = " ";
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
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
                    break;
                case Master.MVMType.CR:
                    gridView1.Columns["Date"].Caption = "Date de rentrée";
                    gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
                    gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        #region
        private void LoadEligibleEmployees(DateTime selectedDate)
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    using (var context = new DAL.DataClasses1DataContext())
                    {
                        // استرجاع أحدث سجل لكل عامل
                        var latestStatusForEachEmployee = context.MVMAgentDetails
                            .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
                            .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
                            .Where(m => m.Statut == "CR" &&m.ID_Attent_Liste==null) // التأكد من أن الحالة "P"
                            .ToList();

                        // الربط بين الجداول بعد التحقق من الحالة
                        var eligibleEmployees = latestStatusForEachEmployee
                            .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
                                  m => m.ItemID,
                                  agent => agent.ID,
                                  (m, agent) => new { m, agent })
                                     .Join(context.Fiche_Postes, // الربط مع جدول Fiche_Poste
                  ma => ma.agent.ID_Post,
                  poste => poste.ID,
                  (ma, poste) => new
                  {
                      ma.agent.Matricule,
                      ma.agent.Name,
                      ma.m.Date,
                      ma.m.Statut,
                      ma.agent.Jour,
                      PosteName = poste.Name,
                      ma.agent.ScreenPosteD,
                      ma.agent.Affecter,
                      CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour.GetValueOrDefault()),
                      DaysCount = ma.m.Statut == "P"
                    ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
                    : ma.m.Statut == "CR"
                        ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
                        : 0 // الحالات الأخرى
                  }) // اختيار Name من Fiche_Agents
                            .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
                            .ToList();

                        // إنشاء BindingList جديدة
                        transferredAgentsList = new BindingList<Models.AgentStatus>(
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Matricule = x.Matricule,
                                Name = x.Name,
                                Poste = x.PosteName,
                                screenPosteD = x.ScreenPosteD ?? 0,
                                Affecter = x.Affecter,
                                Date = x.Date,
                                Jour = x.Jour ?? 0,
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Statut = x.Statut,
                                Difference = (x.DaysCount - (x.Jour ?? 0))
                            }).ToList()
                        );

                        FilterGrid();
                        // gridControl1_Prvu.DataSource = transferredAgentsList;
                        GridName();
                    }
                    break;
                case Master.MVMType.CR:
                    using (var context = new DAL.DataClasses1DataContext())
                    {
                        // استرجاع أحدث سجل لكل عامل
                        var latestStatusForEachEmployee = context.MVMAgentDetails
                            .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
                            .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
                            .Where(m => m.Statut == "P" && m.ID_Attent_Liste == null) // التأكد من أن الحالة "P"
                            .ToList();

                        // الربط بين الجداول بعد التحقق من الحالة
                        var eligibleEmployees = latestStatusForEachEmployee
                            .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
                                  m => m.ItemID,
                                  agent => agent.ID,
                                  (m, agent) => new { m, agent })
                                     .Join(context.Fiche_Postes, // الربط مع جدول Fiche_Poste
                  ma => ma.agent.ID_Post,
                  poste => poste.ID,
                  (ma, poste) => new
                  {
                      ma.agent.Matricule,
                      ma.agent.Name,
                      ma.m.Date,
                      ma.m.Statut,
                      ma.agent.Jour,
                      PosteName = poste.Name,
                      ma.agent.ScreenPosteD,
                      ma.agent.Affecter,
                      CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour.GetValueOrDefault()),// حساب التاريخ
                      DaysCount = ma.m.Statut == "P"
                    ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
                    : ma.m.Statut == "CR"
                        ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
                        : 0 // الحالات الأخرى

                  }) // اختيار Name من Fiche_Agents
                            .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
                            .ToList();

                        // إنشاء BindingList جديدة
                        transferredAgentsList = new BindingList<Models.AgentStatus>(
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Matricule = x.Matricule,
                                Name = x.Name,
                                Poste = x.PosteName,
                                screenPosteD = x.ScreenPosteD ?? 0,
                                Affecter = x.Affecter,
                                Date = x.Date,
                                Jour = x.Jour ?? 0,
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Statut = x.Statut,
                                Difference = (x.DaysCount - (x.Jour ?? 0))
                            }).ToList()
                        );
                        FilterGrid();
                        //gridControl1_Prvu.DataSource = transferredAgentsList;
                        GridName();
                    }

                    break;
                default:
                    break;

            }
        }
        void FilterGrid()
        {// التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            // دمج القائمتين مع الاحتفاظ بعنصر واحد فقط من العناصر المتكررة
            var mergedList = new BindingList<Models.AgentStatus>(
                transferredAgentsList
                    .Concat(activeAgentsList)
                     .Where(agent => isAdmin || agent.screenPosteD == userAccessPosteID)
                    .GroupBy(agent => agent.Name)
                    .Select(group => group.First()) // أخذ العنصر الأول من كل مجموعة
                    .ToList()
            );

            gridControl1_mvm.DataSource = new BindingList<Models.AgentStatus>(mergedList);
            //var mergedList = new BindingList<Models.AgentStatus>(
            //               transferredAgentsList
            //         .Concat(activeAgentsList)
            //            .GroupBy(agent => agent.Name)
            //            .Select(group => group.First()) // أخذ العنصر الأول من كل مجموعة
            //                .ToList()
            //              );
            //gridControl1_mvm.DataSource = mergedList;
        }
        #endregion
        private void btn_list_prevu_Click_1(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue is DateTime selectedDate)
            {
                LoadEligibleEmployees(selectedDate);
                GridName();
            }
            //switch (Type)
            //{
            //    case Master.MVMType.P:
            //        Frm_Prevu frmType1 = new Frm_Prevu(Type);
            //        frmType1.Owner = this;
            //        frmType1.Show();
            //        break;

            //    case Master.MVMType.A:
            //        Frm_Prevu frmType2 = new Frm_Prevu(Type);
            //        frmType2.Owner = this;
            //        frmType2.Show();
            //        break;
            //    case Master.MVMType.CR:
            //        Frm_Prevu frmType3 = new Frm_Prevu(Type);
            //        frmType3.Owner = this;
            //        frmType3.Show();
            //        break;
            //    default:
            //        MessageBox.Show("نوع الاستقبال غير معروف.");
            //        break;
            //}
        }

        private void btn_ovrirEn_Click_1(object sender, EventArgs e)
        {
            switch (Type)
            {

                case Master.MVMType.P:
                    Frm_Operation frmType1 = new Frm_Operation(Type,this);
                    frmType1.Owner = this;
                    frmType1.Show();
                    break;

                case Master.MVMType.A:
                    Frm_Operation frmType2 = new Frm_Operation(Type,this);
                    frmType2.Owner = this;
                    frmType2.Show();
                    break;
                case Master.MVMType.CR:
                    Frm_Operation frmType3 = new Frm_Operation(Type,this);
                    frmType3.Owner = this;
                    frmType3.Show();
                    break;
                default:
                    MessageBox.Show("نوع الاستقبال غير معروف.");
                    break;
            }
        }

        private void btn_attent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

            //string statut = Type == Master.MVMType.P ? "P" : (Type == Master.MVMType.A ? "A" : "CR");
            DateTime date = dateEdit1.DateTime;

            using (var context = new DAL.DataClasses1DataContext())
            {
                var existingAth = context.Attent_Heders.FirstOrDefault(a => a.ID == ATH.ID);

                if (existingAth == null)
                {
                    ATH.Date = date;
                    // إذا لم يكن السجل موجودًا، أضف سجلًا جديدًا
                    context.Attent_Heders.InsertOnSubmit(ATH);
                }
                else
                {
                    // إذا كان السجل موجودًا، قم بتحديث القيم
                    existingAth.Date = date; // تحديث التاريخ

                    // تعيين الحالة إلى Modified
                    context.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, existingAth);
                }

                //if (ATH.ID == 0)
                //{
                //    context.Attent_Heders.InsertOnSubmit(ATH);
                //}
                //else
                //{
                //    context.Attent_Heders.Attach(ATH);
                //}
        
                //ATH.Date = dateEdit1.DateTime;
                context.SubmitChanges(); 

                int hederID = ATH.ID; 
                foreach (var agent in selectedAgents)
                {
                    // جلب معرف ItemID بناءً على الاسم
                    int itemID = context.Fiche_Agents.FirstOrDefault(f => f.Name == agent.Name)?.ID ?? 0;

                    string statut1 = agent.Statut; // استخدم القيمة مباشرة من agent
                    DateTime? dt = context.MVMAgentDetails.FirstOrDefault(f => f.Date == agent.Date)?.Date;

                    if (itemID == 0)
                    {
                        XtraMessageBox.Show($"Agent non trouvé: {agent.Name}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if (!dt.HasValue)
                    {
                        XtraMessageBox.Show($"Date non trouvée pour l'agent: {agent.Name}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                   
                    // استرجاع السجل الحالي
                    var existingAgentDetail = context.MVMAgentDetails.FirstOrDefault(ad => ad.ItemID == itemID && ad.Date == dt.Value && ad.Statut == statut1);

                    if (existingAgentDetail != null)
                    {
                        // تحديث فقط حقل ID_Attent_Liste
                        existingAgentDetail.ID_Attent_Liste = hederID; // تحديث حقل ID_Attent_Liste بقيمة معرف Attent_Heder
                        context.SubmitChanges(); // حفظ التغييرات
                    }
                   
                }
            }
       
            activeAgentsList.Clear();
            RefrechAttent();
        }

        private void btn_clear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            delete = false;
            activeAgentsList.Clear();
            RefrechAttent();
        }

        private void btn_delet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
            if (XtraMessageBox.Show(text: "Voulez-vous supprimer?", caption: "Confirmer la suppression", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                delete = false;
                Delete();
            }
        }
        void Delete()
        {
            gridView1.SelectAll();
            using (var context = new DAL.DataClasses1DataContext())
            {
               
                    var selectedRows = gridView1.GetSelectedRows();

                    List<Models.AgentStatus> selectedAgents = new List<Models.AgentStatus>();

                    foreach (var rowIndex in selectedRows)
                    {
                        var agent = gridView1.GetRow(rowIndex) as Models.AgentStatus;
                        if (agent != null)
                        {
                            selectedAgents.Add(agent);
                        }
                    }
                    foreach (var agent in selectedAgents)
                    {
                        // جلب معرف ItemID بناءً على الاسم
                        int itemID = context.Fiche_Agents.FirstOrDefault(f => f.Name == agent.Name)?.ID ?? 0;

                        string statut1 = agent.Statut; // استخدم القيمة مباشرة من agent
                        DateTime? dt = context.MVMAgentDetails.FirstOrDefault(f => f.Date == agent.Date)?.Date;



                        // استرجاع السجل الحالي
                        var existingAgentDetail = context.MVMAgentDetails.FirstOrDefault(ad => ad.ItemID == itemID && ad.Date == dt.Value && ad.Statut == statut1);

                        if (existingAgentDetail != null)
                        {
                            // تحديث فقط حقل ID_Attent_Liste
                            existingAgentDetail.ID_Attent_Liste = null; // تحديث حقل ID_Attent_Liste بقيمة معرف Attent_Heder
                            context.SubmitChanges(); // حفظ التغييرات
                        }

                    }
                    var log = context.Attent_Heders.FirstOrDefault(a => a.ID == ATH.ID);
                    if (log != null)
                    {
                        context.Attent_Heders.DeleteOnSubmit(log);
                        context.SubmitChanges();
                    }
                    XtraMessageBox.Show("Supprimer succés");
                    activeAgentsList.Clear();
                    RefrechAttent();
                
            }
        }
    }
}