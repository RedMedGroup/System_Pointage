using DevExpress.Charts.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
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
            New();
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
                         layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                case Master.MVMType.A:
                    this.Text = "Liste des Absents";
                    layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlGroup1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    btn_attent.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; 
                    layoutControlItem2.Text = "Date de Absent";
                    btn_ovrirEn.Text = "Séléction Agent Absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "Liste des Partants";
                    btn_list_prevu.Text = "Partant prévue";
                    layoutControlItem2.Text = "Date de Partant";
                    btn_ovrirEn.Text = "Séléction Agent Partant";
                     layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Frm_MVM_Operation_Load(object sender, EventArgs e)
        {
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " ";
            #endregion
            activeAgentsList = new BindingList<Models.AgentStatus>();
            transferredAgentsList = new BindingList<Models.AgentStatus>();

            RefrechAttent();
            gridView2.DoubleClick += GridView2_DoubleClick;

            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            GridName();
        }
        int selectedID;
        private void GridView2_DoubleClick(object sender, EventArgs e)
        {
            delete = true;
            btn_delet.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
          //  var agentDataService = new AgentDataService();
            var selectedRow = gridView2.GetFocusedRow() as DAL.Attent_Heder;
            ATH.ID = selectedRow.ID;
            dateEdit1.DateTime = selectedRow.Date;
            if (selectedRow != null)
            {
                 selectedID = selectedRow.ID;

                GridRefrechAgent();
                #region Ajouter bt

                GridColumnSup();

                // إنشاء RepositoryItemButtonEdit جديد
                RepositoryItemButtonEdit buttonEdit = new RepositoryItemButtonEdit
                {
                    TextEditStyle = TextEditStyles.HideTextEditor
                };
                buttonEdit.Buttons.Clear();
                buttonEdit.Buttons.Add(new EditorButton(ButtonPredefines.Minus));
                buttonEdit.ButtonClick += ButtonEdit_ButtonClick;

                gridControl1_mvm.RepositoryItems.Add(buttonEdit);

                // إنشاء العمود الجديد
                GridColumn clmnDelete = new GridColumn
                {
                    Name = "clmnDelete",
                    Caption = "Sup",
                    FieldName = "Delete",
                    UnboundType = DevExpress.Data.UnboundColumnType.Object,
                    ColumnEdit = buttonEdit,
                    VisibleIndex = gridView1.Columns.Count,
                    Width = 50,
                    MaxWidth = 50,
                    Visible = true
                };

                // إضافة العمود إلى gridView1
                gridView1.Columns.Add(clmnDelete);

                // تحديث بيانات gridView1
                gridView1.RefreshData();

                #endregion

            }
            else
            {
                XtraMessageBox.Show("Veuillez sélectionner une ligne.", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void GridColumnSup()
        {
            // تحقق مما إذا كان العمود suprimer  موجودًا بالفعل
            GridColumn existingColumn = null;
            foreach (GridColumn column in gridView1.Columns)
            {
                if (column.FieldName == "Delete")
                {
                    existingColumn = column;
                    break;
                }
            }

            if (existingColumn != null)
            {
                gridView1.Columns.Remove(existingColumn);
            }
        }
        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
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
                GridRefrechAgent();
            }
        }
      void  GridRefrechAgent()
        {
            var agentDataService = new AgentDataService();
            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager;

            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            var agentData = agentDataService.GetAgentStatuses(userAccessPosteID, Type, isAdminOrManager, selectedID, false, "frm_op");

            gridControl1_mvm.DataSource = agentData;
            activeAgentsList = agentData;
        }
        void RefrechAttent()
        {
            var db = new DAL.DataClasses1DataContext();
            // gridControl1_mvm.DataSource = new BindingList<Models.AgentStatus>(activeAgentsList);
            btn_delet.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            switch (Type)
            {
                case Master.MVMType.P:
                    gridControl1_attent.DataSource = db.Attent_Heders.Where(x=>x.Type==0);
                    gridView2.Columns["ID"].Visible = false;
                    gridView2.Columns["Type"].Visible = false;
                    break;
                case Master.MVMType.CR:
                    gridControl1_attent.DataSource = db.Attent_Heders.Where(x => x.Type == 1);
                    gridView2.Columns["ID"].Visible = false;
                    gridView2.Columns["Type"].Visible = false;
                    break;
                        default:
                    break;
            }   
            gridControl1_mvm.DataSource = activeAgentsList;
            dateEdit1.DateTime = DateTime.Now;

           
          
            
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
        //public bool IsAgentExistse(Models.AgentStatus agent)
        //{
        //    return transferredAgentsList.Any(a => a.Name == agent.Name);
        //}
    public void AddTransferredAgents(BindingList<Models.AgentStatus> transferredAgents)
        {
            foreach (var agent in transferredAgents)
            {
                    activeAgentsList.Add(agent);
            }
            FilterGrid();
            gridControl1_mvm.RefreshDataSource();
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
                var agent = gridView1.GetRow(rowIndex) as Models.AgentStatus;
                if (agent != null)
                {
                    // التحقق من وجود سجل بنفس التاريخ
                    if (agentService.DoesAgentRecordExist(agent.Matricule, dateEdit1.DateTime))
                    {
                        XtraMessageBox.Show($"L'agent avec le matricule {agent.Matricule} possède déjà une entrée pour la date {dateEdit1.DateTime:dd/MM/yyyy}.",
                            "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            //
            List<Models.AgentStatus> selectedAgents = new List<Models.AgentStatus>();
            foreach (var rowIndex in selectedRows)
            {
                var agent = gridView1.GetRow(rowIndex) as Models.AgentStatus;
                if (agent != null)
                {
                    selectedAgents.Add(agent);
                }
            }
            string statut;
            switch (Type)
            {
                case Master.MVMType.P:
                     statut = Type == Master.MVMType.P ? "P" : (Type == Master.MVMType.A ? "A" : "CR");
                    break;
                case Master.MVMType.A:
                    if (comboBoxEdit1.Text == "")
                    {
                        comboBoxEdit1.ErrorText =Classe.Master.ErrorText;
                        return;
                    }
                   statut= comboBoxEdit1.Text;
                    break;
                case Master.MVMType.CR:
                     statut = Type == Master.MVMType.P ? "P" : (Type == Master.MVMType.A ? "A" : "CR");
                    break;
                default:
                    throw new NotImplementedException();
            }    

             DateTime date = dateEdit1.DateTime;
            //using (var context = new DAL.DataClasses1DataContext())
            //{
            //    UserLogAction userLogAction = null;
            //    UserLogAction.ActionType actionType;

            //    switch (Type)
            //    {
            //        case Master.MVMType.P:
            //            actionType = UserLogAction.ActionType.Entrée;
            //            break;
            //        case Master.MVMType.A:
            //            actionType = UserLogAction.ActionType.Absent;
            //            break;
            //        case Master.MVMType.CR:
            //            actionType = UserLogAction.ActionType.Sortie;
            //            break;
            //        default:
            //            actionType = UserLogAction.ActionType.Sortie;  // Default case
            //            break;
            //    }

            //    // التعامل مع كل agent مختار
            //    foreach (var agent in selectedAgents)
            //    {
            //        var newAgentDetail = new DAL.MVMAgentDetail
            //        {
            //            ItemID = context.Fiche_Agents.FirstOrDefault(f => f.Matricule == agent.Matricule)?.ID ?? 0,
            //            Date = date,
            //            Statut = statut
            //        };

                    // تحديد تفاصيل الحفظ بناءً على النوع
            //        switch (Type)
            //        {
            //            case Master.MVMType.P:
            //                userLogAction = new UserLogAction
            //                {
            //                    PartID = newAgentDetail.ItemID,
            //                    PartName = $"Rentrée la Agent: {agent.Name} P-",
            //                    Name = "Frm_MVM_Operation_P",
            //                };
            //                break;
            //            case Master.MVMType.A:
            //                if (comboBoxEdit1.Text == "A")
            //                {
            //                    userLogAction = new UserLogAction
            //                    {
            //                        PartID = newAgentDetail.ItemID,
            //                        PartName = $"Absence la Agent: {agent.Name} A-",
            //                        Name = "Frm_MVM_Operation_A",
            //                    };
            //                }
            //                else if (comboBoxEdit1.Text == "M")
            //                {
            //                    userLogAction = new UserLogAction
            //                    {
            //                        PartID = newAgentDetail.ItemID,
            //                        PartName = $"Maladie la Agent: {agent.Name} M-",
            //                        Name = "Frm_MVM_Operation_A",
            //                    };
            //                }
            //                else if (comboBoxEdit1.Text == "AA")
            //                {
            //                    userLogAction = new UserLogAction
            //                    {
            //                        PartID = newAgentDetail.ItemID,
            //                        PartName = $"Absence autorisée la Agent: {agent.Name} AA-",
            //                        Name = "Frm_MVM_Operation_A",
            //                    };
            //                }
            //                break;
            //            case Master.MVMType.CR:
            //                userLogAction = new UserLogAction
            //                {
            //                    PartID = newAgentDetail.ItemID,
            //                    PartName = $"Sortant la Agent: {agent.Name} CR-",
            //                    Name = "Frm_MVM_Operation_CR",
            //                };
            //                break;
            //            default:
            //                break;
            //        }

            //        context.MVMAgentDetails.InsertOnSubmit(newAgentDetail);

            //        userLogAction.SaveAction(() =>
            //        {
            //            context.SubmitChanges();
            //            XtraMessageBox.Show("Enregistrer succés");
            //        }, actionType);
            //    }
            //}

            using (var context = new DAL.DataClasses1DataContext())
            {
                UserLogAction userLogAction = null;
                //  UserLogAction.ActionType actionType = UserLogAction.ActionType.Sortie;
                UserLogAction.ActionType actionType;


                switch (Type)  // Switch on the actual enum values
                {
                    case Master.MVMType.P:
                        actionType = UserLogAction.ActionType.Entrée;
                        break;
                    case Master.MVMType.A:
                        actionType = UserLogAction.ActionType.Absent;
                        break;
                    case Master.MVMType.CR:
                        actionType = UserLogAction.ActionType.Sortie;
                        break;
                    default:
                        actionType = UserLogAction.ActionType.Sortie;  // Default case
                        break;
                }


                foreach (var agent in selectedAgents)
                {
                    var newAgentDetail = new DAL.MVMAgentDetail
                    {
                        ItemID = context.Fiche_Agents.FirstOrDefault(f => f.Matricule == agent.Matricule)?.ID ?? 0,
                        Date = date,
                        Statut = statut
                    };



                    switch (Type)
            {
                case Master.MVMType.P:
                    actionType = UserLogAction.ActionType.Entrée;
                    userLogAction = new UserLogAction
                    {
                        PartID = newAgentDetail.ItemID,
                        PartName = $"Rentrée la Agent: {agent.Name} P-",
                        Name = "Frm_MVM_Operation_P",
                        //    IsNew = true 
                    };
                    break;
                case Master.MVMType.A:
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
                    else if (comboBoxEdit1.Text == "M")
                    {
                        actionType = UserLogAction.ActionType.Absent;
                        userLogAction = new UserLogAction
                        {
                            PartID = newAgentDetail.ItemID,
                            PartName = $"Maladie la Agent: {agent.Name} M-",
                            Name = "Frm_MVM_Operation_A",
                        };
                    }
                    else if (comboBoxEdit1.Text == "AA")
                    {
                        actionType = UserLogAction.ActionType.Absent;
                        userLogAction = new UserLogAction
                        {
                            PartID = newAgentDetail.ItemID,
                            PartName = $"Absence autorisée la Agent: {agent.Name} AA-",
                            Name = "Frm_MVM_Operation_A",
                        };
                    }


                    break;
                case Master.MVMType.CR:
                    actionType = UserLogAction.ActionType.Sortie;
                    userLogAction = new UserLogAction
                    {
                        PartID = newAgentDetail.ItemID,
                        PartName = $"Sortant la Agent: {agent.Name} CR-",
                        Name = "Frm_MVM_Operation_CR",
                        //    IsNew = true
                    };

                    break;
                default:
                    break;
            }
            context.MVMAgentDetails.InsertOnSubmit(newAgentDetail);
            //  userLogAction.SaveAction(() => { context.SubmitChanges(); }, actionType);
        }

        userLogAction.SaveAction(() =>
                {
                    context.SubmitChanges();
                    XtraMessageBox.Show("Enregistrer succés");
                }, actionType);

            if (delete == true)
            {
                ValiderAgentToListeAttent();
                #region
                var log = context.Attent_Heders.FirstOrDefault(a => a.ID == ATH.ID);
                if (log != null)
                {
                    // التحقق من عدم وجود ID_Attent_Liste في جدول MVMAgentDetails
                    bool existsInMVMAgentDetails = context.MVMAgentDetails.Any(agent => agent.ID_Attent_Liste == ATH.ID);

                    // إذا لم يكن موجودًا في جدول MVMAgentDetails، قم بحذفه
                    if (!existsInMVMAgentDetails)
                    {
                        context.Attent_Heders.DeleteOnSubmit(log);
                        context.SubmitChanges();
                    }

                }
                #endregion
                RefrechAttent();
            }
            activeAgentsList.Clear();
            gridControl1_mvm.DataSource = new BindingList<Models.AgentStatus>(activeAgentsList);
            gridView1.ClearSelection();
        }
        }
        void ValiderAgentToListeAttent()
        {
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
                    int itemID = context.Fiche_Agents.FirstOrDefault(f => f.Matricule == agent.Matricule)?.ID ?? 0;

                    string statut1 = agent.Statut; 
                    DateTime? dt = context.MVMAgentDetails.FirstOrDefault(f => f.Date == agent.Date)?.Date;



                    // استرجاع السجل الحالي
                    var existingAgentDetail = context.MVMAgentDetails.FirstOrDefault(ad => ad.ItemID == itemID && ad.Date == dt.Value && ad.Statut == statut1);

                    if (existingAgentDetail != null)
                    {
                        // تحديث فقط حقل ID_Attent_Liste
                        existingAgentDetail.ID_Attent_Liste = null; 
                        context.SubmitChanges(); 
                    }

                }
            }
        }
        public void GridName()
        {
            gridView1.GroupPanelText = " ";
            gridView1.Columns["Name"].Caption = "Nom";
            gridView1.Columns["FirstName"].Caption = "prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Difference"].Caption = "Ecart";
            gridView1.Columns["Matricule"].VisibleIndex = 1;
            gridView1.Columns["Name"].VisibleIndex = 2;
            gridView1.Columns["FirstName"].VisibleIndex =3;
            gridView1.Columns["Poste"].VisibleIndex =4;
            gridView1.Columns["Affecter"].VisibleIndex =5;
            gridView1.Columns["Date"].VisibleIndex =6;
            gridView1.Columns["Jour"].VisibleIndex =7;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 8;
            gridView1.Columns["DaysCount"].VisibleIndex =9;
            gridView1.Columns["Difference"].VisibleIndex = 10;
            gridView1.Columns["Statut"].VisibleIndex =11;
            gridView1.Columns["screenPosteD"].Visible = false;
            gridView1.Columns["Statutmvm"].Visible = false;
            gridView1.Columns["Jour"].Visible=false;
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
                        // استرجاع قيمة Jour من جدول Config_Jour
                        var configJour = context.Config_Jours.FirstOrDefault(); // افترضنا أن هناك سجل واحد فقط في الجدول

                        if (configJour == null)
                        {
                            throw new Exception("Aucun enregistrement trouvé dans la table Config_Jour.");
                        }

                        int jour = configJour.Jour;

                        // استرجاع أحدث سجل لكل عامل
                        var latestStatusForEachEmployee = context.MVMAgentDetails
                            .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
                            .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
                            .Where(m => m.Statut == "CR" && m.ID_Attent_Liste == null) // التأكد من أن الحالة 
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
                                      ma.agent.FirstName,
                                      ma.m.Date,
                                      ma.m.Statut,
                                      PosteName = poste.Name,
                                      ma.agent.ScreenPosteD,
                                      ma.agent.Affecter,
                                      CalculatedDate = ma.m.Date.AddDays(jour), // استخدام Jour من Config_Jour
                                      DaysCount = ma.m.Statut == "P"
                                        ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
                                        : ma.m.Statut == "CR"
                                            ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
                                            : 0 // الحالات الأخرى
                                  }) 
                            .Where(x => (selectedDate - x.Date).TotalDays >= jour) 
                            .ToList();

                        transferredAgentsList = new BindingList<Models.AgentStatus>(   
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Matricule = x.Matricule,
                                Name = x.Name,
                                FirstName = x.FirstName,
                                Poste = x.PosteName,
                                screenPosteD = x.ScreenPosteD ,
                                Affecter = x.Affecter,
                                Date = x.Date,
                                Jour = jour, 
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Statut = x.Statut,
                                Difference = (x.DaysCount - jour) 
                            }).ToList()
                        );

                        FilterGrid();
                        GridName();
                    }
                    break;
                case Master.MVMType.CR:
                    using (var context = new DAL.DataClasses1DataContext())
                    {
                        var configJour = context.Config_Jours.FirstOrDefault(); 

                        if (configJour == null)
                        {
                            throw new Exception("Aucun enregistrement trouvé dans la table Config_Jour.");
                        }

                        int jour = configJour.Jour;

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
                                      ma.agent.FirstName,
                                      ma.m.Date,
                                      ma.m.Statut,
                                      PosteName = poste.Name,
                                      ma.agent.ScreenPosteD,
                                      ma.agent.Affecter,
                                      CalculatedDate = ma.m.Date.AddDays(jour), // استخدام Jour من Config_Jour
                                      DaysCount = ma.m.Statut == "P"
                                        ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
                                        : ma.m.Statut == "CR"
                                            ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
                                            : 0 // الحالات الأخرى
                                  }) // اختيار Name من Fiche_Agents
                            .Where(x => (selectedDate - x.Date).TotalDays >= jour) // استخدام Jour من Config_Jour
                            .ToList();

                        // إنشاء BindingList جديدة
                        transferredAgentsList = new BindingList<Models.AgentStatus>(
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Matricule = x.Matricule,
                                Name = x.Name,
                                FirstName=x.FirstName,
                                Poste = x.PosteName,
                                screenPosteD = x.ScreenPosteD ,
                                Affecter = x.Affecter,
                                Date = x.Date,
                                Jour = jour, // استخدام Jour من Config_Jour
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Statut = x.Statut,
                                Difference = (x.DaysCount - jour) // استخدام Jour من Config_Jour
                            }).ToList()
                        );

                        FilterGrid();
                        GridName();
                    }
                    break;
                default:
                    break;
            }
        }
        //private void LoadEligibleEmployees(DateTime selectedDate)
        //{
        //    switch (Type)
        //    {
        //        case Master.MVMType.P:
        //            using (var context = new DAL.DataClasses1DataContext())
        //            {
        //                // استرجاع أحدث سجل لكل عامل
        //                var latestStatusForEachEmployee = context.MVMAgentDetails
        //                    .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
        //                    .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
        //                    .Where(m => m.Statut == "CR" &&m.ID_Attent_Liste==null) // التأكد من أن الحالة "P"
        //                    .ToList();

        //                // الربط بين الجداول بعد التحقق من الحالة
        //                var eligibleEmployees = latestStatusForEachEmployee
        //                    .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
        //                          m => m.ItemID,
        //                          agent => agent.ID,
        //                          (m, agent) => new { m, agent })
        //                             .Join(context.Fiche_Postes, // الربط مع جدول Fiche_Poste
        //          ma => ma.agent.ID_Post,
        //          poste => poste.ID,
        //          (ma, poste) => new
        //          {
        //              ma.agent.Matricule,
        //              ma.agent.Name,
        //              ma.m.Date,
        //              ma.m.Statut,
        //              ma.agent.Jour,
        //              PosteName = poste.Name,
        //              ma.agent.ScreenPosteD,
        //              ma.agent.Affecter,
        //              CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour),
        //              DaysCount = ma.m.Statut == "P"
        //            ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
        //            : ma.m.Statut == "CR"
        //                ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
        //                : 0 // الحالات الأخرى
        //          }) // اختيار Name من Fiche_Agents
        //                    .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
        //                    .ToList();

        //                // إنشاء BindingList جديدة
        //                transferredAgentsList = new BindingList<Models.AgentStatus>(
        //                    eligibleEmployees.Select(x => new Models.AgentStatus
        //                    {
        //                        Matricule = x.Matricule,
        //                        Name = x.Name,
        //                        Poste = x.PosteName,
        //                        screenPosteD = x.ScreenPosteD ?? 0,
        //                        Affecter = x.Affecter,
        //                        Date = x.Date,
        //                        Jour = x.Jour ,
        //                        CalculatedDate = x.CalculatedDate,
        //                        DaysCount = x.DaysCount,
        //                        Statut = x.Statut,
        //                        Difference = (x.DaysCount - (x.Jour ))
        //                    }).ToList()
        //                );

        //                FilterGrid();
        //                GridName();
        //            }
        //            break;
        //        case Master.MVMType.CR:
        //            using (var context = new DAL.DataClasses1DataContext())
        //            {
        //                // استرجاع أحدث سجل لكل عامل
        //                var latestStatusForEachEmployee = context.MVMAgentDetails
        //                    .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
        //                    .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
        //                    .Where(m => m.Statut == "P" && m.ID_Attent_Liste == null) // التأكد من أن الحالة "P"
        //                    .ToList();

        //                // الربط بين الجداول بعد التحقق من الحالة
        //                var eligibleEmployees = latestStatusForEachEmployee
        //                    .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
        //                          m => m.ItemID,
        //                          agent => agent.ID,
        //                          (m, agent) => new { m, agent })
        //                             .Join(context.Fiche_Postes, // الربط مع جدول Fiche_Poste
        //          ma => ma.agent.ID_Post,
        //          poste => poste.ID,
        //          (ma, poste) => new
        //          {
        //              ma.agent.Matricule,
        //              ma.agent.Name,
        //              ma.m.Date,
        //              ma.m.Statut,
        //              ma.agent.Jour,
        //              PosteName = poste.Name,
        //              ma.agent.ScreenPosteD,
        //              ma.agent.Affecter,
        //              CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour),// حساب التاريخ
        //              DaysCount = ma.m.Statut == "P"
        //            ? (DateTime.Now - ma.m.Date).Days // إذا كان يعمل
        //            : ma.m.Statut == "CR"
        //                ? (DateTime.Now - ma.m.Date).Days // إذا كان في عطلة
        //                : 0 // الحالات الأخرى

        //          }) // اختيار Name من Fiche_Agents
        //                    .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
        //                    .ToList();

        //                // إنشاء BindingList جديدة
        //                transferredAgentsList = new BindingList<Models.AgentStatus>(
        //                    eligibleEmployees.Select(x => new Models.AgentStatus
        //                    {
        //                        Matricule = x.Matricule,
        //                        Name = x.Name,
        //                        Poste = x.PosteName,
        //                        screenPosteD = x.ScreenPosteD ?? 0,
        //                        Affecter = x.Affecter,
        //                        Date = x.Date,
        //                        Jour = x.Jour,
        //                        CalculatedDate = x.CalculatedDate,
        //                        DaysCount = x.DaysCount,
        //                        Statut = x.Statut,
        //                        Difference = (x.DaysCount - (x.Jour))
        //                    }).ToList()
        //                );
        //                FilterGrid();
        //                //gridControl1_Prvu.DataSource = transferredAgentsList;
        //                GridName();
        //            }

        //            break;
        //        default:
        //            break;

        //    }
        //}
        void FilterGrid()
        {
            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager;

            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            var mergedList = new BindingList<Models.AgentStatus>(
                transferredAgentsList
                    .Concat(activeAgentsList)
                     .Where(agent => isAdminOrManager || agent.screenPosteD == userAccessPosteID)
                    .GroupBy(agent => agent.Name)
                    .Select(group => group.First()) // أخذ العنصر الأول من كل مجموعة
                    .ToList()
            );

            gridControl1_mvm.DataSource = new BindingList<Models.AgentStatus>(mergedList);
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
                    switch (Type)
                    {
                        case Master.MVMType.P:
                            ATH.Type = 0;
                            break;
                        case Master.MVMType.CR:
                            ATH.Type= 1;
                            break;
                        default:
                            break;
                    }
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

                context.SubmitChanges(); 

                int hederID = ATH.ID; 
                foreach (var agent in selectedAgents)
                {
                    // جلب معرف ItemID بناءً على الاسم
                    int itemID = context.Fiche_Agents.FirstOrDefault(f => f.Matricule == agent.Matricule)?.ID ?? 0;

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
                        New();
                    }
                   
                }
            }
       
            activeAgentsList.Clear();
            RefrechAttent();
        }
        void New()
        {
            activeAgentsList = new BindingList<Models.AgentStatus>();
         transferredAgentsList = new BindingList<Models.AgentStatus>();
            ATH = new DAL.Attent_Heder();
            GridColumnSup();
        }
        private void btn_clear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            delete = false;
            activeAgentsList.Clear();
            RefrechAttent();
            New();
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
                        int itemID = context.Fiche_Agents.FirstOrDefault(f => f.Name == agent.Name)?.ID ?? 0;

                        string statut1 = agent.Statut;
                        DateTime? dt = context.MVMAgentDetails.FirstOrDefault(f => f.Date == agent.Date)?.Date;



                        // استرجاع السجل 
                        var existingAgentDetail = context.MVMAgentDetails.FirstOrDefault(ad => ad.ItemID == itemID && ad.Date == dt.Value && ad.Statut == statut1);

                        if (existingAgentDetail != null)
                        {
                            // تحديث فقط حقل ID_Attent_Liste
                            existingAgentDetail.ID_Attent_Liste = null;
                            context.SubmitChanges(); 
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