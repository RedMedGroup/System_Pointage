using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
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

            if (e.Column.FieldName == "POSTE" || e.Column.FieldName == "EFECTIF/CONTRAT")
            {
                string value1 = view.GetRowCellDisplayText(e.RowHandle1, e.Column);
                string value2 = view.GetRowCellDisplayText(e.RowHandle2, e.Column);

                // دمج الخلايا إذا كانت القيم متساوية أو إذا كانت القيم فارغة تحت نفس الـ 
                if (value1 == value2 || string.IsNullOrEmpty(value2))
                {
                    e.Merge = true;
                }
                else
                {
                    e.Merge = false;
                }

                e.Handled = true;
            }
            else
            {
                e.Merge = false;
            }
            e.Handled = true;
        }

        private void btn_recharch_Click(object sender, EventArgs e)
        {
            LoadData();
            gridControl1.DataSource = CreateDataTable();

            gridView1.PopulateColumns();
            gridView1.BestFitColumns();

        }

    

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

            if (e.Column.FieldName == "EFECTIF/CONTRAT")
            {
                string cellValue = e.CellValue?.ToString();
                if (cellValue != "")
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FFECB3");
            }
            if (e.Column.FieldName == "Name")
            {
                string cellValue = e.CellValue?.ToString();
                if (cellValue == "P/Total" || cellValue == "A/Total")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#00897B");

                }
            }
            if (e.Column.FieldName != "POSTE" && e.Column.FieldName != "Name")
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

            // إضافة أعمدة التواريخ
            for (int i = 0; i < totalDays; i++)
            {
                DateTime currentDay = startDate.AddDays(i);
                table.Columns.Add($"{currentDay.Day}", typeof(string));
            }

            table.Columns.Add("Total", typeof(int));

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            var context = new DAL.DataClasses1DataContext();
            var groups = FicheAgentList.Where(x=>x.Statut==true && x.Affecter == cmb_base.Text)//new
                .GroupBy(agent => agent.ID_Post)
                .Where(g => isAdmin || g.Any(agent => agent.ScreenPosteD == userAccessPosteID ))
                .Select(g => new
                {
                    Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
                    RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
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

                    // جلب الحركات من جدول MVMAgentDetails
                    var agentMovements = context.MVMAgentDetails
                        .Where(m => m.ItemID == agent.ID)
                        .OrderBy(m => m.Date)
                        .ToList();

                    string currentStatus = "-"; // الحالة الافتراضية
                    DateTime? lastMovementDate = null;

                    // تحديث الحالة بناءً على الحركات المسجلة
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
                            // إذا كان اليوم الحالي يتجاوز تاريخ اليوم، توقف عن التكرار
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

                // إضافة صف لحساب الحضور "P/Total" لكل قسم
                DataRow presentCountRow = table.NewRow();
                presentCountRow["Name"] = "P/Total";
                for (int i = 0; i < totalDays; i++)
                {
                    presentCountRow[$"{startDate.AddDays(i).Day}"] = presentCountPerDay[i].ToString();
                }
                presentCountRow["Total"] = totalPresent;
                table.Rows.Add(presentCountRow);

                // إضافة صف لحساب الغياب "A/Total" لكل قسم
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
                // تحميل قائمة الوكلاء من جدول Fich_Agents
                FicheAgentList = context.Fiche_Agents

                    .ToList();

                // تحميل قائمة التخصصات من جدول Fiche_DePosts
                FichePOSTE = context.Fiche_Postes.ToList();
            }
        }

        private void GenerateReport() // pénalité
        {
            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            int totalDays = (endDate - startDate).Days + 1;

            rpt_penalite report = new rpt_penalite();
            report.DataSource = CreateAbsenceReport(totalDays, startDate, endDate); // تحديث هنا لاستدعاء الدالة الجديدة
            report.ShowPreview();
        }

        private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();

            // إضافة الأعمدة
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("TotalAbsences", typeof(int));
            table.Columns.Add("M_Penalite", typeof(float));
            table.Columns.Add("TotalPenalties", typeof(float));
            table.Columns.Add("Nombre du personnel absent", typeof(int));

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            using (var context = new DAL.DataClasses1DataContext())
            {
  


                // استرجاع الأقسام من جدول Fiche_DePosts
                var departments = context.Fiche_Postes.Select(post => new
                {
                    post.ID,
                    DepartmentName = post.Name,
                    M_Penalite = post.M_Penalite,
                    Nembre_Contra = post.Nembre_Contra,
                    EmployeeCount_tfw=post.EmployeeCount_tfw,
                    Nembre_Contra_tfw=post.Nembre_Contra_tfw,
                }).ToList();



                foreach (var department in departments)
                {
                    // استرجاع جميع السجلات للعاملين في هذا القسم ضمن الفترة المحددة
                    var agentDetails = context.MVMAgentDetails
                        .Where(x => x.Date <= endDate) // تحقق من أي سجل حتى تاريخ endDate
                        .Join(context.Fiche_Agents.Where(x => x.Statut == true&&x.Affecter==cmb_base.Text),
                              agent => agent.ItemID,
                              worker => worker.ID,
                              (agent, worker) => new { agent, worker })
                        .Where(x => x.worker.ID_Post == department.ID && (isAdmin || x.worker.ScreenPosteD == userAccessPosteID))
                        .OrderBy(x => x.agent.Date)
                        .ToList();

                    // تأكد من أن هناك موظفين في هذا القسم
                    if (!agentDetails.Any()) continue;

                    // إنشاء مجموعة لتخزين فترات الحضور لكل موظف
                    var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();

                    // مجموعة لتخزين الموظفين الغائبين
                    var absentAgents = new HashSet<int>();

                    foreach (var record in agentDetails)
                    {
                        int agentId = record.worker.ID;

                        if (!agentPresencePeriods.ContainsKey(agentId))
                        {
                            agentPresencePeriods[agentId] = new List<(DateTime, DateTime)>();
                        }

                        // إذا كانت الحالة حضور أو عطلة
                        if (record.agent.Statut == "P")
                        {
                            agentPresencePeriods[agentId].Add((record.agent.Date, endDate)); // افتراض نهاية الفترة
                        }
                        else if (record.agent.Statut == "CR" || record.agent.Statut == "A")
                        {
                            var lastPeriod = agentPresencePeriods[agentId].LastOrDefault();
                            if (lastPeriod != default)
                            {
                                // تحديث نهاية فترة الحضور
                                agentPresencePeriods[agentId][agentPresencePeriods[agentId].Count - 1] =
                                    (lastPeriod.Start, record.agent.Date.AddDays(-1));
                            }

                            // إضافة الموظف إلى مجموعة الغائبين
                            absentAgents.Add(agentId);
                        }
                    }

                    // حساب إجمالي الغيابات
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

                        //    int dailyAbsences = Math.Max(0, (int)department.Nembre_Contra - dailyPresenceCount);
                        // حساب الغيابات اليومية بناءً على القاعدة
                        int dailyAbsences = Math.Max(0, (int)(cmb_base.Text == "TFT"
                            ? department.Nembre_Contra
                            : department.Nembre_Contra_tfw) - dailyPresenceCount);
                       

                        totalAbsences += dailyAbsences;
                    }

                    // إذا كان هناك غيابات، أضف الصف إلى التقرير
                    if (totalAbsences > 0)
                    {
                        // حساب إجمالي الغرامات
                        //float totalPenalties = (float)(totalAbsences * department.M_Penalite);
                        // حساب إجمالي الغرامات بناءً على القاعدة
                        float totalPenalties = (float)(totalAbsences * (cmb_base.Text == "TFT"
                            ? department.M_Penalite
                            : department.EmployeeCount_tfw));
                        // إضافة سطر جديد للتقرير
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["TotalAbsences"] = totalAbsences;
                        row["M_Penalite"] = (cmb_base.Text == "TFT") ? department.M_Penalite : department.EmployeeCount_tfw;
                   //     row["M_Penalite"] = department.M_Penalite;
                        row["TotalPenalties"] = totalPenalties;
                        row["Nombre du personnel absent"] = absentAgents.Count; // حساب عدد الموظفين الغائبين

                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }


        //    private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //    {
        //        DataTable table = new DataTable();

        //        // إضافة الأعمدة
        //        table.Columns.Add("Department", typeof(string));
        //        table.Columns.Add("TotalAbsences", typeof(int));
        //        table.Columns.Add("M_Penalite", typeof(float));
        //        table.Columns.Add("TotalPenalties", typeof(float));
        //        table.Columns.Add("Nombre du personnel absent", typeof(int));


        //        // التحقق مما إذا كان المستخدم أدمن
        //        bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

        //        // إذا لم يكن أدمن، استخدم userAccessPosteID
        //        int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

        //        using (var context = new DAL.DataClasses1DataContext())
        //        {
        //            // استرجاع الأقسام من جدول Fiche_DePosts
        //            var departments = context.Fiche_Postes.Select(post => new
        //            {
        //                post.ID,
        //                DepartmentName = post.Name,
        //                M_Penalite = post.M_Penalite,
        //                Nembre_Contra = post.Nembre_Contra
        //            }).ToList();

        //            foreach (var department in departments)
        //            {
        //                var agentDetails = context.MVMAgentDetails
        //.Where(x => x.Date <= endDate) // تحقق من أي سجل حتى تاريخ endDate
        //.Join(context.Fiche_Agents,
        //      agent => agent.ItemID,
        //      worker => worker.ID,
        //      (agent, worker) => new { agent, worker })
        //.Where(x => x.worker.ID_Post == department.ID && (isAdmin || x.worker.ScreenPosteD == userAccessPosteID))
        //.OrderBy(x => x.agent.Date)
        //.ToList();

        //                // استرجاع جميع السجلات للعاملين في هذا القسم ضمن الفترة المحددة
        //                //var agentDetails = context.MVMAgentDetails
        //                //    .Where(x => x.Date >= startDate && x.Date <= endDate)
        //                //    .Join(context.Fiche_Agents,
        //                //          agent => agent.ItemID,
        //                //          worker => worker.ID,
        //                //          (agent, worker) => new { agent, worker })
        //                //    .Where(x => x.worker.ID_Post == department.ID && (isAdmin || x.worker.ScreenPosteD == userAccessPosteID))
        //                //    .OrderBy(x => x.agent.Date)
        //                //    .ToList();

        //                // إنشاء مجموعة لتخزين فترات الحضور لكل موظف
        //                var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();

        //                foreach (var record in agentDetails)
        //                {
        //                    int agentId = record.worker.ID;

        //                    if (!agentPresencePeriods.ContainsKey(agentId))
        //                    {
        //                        agentPresencePeriods[agentId] = new List<(DateTime, DateTime)>();
        //                    }

        //                    // إذا كانت الحالة حضور أو عطلة
        //                    if (record.agent.Statut == "P")
        //                    {
        //                        agentPresencePeriods[agentId].Add((record.agent.Date, endDate)); // افتراض نهاية الفترة
        //                    }
        //                    else if (record.agent.Statut == "CR" || record.agent.Statut == "A")
        //                    {
        //                        var lastPeriod = agentPresencePeriods[agentId].LastOrDefault();
        //                        if (lastPeriod != default)
        //                        {
        //                            // تحديث نهاية فترة الحضور
        //                            agentPresencePeriods[agentId][agentPresencePeriods[agentId].Count - 1] =
        //                                (lastPeriod.Start, record.agent.Date.AddDays(-1));
        //                        }
        //                    }
        //                }

        //                // حساب إجمالي الغيابات
        //                int totalAbsences = 0;

        //                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //                {
        //                    int dailyPresenceCount = 0;

        //                    foreach (var periods in agentPresencePeriods.Values)
        //                    {
        //                        if (periods.Any(p => date >= p.Start && date <= p.End))
        //                        {
        //                            dailyPresenceCount++;
        //                        }
        //                    }

        //                    int dailyAbsences = Math.Max(0, (int)department.Nembre_Contra - dailyPresenceCount);
        //                    totalAbsences += dailyAbsences;
        //                }



        //                // إذا كان هناك غيابات، أضف الصف إلى التقرير
        //                if (totalAbsences > 0)
        //                {
        //                    float totalPenalties = (float)(totalAbsences * department.M_Penalite);

        //                    DataRow row = table.NewRow();
        //                    row["Department"] = department.DepartmentName;
        //                    row["TotalAbsences"] = totalAbsences;
        //                    row["M_Penalite"] = department.M_Penalite;
        //                    row["TotalPenalties"] = totalPenalties;
        //                    row["Nombre du personnel absent"] = agentPresencePeriods.Count;

        //                    table.Rows.Add(row);
        //                }
        //            }
        //        }
        //        return table;
        //    }




        //private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    // إضافة الأعمدة
        //    table.Columns.Add("Department", typeof(string));
        //    table.Columns.Add("TotalAbsences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("TotalPenalties", typeof(float));
        //    table.Columns.Add("Nombre du personnel absent", typeof(int));

        //    // التحقق مما إذا كان المستخدم أدمن
        //    bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

        //    // إذا لم يكن أدمن، استخدم userAccessPosteID
        //    int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // استرجاع الأقسام من جدول Fiche_DePosts
        //        var departments = context.Fiche_Postes.Select(post => new
        //        {
        //            post.ID,
        //            DepartmentName = post.Name,
        //            M_Penalite = post.M_Penalite,
        //            Nembre_Contra = post.Nembre_Contra // الحصول على Nembre_Contra من جدول Fiche_Postes
        //        }).ToList();

        //        // حساب الغيابات والمعلومات المطلوبة لكل قسم خلال الفترة المحددة
        //        foreach (var department in departments)
        //        {
        //            // استرجاع جميع السجلات للعاملين في هذا القسم خلال الفترة المحددة
        //            var agentAbsences = context.MVMAgentDetails
        //                .Where(x => x.Date >= startDate && x.Date <= endDate)
        //                .Join(context.Fiche_Agents,
        //                      agent => agent.ItemID,
        //                      worker => worker.ID,
        //                      (agent, worker) => new { agent, worker })
        //                .Where(x => x.worker.ID_Post == department.ID && (isAdmin || x.worker.ScreenPosteD == userAccessPosteID)) // الشرط لإظهار السجلات بناءً على حالة المستخدم
        //                .OrderBy(x => x.agent.Date)
        //                .ToList();


        //            // إذا لم يكن هناك أي سجلات للأشخاص في هذا القسم، تخطاه
        //            if (!agentAbsences.Any())
        //                continue;

        //            // إنشاء مجموعة لتخزين الغيابات لكل موظف
        //            var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
        //            var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

        //            // ملء قائمة حالات الموظفين
        //            foreach (var record in agentAbsences)
        //            {
        //                if (!absenceRecordsByAgent.ContainsKey(record.worker.ID))
        //                {
        //                    absenceRecordsByAgent[record.worker.ID] = new List<(DateTime, string)>();
        //                }
        //                absenceRecordsByAgent[record.worker.ID].Add((record.agent.Date, record.agent.Statut));
        //            }

        //            // حساب الغيابات لكل موظف
        //            int totalAbsences = 0;
        //            int totalPersonnelPresent = 0; // لحساب عدد الحضور الفعلي

        //            foreach (var agent in absenceRecordsByAgent)
        //            {
        //                var absenceRecords = agent.Value;

        //                for (int i = 0; i < absenceRecords.Count; i++)
        //                {
        //                    if (absenceRecords[i].Status == "A")
        //                    {
        //                        uniqueAbsentPersonnelSet.Add(agent.Key); // إضافة ID الموظف إلى المجموعة

        //                        DateTime absenceStart = absenceRecords[i].Date;

        //                        // البحث عن أول تاريخ يتغير فيه الحالة
        //                        DateTime absenceEnd = endDate; // افتراضياً نهاية الغياب هي نهاية الفترة
        //                        for (int j = i + 1; j < absenceRecords.Count; j++)
        //                        {
        //                            if (absenceRecords[j].Status != "A")
        //                            {
        //                                absenceEnd = absenceRecords[j].Date.AddDays(-1); // اليوم الذي يسبق حالة الحضور
        //                                break;
        //                            }
        //                        }

        //                        // التأكد من أن التواريخ تقع ضمن النطاق المحدد
        //                        absenceStart = (absenceStart < startDate) ? startDate : absenceStart;
        //                        absenceEnd = (absenceEnd > endDate) ? endDate : absenceEnd;

        //                        // حساب عدد أيام الغياب ضمن الفترة
        //                        int absenceCount = (absenceEnd - absenceStart).Days + 1;
        //                        totalAbsences += absenceCount; // إضافة إلى الإجمالي

        //                        // إذا انتهى الغياب بنهاية الفترة، لا حاجة للاستمرار
        //                        if (absenceEnd == endDate)
        //                            break;
        //                    }
        //                }
        //            }

        //            // حساب عدد الحضور الفعلي في القسم
        //            totalPersonnelPresent = absenceRecordsByAgent.Count;

        //            // إذا كان عدد الحضور الفعلي أقل من Nembre_Contra، حساب الفرق
        //            int missingPersonnel = (int)department.Nembre_Contra - totalPersonnelPresent;
        //            if (missingPersonnel > 0)
        //            {
        //                // إضافة الفرق كغيباب يومي
        //                totalAbsences += missingPersonnel * totalDays;

        //                // إضافة هؤلاء الأشخاص كغائبين أيضًا
        //                // نعتبر هؤلاء الأشخاص الغائبين بسبب النقص في الحضور
        //                for (int i = 0; i < missingPersonnel; i++)
        //                {
        //                    uniqueAbsentPersonnelSet.Add(i); // إضافة أشخاص غائبين افتراضيين
        //                }
        //            }

        //            // إذا كان هناك غيابات في القسم، أضف الصف إلى التقرير
        //            if (totalAbsences > 0)
        //            {
        //                // حساب الغرامات الكلية
        //                float totalPenalties = (float)(totalAbsences * department.M_Penalite);

        //                // إضافة صف جديد إلى الجدول
        //                DataRow row = table.NewRow();
        //                row["Department"] = department.DepartmentName;
        //                row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
        //                row["M_Penalite"] = department.M_Penalite;
        //                row["TotalPenalties"] = totalPenalties;
        //                row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

        //                table.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return table;
        //}


        //private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    // إضافة الأعمدة
        //    table.Columns.Add("Department", typeof(string));
        //    table.Columns.Add("TotalAbsences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("TotalPenalties", typeof(float));
        //    table.Columns.Add("Nombre du personnel absent", typeof(int));

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // استرجاع الأقسام من جدول Fiche_DePosts
        //        var departments = context.Fiche_Postes.Select(post => new
        //        {
        //            post.ID,
        //            DepartmentName = post.Name,
        //            M_Penalite = post.M_Penalite,
        //            Nembre_Contra = post.Nembre_Contra // الحصول على Nembre_Contra من جدول Fiche_Postes
        //        }).ToList();

        //        // حساب الغيابات والمعلومات المطلوبة لكل قسم خلال الفترة المحددة
        //        foreach (var department in departments)
        //        {
        //            // استرجاع جميع السجلات للعاملين في هذا القسم خلال الفترة المحددة
        //            var agentAbsences = context.MVMAgentDetails
        //                .Where(x => x.Date >= startDate && x.Date <= endDate)
        //                .Join(context.Fiche_Agents,
        //                      agent => agent.ItemID,
        //                      worker => worker.ID,
        //                      (agent, worker) => new { agent, worker })
        //                .Where(x => x.worker.ID_Post == department.ID)
        //                .OrderBy(x => x.agent.Date)
        //                .ToList();

        //            // إنشاء مجموعة لتخزين الغيابات لكل موظف
        //            var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
        //            var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

        //            // ملء قائمة حالات الموظفين
        //            foreach (var record in agentAbsences)
        //            {
        //                if (!absenceRecordsByAgent.ContainsKey(record.worker.ID))
        //                {
        //                    absenceRecordsByAgent[record.worker.ID] = new List<(DateTime, string)>();
        //                }
        //                absenceRecordsByAgent[record.worker.ID].Add((record.agent.Date, record.agent.Statut));
        //            }

        //            // حساب الغيابات لكل موظف
        //            int totalAbsences = 0;
        //            int totalPersonnelPresent = 0; // لحساب عدد الحضور الفعلي

        //            foreach (var agent in absenceRecordsByAgent)
        //            {
        //                var absenceRecords = agent.Value;

        //                for (int i = 0; i < absenceRecords.Count; i++)
        //                {
        //                    if (absenceRecords[i].Status == "A")
        //                    {
        //                        uniqueAbsentPersonnelSet.Add(agent.Key); // إضافة ID الموظف إلى المجموعة

        //                        DateTime absenceStart = absenceRecords[i].Date;

        //                        // البحث عن أول تاريخ يتغير فيه الحالة
        //                        DateTime absenceEnd = endDate; // افتراضياً نهاية الغياب هي نهاية الفترة
        //                        for (int j = i + 1; j < absenceRecords.Count; j++)
        //                        {
        //                            if (absenceRecords[j].Status != "A")
        //                            {
        //                                absenceEnd = absenceRecords[j].Date.AddDays(-1); // اليوم الذي يسبق حالة الحضور
        //                                break;
        //                            }
        //                        }

        //                        // التأكد من أن التواريخ تقع ضمن النطاق المحدد
        //                        absenceStart = (absenceStart < startDate) ? startDate : absenceStart;
        //                        absenceEnd = (absenceEnd > endDate) ? endDate : absenceEnd;

        //                        // حساب عدد أيام الغياب ضمن الفترة
        //                        int absenceCount = (absenceEnd - absenceStart).Days + 1;
        //                        totalAbsences += absenceCount; // إضافة إلى الإجمالي

        //                        // إذا انتهى الغياب بنهاية الفترة، لا حاجة للاستمرار
        //                        if (absenceEnd == endDate)
        //                            break;
        //                    }
        //                }
        //            }

        //            // حساب عدد الحضور الفعلي في القسم
        //            totalPersonnelPresent = absenceRecordsByAgent.Count;

        //            // إذا كان عدد الحضور الفعلي أقل من Nembre_Contra، حساب الفرق
        //            int missingPersonnel = (int)department.Nembre_Contra - totalPersonnelPresent;
        //            if (missingPersonnel > 0)
        //            {
        //                // إضافة الفرق كغيباب يومي
        //                totalAbsences += missingPersonnel * totalDays;
        //            }

        //            // إذا كان هناك غيابات في القسم، أضف الصف إلى التقرير
        //            if (totalAbsences > 0)
        //            {
        //                // حساب الغرامات الكلية
        //                float totalPenalties = (float)(totalAbsences * department.M_Penalite);

        //                // إضافة صف جديد إلى الجدول
        //                DataRow row = table.NewRow();
        //                row["Department"] = department.DepartmentName;
        //                row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
        //                row["M_Penalite"] = department.M_Penalite;
        //                row["TotalPenalties"] = totalPenalties;
        //                row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

        //                table.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return table;
        //}


        #region
        //private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    // إضافة الأعمدة
        //    table.Columns.Add("Department", typeof(string));
        //    table.Columns.Add("TotalAbsences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("TotalPenalties", typeof(float));
        //    table.Columns.Add("Nombre du personnel absent", typeof(int));

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // استرجاع الأقسام من جدول Fiche_DePosts
        //        var departments = context.Fiche_Postes.Select(post => new
        //        {
        //            post.ID,
        //            DepartmentName = post.Name,
        //            M_Penalite = post.M_Penalite
        //        }).ToList();

        //        // حساب الغيابات والمعلومات المطلوبة لكل قسم خلال الفترة المحددة
        //        foreach (var department in departments)
        //        {
        //            // استرجاع جميع السجلات للعاملين في هذا القسم خلال الفترة المحددة
        //            var agentAbsences = context.MVMAgentDetails
        //                .Where(x => x.Date >= startDate && x.Date <= endDate)
        //                .Join(context.Fiche_Agents,
        //                      agent => agent.ItemID,
        //                      worker => worker.ID,
        //                      (agent, worker) => new { agent, worker })
        //                .Where(x => x.worker.ID_Post == department.ID)
        //                .OrderBy(x => x.agent.Date)
        //                .ToList();

        //            // إنشاء مجموعة لتخزين الغيابات لكل موظف
        //            var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
        //            var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

        //            // ملء قائمة حالات الموظفين
        //            foreach (var record in agentAbsences)
        //            {
        //                if (!absenceRecordsByAgent.ContainsKey(record.worker.ID))
        //                {
        //                    absenceRecordsByAgent[record.worker.ID] = new List<(DateTime, string)>();
        //                }
        //                absenceRecordsByAgent[record.worker.ID].Add((record.agent.Date, record.agent.Statut));
        //            }

        //            // حساب الغيابات لكل موظف
        //            int totalAbsences = 0;

        //            foreach (var agent in absenceRecordsByAgent)
        //            {
        //                var absenceRecords = agent.Value;

        //                for (int i = 0; i < absenceRecords.Count; i++)
        //                {
        //                    if (absenceRecords[i].Status == "A")
        //                    {
        //                        uniqueAbsentPersonnelSet.Add(agent.Key); // إضافة ID الموظف إلى المجموعة

        //                        DateTime absenceStart = absenceRecords[i].Date;

        //                        // البحث عن أول تاريخ يتغير فيه الحالة
        //                        DateTime absenceEnd = endDate; // افتراضياً نهاية الغياب هي نهاية الفترة
        //                        for (int j = i + 1; j < absenceRecords.Count; j++)
        //                        {
        //                            if (absenceRecords[j].Status != "A")
        //                            {
        //                                absenceEnd = absenceRecords[j].Date.AddDays(-1); // اليوم الذي يسبق حالة الحضور
        //                                break;
        //                            }
        //                        }
        //                        // التأكد من أن التواريخ تقع ضمن النطاق المحدد
        //                        absenceStart = (absenceStart < startDate) ? startDate : absenceStart;
        //                        absenceEnd = (absenceEnd > endDate) ? endDate : absenceEnd;

        //                        // حساب عدد أيام الغياب ضمن الفترة
        //                        int absenceCount = (absenceEnd - absenceStart).Days + 1;
        //                        totalAbsences += absenceCount; // إضافة إلى الإجمالي

        //                        // إذا انتهى الغياب بنهاية الفترة، لا حاجة للاستمرار
        //                        if (absenceEnd == endDate)
        //                            break;
        //                    }
        //                }
        //            }


        //            // إذا كان هناك غيابات في القسم، أضف الصف إلى التقرير
        //            if (totalAbsences > 0)
        //            {
        //                // حساب الغرامات الكلية
        //                float totalPenalties = (float)(totalAbsences * department.M_Penalite);

        //                // إضافة صف جديد إلى الجدول
        //                DataRow row = table.NewRow();
        //                row["Department"] = department.DepartmentName;
        //                row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
        //                row["M_Penalite"] = department.M_Penalite;
        //                row["TotalPenalties"] = totalPenalties;
        //                row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

        //                table.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return table;
        //}
        #endregion
        private void btn_print_pinalite_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {

            //dateEdit2.DateTime = dateEdit1.DateTime.AddDays(29);
            DateTime selectedDate = dateEdit1.DateTime;

            // حساب عدد الأيام المتبقية في الشهر
            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            int remainingDays = daysInMonth - selectedDate.Day;

            // تحديث dateEdit2 ليكون في آخر الشهر
            dateEdit2.DateTime = selectedDate.AddDays(remainingDays);
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateEdit2.DateTime;

            // ضبط dateEdit1 ليكون اليوم الأول من الشهر المحدد في dateEdit2
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
            printTool.ShowPreview();
        }

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

            // إضافة الأعمدة
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("RequiredEmployees", typeof(int));
            table.Columns.Add("WorkerName", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("PresentCount", typeof(int));
            table.Columns.Add("cell_Ecart", typeof(int));

            using (var context = new DAL.DataClasses1DataContext())
            {
                // التحقق مما إذا كان المستخدم أدمن
                bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

                // إذا لم يكن أدمن، استخدم userAccessPosteID
                int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

                // استرجاع الأقسام
                var departments = context.Fiche_Postes
                    .Select(post => new
                    {
                        ID_Post = post.ID,
                        DepartmentName = post.Name,
                        RequiredEmployees = post.Nembre_Contra
                    })
                    .ToList();

                // استرجاع العمال وحالاتهم في التاريخ المحدد
                var workers = context.Fiche_Agents.Where(agent => isAdmin || agent.ScreenPosteD == userAccessPosteID)
                    .Select(agent => new
                    {
                        WorkerName = agent.Name,
                        DepartmentID = agent.ID_Post,
                        LastStatus = context.MVMAgentDetails
                            .Where(detail =>
                                detail.ItemID == agent.ID &&
                                detail.Date <= reportDate)
                            .OrderByDescending(detail => detail.Date)
                            .Select(detail => detail.Statut)
                            .FirstOrDefault(), // أخذ آخر حالة قبل أو في التاريخ المحدد
                        NextAbsenceDate = context.MVMAgentDetails
                            .Where(detail =>
                                detail.ItemID == agent.ID &&
                                detail.Date > reportDate && // تحقق من وجود غياب بعد تاريخ التقرير
                                detail.Statut == "CR")
                            .OrderBy(detail => detail.Date)
                            .Select(detail => (DateTime?)detail.Date) // تحويل إلى نوع Nullable<DateTime>
                            .FirstOrDefault() // الحصول على أقرب تاريخ غياب بعد تاريخ التقرير
                    })
                    .Where(worker => worker.LastStatus != null && worker.LastStatus != "CR") // تجاهل العمال الذين لا حالة لهم
                    .ToList();

                // إضافة البيانات إلى الجدول
                foreach (var department in departments)
                {
                    // استرجاع عمال القسم
                    var departmentWorkers = workers.Where(w => w.DepartmentID == department.ID_Post).ToList();

                    // حساب عدد الحضور
                    int presentCount = departmentWorkers.Count(w => w.LastStatus == "P" && (w.NextAbsenceDate == null || w.NextAbsenceDate > reportDate));

                    // حساب الفارق
                    int cellEcart = (int)(presentCount - department.RequiredEmployees);

                    foreach (var worker in departmentWorkers)
                    {
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["RequiredEmployees"] = department.RequiredEmployees;
                        row["WorkerName"] = worker.WorkerName;

                        // تحديد الحالة النهائية
                        string status = worker.LastStatus ?? "A"; // افتراض الحالة "A" عند عدم وجود حالة
                        if (worker.LastStatus == "P" && (worker.NextAbsenceDate == null || worker.NextAbsenceDate > reportDate))
                        {
                            status = "P"; // العامل حاضر
                        }
                        else if (worker.LastStatus == "P" && worker.NextAbsenceDate <= reportDate)
                        {
                            status = "A"; // العامل غائب لأن لديه غياب مسجل بعد تاريخ التقرير
                        }

                        row["Status"] = status;
                        row["PresentCount"] = presentCount;
                        row["cell_Ecart"] = cellEcart;

                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }

        private void GenerateDailyReport()
        {
            SetStartDate();
            DateTime reportDate = dateEdit1.DateTime;
            rpt_DailyReport report = new rpt_DailyReport(this);
            report.DataSource = CreateDailyReport(reportDate);
            report.ShowPreview();
        }

        private void btn_reportAparJour_Click(object sender, EventArgs e)
        {
            GenerateDailyReport();
        }

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

        private DataTable CreateDataTableMensuel()
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

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            var groups = FicheAgentList.Where(x => x.Statut == true&&(isAdmin || x.ScreenPosteD == userAccessPosteID))
                .GroupBy(agent => agent.ID_Post)
                .Select(g => new
                {
                    Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
                    RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
                    Penalty = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.M_Penalite ?? 0,
                    Agents = g.ToList()
                });

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
                        break; // إذا تجاوز اليوم تاريخ اليوم الحالي، توقف
                    }

                    // **عدد الحاضرين الفعليين يوميًا**
                    int dailyPresentCount = 0;

                    foreach (var agent in group.Agents)
                    {
                        // ابحث عن آخر حركة للموظف
                        var lastMovement = context.MVMAgentDetails
                            .Where(m => m.ItemID == agent.ID && m.Date.Date <= currentDay.Date)
                            .OrderByDescending(m => m.Date)
                            .FirstOrDefault();

                        // تحقق إذا كان هناك حركة
                        if (lastMovement != null)
                        {
                            // اعتبر الموظف حاضرًا إذا كانت آخر حركة هي "P"
                            if (lastMovement.Statut == "P")
                            {
                                dailyPresentCount++;
                            }
                            // إذا كانت آخر حركة "A" أو "CR"، فلا نعتبره حاضرًا
                            // يمكن أن نترك هذه الحالة بدون إجراء أي شيء لأننا نتحقق فقط من "P"
                        }
                    }
                    //int dailyPresentCount = group.Agents.Count(agent =>
                    //{

                    //    var movement = context.MVMAgentDetails
                    //        .FirstOrDefault(m => m.ItemID == agent.ID&& m.Date.Date == currentDay.Date);
                    //    return movement != null && movement.Statut == "P"; // عد فقط الحاضرين
                    //});

                    // **عدد الغيابات المسجل يدويًا (A)**
                    int dailyAbsentAgentsManual = group.Agents.Count(agent =>
                    {
                        var movements = context.MVMAgentDetails
                            .Where(m => m.ItemID == agent.ID && m.Date.Date <= currentDay.Date)
                            .OrderBy(m => m.Date)
                            .ToList();

                        return movements.Any(m => m.Statut == "A") && !movements.Any(m => m.Statut == "P");
                    });

                    // **إجمالي الغياب = العدد المطلوب - الحضور الفعلي + الغياب المسجل يدويًا**
                    int totalDailyAbsences = Math.Max(0, (int)group.RequiredQuantity - dailyPresentCount) + dailyAbsentAgentsManual;

                    absentCountPerDay[i] = totalDailyAbsences;
                    totalAbsent += totalDailyAbsences;
                }

                // ملء بيانات الغيابات لكل يوم في الصف
                for (int i = 0; i < totalDays; i++)
                {
                    specializationRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i];
                }

                specializationRow["Total Absences"] = totalAbsent;
                specializationRow["M_Penalite"] = group.Penalty;
                specializationRow["Total Penalty"] = totalAbsent * group.Penalty; // حساب الغرامة الإجمالية
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
        }
        //private DataTable CreateDataTableMensuel()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("POSTE", typeof(string));
        //    table.Columns.Add("EFECTIF/CONTRAT", typeof(int));

        //    DateTime startDate = dateEdit1.DateTime.Date;
        //    DateTime endDate = dateEdit2.DateTime.Date;

        //    if (startDate > endDate)
        //    {
        //        MessageBox.Show("La date de début doit être antérieure ou égale à la date de fin.");
        //        return table;
        //    }

        //    int totalDays = (endDate - startDate).Days + 1;

        //    for (int i = 0; i < totalDays; i++)
        //    {
        //        DateTime currentDay = startDate.AddDays(i);
        //        table.Columns.Add($"{currentDay.Day}", typeof(int));
        //    }

        //    table.Columns.Add("Total Absences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("Total Penalty", typeof(float));

        //    var context = new DAL.DataClasses1DataContext();
        //    var groups = FicheAgentList.Where(x => x.Statut == true)
        //        .GroupBy(agent => agent.ID_Post)
        //        .Select(g => new
        //        {
        //            Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
        //            RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
        //            Penalty = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.M_Penalite ?? 0,
        //            Agents = g.ToList()
        //        });

        //    foreach (var group in groups)
        //    {
        //        DataRow specializationRow = table.NewRow();
        //        specializationRow["POSTE"] = group.Specialization;
        //        specializationRow["EFECTIF/CONTRAT"] = group.RequiredQuantity;

        //        int[] absentCountPerDay = new int[totalDays];
        //        int totalAbsent = 0;

        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            DateTime currentDay = startDate.AddDays(i);

        //            if (currentDay > DateTime.Now)
        //            {
        //                break; // إذا تجاوز اليوم تاريخ اليوم الحالي، توقف
        //            }

        //            // **عدد الحاضرين الفعليين يوميًا**
        //            int dailyPresentCount = group.Agents.Count(agent =>
        //            {
        //                var movement = context.MVMAgentDetails
        //                    .FirstOrDefault(m => m.ItemID == agent.ID /*&& m.Date.Date == currentDay.Date*/);
        //                return movement != null && movement.Statut == "P"; // عد فقط الحاضرين
        //            });
        //            // **عدد الغياب المسجل يدويًا (A)**
        //            int dailyAbsentAgentsManual = group.Agents.Count(agent =>
        //            {
        //                // إذا كان الشخص غائبًا في يوم معين (غاب في 10/11/2024)، نريد أن نعتبره غائبًا أيضًا في الأيام التالية
        //                bool isAbsent = false;
        //                var movements = context.MVMAgentDetails
        //                    .Where(m => m.ItemID == agent.ID && m.Date.Date <= currentDay.Date) // تحقق من كل الحركات حتى التاريخ الحالي
        //                    .OrderBy(m => m.Date) // ترتيب الحركات حسب التاريخ
        //                    .ToList();

        //                // إذا كانت هناك حالة غياب في الأيام السابقة
        //                foreach (var movement in movements)
        //                {
        //                    if (movement.Statut == "A")
        //                    {
        //                        isAbsent = true; // إذا كان هناك غياب، يعتبر الشخص غائبًا
        //                    }
        //                    // إذا تم العثور على حالة حضور (P)، لا نحتسب الأيام التالية غيابًا
        //                    else if (movement.Statut == "P")
        //                    {
        //                        isAbsent = false; // إذا كانت هناك حالة حضور، نوقف احتساب الغياب
        //                    }
        //                }

        //                return isAbsent; // إذا كان الشخص غائبًا في أي يوم من الأيام حتى الآن
        //            });

        //            // **عدد الغياب المسجل يدويًا (A)**
        //            //int dailyAbsentAgentsManual = group.Agents.Count(agent =>
        //            //{
        //            //    var movement = context.MVMAgentDetails
        //            //        .FirstOrDefault(m => m.ItemID == agent.ID && m.Date.Date == currentDay.Date);
        //            //    return movement != null && movement.Statut == "A"; // عد فقط الغائبين المسجلين
        //            //});

        //            // **إجمالي الغياب = العدد المطلوب - الحضور الفعلي + الغياب المسجل يدويًا**
        //            int totalDailyAbsences = Math.Max(0, (int)group.RequiredQuantity - dailyPresentCount) + dailyAbsentAgentsManual;

        //            absentCountPerDay[i] = totalDailyAbsences;
        //            totalAbsent += totalDailyAbsences;
        //        }

        //        // ملء بيانات الغيابات لكل يوم في الصف
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            specializationRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i];
        //        }

        //        specializationRow["Total Absences"] = totalAbsent;
        //        specializationRow["M_Penalite"] = group.Penalty;
        //        specializationRow["Total Penalty"] = totalAbsent * group.Penalty; // حساب الغرامة الإجمالية
        //        table.Rows.Add(specializationRow);
        //    }

        //    // Add total row
        //    DataRow totalRow = table.NewRow();
        //    totalRow["POSTE"] = "Total";

        //    int[] dailyTotals = new int[totalDays];
        //    int totalAbsencesSum = 0;
        //    float totalPenaltySum = 0;

        //    foreach (DataRow row in table.Rows)
        //    {
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            dailyTotals[i] += Convert.ToInt32(row[$"{startDate.AddDays(i).Day}"]);
        //        }
        //        totalAbsencesSum += Convert.ToInt32(row["Total Absences"]);
        //        totalPenaltySum += Convert.ToSingle(row["Total Penalty"]);
        //    }

        //    for (int i = 0; i < totalDays; i++)
        //    {
        //        totalRow[$"{startDate.AddDays(i).Day}"] = dailyTotals[i];
        //    }

        //    totalRow["Total Absences"] = totalAbsencesSum;
        //    totalRow["Total Penalty"] = totalPenaltySum;
        //    table.Rows.Add(totalRow);

        //    return table;
        //}
        #region
        //private DataTable CreateDataTableMensuel()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("POSTE", typeof(string));
        //    table.Columns.Add("EFECTIF/CONTRAT", typeof(int));

        //    DateTime startDate = dateEdit1.DateTime;
        //    DateTime endDate = dateEdit2.DateTime;

        //    if (startDate > endDate)
        //    {
        //        MessageBox.Show("La date de début doit être antérieure ou égale à la date de fin.");
        //        return table;
        //    }

        //    TimeSpan dateRange = endDate - startDate;
        //    int totalDays = dateRange.Days + 1;

        //    // إضافة أعمدة التواريخ
        //    for (int i = 0; i < totalDays; i++)
        //    {
        //        DateTime currentDay = startDate.AddDays(i);
        //        table.Columns.Add($"{currentDay.Day}", typeof(int));
        //    }

        //    table.Columns.Add("Total Absences", typeof(int)); // عدد الغيابات الإجمالي
        //    table.Columns.Add("M_Penalite", typeof(float)); // إضافة عمود الغرامة
        //    table.Columns.Add("Total Penalty", typeof(float)); // إضافة عمود قيمة الغرامة الإجمالية

        //    var context = new DAL.DataClasses1DataContext();
        //    var groups = FicheAgentList.Where(x => x.Statut == true)
        //        .GroupBy(agent => agent.ID_Post)
        //        .Select(g => new
        //        {
        //            Specialization = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
        //            RequiredQuantity = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
        //            Penalty = context.Fiche_Postes.FirstOrDefault(sp => sp.ID == g.Key)?.M_Penalite ?? 0, // الغرامة لكل قسم
        //            Agents = g.ToList()
        //        });

        //    foreach (var group in groups)
        //    {
        //        DataRow specializationRow = table.NewRow();
        //        specializationRow["POSTE"] = group.Specialization;
        //        specializationRow["EFECTIF/CONTRAT"] = group.RequiredQuantity;

        //        int[] absentCountPerDay = new int[totalDays];
        //        int totalAbsent = 0;

        //        foreach (var agent in group.Agents)
        //        {
        //            // جلب الحركات من جدول MVMAgentDetails
        //            var agentMovements = context.MVMAgentDetails
        //                .Where(m => m.ItemID == agent.ID)
        //                .OrderBy(m => m.Date)
        //                .ToList();

        //            string currentStatus = "-"; // الحالة الافتراضية

        //            for (int i = 0; i < totalDays; i++)
        //            {
        //                DateTime currentDay = startDate.AddDays(i);

        //                if (currentDay > DateTime.Now)
        //                {
        //                    break; // إذا تجاوز اليوم تاريخ اليوم الحالي، توقف
        //                }

        //                var movement = agentMovements.FirstOrDefault(m => m.Date.Date == currentDay.Date);

        //                if (movement != null)
        //                {
        //                    // إذا كانت هناك حركة في اليوم الحالي
        //                    currentStatus = movement.Statut;
        //                }

        //                if (currentStatus == "A")
        //                {
        //                    absentCountPerDay[i]++;
        //                    totalAbsent++;
        //                }
        //            }
        //        }

        //        // ملء بيانات الغيابات لكل يوم في الصف
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            specializationRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i];
        //        }

        //        specializationRow["Total Absences"] = totalAbsent;
        //        specializationRow["M_Penalite"] = group.Penalty;
        //        specializationRow["Total Penalty"] = totalAbsent * group.Penalty; // حساب الغرامة الإجمالية
        //        table.Rows.Add(specializationRow);
        //    }

        //    // إضافة سطر المجموع
        //    DataRow totalRow = table.NewRow();
        //    totalRow["POSTE"] = "Total";

        //    int[] dailyTotals = new int[totalDays];
        //    int totalAbsencesSum = 0;
        //    float totalPenaltySum = 0;

        //    foreach (DataRow row in table.Rows)
        //    {
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            dailyTotals[i] += Convert.ToInt32(row[$"{startDate.AddDays(i).Day}"]);
        //        }
        //        totalAbsencesSum += Convert.ToInt32(row["Total Absences"]);
        //        totalPenaltySum += Convert.ToSingle(row["Total Penalty"]);
        //    }

        //    for (int i = 0; i < totalDays; i++)
        //    {
        //        totalRow[$"{startDate.AddDays(i).Day}"] = dailyTotals[i];
        //    }

        //    totalRow["Total Absences"] = totalAbsencesSum;
        //    totalRow["Total Penalty"] = totalPenaltySum;
        //    table.Rows.Add(totalRow);

        //    return table;
        //}
        #endregion
        #endregion

        private void btn_mensuel_Click(object sender, EventArgs e)
        {
            LoadData();
            gridControl1.DataSource = CreateDataTableMensuel();

            gridView1.PopulateColumns();
            gridView1.BestFitColumns();
        }

        private void btn_print_mensuel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
            PrintReportMensuel();
        }
        private void PrintReportMensuel()
        {

            report.rpt_pointage_mensuel report = new report.rpt_pointage_mensuel();

            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            report.AddDateColumnsToReport(report, startDate, endDate);
            DataTable table = CreateDataTableMensuel();
            report.DataSource = table;

            report.BindAttendanceDataToCells(table);

            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }
    }
}