using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " ";
            #endregion
            gridView1.OptionsBehavior.Editable = false;
            RefrecheData();
        }
        int selectedID;
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن أو مانجر
            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager;

            // إذا لم يكن أدمن أو مانجر، استخدم userAccessPosteID
            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            var filteredAgents = agentDataService
         .GetAgentStatuses(userAccessPosteID, Master.MVMType.CR, isAdminOrManager,selectedID, true, "Frm_WorkDays")
         .Where(agent => agent.DaysCount > agent.Jour)
         .ToList();

           
            activeAgentsList = new BindingList<Models.AgentStatus>(filteredAgents);

            gridControl1.DataSource = activeAgentsList;

            GridName();
        }
        public void GridName()
        {
            gridView1.GroupPanelText = " ";
            gridView1.Columns["Name"].Caption = "Nom";
            gridView1.Columns["FirstName"].Caption = "Prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
            gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
            gridView1.Columns["Date"].Caption = "Date de rentrée";
            gridView1.Columns["Difference"].Caption = "écart";
            gridView1.Columns["Matricule"].VisibleIndex = 0; 
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["FirstName"].VisibleIndex = 2;
            gridView1.Columns["Poste"].VisibleIndex = 3;
            gridView1.Columns["Affecter"].VisibleIndex = 4;
            gridView1.Columns["Date"].VisibleIndex = 5;
            gridView1.Columns["Jour"].VisibleIndex = 6;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 7;
            gridView1.Columns["DaysCount"].VisibleIndex = 8;
            gridView1.Columns["Difference"].VisibleIndex = 9;
            gridView1.Columns["Statut"].VisibleIndex = 10;
            gridView1.Columns["screenPosteD"].Visible = false;
            gridView1.Columns["Statutmvm"].Visible = false;
            gridView1.Columns["Jour"].Visible = false;

        }

        private void ShowWorkDayReport()
        {
            // تحميل التقرير المعدل (إذا وجد)
            rpt_WorkDay report = LoadModifiedReport();

            // إذا لم يتم العثور على التقرير المعدل، استخدم التقرير الافتراضي
            if (report == null)
            {
                report = new rpt_WorkDay();
            }

            // تصفية البيانات
            var filteredData = activeAgentsList
                .Where(agent => agent.DaysCount > agent.Jour)
                .ToList();

            // استخدام LoadData لربط البيانات بالتقرير
            report.LoadData(filteredData);

            // عرض التقرير
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreviewDialog();
        }
        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowWorkDayReport();
         
        }

        private rpt_WorkDay LoadModifiedReport()
        {
            using (var context = new DAL.DataClasses1DataContext()) // استبدل بسياق قاعدة البيانات الخاص بك
            {
                // استرجاع أحدث تقرير محفوظ
                var reportEntity = context.Reports
                    .OrderByDescending(r => r.ModifiedDate)
                    .FirstOrDefault(r => r.ReportName == "rpt_WorkDay");

                if (reportEntity != null && reportEntity.ReportData != null)
                {
                    // تحويل byte[] إلى XtraReport
                    using (MemoryStream ms = new MemoryStream(reportEntity.ReportData.ToArray())) // استخدام ToArray()
                    {
                        rpt_WorkDay report = new rpt_WorkDay();
                        report.LoadLayoutFromXml(ms); // تحميل التقرير من MemoryStream
                        return report;
                    }
                }
                else
                {
                    MessageBox.Show("Aucun rapport modifié trouvé dans la base de données, le rapport par défaut sera utilisé.");
                    return null; // إرجاع null إذا لم يتم العثور على التقرير المعدل
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            int searchValue = Convert.ToInt32(spinEdit1.Value);

            // تصفية القائمة بناءً على الرقم المدخل
            var filteredList = activeAgentsList
                .Where(agent => agent.Difference <= searchValue)
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