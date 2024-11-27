using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace System_Pointage.report
{
    public partial class rpt_DailyReport : DevExpress.XtraReports.UI.XtraReport
    {
        public rpt_DailyReport()
        {
            InitializeComponent();
            BindData();
            cell_Department.BeforePrint += Cell_Department_BeforePrint;
            cell_ncontra.BeforePrint += Cell_ncontra_BeforePrint;
            cell_n.BeforePrint += Cell_n_BeforePrint;
            Cell_PresentCount.BeforePrint += Cell_PresentCount_BeforePrint;
            cell_Ecart.BeforePrint += Cell_Ecart_BeforePrint;
            BeforePrint += Rpt_DailyReport_BeforePrint;
        }

        private void Rpt_DailyReport_BeforePrint(object sender, CancelEventArgs e)
        {
            lastCellByDepartment.Clear();
            lastValueByDepartment.Clear();
            lastCellByDepartment2.Clear();
            lastValueByDepartment2.Clear();
            lastCellByDepartment3.Clear();
            lastValueByDepartment3.Clear();
        }
        private Dictionary<string, XRTableCell> lastCellByDepartment = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment = new Dictionary<string, object>();
        private void Cell_Ecart_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell currentCell = (XRTableCell)sender;
            string currentDepartment = GetCurrentColumnValue("Department")?.ToString();
            string currentEcartValue = GetCurrentColumnValue("cell_Ecart")?.ToString();

            // تحقق إذا كانت القيم الحالية فارغة
            if (string.IsNullOrEmpty(currentDepartment) || string.IsNullOrEmpty(currentEcartValue))
            {
                return;
            }

            // تحقق مما إذا كانت هناك خلية سابقة بنفس القسم
            if (lastCellByDepartment.ContainsKey(currentDepartment) &&
                lastValueByDepartment[currentDepartment]?.ToString() == currentEcartValue)
            {
                XRTableCell lastCell = lastCellByDepartment[currentDepartment];
                // زيادة قيمة RowSpan للخلية السابقة
                lastCell.RowSpan = lastCell.RowSpan == 0 ? 2 : lastCell.RowSpan + 1;

                currentCell.Visible = false;
                lastCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            }
            else
            {
                // إذا كانت القيم مختلفة، تحديث القيم للخلية الحالية
                lastCellByDepartment[currentDepartment] = currentCell;
                lastValueByDepartment[currentDepartment] = currentEcartValue;

                // اجعل الخلية الحالية مرئية وأعد ضبط RowSpan
                currentCell.Visible = true;
                currentCell.RowSpan = 1;
            }
        }
        private Dictionary<string, XRTableCell> lastCellByDepartment3 = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment3 = new Dictionary<string, object>();
        private void Cell_PresentCount_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            cell.Text = cell.Text;
            //XRTableCell currentCell = (XRTableCell)sender;
            //string currentDepartment = GetCurrentColumnValue("Department")?.ToString();
            //string currentEcartValue = GetCurrentColumnValue("PresentCount")?.ToString();

            //// تحقق إذا كانت القيم الحالية فارغة
            //if (string.IsNullOrEmpty(currentDepartment) || string.IsNullOrEmpty(currentEcartValue))
            //{
            //    return;
            //}

            //// تحقق مما إذا كانت هناك خلية سابقة بنفس القسم
            //if (lastCellByDepartment3.ContainsKey(currentDepartment) &&
            //    lastValueByDepartment3[currentDepartment]?.ToString() == currentEcartValue)
            //{
            //    XRTableCell lastCell = lastCellByDepartment3[currentDepartment];

            //    // زيادة قيمة RowSpan للخلية السابقة
            //    lastCell.RowSpan = lastCell.RowSpan == 0 ? 2 : lastCell.RowSpan + 1;

            //    // تعيين الخلية الحالية كغير مرئية
            //    currentCell.Visible = false;
            //}
            //else
            //{
            //    // إذا كانت القيم مختلفة، تحديث القيم للخلية الحالية
            //    lastCellByDepartment3[currentDepartment] = currentCell;
            //    lastValueByDepartment3[currentDepartment] = currentEcartValue;

            //    // اجعل الخلية الحالية مرئية وأعد ضبط RowSpan
            //    currentCell.Visible = true;
            //    currentCell.RowSpan = 1;
            //}
        }
        private string currentDepartment = string.Empty;
        int index = 0;
        private void Cell_n_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell departmentCell = FindControl("cell_Department", true) as XRTableCell;
            if (departmentCell != null)
            {
                string department = departmentCell.Text;
                XRTableCell cell = (XRTableCell)sender;
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
                cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

                cell.Text = cell.Text;
                // تحقق مما إذا كان القسم الحالي يختلف عن القسم السابق
                if (currentDepartment != department)
                {
                    currentDepartment = department;
                    index++;
                }
            }

            cell_n.Text = (index).ToString();
        }
        private Dictionary<string, XRTableCell> lastCellByDepartment2 = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment2 = new Dictionary<string, object>();
        private void Cell_ncontra_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            // إعداد التوسيط
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // تعيين وضع الدمج
            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            // استرجاع القسم وعدد الموظفين المطلوبين
            var department = GetCurrentColumnValue("Department")?.ToString();
            var requiredEmployees = GetCurrentColumnValue("RequiredEmployees")?.ToString();

            if (!string.IsNullOrEmpty(department) && !string.IsNullOrEmpty(requiredEmployees))
            {
                // تخزين النصوص في متغيرات
                string cell1 = department; // القسم
                string cell2 = requiredEmployees; // عدد الموظفين المطلوبين

                // تكوين النص النهائي مع دمج cell1 و cell2
                cell.Text = $"{cell1}: {cell2}";

                // جعل النص الخاص بـ cell1 غير مرئي (مطابقًا للخلفية)
                if (!string.IsNullOrEmpty(cell1))
                {
                    // جعل النص الخاص بـ cell1 بنفس لون الخلفية لإخفائه
                    // إخفاء cell1 فقط
                    string hiddenCell1 = $"{cell1}: "; // نص الخلية 1 فقط بدون إظهارها
                    string visibleCell2 = cell2; // نص الخلية 2 فقط

                    // دمج النصين مع الحفاظ على cell1 مخفيًا
                    cell.Text = $"{hiddenCell1}{visibleCell2}";
                    cell.ForeColor = cell.BackColor; // تغيير اللون ليتطابق مع الخلفية (إخفاء cell1)
                }
            }



            //XRTableCell cell = (XRTableCell)sender;
            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            ////// تعيين وضع الدمج
            //cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            //cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;
            //// استرجاع القسم وعدد الموظفين المطلوبين
            //var department = GetCurrentColumnValue("Department")?.ToString();
            //var requiredEmployees = GetCurrentColumnValue("RequiredEmployees")?.ToString();

            //if (department != null && requiredEmployees != null)
            //{
            //    // تخزين القسم وعدد الموظفين المطلوبين في متغيرين
            //    string cell1 = department;
            //    string cell2 = requiredEmployees;

            //    // تكوين النص النهائي داخل الخلية
            //    cell.Text = $"{cell1}: {cell2}";

            //    // إخفاء النص الخاص بـ cell1 (القسم) عن طريق جعله شفافاً
            //    cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //    cell.ForeColor = Color.Black; // اللون العام للنص

            //    // لإخفاء النص الخاص بالقسم (cell1)، استخدم هذه الطريقة
            //    if (cell1 != null)
            //    {
            //        cell.Text = $"{cell1}: {cell2}"; // دمج النصوص
            //       cell1.ForeColor = Color.Transparent; // جعل النص الخاص بـ cell1 غير مرئي
            //    }
            //}

            ///////////////////////////////////////////////////////////////////////
            //XRTableCell cell = (XRTableCell)sender;

            //// إعدادات التوسيط
            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            //// تعيين وضع الدمج
            //cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            //cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            //// تحقق من وجود قسم (Department) للتأكد من أن الدمج يتم فقط ضمن المجموعة
            //XRTableRow row = cell.Parent as XRTableRow;
            //if (row != null)
            //{
            //    var department = GetCurrentColumnValue("Department"); // استرجاع اسم القسم الحالي
            //    if (department != null)
            //    {
            //        cell.Text = $" {cell.Text}"; // إضافة القسم إلى النص إذا أردت
            //    }
            //}
            ///////////////////////////////////////////////////////////////////////
            //XRTableCell cell = (XRTableCell)sender;

            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            //cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            //cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            //cell.Text = cell.Text;

        }

        private string GetPreviousValue(XRTableCell currentCell)
        {
            // هنا تحتاج إلى تنفيذ المنطق للحصول على القيمة السابقة بناءً على القسم
            // يمكنك استخدام السياق أو أي طريقة أخرى لتحديد القيمة السابقة
            return ""; // قم بإرجاع القيمة السابقة المناسبة هنا
        }
        private void Cell_Department_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            cell.Text = cell.Text;
        }

        public void BindData()
        {
            cell_Department.DataBindings.Add("Text", this.DataSource, "Department");
            cell_ncontra.DataBindings.Add("Text", this.DataSource, "RequiredEmployees");
            cell_M_Penalite.DataBindings.Add("Text", this.DataSource, "WorkerName");
            cell_TotalPenalties.DataBindings.Add("Text", this.DataSource, "Status");
            Cell_PresentCount.DataBindings.Add("Text", this.DataSource, "PresentCount");
            cell_Ecart.DataBindings.Add("Text", this.DataSource, "cell_Ecart");

            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"));
            lbl_date.Text = formattedDate;
        }

    }
}
