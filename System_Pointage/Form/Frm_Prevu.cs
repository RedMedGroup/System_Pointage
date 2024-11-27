using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
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
using static System_Pointage.Classe.Models;

namespace System_Pointage.Form
{
    public partial class Frm_Prevu : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.AgentStatus> activeAgentsList;
        public BindingList<Models.AgentStatus> ActiveAgentsList
        {
            get { return activeAgentsList; }
            set { activeAgentsList = value; }
        }
        Master.MVMType Type;
        private BindingList<Models.AgentStatus> transferredAgentsList;       

        public Frm_Prevu(Master.MVMType _Type)
        {
            InitializeComponent();
            Type = _Type;
            SetFormType();
        }
 
        private void Frm_Prevu_Load(object sender, EventArgs e)
        {
            activeAgentsList = new BindingList<Models.AgentStatus>();
            activeAgentsList = new BindingList<Models.AgentStatus>();
            gridControl1_Prvu.DataSource = activeAgentsList;

            dateEdit1.DateTime=DateTime.Now;
            dateEdit1.EditValueChanged += DateEdit1_EditValueChanged;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            transferredAgentsList = new BindingList<Models.AgentStatus>();
        }
        void SetFormType()
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    this.Text = "Présent";
                    btn_envoyer.Caption = "Envoyer sur liste rentrant";
                    this.Text = "Ajouter des Rentrants";
                    break;
                case Master.MVMType.A:
                    this.Text = "Ajouter des Absent";
                    break;
                case Master.MVMType.CR:
                    this.Text = "cr";
                    btn_envoyer.Caption = "Envoyer sur liste partant";
                    this.Text = "Ajouter des Partant";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void DateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            
        }
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
                            .Where(m => m.Statut == "CR") // التأكد من أن الحالة "P"
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
                      ma.agent.Name,
                      ma.m.Date,
                      ma.m.Statut,
                      ma.agent.Jour,
                      PosteName = poste.Name,
                      ma.agent.Matricule,
                      ma.agent.Affecter,
                      CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour.GetValueOrDefault()) ,
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
                                Name = x.Name,
                                Date = x.Date,
                                Statut = x.Statut,
                                Jour = x.Jour ?? 0,
                                Poste = x.PosteName,
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Matricule = x.Matricule,
                                Affecter = x.Affecter,
                            }).ToList()
                        );

                        var mergedList = new BindingList<Models.AgentStatus>(transferredAgentsList.Concat(activeAgentsList).ToList());
                        gridControl1_Prvu.DataSource = mergedList;
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
                            .Where(m => m.Statut == "P") // التأكد من أن الحالة "P"
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
                      ma.agent.Name,
                      ma.m.Date,
                      ma.m.Statut,
                      ma.agent.Jour,
                      PosteName = poste.Name,
                      ma.agent.Matricule,
                      ma.agent.Affecter,
                      CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour.GetValueOrDefault()) ,// حساب التاريخ
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
                                Name = x.Name,
                                Date = x.Date,
                                Statut = x.Statut,
                                Jour = x.Jour ?? 0,
                                Poste = x.PosteName,
                                CalculatedDate = x.CalculatedDate,
                                DaysCount = x.DaysCount,
                                Matricule = x.Matricule,
                                Affecter = x.Affecter,

                            }).ToList()
                        );
                        var mergedList = new BindingList<Models.AgentStatus>(transferredAgentsList.Concat(activeAgentsList).ToList());
                        gridControl1_Prvu.DataSource = mergedList;
                        //gridControl1_Prvu.DataSource = transferredAgentsList;
                        GridName();
                    }
                  
                    break;
                default:
                    break;
                   
            }
        }
        public void GridName()
        {
            gridView1.GroupPanelText = " ";
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Matricule"].VisibleIndex =1;
            gridView1.Columns["Name"].VisibleIndex = 2;
            gridView1.Columns["Poste"].VisibleIndex = 3;
            gridView1.Columns["Affecter"].VisibleIndex =4;
            gridView1.Columns["Date"].VisibleIndex =5;
            gridView1.Columns["Jour"].VisibleIndex = 6;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 7;
            gridView1.Columns["DaysCount"].VisibleIndex =8;
            gridView1.Columns["Statut"].VisibleIndex =9;
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
        private void btn_prevu_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue is DateTime selectedDate)
            {
                LoadEligibleEmployees(selectedDate);
            }

        }
        private void btn_envoyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedAgents = gridView1.GetSelectedRows()
    .Select(rowHandle => gridView1.GetRow(rowHandle) as Models.AgentStatus)
    .Where(agent => agent != null)
    .ToList();
            // تحويل List إلى BindingList
            var selectedAgentsBindingList = new BindingList<Models.AgentStatus>(selectedAgents);

            if (selectedAgentsBindingList.Count > 0)
            {
                // الحصول على الفورم الأصلية (Frm_MVM_Operation)
                var mainForm = this.Owner as Frm_MVM_Operation;
                if (mainForm != null)
                {
                    // التحقق من التكرارات
                    var duplicateAgents = selectedAgentsBindingList
                        .Where(agent => mainForm.IsAgentExists(agent))
                        .ToList();

                    if (duplicateAgents.Any())
                    {
                        // عرض رسالة تحذيرية
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
                        // استدعاء الطريقة لإضافة البيانات إلى Frm_MVM_Operation
                        mainForm.AddTransferredAgents(selectedAgentsBindingList);

                        // إغلاق الفورم الحالية
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Il n'y a aucune donnée à envoyer.", "avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

          
        }
        public void AddTransferredAgentsPR(BindingList<Models.AgentStatus> transferredAgents)
        {
            foreach (var agent in transferredAgents)
            {
                if (!IsAgentExists(agent))
                {
                    activeAgentsList.Add(agent);
                }
            }

            // تحديث البيانات في واجهة المستخدم
            gridControl1_Prvu.RefreshDataSource();
        }
        public bool IsAgentExists(Models.AgentStatus agent)
        {
            return activeAgentsList != null && activeAgentsList.Any(existingAgent =>
                existingAgent.Name == agent.Name &&
                existingAgent.Date.Date == agent.Date.Date &&
                existingAgent.Statut == agent.Statut);
        }
        private void btn_ovrirEn_Click(object sender, EventArgs e)
        {
            switch (Type)
            {

                case Master.MVMType.P:
                    Frm_Operation frmType1 = new Frm_Operation(Type,this);
                    frmType1.Owner = this;
                    frmType1.Show();
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
    }
}