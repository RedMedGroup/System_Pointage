using DevExpress.ReportServer.ServiceModel.DataContracts;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraSplashScreen;
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
using System_Pointage.DAL;
using System_Pointage.report;

namespace System_Pointage.Form
{
    public partial class Frm_Pointage : DevExpress.XtraEditors.XtraForm
    {
        List<DAL.Fiche_Agent> FicheAgentList;
        List<DAL.Fiche_Poste> FichePOSTE;
        List<DAL.MVMAgentDetail> listOfDays;
        public Frm_Pointage()
        {
            InitializeComponent();
        }

        private void Frm_Pointage_Load(object sender, EventArgs e)
        {
            btn_print.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            DateTime today = DateTime.Today;
            dateEdit1.DateTime = new DateTime(today.Year, today.Month, 1);
            dateEdit2.DateTime = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            gridView1.OptionsView.AllowCellMerge = true;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
            gridView1.CellMerge += GridView1_CellMerge;
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
        }

        private void GridView1_CellMerge(object sender, CellMergeEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "POSTE" || e.Column.FieldName == "EFFC/CRT")
            {
                string value1 = view.GetRowCellDisplayText(e.RowHandle1, e.Column);
                string value2 = view.GetRowCellDisplayText(e.RowHandle2, e.Column);

                if (e.Column.FieldName == "POSTE")
                {
                    if (value1 == value2 || string.IsNullOrEmpty(value2))
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
                else if (e.Column.FieldName == "EFFC/CRT")
                {
                    if (value1 == value2)
                    {
                        e.Merge = true;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }

                e.Handled = true; 
            }
            else
            {
                e.Merge = false;
                e.Handled = true; 
            }
 
        }

        //private void btn_recharch_Click(object sender, EventArgs e)
        //{
        //    LoadData();
        //    gridControl1.DataSource = CreateDataTable();

        //    gridView1.PopulateColumns();
        //    gridView1.BestFitColumns();
        //    report.rpt_WorkDay repo = new rpt_WorkDay();
        //    repo.ShowDesigner();
        //}
       
        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "POSTE")
            {
                if (string.IsNullOrEmpty(e.CellValue as string))
                {
                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }

        }

        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "POSTE")
            {
                string cellValue = e.CellValue?.ToString();
                if (cellValue != "")
                    e.Appearance.BackColor = Color.Yellow;
            }          
            //if (e.Column.FieldName == "EFFC/CRT") 
            //{
            //    string cellValue = e.CellValue?.ToString();
            //    if (cellValue != "")
            //        e.Appearance.BackColor = ColorTranslator.FromHtml("#FFECB3");
            //}
            if (/*e.Column.FieldName == "Nom"||*/ e.Column.FieldName == "POSTE")
            {
                string cellValue = e.CellValue?.ToString();
                if (cellValue == "S/TOTAL" || cellValue == "Ecart")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#00897B");

                }
            }
            if (e.Column.FieldName != "POSTE" && e.Column.FieldName != "Nom")
            {
                string cellValue = e.CellValue?.ToString();

                if (cellValue == "P")
                {
                    e.Appearance.BackColor = Color.LightGreen;
                }
                else if (cellValue == "A")
                {
                    e.Appearance.BackColor = Color.LightCoral;
                }
                else if (cellValue == "CR")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFA726");
                }
                else if (cellValue == "CE")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFE082");
                }
                else if (cellValue == "M")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#F48FB1");
                }
                else if (cellValue == "AA")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FF9E80");
                }
                else
                {
                    int cellValue1;
                    if (int.TryParse(cellValue, out cellValue1))
                    {
                        if (cellValue1 > 0)
                        {
                            e.Appearance.BackColor = ColorTranslator.FromHtml("#4DB6AC");
                        }
                    }
                }
            }
        }     
        private DataTable CreateDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("POSTE", typeof(string));
            table.Columns.Add("EFECTIF/CONTRAT", typeof(int));
            table.Columns.Add("Name", typeof(string));

            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;

            if (startDate > endDate)
            {
                MessageBox.Show("La date de début doit être antérieure ou égale à la date de fin.");
                return table;
            }

            TimeSpan dateRange = endDate - startDate;
            int totalDays = dateRange.Days + 1;

            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                table.Columns.Add($"{currentDay.Day}", typeof(string));
            }

            table.Columns.Add("Total", typeof(int));

            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            var context = new DAL.DataClasses1DataContext();
            var groups = FicheAgentList
    .Where(x => x.Statut == true) 
    .GroupBy(agent => agent.ID_Post)
    .Where(g => isAdmin || g.Any(agent => agent.ScreenPosteD == userAccessPosteID))
    .Select(g => new
    {
        Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
        RequiredQuantity =
             context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
        Agents = g.ToList()
    });         
            foreach (var group in groups)
            {
                DataRow specializationRow = table.NewRow();
                specializationRow["POSTE"] = group.Specialization;
                specializationRow["EFECTIF/CONTRAT"] = group.RequiredQuantity;
                table.Rows.Add(specializationRow);

                int[] presentCountPerDay = new int[totalDays];
                int[] absentCountPerDay = new int[totalDays];
                int totalPresent = 0;
                int totalAbsent = 0;

                foreach (var agent in group.Agents)
                {
                    DataRow row = table.NewRow();
                    row["Name"] = agent.Name;

                    var agentMovements = context.MVMAgentDetails
                        .Where(m => m.ItemID == agent.ID)
                        .OrderBy(m => m.Date)
                        .ToList();

                    string currentStatus = "-"; 
                    DateTime? lastMovementDate = null;

                    foreach (var movement in agentMovements)
                    {
                        if (movement.Date <= startDate)
                        {
                            currentStatus = movement.Statut;
                            lastMovementDate = movement.Date;
                        }
                    }

                    for (int i = 0; i < totalDays; i++)
                    {
                        DateTime currentDay = startDate.AddDays(i);

                        if (currentDay > DateTime.Now)
                        {
                            
                            break;
                        }

                        var movement = agentMovements.FirstOrDefault(m => m.Date.Date == currentDay.Date);

                        if (movement != null)
                        {
                            // إذا كانت هناك حركة في اليوم الحالي
                            currentStatus = movement.Statut;
                            lastMovementDate = movement.Date;
                        }

                        // استمرار الحالة الأخيرة إذا لم تكن هناك حركة جديدة
                        row[$"{currentDay.Day}"] = currentStatus;

                        if (currentDay <= DateTime.Now)
                        {
                            if (currentStatus == "P")
                            {
                                presentCountPerDay[i]++;
                                totalPresent++;
                            }
                            else if (currentStatus == "A")
                            {
                                absentCountPerDay[i]++;
                                totalAbsent++;
                            }
                        }
                    }

                    table.Rows.Add(row);
                }

                DataRow presentCountRow = table.NewRow();
                presentCountRow["Name"] = "P/Total";
                for (int i = 0; i < totalDays; i++)
                {
                    presentCountRow[$"{startDate.AddDays(i).Day}"] = presentCountPerDay[i].ToString();
                }
                presentCountRow["Total"] = totalPresent;
                table.Rows.Add(presentCountRow);

                DataRow absentCountRow = table.NewRow();
                absentCountRow["Name"] = "A/Total";
                for (int i = 0; i < totalDays; i++)
                {
                    absentCountRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i].ToString();
                }
                absentCountRow["Total"] = totalAbsent;
                table.Rows.Add(absentCountRow);
            }

            return table;
        }// recharch
        private void LoadData()
        {
            using (var context = new DAL.DataClasses1DataContext())
            {
                listOfDays = context.MVMAgentDetails.ToList();
                FicheAgentList = context.Fiche_Agents

                    .ToList();

                FichePOSTE = context.Fiche_Postes.ToList();
            }
        }

        private void GenerateReport() // pénalité
        {
            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            int totalDays = (endDate - startDate).Days + 1;

            rpt_penalite report = new rpt_penalite();
            string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
            int annee = startDate.Year;
            report.lbl_mois.Text = $"Mois de {mois} {annee}";
            report.BindAttendanceDataToCells();
            report.DataSource = CreateAbsenceReport(totalDays, startDate, endDate);
            report.SumPinality = Sum;
            report.ShowRibbonPreview();
       
        }

        private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("TotalAbsences", typeof(int));
            table.Columns.Add("M_Penalite", typeof(float));
            table.Columns.Add("TotalPenalties", typeof(float));
            table.Columns.Add("Nombre du personnel absent", typeof(int));

            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            using (var context = new DAL.DataClasses1DataContext())
            {
                  var departments = context.Fiche_Postes.Select(post => new
                {
                    post.ID,
                    DepartmentName = post.Name,
                    M_Penalite = post.M_Penalite,
                    Nembre_Contra = post.Nembre_Contra,

                }).ToList();


                float totalSum = 0;
                foreach (var department in departments)
                {
                    var agentDetails = context.MVMAgentDetails
                        .Where(x => x.Date <= endDate) 
                        .Join(context.Fiche_Agents.Where(x => x.Statut == true),
                              agent => agent.ItemID,
                              worker => worker.ID,
                              (agent, worker) => new { agent, worker })
                        .Where(x => x.worker.ID_Post == department.ID && (isAdminOrManager || x.worker.ScreenPosteD == userAccessPosteID))
                        .OrderBy(x => x.agent.Date)
                        .ToList();

                    if (!agentDetails.Any()) continue;

                    var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();

                    var absentAgents = new HashSet<int>();

                    foreach (var record in agentDetails)
                    {
                        int agentId = record.worker.ID;

                        if (!agentPresencePeriods.ContainsKey(agentId))
                        {
                            agentPresencePeriods[agentId] = new List<(DateTime, DateTime)>();
                        }

                        if (record.agent.Statut == "P")
                        {
                            agentPresencePeriods[agentId].Add((record.agent.Date, endDate)); 
                        }
                        else if (record.agent.Statut == "CR" || record.agent.Statut == "A")
                        {
                            var lastPeriod = agentPresencePeriods[agentId].LastOrDefault();
                            if (lastPeriod != default)
                            {
                                agentPresencePeriods[agentId][agentPresencePeriods[agentId].Count - 1] =
                                    (lastPeriod.Start, record.agent.Date.AddDays(-1));
                            }

                            absentAgents.Add(agentId);
                        }
                    }
                    #region new 08/01
                    // تحقق من العدد الأقصى absent 
                    int maxAllowedAbsences =
                         (int)department.Nembre_Contra;
                       

                    //  الغائبين  العدد المسموح به بضبطه
                    int validAbsentCount = Math.Min(absentAgents.Count, maxAllowedAbsences);

                    absentAgents = absentAgents.Take(validAbsentCount).ToHashSet();
                    #endregion


                    int totalAbsences = 0;

                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        int dailyPresenceCount = 0;

                        foreach (var periods in agentPresencePeriods.Values)
                        {
                            if (periods.Any(p => date >= p.Start && date <= p.End))
                            {
                                dailyPresenceCount++;
                            }
                        }

                        int dailyAbsences = Math.Max(0, (int)(
                            department.Nembre_Contra - dailyPresenceCount));
                            
                       

                        totalAbsences += dailyAbsences;
                    }

                    if (totalAbsences > 0)
                    {
                        int distinctAbsentCount = absentAgents.Count;
                        distinctAbsentCount = Math.Min(distinctAbsentCount, totalAbsences);
                        float totalPenalties = (float)(totalAbsences * (
                             department.M_Penalite));
                         
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["TotalAbsences"] = totalAbsences;
                        row["M_Penalite"] =  department.M_Penalite ;
                        row["TotalPenalties"] = totalPenalties;
                        row["Nombre du personnel absent"] = distinctAbsentCount; 

                        totalSum += totalPenalties;
                        Sum = (decimal)totalSum;
                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }

  


        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {

            DateTime selectedDate = dateEdit1.DateTime;

            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            int remainingDays = daysInMonth - selectedDate.Day;

            dateEdit2.DateTime = selectedDate.AddDays(remainingDays);
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateEdit2.DateTime;

            dateEdit1.DateTime = new DateTime(selectedDate.Year, selectedDate.Month, 1);
        }

        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            PrintReport();
        }
        private void PrintReport()
        {

            report.rpt_pointage report = new report.rpt_pointage();

            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            report.AddDateColumnsToReport(report, startDate, endDate);
            DataTable table = CreateDataTable();
            report.DataSource = table;

            report.BindAttendanceDataToCells(table);

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowRibbonPreview();
        }
        private void PrintReport2()
        {
            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            LoadData(); 

            var reportLoader = new LoadModifiedReportClass(new DAL.DataClasses1DataContext(), this);

            rpt_pointage2 report = reportLoader.LoadModifiedReport<rpt_pointage2>("rpt_pointage2")
                                   ?? new rpt_pointage2();

            string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
            int annee = startDate.Year;
            report.lbl_mois.Text = $"Mois de {mois} {annee}";

            report.AddDateColumnsToReport(report, startDate, endDate);

            DataTable table = CreateDataTablePointge();
            report.DataSource = table;

            report.BindAttendanceDataToCells(table);

            ReportPrintTool printTool = new ReportPrintTool(report);
            //  printTool.ShowPreview();
            printTool.ShowRibbonPreview();
        }

        //private void PrintReport2()
        //{
        //    DateTime startDate = dateEdit1.DateTime;
        //    DateTime endDate = dateEdit2.DateTime;
        //    LoadData();

        //    // تحميل التقرير المعدل (إذا وجد)
        //    rpt_pointage2 report = LoadModifiedReport2();

        //    // إذا لم يتم العثور على التقرير المعدل، استخدم التقرير الافتراضي
        //    if (report == null)
        //    {
        //        report = new rpt_pointage2(); // استخدام التقرير الافتراضي
        //    }

        //    // تعيين النص الخاص بالشهر والسنة
        //    string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        //    int annee = startDate.Year;
        //    report.lbl_mois.Text = $"Mois de {mois} {annee}";

        //    // إضافة أعمدة التاريخ إلى التقرير
        //    report.AddDateColumnsToReport(report, startDate, endDate);

        //    // إنشاء وتعيين مصدر البيانات
        //    DataTable table = CreateDataTablePointge();
        //    report.DataSource = table;

        //    // ربط بيانات الحضور بالخلايا
        //    report.BindAttendanceDataToCells(table);

        //    // عرض التقرير
        //    ReportPrintTool printTool = new ReportPrintTool(report);
        //    printTool.ShowPreview();
        //    //DateTime startDate = dateEdit1.DateTime;
        //    //DateTime endDate = dateEdit2.DateTime;
        //    //LoadData();
        //    //report.rpt_pointage2 report = new report.rpt_pointage2();
        //    //string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        //    //int annee = startDate.Year;
        //    //report.lbl_mois.Text = $"Mois de {mois} {annee}";


        //    //report.AddDateColumnsToReport(report, startDate, endDate);
        //    //DataTable table = CreateDataTablePointge();
        //    //report.DataSource = table;

        //    //report.BindAttendanceDataToCells(table);

        //    //ReportPrintTool printTool = new ReportPrintTool(report);
        //    //printTool.ShowPreview();


        //}
        private void btn_print2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.ShowRibbonPrintPreview();
        }
        #region date of report
        public DateTime StartDate { get; set; }
        private void SetStartDate()
        {
            StartDate = dateEdit1.DateTime;
        }
        #endregion
        private DataTable CreateDailyReport(DateTime reportDate)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("RequiredEmployees", typeof(int));
            table.Columns.Add("WorkerName", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("PresentCount", typeof(int));
            table.Columns.Add("cell_Ecart", typeof(int));

            using (var context = new DAL.DataClasses1DataContext())
            {
                bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

                int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

                var departments = context.Fiche_Postes
    .Select(post => new
    {
        ID_Post = post.ID,
        DepartmentName = post.Name,
        Department=post.Departement,
        RequiredEmployees =  post.Nembre_Contra     
    })
    .ToList();

            
                var workers = context.Fiche_Agents .Where( agent => (isAdminOrManager || agent.ScreenPosteD == userAccessPosteID) &&agent.Statut==true)
                    .Select(agent => new
                    {
                        WorkerName = agent.Name,
                        Firstname=agent.FirstName,
                        DepartmentID = agent.ID_Post,
                        LastStatus = context.MVMAgentDetails
                            .Where(detail =>
                                detail.ItemID == agent.ID &&
                                detail.Date <= reportDate)
                            .OrderByDescending(detail => detail.Date)
                            .Select(detail => detail.Statut)
                            .FirstOrDefault(), 
                        NextAbsenceDate = context.MVMAgentDetails
                            .Where(detail =>
                                detail.ItemID == agent.ID &&
                                detail.Date > reportDate && 
                                detail.Statut == "CR")
                            .OrderBy(detail => detail.Date)
                            .Select(detail => (DateTime?)detail.Date) 
                            .FirstOrDefault() 
                    })
                    .Where(worker => worker.LastStatus != null && worker.LastStatus != "CR") 
                    .ToList();

                foreach (var department in departments)
                {

                    var departmentWorkers = workers.Where(w => w.DepartmentID == department.ID_Post).ToList();

                    int presentCount = departmentWorkers.Count(w => w.LastStatus == "P" && (w.NextAbsenceDate == null || w.NextAbsenceDate > reportDate));

                    int cellEcart = (int)(presentCount - department.RequiredEmployees);

                    foreach (var worker in departmentWorkers)
                    {
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["RequiredEmployees"] = department.RequiredEmployees;
                        row["WorkerName"] = worker.WorkerName;
                        row["FirstName"] = worker.Firstname;

                        string status = worker.LastStatus ?? "A"; 
                        if (worker.LastStatus == "P" && (worker.NextAbsenceDate == null || worker.NextAbsenceDate > reportDate))
                        {
                            status = "P"; 
                        }
                        else if (worker.LastStatus == "P" && worker.NextAbsenceDate <= reportDate)
                        {
                            status = "A"; 
                        }

                        row["Status"] = status;
                        row["PresentCount"] = presentCount;
                        row["cell_Ecart"] = cellEcart;

                        table.Rows.Add(row);
                    }

                    if (!departmentWorkers.Any())
                    {
                        bool isDepartmentVisible = isAdminOrManager || department.Department == userAccessPosteID;

                   

                        if (isDepartmentVisible) // القسم مرئيًا بناءً على الصلاحيات
                        {
                            DataRow row = table.NewRow();
                            row["Department"] = department.DepartmentName;
                            row["RequiredEmployees"] = department.RequiredEmployees;
                            row["WorkerName"] = DBNull.Value;
                            row["FirstName"] = DBNull.Value;
                            row["Status"] = "No Workers";
                            row["PresentCount"] = 0; 
                            row["cell_Ecart"] = -department.RequiredEmployees;

                            table.Rows.Add(row);
                        }
                    }

                }
            }

            return table;
        }
        #region etat journaly
        private void GenerateDailyReport()
        {
            SetStartDate();
            DateTime reportDate = dateEdit1.DateTime;

            using (var context = new DAL.DataClasses1DataContext())
            {
                var reportLoader = new LoadModifiedReportClass(context, this);

                rpt_DailyReport report = reportLoader.LoadModifiedReport<rpt_DailyReport>("rpt_DailyReport");

                if (report == null)
                {
                    report = new rpt_DailyReport(this);
                }

                report.DataSource = CreateDailyReport(reportDate);

                report.ShowPreview();
            }
        }

        //private void GenerateDailyReport()
        //{
        //    SetStartDate();
        //    DateTime reportDate = dateEdit1.DateTime;

        //    rpt_DailyReport report = LoadModifiedReport();

        //    if (report == null)
        //    {
        //        report = new rpt_DailyReport(this); // إنشاء التقرير الافتراضي
        //    }

        //    report.DataSource = CreateDailyReport(reportDate);
        //    report.ShowPreview();
        //    //SetStartDate();
        //    //DateTime reportDate = dateEdit1.DateTime;
        //    //rpt_DailyReport report = new rpt_DailyReport(this);
        //    //report.DataSource = CreateDailyReport(reportDate);
        //    //report.ShowPreview();
        //}
        //private rpt_DailyReport LoadModifiedReport()
        //{
        //    using (var context = new DAL.DataClasses1DataContext()) // استبدل بسياق قاعدة البيانات الخاص بك
        //    {
        //        // استرجاع أحدث تقرير محفوظ
        //        var reportEntity = context.Reports
        //            .OrderByDescending(r => r.ModifiedDate)
        //            .FirstOrDefault(r => r.ReportName == "rpt_DailyReport");

        //        if (reportEntity != null && reportEntity.ReportData != null)
        //        {
        //            // تحويل byte[] إلى XtraReport
        //            using (MemoryStream ms = new MemoryStream(reportEntity.ReportData.ToArray())) // استخدام ToArray()
        //            {
        //                rpt_DailyReport report = new rpt_DailyReport();
        //                report.LoadLayoutFromXml(ms); // تحميل التقرير من MemoryStream
        //                report.Initialize(this);
        //                return report;
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Aucun rapport modifié trouvé dans la base de données, le rapport par défaut sera utilisé."); return null;
        //        }
        //    }
        //}
        private void btn_reportAparJour_Click(object sender, EventArgs e)
        {
            GenerateDailyReport();
        }
        #endregion
        private void btn_workdays_Click(object sender, EventArgs e)
        {
            Frm_WorkDays frm = new Frm_WorkDays();
            frm.ShowDialog();
        }
        private void btn_mvm_Click(object sender, EventArgs e)
        {
            Frm_Heir frm = new Frm_Heir();
            frm.ShowDialog();
        }
        #region ETAT MENSEALLE

        private async Task<DataTable> CreateDataTableMensuel()
        {
            return await Task.Run(() =>
            {
                DataTable table = new DataTable();
            table.Columns.Add("POSTE", typeof(string));
            table.Columns.Add("EFECTIF/CONTRAT", typeof(int));

            DateTime startDate = dateEdit1.DateTime.Date;
            DateTime endDate = dateEdit2.DateTime.Date;

            if (startDate > endDate)
            {
                MessageBox.Show("La date de début doit être antérieure ou égale à la date de fin.");
                return table;
            }

            int totalDays = (endDate - startDate).Days + 1;

            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                table.Columns.Add($"{currentDay.Day}", typeof(int));
            }

            table.Columns.Add("Total Absences", typeof(int));
            table.Columns.Add("M_Penalite", typeof(float));
            table.Columns.Add("Total Penalty", typeof(float));

            var context = new DAL.DataClasses1DataContext();

                bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

                int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

                var groups = FicheAgentList
    .Where(x => x.Statut == true  && (isAdminOrManager || x.ScreenPosteD == userAccessPosteID))
    .GroupBy(agent => agent.ID_Post)
    .Select(g =>
    {
        var fichePost = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key);

        return new
        {
            Specialization = fichePost?.Name,
            RequiredQuantity =  fichePost?.Nembre_Contra ?? 0,              
            Penalty = 
                fichePost?.M_Penalite ?? 0,              
            Agents = g.ToList()
        };
    });

            //var groups = FicheAgentList.Where(x => x.Statut == true && x.Affecter == cmb_base.Text && (isAdmin || x.ScreenPosteD == userAccessPosteID))
            //    .GroupBy(agent => agent.ID_Post)
            //    .Select(g => new
            //    {
            //        Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
            //        RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
            //        Penalty = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.M_Penalite ?? 0,
            //        Agents = g.ToList()
            //    });

            foreach (var group in groups)
            {
                DataRow specializationRow = table.NewRow();
                specializationRow["POSTE"] = group.Specialization;
                specializationRow["EFECTIF/CONTRAT"] = group.RequiredQuantity;

                int[] absentCountPerDay = new int[totalDays];
                int totalAbsent = 0;

                for (int i = 0; i < totalDays; i++)
                {
                    DateTime currentDay = startDate.AddDays(i);

                    if (currentDay > DateTime.Now)
                    {
                        break; 
                    }
                    int dailyPresentCount = 0;

                    foreach (var agent in group.Agents)
                    {
                        var lastMovement = context.MVMAgentDetails
                            .Where(m => m.ItemID == agent.ID && m.Date.Date <= currentDay.Date)
                            .OrderByDescending(m => m.Date)
                            .FirstOrDefault();

                        if (lastMovement != null)
                        {
                            if (lastMovement.Statut == "P")
                            {
                                dailyPresentCount++;
                            }
                            // إذا كانت آخر حركة "A" أو "CR"، فلا نعتبره حاضرًا
                           
                        }
                    }
                  

                    int dailyAbsentAgentsManual = group.Agents.Count(agent =>
                    {
                        var movements = context.MVMAgentDetails
                            .Where(m => m.ItemID == agent.ID && m.Date.Date <= currentDay.Date)
                            .OrderBy(m => m.Date)
                            .ToList();

                        return movements.Any(m => m.Statut == "A") && !movements.Any(m => m.Statut == "P");
                    });

                    int totalDailyAbsences = Math.Max(0, (int)group.RequiredQuantity - dailyPresentCount) + dailyAbsentAgentsManual;

                    absentCountPerDay[i] = totalDailyAbsences;
                    totalAbsent += totalDailyAbsences;
                }

                for (int i = 0; i < totalDays; i++)
                {
                    specializationRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i];
                }

                specializationRow["Total Absences"] = totalAbsent;
                specializationRow["M_Penalite"] = group.Penalty;
                specializationRow["Total Penalty"] = totalAbsent * group.Penalty;
                table.Rows.Add(specializationRow);
            }

            // Add total row
            DataRow totalRow = table.NewRow();
            totalRow["POSTE"] = "Total";

            int[] dailyTotals = new int[totalDays];
            int totalAbsencesSum = 0;
            float totalPenaltySum = 0;

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < totalDays; i++)
                {
                    dailyTotals[i] += Convert.ToInt32(row[$"{startDate.AddDays(i).Day}"]);
                }
                totalAbsencesSum += Convert.ToInt32(row["Total Absences"]);
                totalPenaltySum += Convert.ToSingle(row["Total Penalty"]);
            }

            for (int i = 0; i < totalDays; i++)
            {
                totalRow[$"{startDate.AddDays(i).Day}"] = dailyTotals[i];
            }

            totalRow["Total Absences"] = totalAbsencesSum;
            totalRow["Total Penalty"] = totalPenaltySum;
            table.Rows.Add(totalRow);

            return table;
            });
        }


        #endregion
   
        private async void btn_mensuel_Click(object sender, EventArgs e)
        {
            //LoadData();
            //gridControl1.DataSource = CreateDataTableMensuel();

            //gridView1.PopulateColumns();
            //gridView1.BestFitColumns();

            //btn_mensuel.Enabled = false;
            SplashScreenManager.ShowDefaultWaitForm("Chargement en cours", "Veuillez patienter...");

            try
            {
                await Task.Run(() => LoadData());
                var dataTable = await CreateDataTableMensuel();
                gridControl1.DataSource = dataTable;
                gridView1.PopulateColumns();
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}");
            }
            finally
            {
                SplashScreenManager.CloseDefaultWaitForm();
                btn_mensuel.Enabled = true;
            }


        }

        private void btn_print_mensuel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Master.User.UserType == (byte)Master.UserType.Guest)
            {
                XtraMessageBox.Show("Vous n'avez pas l'autorisation d'accéder à ce rapport.",
                     "Autorisations insuffisantes",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }
            LoadData();
            report.rpt_pointage_mensuel report = new report.rpt_pointage_mensuel();
           
            PrintReportMensuel(report);
        }
     

        private async void btn_print_Click(object sender, EventArgs e)
        {
            report.rpt_pointage_mensuel report = new report.rpt_pointage_mensuel();
            await PrintReportMensuel(report);
        }
        private async Task PrintReportMensuel(report.rpt_pointage_mensuel report)
        {
            SplashScreenManager.ShowDefaultWaitForm("Chargement en cours", "Veuillez patienter...");

            try
            {
                DateTime startDate = dateEdit1.DateTime;
                DateTime endDate = dateEdit2.DateTime;

                string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
                int annee = startDate.Year;
                report.lbl_mois.Text = $"Mois de {mois} {annee}";

                report.AddDateColumnsToReport(report, startDate, endDate);
                DataTable table = await CreateDataTableMensuel();
                report.DataSource = table;

                report.BindAttendanceDataToCells(table);

                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseDefaultWaitForm();
            }
        }
        #region
        //private async Task PrintReportMensuel(report.rpt_pointage_mensuel report)
        //{

        //   // report.rpt_pointage_mensuel report = new report.rpt_pointage_mensuel();

        //    DateTime startDate = dateEdit1.DateTime;
        //    DateTime endDate = dateEdit2.DateTime;

        //    // تعيين اسم الشهر والسنة
        //    string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        //    int annee = startDate.Year;
        //    report.lbl_mois.Text = $"Mois de {mois} {annee}";

        //    report.AddDateColumnsToReport(report, startDate, endDate);
        //    DataTable table = await CreateDataTableMensuel();
        //    report.DataSource = table;

        //    report.BindAttendanceDataToCells(table);

        //    ReportPrintTool printTool = new ReportPrintTool(report);
        //    printTool.ShowPreview();
        //}
        #endregion
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            LoadData();
            gridControl1.DataSource = CreateDataTablePointge();

            gridView1.PopulateColumns();
            gridView1.BestFitColumns();
        }
        private DataTable CreateDataTablePointge()
        {
            DataTable table = new DataTable();
            table.Columns.Add("POSTE", typeof(string));
            table.Columns.Add("Nom", typeof(string));
            table.Columns.Add("Prénom", typeof(string));
            table.Columns.Add("EFFC/CRT", typeof(int));

            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;

            if (startDate > endDate)
            {
                MessageBox.Show("La date de début doit être antérieure ou égale à la date de fin.");
                return table;
            }

            TimeSpan dateRange = endDate - startDate;
            int totalDays = dateRange.Days + 1;

            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                table.Columns.Add($"{currentDay.Day}", typeof(string));
            }

            table.Columns.Add("S/TOTAL", typeof(int));

           
            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            var context = new DAL.DataClasses1DataContext();
            //var groups = FicheAgentList
            //    .Where(x => x.Statut == true ) 
            //    .GroupBy(agent => agent.ID_Post)
            //    .Where(g => isAdminOrManager || g.Any(agent => agent.ScreenPosteD == userAccessPosteID))
            //    .Select(g => new
            //    {
            //        Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
            //        RequiredQuantity =  context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
                       
            //        Agents = g.ToList()
            //    });

            var groups = FicheAgentList
    .Where(x => x.Statut == true)
    .GroupBy(agent => agent.ID_Post)
    .Where(g => isAdminOrManager || g.Any(agent => agent.ScreenPosteD == userAccessPosteID))
    .Select(g => new
    {
        PostID = g.Key,
        Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
        RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
        Agents = g.ToList()
    })
    .OrderBy(g => g.PostID) // ترتيب المجموعات حسب ID_Post
    .ToList();



            foreach (var group in groups)
            {
                int[] presentCountPerDay = new int[totalDays];
                int totalPresent = 0;

                foreach (var agent in group.Agents)
                {
                    DataRow row = table.NewRow();
                    row["POSTE"] = group.Specialization; 
                    row["Nom"] = agent.Name;
                    row["Prénom"] = agent.FirstName; 
                        row["EFFC/CRT"] = group.RequiredQuantity;
                    var agentMovements = context.MVMAgentDetails
                        .Where(m => m.ItemID == agent.ID)
                        .OrderBy(m => m.Date)
                        .ToList();

                    string currentStatus = "-"; 
                    DateTime? lastMovementDate = null;

                    foreach (var movement in agentMovements)
                    {
                        if (movement.Date <= startDate)
                        {
                            currentStatus = movement.Statut;
                            lastMovementDate = movement.Date;
                        }
                    }

                    int individualPresentCount = 0;

                    for (int i = 0; i < totalDays; i++)
                    {
                        DateTime currentDay = startDate.AddDays(i);

                        if (currentDay > DateTime.Now)
                        {
                            break;
                        }

                        var movement = agentMovements.FirstOrDefault(m => m.Date.Date == currentDay.Date);

                        if (movement != null)
                        {
                            currentStatus = movement.Statut;
                            lastMovementDate = movement.Date;
                        }

                        row[$"{currentDay.Day}"] = currentStatus;

                        if (currentDay <= DateTime.Now && currentStatus == "P")
                        {
                            presentCountPerDay[i]++;
                            totalPresent++;
                            individualPresentCount++; 
                        }
                    }

                    row["S/TOTAL"] = individualPresentCount;
                    table.Rows.Add(row);
                }



                DataRow presentCountRow = table.NewRow();
                presentCountRow["POSTE"] = group.Specialization;
                //presentCountRow["Nom"] = "S/TOTAL";
                //presentCountRow["Prénom"] = "S/TOTAL";
                presentCountRow["POSTE"] = "S/TOTAL";
                for (int i = 0; i < totalDays; i++)
                {
                    presentCountRow[$"{startDate.AddDays(i).Day}"] = presentCountPerDay[i].ToString();
                }
                presentCountRow["S/TOTAL"] = totalPresent;
                table.Rows.Add(presentCountRow);

                DataRow ecartRow = table.NewRow();
                ecartRow["POSTE"] = group.Specialization;
                //ecartRow["Nom"] = "Ecart";
                //ecartRow["Prénom"] = "Ecart";
                ecartRow["POSTE"] = "Ecart";
                int totalEcart = 0;
                for (int i = 0; i < totalDays; i++)
                {
                    int ecartValue = (int)group.RequiredQuantity - presentCountPerDay[i];
                    ecartRow[$"{startDate.AddDays(i).Day}"] = ecartValue < 0 ? 0 : ecartValue;
                //    totalEcart += ecartValue;
                    totalEcart += (ecartValue < 0 ? 0 : ecartValue); // اجمع القيم المعدلة (السلبية = 0)
                }
                ecartRow["S/TOTAL"] = Math.Max(totalEcart, 0);
                table.Rows.Add(ecartRow);

            }
         
            return table;
        }
  
        private void btn_print_personell_monsuelle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            PrintReport2();
         
        }
        //private void GenerateReport2() // pénalité
        //{
        //    DateTime startDate = dateEdit1.DateTime;
        //    DateTime endDate = dateEdit2.DateTime;
        //  //  string reportTitle = cmb_base.Text == "TFT" ? "TFT" : "TFY";
        //    int totalDays = (endDate - startDate).Days + 1;
        //    rpt_penalite2 report = new rpt_penalite2();
        //   // report.ReportTitle = reportTitle; // تعيين قيمة reportTitle
            
        //    string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        //    int annee = startDate.Year;
        //    report.lbl_mois.Text = $"Mois de {mois} {annee}";
        //    report.DataSource = CreateAbsenceReport2(totalDays, startDate, endDate); // تحديث هنا لاستدعاء الدالة الجديدة
        //    report.SumPinality = Sum;
        //    report.BindAttendanceDataToCells();
        //    report.ShowPreviewDialog(); 
        //}
        decimal Sum;
        //private DataTable CreateAbsenceReport2(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    table.Columns.Add("Department", typeof(string));
        //    table.Columns.Add("Nembre_Contra", typeof(int)); 
        //    table.Columns.Add("CalculatedField3", typeof(int)); // الحقل 3: Nembre_Contra أو Nembre_Contra_tfw مضروب في عدد أيام الشهر
        //    table.Columns.Add("AttendanceDays", typeof(int)); // الحقل 4: عدد أيام الحضور لكل قسم
        //    table.Columns.Add("CalculatedField5", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float)); // الحقل 6: M_Penalite
        //    table.Columns.Add("TotalPenalties", typeof(float)); // الحقل 7: TotalPenalties

        //    // التحقق مما إذا كان المستخدم أدمن أو مانجر
        //    bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager;

        //    // إذا لم يكن أدمن أو مانجر، استخدم userAccessPosteID
        //    int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // استرجاع الأقسام من جدول Fiche_DePosts
        //        var departments = context.Fiche_Postes.Select(post => new
        //        {
        //            post.ID,
        //            DepartmentName = post.Name,
        //            M_Penalite = post.M_Penalite,
        //            Nembre_Contra = post.Nembre_Contra,
        //            Nembre_Contra_tfw = post.Nembre_Contra_tfw,
        //            EmployeeCount_tfw = post.EmployeeCount_tfw
        //        }).ToList();

        //        // متغير لحساب مجموع TotalPenalties
        //        float totalSum = 0;

        //        foreach (var department in departments)
        //        {
        //            // استرجاع جميع السجلات للعاملين في هذا القسم ضمن الفترة المحددة
        //            var agentDetails = context.MVMAgentDetails
        //                .Where(x => x.Date <= endDate) // تحقق من أي سجل حتى تاريخ endDate
        //                .Join(context.Fiche_Agents.Where(x => x.Statut == true ),
        //                      agent => agent.ItemID,
        //                      worker => worker.ID,
        //                      (agent, worker) => new { agent, worker })
        //                .Where(x => x.worker.ID_Post == department.ID && (isAdminOrManager || x.worker.ScreenPosteD == userAccessPosteID))
        //                .OrderBy(x => x.agent.Date)
        //                .ToList();

        //            // تأكد من أن هناك موظفين في هذا القسم
        //            if (!agentDetails.Any()) continue;

        //            // إنشاء مجموعة لتخزين فترات الحضور لكل موظف
        //            var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();

        //            // مجموعة لتخزين الموظفين الغائبين
        //            var absentAgents = new HashSet<int>();

        //            foreach (var record in agentDetails)
        //            {
        //                int agentId = record.worker.ID;

        //                if (!agentPresencePeriods.ContainsKey(agentId))
        //                {
        //                    agentPresencePeriods[agentId] = new List<(DateTime, DateTime)>();
        //                }

        //                // إذا كانت الحالة حضور أو عطلة
        //                if (record.agent.Statut == "P")
        //                {
        //                    agentPresencePeriods[agentId].Add((record.agent.Date, endDate)); // افتراض نهاية الفترة
        //                }
        //                else if (record.agent.Statut == "CR" || record.agent.Statut == "A")
        //                {
        //                    var lastPeriod = agentPresencePeriods[agentId].LastOrDefault();
        //                    if (lastPeriod != default)
        //                    {
        //                        // تحديث نهاية فترة الحضور
        //                        agentPresencePeriods[agentId][agentPresencePeriods[agentId].Count - 1] =
        //                            (lastPeriod.Start, record.agent.Date.AddDays(-1));
        //                    }

        //                    // إضافة الموظف إلى مجموعة الغائبين
        //                    absentAgents.Add(agentId);
        //                }
        //            }

        //            // تحقق من العدد الأقصى للغائبين بناءً على القاعدة
        //            int maxAllowedAbsences = (int)department.Nembre_Contra;
                        

        //            // إذا كان عدد الغائبين أكبر من العدد المسموح به، قم بضبطه
        //            int validAbsentCount = Math.Min(absentAgents.Count, maxAllowedAbsences);

        //            // النتيجة النهائية لعدد الغائبين
        //            absentAgents = absentAgents.Take(validAbsentCount).ToHashSet();

        //            // حساب إجمالي الغيابات
        //            int totalAbsences = 0;
        //            int attendanceDays = 0;

        //            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //            {
        //                int dailyPresenceCount = 0;

        //                foreach (var periods in agentPresencePeriods.Values)
        //                {
        //                    if (periods.Any(p => date >= p.Start && date <= p.End))
        //                    {
        //                        dailyPresenceCount++;
        //                    }
        //                }

        //                // حساب الغيابات اليومية بناءً على القاعدة
        //                int dailyAbsences = Math.Max(0, (int)(department.Nembre_Contra));
                           

        //                totalAbsences += dailyAbsences;
        //                attendanceDays += dailyPresenceCount;
        //            }

        //            // حساب الحقل 3: Nembre_Contra أو Nembre_Contra_tfw مضروب في عدد أيام الشهر
        //            int calculatedField3 = ((int)department.Nembre_Contra);
        //                    // حساب الحقل 5: حقل 3 - عدد أيام الحضور
        //            int calculatedField5 = Math.Max(0, calculatedField3 - attendanceDays);


        //            // حساب إجمالي الغرامات
        //            float totalPenalties = (float)(totalAbsences * (department.M_Penalite));
                       
        //            // إضافة سطر جديد للتقرير
        //            DataRow row = table.NewRow();
        //            row["Department"] = department.DepartmentName;
        //            row["Nembre_Contra"] =  department.Nembre_Contra;
        //            row["CalculatedField3"] = calculatedField3;
        //            row["AttendanceDays"] = attendanceDays;
        //            row["CalculatedField5"] = calculatedField5;
        //            row["M_Penalite"] =  department.M_Penalite;
        //            row["TotalPenalties"] = totalPenalties;

        //            // إضافة القيمة إلى المجموع
        //            totalSum += totalPenalties;
        //            Sum = (decimal) totalSum;
        //            table.Rows.Add(row);
        //        }
        //    }

        //    return table;
        //}
       
        private void btn_penality2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Master.User.UserType == (byte)Master.UserType.Guest)
            {
                XtraMessageBox.Show("Vous n'avez pas l'autorisation d'accéder à ce rapport.",
                     "Autorisations insuffisantes",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }

            GenerateReport3(); 
        }

        private void btn_print_pinalite2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Master.User.UserType == (byte)Master.UserType.Guest)
            {
                XtraMessageBox.Show("Vous n'avez pas l'autorisation d'accéder à ce rapport.",
                     "Autorisations insuffisantes",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }
            GenerateReport();
        }
        private void GenerateReport3() // pénalité
        {
            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;

            LoadData(); // إذا كان ضروريًا لتحميل البيانات

            var reportLoader = new LoadModifiedReportClass(new DAL.DataClasses1DataContext(), this);

            // تحميل التقرير مع التحقق من null
            rpt_penalite2 report = reportLoader.LoadModifiedReport<rpt_penalite2>("rpt_penalite2")
                                    ?? new rpt_penalite2();

            // إعداد النص الخاص بالشهر والسنة
            string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
            int annee = startDate.Year;
            report.lbl_mois.Text = $"Mois de {mois} {annee}";

            // إنشاء الجدول وتعيين مصدر البيانات
            DataTable table = CreateAbsenceReport3((endDate - startDate).Days + 1, startDate, endDate);
            report.DataSource = table;

            // تعيين خاصية الجمع إذا لزم الأمر
            report.SumPinality = Sum;

            // إعادة ربط البيانات (سيتم تنفيذ BindData تلقائيًا في الكونستركتور)
            report.BindAttendanceDataToCells(); // إذا كانت تحتاج تعديلات

            // عرض التقرير
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowRibbonPreview();
        }
        //private void GenerateReport3() // pénalité
        //{
        //    DateTime startDate = dateEdit1.DateTime;
        //    DateTime endDate = dateEdit2.DateTime;


        //    int totalDays = (endDate - startDate).Days + 1;

        //    rpt_penalite2 report = new rpt_penalite2();

        //    string mois = startDate.ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        //    int annee = startDate.Year;
        //    report.lbl_mois.Text = $"Mois de {mois} {annee}";

        //    report.DataSource = CreateAbsenceReport3(totalDays, startDate, endDate);

        //    report.SumPinality = Sum;

        //    report.BindAttendanceDataToCells();
        //    report.ShowPreviewDialog();
        //}
        private DataTable CreateAbsenceReport3(int totalDays, DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("Nembre_Contra", typeof(int)); 
            table.Columns.Add("CalculatedField3", typeof(int)); 
            table.Columns.Add("AttendanceDays", typeof(int)); 
            table.Columns.Add("CalculatedField5", typeof(int)); 
            table.Columns.Add("M_Penalite", typeof(float)); 
            table.Columns.Add("TotalPenalties", typeof(float)); 

            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;


            using (var context = new DAL.DataClasses1DataContext())
            {
                var departments = context.Fiche_Postes.Select(post => new
                {
                    post.ID,
                    DepartmentName = post.Name,
                    M_Penalite = post.M_Penalite,
                    Nembre_Contra = post.Nembre_Contra,
                
                }).ToList();

                float totalSum = 0;

                foreach (var department in departments)
                {
                    var agentDetails = context.MVMAgentDetails
                        .Where(x => x.Date <= endDate) 
                        .Join(context.Fiche_Agents.Where(x => x.Statut == true/* && x.Affecter == "TFT"*/),
                              agent => agent.ItemID,
                              worker => worker.ID,
                              (agent, worker) => new { agent, worker })
                        .Where(x => x.worker.ID_Post == department.ID && (isAdminOrManager || x.worker.ScreenPosteD == userAccessPosteID))
                        .OrderBy(x => x.agent.Date)
                        .ToList();

                
                    if (!agentDetails.Any()) continue;
                    var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();


                    int attendanceDays = CalculateAttendanceDays(agentDetails, startDate, endDate, agentPresencePeriods, (int)department.Nembre_Contra);

                    int totalAttendanceDays = attendanceDays ;

                    int totalNembreContra = (int)(department.Nembre_Contra );

                    int calculatedField3 = totalNembreContra * totalDays;

                    int calculatedField5 = Math.Max(0, calculatedField3 - totalAttendanceDays);

                    float totalPenalties = (float)((totalNembreContra * totalDays - totalAttendanceDays) * department.M_Penalite);
                    if (totalPenalties < 0)
                    {
                        totalPenalties = 0;
                    }
                    if (calculatedField5 > 0)
                    {
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["Nembre_Contra"] = totalNembreContra;
                        row["CalculatedField3"] = calculatedField3;
                        row["AttendanceDays"] = totalAttendanceDays;
                        row["CalculatedField5"] = calculatedField5;
                        row["M_Penalite"] = department.M_Penalite;
                        row["TotalPenalties"] = totalPenalties;

                        totalSum += totalPenalties;
                        Sum = (decimal)totalSum;
                        table.Rows.Add(row);
                    }
                     
                }
            }

            return table;
        }

        private int CalculateAttendanceDays<T>(List<T> agentDetails, DateTime startDate, DateTime endDate, Dictionary<int, List<(DateTime Start, DateTime End)>> agentPresencePeriods, int maxAllowedPresence)
            where T : class
        {
            int attendanceDays = 0;

            foreach (var record in agentDetails)
            {
                var agentId = (int)record.GetType().GetProperty("worker").GetValue(record).GetType().GetProperty("ID").GetValue(record.GetType().GetProperty("worker").GetValue(record));
                var agentStatut = (string)record.GetType().GetProperty("agent").GetValue(record).GetType().GetProperty("Statut").GetValue(record.GetType().GetProperty("agent").GetValue(record));
                var agentDate = (DateTime)record.GetType().GetProperty("agent").GetValue(record).GetType().GetProperty("Date").GetValue(record.GetType().GetProperty("agent").GetValue(record));

                if (!agentPresencePeriods.ContainsKey(agentId))
                {
                    agentPresencePeriods[agentId] = new List<(DateTime, DateTime)>();
                }

                if (agentStatut == "P")
                {
                    agentPresencePeriods[agentId].Add((agentDate, endDate)); 
                }
                else if (agentStatut == "CR" || agentStatut == "A")
                {
                    var lastPeriod = agentPresencePeriods[agentId].LastOrDefault();
                    if (lastPeriod != default)
                    {
                        agentPresencePeriods[agentId][agentPresencePeriods[agentId].Count - 1] =
                            (lastPeriod.Start, agentDate.AddDays(-1));
                    }
                }
            }

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                int dailyPresenceCount = 0;

                foreach (var periods in agentPresencePeriods.Values)
                {
                    if (periods.Any(p => date >= p.Start && date <= p.End))
                    {
                        dailyPresenceCount++;
                    }
                }

                // تقييد الحضور اليومي بالحد الأقصى المسموح به
                attendanceDays += Math.Min(dailyPresenceCount, maxAllowedPresence);
            }

            return attendanceDays;
        }

        private void btn_Import_xlsx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_MVM_Import_XLSX frm =new Frm_MVM_Import_XLSX();
            frm.ShowDialog();
        }
    }
}