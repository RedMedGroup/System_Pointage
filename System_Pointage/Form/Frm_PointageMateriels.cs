using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.DAL;
using System_Pointage.report;

namespace System_Pointage.Form
{
    public partial class Frm_PointageMateriels : DevExpress.XtraEditors.XtraForm
    {
        List<DAL.Fiche_Matricule> FicheMatreculeList;
        List<DAL.Fiche_materiel> FicheMateriels;
        List<DAL.MVMmaterielsDetail> listOfDays;
        public Frm_PointageMateriels()
        {
            InitializeComponent();
        }

        private void btn_recharch_Click(object sender, EventArgs e)
        {
            LoadData();
            gridControl1.DataSource = CreateDataTable();

            gridView1.PopulateColumns();
            gridView1.BestFitColumns();
        }

        private void Frm_PointageMateriels_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            dateEdit1.DateTime = new DateTime(today.Year, today.Month, 1);
            dateEdit2.DateTime = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            gridView1.OptionsView.AllowCellMerge = true;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
            gridView1.CellMerge += GridView1_CellMerge;
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "materiels")
            {
                if (string.IsNullOrEmpty(e.CellValue as string))
                {
                    e.Appearance.BackColor = Color.Orange; // لون الخلفية
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold); // جعل النص عريضًا
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center; // محاذاة النص إلى المنتصف
                    e.Appearance.TextOptions.VAlignment = VertAlignment.Center; // محاذاة رأسية
                    e.Appearance.TextOptions.WordWrap = WordWrap.Wrap; // السماح بتغليف النصوص

                    // التأكد من ضبط ارتفاع الصف تلقائياً
                    view.OptionsView.RowAutoHeight = true;
                }
            }
            if (gridView1.Columns["materiels"] != null)
            {

                gridView1.Columns["materiels"].Width = 300;
                //gridView1.OptionsView.RowAutoHeight = true;
                gridView1.OptionsView.ColumnAutoWidth = false; gridView1.Columns["materiels"].AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
                gridView1.Columns["materiels"].ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            }
        }

        private void GridView1_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Column.FieldName == "materiels" || e.Column.FieldName == "EFECTIF/CONTRAT")
            {
                string value1 = view.GetRowCellDisplayText(e.RowHandle1, e.Column);
                string value2 = view.GetRowCellDisplayText(e.RowHandle2, e.Column);

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

        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "materiels")
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
            if (e.Column.FieldName == "Matricule")
            {
                string cellValue = e.CellValue?.ToString();
                if (cellValue == "P/Total" || cellValue == "A/Total")
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#00897B");
                }
            }
            if (e.Column.FieldName != "materiels" && e.Column.FieldName != "Matricule")
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
            table.Columns.Add("Désignation", typeof(string)); // التخصص (Name من Fiche_materiels)
            table.Columns.Add("EFECTIF/CONTRAT", typeof(int)); // عدد العقود (Nembre_Contra)
            table.Columns.Add("Matricule", typeof(string)); // رقم السيارة (Matricule)

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
                table.Columns.Add($"{currentDay.Day}/{currentDay.Month}", typeof(string)); // تنسيق التاريخ (يوم/شهر)
            }

            table.Columns.Add("Total", typeof(int)); // العمود الإجمالي

            var context = new DAL.DataClasses1DataContext();

            // الحصول على قائمة السيارات (Matricule) من FicheMatreculeList
            var vehicles = FicheMatreculeList.Where(x => x.Statut == true).ToList();

            // الحصول على التخصصات من Fiche_materiels
            var specializations = context.Fiche_materiels.ToList();

            // لكل سيارة، ننشئ صفًا في الجدول
            foreach (var vehicle in vehicles)
            {
                // الحصول على حركات السيارة من MVMmaterielsDetails
                var vehicleMovements = context.MVMmaterielsDetails
                    .Where(m => m.ItemID == vehicle.ID)
                    .OrderBy(m => m.Date)
                    .ToList();

                // الحصول على التخصصات التي عملت فيها السيارة
                var vehicleSpecializations = vehicleMovements
                    .Select(m => m.Specialization) // نفترض أن هناك حقل SpecializationID في MVMmaterielsDetails
                    .Distinct()
                    .ToList();

                foreach (var specializationId in vehicleSpecializations)
                {
                    // الحصول على معلومات التخصص من Fiche_materiels
                    var specialization = specializations.FirstOrDefault(sp => sp.ID == specializationId);
                    if (specialization == null) continue;

                    // إنشاء صف للتخصص
                    DataRow specializationRow = table.NewRow();
                    specializationRow["Désignation"] = specialization.Name; // اسم التخصص
                    specializationRow["EFECTIF/CONTRAT"] = specialization.Nembre_Contra; // عدد العقود
                    specializationRow["Matricule"] = vehicle.Matricule; // رقم السيارة

                    int totalPresent = 0;
                    int totalAbsent = 0;

                    string lastStatus = "-"; // الحالة الأخيرة المسجلة
                    DateTime lastMovementDate = startDate;

                    // ملء الحالات لكل يوم
                    for (int i = 0; i < totalDays; i++)
                    {
                        DateTime currentDay = startDate.AddDays(i);

                        if (currentDay > DateTime.Now)
                        {
                            // إذا كان اليوم الحالي يتجاوز تاريخ اليوم، توقف عن التكرار
                            break;
                        }

                        // الحصول على الحركة لهذا اليوم
                        var movement = vehicleMovements
                            .FirstOrDefault(m => m.Date.Date == currentDay.Date && m.Specialization == specializationId);

                        if (movement != null)
                        {
                            lastStatus = movement.Statut; // تحديث الحالة الأخيرة
                            lastMovementDate = currentDay;
                        }

                        // تعيين الحالة للعمود المناسب
                        specializationRow[$"{currentDay.Day}/{currentDay.Month}"] = lastStatus;

                        // حساب الحضور والغياب
                        if (lastStatus == "P" && currentDay <= DateTime.Now)
                        {
                            totalPresent++;
                        }
                        else if (lastStatus == "A" && currentDay <= DateTime.Now)
                        {
                            totalAbsent++;
                        }
                    }

                    specializationRow["Total"] = totalPresent; // إجمالي الحضور
                    table.Rows.Add(specializationRow);
                }
            }

            return table;
        }
        #region
        //private DataTable CreateDataTable()
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("materiels", typeof(string));
        //    table.Columns.Add("EFECTIF/CONTRAT", typeof(int));
        //    table.Columns.Add("Matricule", typeof(string));

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
        //        table.Columns.Add($"{currentDay.Day}", typeof(string));
        //    }

        //    table.Columns.Add("Total", typeof(int));

        //    var context = new DAL.DataClasses1DataContext();
        //    var groups = FicheMatreculeList.Where(x => x.Statut == true)
        //        .GroupBy(agent => agent.ID_materiels)
        //        .Select(g => new
        //        {
        //            Specialization = context.Fiche_materiels.FirstOrDefault(sp => sp.ID == g.Key)?.Name,
        //            RequiredQuantity = context.Fiche_materiels.FirstOrDefault(sp => sp.ID == g.Key)?.Nembre_Contra ?? 0,
        //            Agents = g.ToList()
        //        });

        //    foreach (var group in groups)
        //    {
        //        DataRow specializationRow = table.NewRow();
        //        specializationRow["materiels"] = group.Specialization;
        //        specializationRow["EFECTIF/CONTRAT"] = group.RequiredQuantity;
        //        table.Rows.Add(specializationRow);

        //        int[] presentCountPerDay = new int[totalDays];
        //        int[] absentCountPerDay = new int[totalDays];
        //        int totalPresent = 0;
        //        int totalAbsent = 0;

        //        foreach (var agent in group.Agents)
        //        {
        //            DataRow row = table.NewRow();
        //            row["Matricule"] = agent.Matricule;

        //            // جلب الحركات من جدول MVMAgentDetails
        //            var agentMovements = context.MVMmaterielsDetails
        //                .Where(m => m.ItemID == agent.ID)
        //                .OrderBy(m => m.Date)
        //                .ToList();

        //            string currentStatus = "-"; // الحالة الافتراضية
        //            DateTime? lastMovementDate = null;

        //            // تحديث الحالة بناءً على الحركات المسجلة
        //            foreach (var movement in agentMovements)
        //            {
        //                if (movement.Date <= startDate)
        //                {
        //                    currentStatus = movement.Statut;
        //                    lastMovementDate = movement.Date;
        //                }
        //            }

        //            for (int i = 0; i < totalDays; i++)
        //            {
        //                DateTime currentDay = startDate.AddDays(i);

        //                if (currentDay > DateTime.Now)
        //                {
        //                    // إذا كان اليوم الحالي يتجاوز تاريخ اليوم، توقف عن التكرار
        //                    break;
        //                }

        //                var movement = agentMovements.FirstOrDefault(m => m.Date.Date == currentDay.Date);

        //                if (movement != null)
        //                {
        //                    // إذا كانت هناك حركة في اليوم الحالي
        //                    currentStatus = movement.Statut;
        //                    lastMovementDate = movement.Date;
        //                }

        //                // استمرار الحالة الأخيرة إذا لم تكن هناك حركة جديدة
        //                row[$"{currentDay.Day}"] = currentStatus;

        //                if (currentDay <= DateTime.Now)
        //                {
        //                    if (currentStatus == "P")
        //                    {
        //                        presentCountPerDay[i]++;
        //                        totalPresent++;
        //                    }
        //                    else if (currentStatus == "A")
        //                    {
        //                        absentCountPerDay[i]++;
        //                        totalAbsent++;
        //                    }
        //                }
        //            }

        //            table.Rows.Add(row);
        //        }

        //        // إضافة صف لحساب الحضور "P/Total" لكل قسم
        //        DataRow presentCountRow = table.NewRow();
        //        presentCountRow["Matricule"] = "P/Total";
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            presentCountRow[$"{startDate.AddDays(i).Day}"] = presentCountPerDay[i].ToString();
        //        }
        //        presentCountRow["Total"] = totalPresent;
        //        table.Rows.Add(presentCountRow);

        //        // إضافة صف لحساب الغياب "A/Total" لكل قسم
        //        DataRow absentCountRow = table.NewRow();
        //        absentCountRow["Matricule"] = "A/Total";
        //        for (int i = 0; i < totalDays; i++)
        //        {
        //            absentCountRow[$"{startDate.AddDays(i).Day}"] = absentCountPerDay[i].ToString();
        //        }
        //        absentCountRow["Total"] = totalAbsent;
        //        table.Rows.Add(absentCountRow);
        //    }
        //    return table;
        //}// recharch
        #endregion
        private void LoadData()
        {
            using (var context = new DAL.DataClasses1DataContext())
            {
                listOfDays = context.MVMmaterielsDetails.ToList();
                // تحميل قائمة الوكلاء من جدول Fich_Agents
                FicheMatreculeList = context.Fiche_Matricules

                    .ToList();

                // تحميل قائمة التخصصات من جدول Fiche_DePosts
                FicheMateriels = context.Fiche_materiels.ToList();
            }
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

        private void btn_print_pinalite_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        private void GenerateReport() // pénalité
        {
            DateTime startDate = dateEdit1.DateTime;
            DateTime endDate = dateEdit2.DateTime;
            int totalDays = (endDate - startDate).Days + 1;

            rpt_penalite_Materiel report = new rpt_penalite_Materiel();
            report.DataSource = CreateAbsenceReport(totalDays, startDate, endDate); // تحديث هنا لاستدعاء الدالة الجديدة
            report.ShowPreview();
        }
        #region date of report
        public DateTime StartDate { get; set; }
        private void SetStartDate()
        {
            StartDate = dateEdit1.DateTime;
        }
        #endregion
        private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();

            table.Columns.Add("camion", typeof(string));
            table.Columns.Add("TotalAbsences", typeof(int));
            table.Columns.Add("M_Penalite", typeof(float));
            table.Columns.Add("TotalPenalties", typeof(float));
            table.Columns.Add("Nombre du personnel absent", typeof(int));

            using (var context = new DAL.DataClasses1DataContext())
            {
                // الحصول على قائمة التخصصات من Fiche_materiels
                var specializations = context.Fiche_materiels.Select(post => new
                {
                    post.ID,
                    specializationName = post.Name,
                    M_Penalite = post.M_Penalite,
                    Nembre_Contra = post.Nembre_Contra
                }).ToList();

                // الحصول على قائمة السيارات من Fiche_Matricules
                var vehicles = context.Fiche_Matricules.Select(vehicle => new
                {
                    vehicle.ID,
                    vehicle.Matricule
                }).ToList();

                foreach (var vehicle in vehicles)
                {
                    // الحصول على حركات السيارة في كل تخصص من MVMmaterielsDetails
                    var vehicleMovements = context.MVMmaterielsDetails
                        .Where(x => x.Date >= startDate && x.Date <= endDate && x.ItemID == vehicle.ID)
                        .OrderBy(x => x.Date)
                        .ToList();

                    // الحصول على التخصصات التي عملت فيها السيارة
                    var vehicleSpecializations = vehicleMovements
                        .Select(m => m.Specialization) // نفترض أن هناك حقل ID_Attent_Liste في MVMmaterielsDetails
                        .Distinct()
                        .ToList();

                    foreach (var specializationId in vehicleSpecializations)
                    {
                        // الحصول على معلومات التخصص من Fiche_materiels
                        var specialization = specializations.FirstOrDefault(sp => sp.ID == specializationId);
                        if (specialization == null) continue;

                        // إنشاء مجموعة لتخزين فترات الحضور لكل موظف
                        var agentPresencePeriods = new Dictionary<int, List<(DateTime Start, DateTime End)>>();

                        // الحصول على حركات السيارة في هذا التخصص
                        var specializationMovements = vehicleMovements
                            .Where(m => m.Specialization == specializationId)
                            .ToList();

                        // ملء قائمة فترات الحضور لكل موظف
                        foreach (var movement in specializationMovements)
                        {
                            if (!agentPresencePeriods.ContainsKey(movement.ItemID))
                            {
                                agentPresencePeriods[movement.ItemID] = new List<(DateTime, DateTime)>();
                            }

                            if (movement.Statut == "P")
                            {
                                // إذا كانت الحالة "P"، نضيف الفترة من تاريخ الحركة حتى تاريخ اليوم
                                DateTime start = movement.Date;
                                DateTime end = endDate;

                                // البحث عن أول حركة تغير الحالة بعد هذه الحركة
                                var nextMovement = specializationMovements
                                    .FirstOrDefault(m => m.Date > movement.Date && m.Statut != "P");

                                if (nextMovement != null)
                                {
                                    end = nextMovement.Date.AddDays(-1); // اليوم الذي يسبق تغيير الحالة
                                }

                                agentPresencePeriods[movement.ItemID].Add((start, end));
                            }
                        }

                        // حساب الغيابات اليومية
                        int totalAbsences = 0;

                        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                        {
                            int dailyPresenceCount = 0;

                            // حساب عدد الحضور في اليوم
                            foreach (var periods in agentPresencePeriods.Values)
                            {
                                if (periods.Any(p => date >= p.Start && date <= p.End))
                                {
                                    dailyPresenceCount++;
                                }
                            }

                            // حساب الغيابات اليومية بناءً على القاعدة
                            int dailyAbsences = Math.Max(0, (int)(specialization.Nembre_Contra) - dailyPresenceCount);
                            totalAbsences += dailyAbsences;
                        }

                        // إذا كان هناك غيابات في السيارة في هذا التخصص، أضف الصف إلى التقرير
                        if (totalAbsences > 0)
                        {
                            // حساب الغرامات الكلية
                            float totalPenalties = (float)(totalAbsences * specialization.M_Penalite);

                            // إضافة صف جديد إلى الجدول
                            DataRow row = table.NewRow();
                            row["camion"] = $"{specialization.specializationName} - {vehicle.Matricule}"; // اسم التخصص ورقم السيارة
                            row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
                            row["M_Penalite"] = specialization.M_Penalite;
                            row["TotalPenalties"] = totalPenalties;
                            row["Nombre du personnel absent"] = (int)specialization.Nembre_Contra;  // عدد الموظفين الفريدين

                            table.Rows.Add(row);
                        }
                    }
                }
            }
            return table;
        }
        //private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    table.Columns.Add("camion", typeof(string));
        //    table.Columns.Add("TotalAbsences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("TotalPenalties", typeof(float));
        //    table.Columns.Add("Nombre du personnel absent", typeof(int));

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // الحصول على قائمة التخصصات من Fiche_materiels
        //        var specializations = context.Fiche_materiels.Select(post => new
        //        {
        //            post.ID,
        //            specializationName = post.Name,
        //            M_Penalite = post.M_Penalite
        //        }).ToList();

        //        // الحصول على قائمة السيارات من Fiche_Matricules
        //        var vehicles = context.Fiche_Matricules.Select(vehicle => new
        //        {
        //            vehicle.ID,
        //            vehicle.Matricule
        //        }).ToList();

        //        foreach (var vehicle in vehicles)
        //        {
        //            // الحصول على حركات السيارة في كل تخصص من MVMmaterielsDetails
        //            var vehicleMovements = context.MVMmaterielsDetails
        //                .Where(x => x.Date >= startDate && x.Date <= endDate && x.ItemID == vehicle.ID)
        //                .OrderBy(x => x.Date)
        //                .ToList();

        //            // الحصول على التخصصات التي عملت فيها السيارة
        //            var vehicleSpecializations = vehicleMovements
        //                .Select(m => m.ID_Attent_Liste) // نفترض أن هناك حقل ID_Attent_Liste في MVMmaterielsDetails
        //                .Distinct()
        //                .ToList();

        //            foreach (var specializationId in vehicleSpecializations)
        //            {
        //                // الحصول على معلومات التخصص من Fiche_materiels
        //                var specialization = specializations.FirstOrDefault(sp => sp.ID == specializationId);
        //                if (specialization == null) continue;

        //                // إنشاء مجموعة لتخزين الغيابات لكل موظف
        //                var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
        //                var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

        //                // الحصول على حركات السيارة في هذا التخصص
        //                var specializationMovements = vehicleMovements
        //                    .Where(m => m.ID_Attent_Liste == specializationId)
        //                    .ToList();

        //                // حساب الغيابات لكل موظف
        //                int totalAbsences = 0;

        //                foreach (var movement in specializationMovements)
        //                {
        //                    if (movement.Statut == "A")
        //                    {
        //                        uniqueAbsentPersonnelSet.Add(movement.ItemID); // إضافة ID الموظف إلى المجموعة

        //                        DateTime absenceStart = movement.Date;

        //                        // البحث عن أول تاريخ يتغير فيه الحالة
        //                        DateTime absenceEnd = endDate; // افتراضياً نهاية الغياب هي نهاية الفترة
        //                        var nextMovement = specializationMovements
        //                            .FirstOrDefault(m => m.Date > movement.Date && m.Statut != "A");

        //                        if (nextMovement != null)
        //                        {
        //                            absenceEnd = nextMovement.Date.AddDays(-1); // اليوم الذي يسبق حالة الحضور
        //                        }

        //                        // التأكد من أن التواريخ تقع ضمن النطاق المحدد
        //                        absenceStart = (absenceStart < startDate) ? startDate : absenceStart;
        //                        absenceEnd = (absenceEnd > endDate) ? endDate : absenceEnd;

        //                        // حساب عدد أيام الغياب ضمن الفترة
        //                        int absenceCount = (absenceEnd - absenceStart).Days + 1;
        //                        totalAbsences += absenceCount; // إضافة إلى الإجمالي
        //                    }
        //                }

        //                // إذا كان هناك غيابات في السيارة في هذا التخصص، أضف الصف إلى التقرير
        //                if (totalAbsences > 0)
        //                {
        //                    // حساب الغرامات الكلية
        //                    float totalPenalties = (float)(totalAbsences * specialization.M_Penalite);

        //                    // إضافة صف جديد إلى الجدول
        //                    DataRow row = table.NewRow();
        //                    row["camion"] = $"{specialization.specializationName} - {vehicle.Matricule}"; // اسم التخصص ورقم السيارة
        //                    row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
        //                    row["M_Penalite"] = specialization.M_Penalite;
        //                    row["TotalPenalties"] = totalPenalties;
        //                    row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

        //                    table.Rows.Add(row);
        //                }
        //            }
        //        }
        //    }
        //    return table;
        //}
        #region
        //private DataTable CreateAbsenceReport(int totalDays, DateTime startDate, DateTime endDate)
        //{
        //    DataTable table = new DataTable();

        //    table.Columns.Add("camion", typeof(string));
        //    table.Columns.Add("TotalAbsences", typeof(int));
        //    table.Columns.Add("M_Penalite", typeof(float));
        //    table.Columns.Add("TotalPenalties", typeof(float));
        //    table.Columns.Add("Nombre du personnel absent", typeof(int));

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        var camions = context.Fiche_materiels.Select(post => new
        //        {
        //            post.ID,
        //            camionName = post.Name,
        //            M_Penalite = post.M_Penalite
        //        }).ToList();

        //        foreach (var camion in camions)
        //        {
        //            var agentAbsences = context.MVMmaterielsDetails
        //                .Where(x => x.Date >= startDate && x.Date <= endDate)
        //                .Join(context.Fiche_Matricules,
        //                      agent => agent.ItemID,
        //                      worker => worker.ID,
        //                      (agent, worker) => new { agent, worker })
        //                .Where(x => x.worker.ID_materiels == camion.ID)
        //                .OrderBy(x => x.agent.Date)
        //                .ToList();

        //            // إنشاء مجموعة لتخزين الغيابات لكل 
        //            var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
        //            var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

        //            // ملء قائمة حالات 
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
        //                float totalPenalties = (float)(totalAbsences * camion.M_Penalite);

        //                // إضافة صف جديد إلى الجدول
        //                DataRow row = table.NewRow();
        //                row["camion"] = camion.camionName;
        //                row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
        //                row["M_Penalite"] = camion.M_Penalite;
        //                row["TotalPenalties"] = totalPenalties;
        //                row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

        //                table.Rows.Add(row);
        //            }
        //        }
        //    }
        //    return table;
        //}
        #endregion
        private void btn_reportAparJour_Click(object sender, EventArgs e)
        {
            GenerateDailyReport();
        }
        private void GenerateDailyReport()
        {
            SetStartDate();
            DateTime reportDate = dateEdit1.DateTime;
            rpt_EtatJournal report = new rpt_EtatJournal(this);
            report.DataSource = CreateDailyReport(reportDate);
            report.ShowPreview();
        }
        private DataTable CreateDailyReport(DateTime reportDate)
        {
            DataTable table = new DataTable();

            // إضافة الأعمدة
            table.Columns.Add("camion", typeof(string));
            table.Columns.Add("RequiredEmployees", typeof(int));
            table.Columns.Add("WorkerName", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("PresentCount", typeof(int));
            table.Columns.Add("cell_Ecart", typeof(int));

            using (var context = new DAL.DataClasses1DataContext())
            {
                // استرجاع الأقسام (التخصصات)
                var specializations = context.Fiche_materiels
                    .Select(post => new
                    {
                        ID_Post = post.ID,
                        camionName = post.Name,
                        RequiredEmployees = post.Nembre_Contra
                    })
                    .ToList();

                // استرجاع جميع الحركات حتى نهاية الشهر إلى قائمة محلية
                var endOfMonth = new DateTime(reportDate.Year, reportDate.Month, DateTime.DaysInMonth(reportDate.Year, reportDate.Month));
                var vehicleMovements = context.MVMmaterielsDetails
                    .Where(detail => detail.Date <= endOfMonth)
                    .ToList(); // تحميل الحركات في الذاكرة

                // استرجاع جميع العمال
                var allWorkers = context.Fiche_Matricules
                    .Select(agent => new
                    {
                        WorkerID = agent.ID,
                        WorkerName = agent.Matricule
                    })
                    .ToList();

                // معالجة البيانات محليًا
                foreach (var specialization in specializations)
                {
                    // تصفية الحركات المرتبطة بهذا التخصص
                    var specializationMovements = vehicleMovements
                        .Where(x => x.Specialization == specialization.ID_Post)
                        .ToList();

                    var specializationWorkers = allWorkers
                        .Select(worker => new
                        {
                            WorkerID = worker.WorkerID,
                            WorkerName = worker.WorkerName,
                            LastMovement = specializationMovements
                                .Where(m => m.ItemID == worker.WorkerID && m.Date <= reportDate)
                                .OrderByDescending(m => m.Date)
                                .FirstOrDefault(),
                            NextMovement = specializationMovements
                                .Where(m => m.ItemID == worker.WorkerID && m.Date > reportDate)
                                .OrderBy(m => m.Date)
                                .FirstOrDefault()
                        })
                        .ToList();

                    // حساب عدد الحضور
                    int presentCount = 0;

                    foreach (var worker in specializationWorkers)
                    {
                        string status = "A"; // افتراض الغياب
                        if (worker.LastMovement != null && worker.LastMovement.Statut == "P")
                        {
                            // إذا لم يكن هناك حركة جديدة بعد التاريخ الحالي، يعتبر حاضرًا
                            if (worker.NextMovement == null || worker.NextMovement.Date > endOfMonth)
                            {
                                status = "P";
                                presentCount++;
                            }
                            else if (worker.NextMovement.Date > reportDate)
                            {
                                status = "P";
                                presentCount++;
                            }
                        }

                        DataRow row = table.NewRow();
                        row["camion"] = specialization.camionName;
                        row["RequiredEmployees"] = specialization.RequiredEmployees;
                        row["WorkerName"] = worker.WorkerName;
                        row["Status"] = status;
                        row["PresentCount"] = presentCount;
                        row["cell_Ecart"] = presentCount - specialization.RequiredEmployees;

                        table.Rows.Add(row);
                    }
                }
            }

            return table;
        }


        #region
        //private DataTable CreateDailyReport(DateTime reportDate)
        //{
        //    DataTable table = new DataTable();

        //    // إضافة الأعمدة
        //    table.Columns.Add("camion", typeof(string));
        //    table.Columns.Add("RequiredEmployees", typeof(int));
        //    table.Columns.Add("WorkerName", typeof(string));
        //    table.Columns.Add("Status", typeof(string));
        //    table.Columns.Add("PresentCount", typeof(int));
        //    table.Columns.Add("cell_Ecart", typeof(int));

        //    using (var context = new DAL.DataClasses1DataContext())
        //    {
        //        // استرجاع الأقسام
        //        var camions = context.Fiche_materiels
        //            .Select(post => new
        //            {
        //                ID_Post = post.ID,
        //                camionName = post.Name,
        //                RequiredEmployees = post.Nembre_Contra
        //            })
        //            .ToList();

        //        // استرجاع العمال وحالاتهم في التاريخ المحدد
        //        var workers = context.Fiche_Matricules
        //            .Select(agent => new
        //            {
        //                WorkerName = agent.Matricule,
        //                camionID = agent.ID_materiels,
        //                LastStatus = context.MVMmaterielsDetails
        //                    .Where(detail =>
        //                        detail.ItemID == agent.ID &&
        //                        detail.Date <= reportDate)
        //                    .OrderByDescending(detail => detail.Date)
        //                    .Select(detail => detail.Statut)
        //                    .FirstOrDefault(), // أخذ آخر حالة قبل أو في التاريخ المحدد
        //                NextAbsenceDate = context.MVMmaterielsDetails
        //                    .Where(detail =>
        //                        detail.ItemID == agent.ID &&
        //                        detail.Date > reportDate && // تحقق من وجود غياب بعد تاريخ التقرير
        //                        detail.Statut == "CR")
        //                    .OrderBy(detail => detail.Date)
        //                    .Select(detail => (DateTime?)detail.Date) // تحويل إلى نوع Nullable<DateTime>
        //                    .FirstOrDefault() // الحصول على أقرب تاريخ غياب بعد تاريخ التقرير
        //            })
        //            .Where(worker => worker.LastStatus != null && worker.LastStatus != "CR") // تجاهل العمال الذين لا حالة لهم
        //            .ToList();

        //        // إضافة البيانات إلى الجدول
        //        foreach (var camion in camions)
        //        {
        //            // استرجاع عمال القسم
        //            var camionWorkers = workers.Where(w => w.camionID == camion.ID_Post).ToList();

        //            // حساب عدد الحضور
        //            int presentCount = camionWorkers.Count(w => w.LastStatus == "P" && (w.NextAbsenceDate == null || w.NextAbsenceDate > reportDate));

        //            // حساب الفارق
        //            int cellEcart = (int)(presentCount - camion.RequiredEmployees);

        //            foreach (var worker in camionWorkers)
        //            {
        //                DataRow row = table.NewRow();
        //                row["camion"] = camion.camionName;
        //                row["RequiredEmployees"] = camion.RequiredEmployees;
        //                row["WorkerName"] = worker.WorkerName;

        //                // تحديد الحالة النهائية
        //                string status = worker.LastStatus ?? "A"; // افتراض الحالة "A" عند عدم وجود حالة
        //                if (worker.LastStatus == "P" && (worker.NextAbsenceDate == null || worker.NextAbsenceDate > reportDate))
        //                {
        //                    status = "P"; // العامل حاضر
        //                }
        //                else if (worker.LastStatus == "P" && worker.NextAbsenceDate <= reportDate)
        //                {
        //                    status = "A"; // العامل غائب لأن لديه غياب مسجل بعد تاريخ التقرير
        //                }

        //                row["Status"] = status;
        //                row["PresentCount"] = presentCount;
        //                row["cell_Ecart"] = cellEcart;

        //                table.Rows.Add(row);
        //            }
        //        }
        //    }

        //    return table;
        //}
        #endregion
    }
}