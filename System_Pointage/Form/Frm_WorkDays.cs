using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
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
using System_Pointage.Classe;
using System_Pointage.report;

namespace System_Pointage.Form
{
    public partial class Frm_WorkDays : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.AgentStatus> activeAgentsList;

        public Frm_WorkDays()
        {
            InitializeComponent();
            
        }
        private void Frm_WorkDays_Load(object sender, EventArgs e)
        {
            RefrecheData();
        }
        int selectedID;
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            var filteredAgents = agentDataService
         .GetAgentStatuses(userAccessPosteID, Master.MVMType.CR, isAdmin,selectedID, true)
         .Where(agent => agent.DaysCount > agent.Jour)
         .ToList();

           
            activeAgentsList = new BindingList<Models.AgentStatus>(filteredAgents);

            gridControl1.DataSource = activeAgentsList;

            GridName();
        }
        public void GridName()
        {
            gridView1.GroupPanelText = " ";
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
            gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
            gridView1.Columns["Date"].Caption = "Date de rentrée";
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["Poste"].VisibleIndex = 2;
            gridView1.Columns["Affecter"].VisibleIndex = 3;
            gridView1.Columns["Date"].VisibleIndex = 4;
            gridView1.Columns["Jour"].VisibleIndex = 5;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 6;
            gridView1.Columns["DaysCount"].VisibleIndex = 7;
            gridView1.Columns["Difference"].VisibleIndex = 8;
            gridView1.Columns["Statut"].VisibleIndex = 9;
            gridView1.Columns["screenPosteD"].Visible = false;
        }
        private void GenerateReport()
        {
            if (activeAgentsList == null || !activeAgentsList.Any())
            {
                MessageBox.Show("Il n'y a aucune donnée pour générer le rapport.");
                return;
            }

            rpt_WorkDay report = new rpt_WorkDay();

            report.DataSource = activeAgentsList;
            report.DataMember = "";

            var nameField = new XRLabel { Text = "Name" };
            var matriculeField = new XRLabel { Text = "Matricule" };
            var posteField = new XRLabel { Text = "Poste" };
            var daysCountField = new XRLabel { Text = "DaysCount" };
            var calculatedDateField = new XRLabel { Text = "CalculatedDate" };

            // ajouter les champs dans rapport
            report.Bands.Add(new DetailBand());
            report.Bands[0].Controls.AddRange(new XRControl[]
            {
        nameField,
        matriculeField,
        posteField,
        daysCountField,
        calculatedDateField
            });

      
        }
        private void ShowWorkDayReport()
        {
            var report = new rpt_WorkDay();
            var filteredData = activeAgentsList
       .Where(agent => agent.DaysCount > agent.Jour)
       .ToList();

            report.LoadData(filteredData); 

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }

        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowWorkDayReport();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int searchValue = Convert.ToInt32(spinEdit1.Value);

            // تصفية القائمة بناءً على الرقم المدخل
            var filteredList = activeAgentsList
                .Where(agent => agent.Difference == searchValue)
                .ToList();

            // التحقق إذا كانت النتائج فارغة
            if (filteredList.Count == 0)
            {
                MessageBox.Show("Aucun résultat .");
            }

            // تحديث مصدر البيانات لـ GridControl
            gridControl1.DataSource = new BindingList<Models.AgentStatus>(filteredList);
        }

        private void btn_liste_Click(object sender, EventArgs e)
        {
            RefrecheData();
        }
    }
}