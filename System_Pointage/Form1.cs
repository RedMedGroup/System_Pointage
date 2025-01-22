using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System_Pointage.DAL;
using System_Pointage.report;

namespace System_Pointage
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        List<DAL.Fiche_Agent> FicheAgentList;
        List<DAL.Fiche_Poste> FichePOSTE;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btn_recharch_Click(object sender, EventArgs e)
        {
            LoadData();

            gridControl1.DataSource = CreateDataTable();

            gridView1.PopulateColumns();
            gridView1.BestFitColumns();
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

            var context = new DAL.DataClasses1DataContext();
            var groups = FicheAgentList
                .GroupBy(agent => agent.ID_Post)
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



                //foreach (var agent in group.Agents)
                //{
                //    DataRow row = table.NewRow();
                //    row["Name"] = agent.Name;

                //    // جلب الحركات من جدول MVMAgentDetails
                //    var agentMovements = context.MVMAgentDetails
                //        .Where(m => m.ItemID == agent.ID)
                //        .OrderBy(m => m.Date)
                //        .ToList();

                //    string currentStatus = "P"; // الحالة الافتراضية
                //    DateTime? lastDate = startDate;

                //    if (!agentMovements.Any())
                //    {
                //        // في حالة عدم وجود أي حركات، يتم تعيين جميع الأيام كـ "P" وزيادة عدد أيام الحضور
                //        for (int i = 0; i < totalDays; i++)
                //        {
                //            DateTime currentDay = startDate.AddDays(i);
                //            if (currentDay <= DateTime.Now)
                //            {
                //                row[$"{currentDay.Day}"] = " ";
                //                //presentCountPerDay[i]++;
                //                //totalPresent++;
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // إذا كانت هناك حركات مسجلة، يتم تحديث الحالات بناءً على آخر حركة
                //        for (int i = 0; i < totalDays; i++)
                //        {
                //            DateTime currentDay = startDate.AddDays(i);
                //            var movement = agentMovements.FirstOrDefault(m => m.Date.Date == currentDay.Date);

                //            if (movement != null)
                //            {
                //                currentStatus = movement.Statut;
                //                lastDate = movement.Date;
                //            }

                //            if (currentDay <= DateTime.Now)
                //            {
                //                row[$"{currentDay.Day}"] = currentStatus;
                //                if (currentStatus == "P")
                //                {
                //                    presentCountPerDay[i]++;
                //                    totalPresent++;
                //                }
                //                else if (currentStatus == "A")
                //                {
                //                    absentCountPerDay[i]++;
                //                    totalAbsent++;
                //                }
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }
                //    }

                //    table.Rows.Add(row);
                //}

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

            using (var context = new DAL.DataClasses1DataContext())
            {
                // استرجاع الأقسام من جدول Fiche_DePosts
                var departments = context.Fiche_Postes.Select(post => new
                {
                    post.ID,
                    DepartmentName = post.Name,
                    M_Penalite = post.M_Penalite
                }).ToList();

                // حساب الغيابات والمعلومات المطلوبة لكل قسم خلال الفترة المحددة
                foreach (var department in departments)
                {
                    // استرجاع جميع السجلات للعاملين في هذا القسم خلال الفترة المحددة
                    var agentAbsences = context.MVMAgentDetails
                        .Where(x => x.Date >= startDate && x.Date <= endDate)
                        .Join(context.Fiche_Agents,
                              agent => agent.ItemID,
                              worker => worker.ID,
                              (agent, worker) => new { agent, worker })
                        .Where(x => x.worker.ID_Post == department.ID)
                        .OrderBy(x => x.agent.Date)
                        .ToList();

                    // إنشاء مجموعة لتخزين الغيابات لكل موظف
                    var absenceRecordsByAgent = new Dictionary<int, List<(DateTime Date, string Status)>>();
                    var uniqueAbsentPersonnelSet = new HashSet<int>(); // مجموعة لتخزين IDs فريدة للموظفين الغائبين

                    // ملء قائمة حالات الموظفين
                    foreach (var record in agentAbsences)
                    {
                        if (!absenceRecordsByAgent.ContainsKey(record.worker.ID))
                        {
                            absenceRecordsByAgent[record.worker.ID] = new List<(DateTime, string)>();
                        }
                        absenceRecordsByAgent[record.worker.ID].Add((record.agent.Date, record.agent.Statut));
                    }

                    // حساب الغيابات لكل موظف
                    int totalAbsences = 0;

                    foreach (var agent in absenceRecordsByAgent)
                    {
                        bool hasAbsence = false;
                        var absenceRecords = agent.Value;

                        for (int i = 0; i < absenceRecords.Count; i++)
                        {
                            if (absenceRecords[i].Status == "A")
                            {
                                hasAbsence = true;
                                uniqueAbsentPersonnelSet.Add(agent.Key); // إضافة ID الموظف إلى المجموعة

                                int absenceStartIndex = i;

                                // البحث عن أول يوم حضور بعد يوم الغياب
                                for (int j = i + 1; j < absenceRecords.Count; j++)
                                {
                                    if (absenceRecords[j].Status == "P")
                                    {
                                        // حساب عدد أيام الغياب مع استبعاد يوم العودة
                                        int absenceCount = (absenceRecords[j].Date - absenceRecords[absenceStartIndex].Date).Days;
                                        totalAbsences += absenceCount; // إضافة إلى الإجمالي
                                        i = j; // تحديث الفهرس للانتقال إلى سجل الحضور
                                        break; // الخروج من الحلقة الداخلية
                                    }
                                }

                                // إذا لم يتم العثور على يوم حضور، يتم حساب الغيابات حتى اليوم الحالي
                                if (i == absenceStartIndex) // يعني أنه لم نجد أي يوم حضور
                                {
                                    // حساب الغيابات حتى نهاية الفترة المحددة
                                    if (absenceRecords[absenceStartIndex].Date <= endDate)
                                    {
                                        // إضافة 1 لأننا نريد حساب يوم الغياب نفسه
                                        int absenceCount = (endDate - absenceRecords[absenceStartIndex].Date).Days + 1;
                                        totalAbsences += absenceCount; // إضافة إلى الإجمالي
                                    }
                                    break; // الخروج من الحلقة لأن الفترة انتهت
                                }
                            }
                        }
                    }

                    // إذا كان هناك غيابات في القسم، أضف الصف إلى التقرير
                    if (totalAbsences > 0)
                    {
                        // حساب الغرامات الكلية
                        float totalPenalties = (float)(totalAbsences * department.M_Penalite);

                        // إضافة صف جديد إلى الجدول
                        DataRow row = table.NewRow();
                        row["Department"] = department.DepartmentName;
                        row["TotalAbsences"] = totalAbsences; // إجمالي الغيابات
                        row["M_Penalite"] = department.M_Penalite;
                        row["TotalPenalties"] = totalPenalties;
                        row["Nombre du personnel absent"] = uniqueAbsentPersonnelSet.Count; // عدد الموظفين الفريدين الغائبين

                        table.Rows.Add(row);
                    }
                }
            }
            return table;
        }
       

        private void btn_print_pinalite_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
    }
}
