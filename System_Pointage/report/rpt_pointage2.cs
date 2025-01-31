using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace System_Pointage.report
{
    public partial class rpt_pointage2 : DevExpress.XtraReports.UI.XtraReport
    {
        public List<DateTime> ListOfDays { get; set; }

        public rpt_pointage2()
        {
            InitializeComponent();
            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"));
            //lbl_date.Text = formattedDate;
        }

        private TimeSpan dateRange;
        private int totalDays;
        private XRTable headerTable;
        private XRTableRow headerRow;
        private XRTable detailTable;
        private XRTableRow detailRow;
        private float availableWidth;
        private float columnWidth;

        public void AddDateColumnsToReport(XtraReport report, DateTime startDate, DateTime endDate)
        {
            dateRange = endDate - startDate;
            totalDays = dateRange.Days + 1;
            headerTable = (XRTable)FindControl("xrTableHeader", true);
            headerRow = headerTable.Rows[0];
            detailTable = (XRTable)FindControl("xrTableDetail", true);
            detailRow = detailTable.Rows[0];

            if (headerRow == null || detailRow == null)
            {
                throw new Exception("لم يتم العثور على جدول العناوين أو التفاصيل في التقرير.");
            }

            availableWidth = PageWidth - Margins.Left - Margins.Right;
            columnWidth = availableWidth / (totalDays + 3);
        }

        public void BindAttendanceDataToCells(DataTable table)
        {
            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                DataRow row = table.Rows[rowIndex];

                // ربط POSTE لكل فرد
                XRControl posteCell = FindControl("cell_sp", true);
                XRLabel lblBase = (XRLabel)FindControl("lbl_base", true);

                if (posteCell != null)
                {
                    posteCell.DataBindings.Add("Text", table, "POSTE"); 
                }

                // ربط البيانات للأيام
                for (int dayIndex = 1; dayIndex <= 31; dayIndex++)
                {
                    string cellName = $"cell_{dayIndex}";
                    XRControl cell = FindControl(cellName, true);
                    if (cell != null)
                    {
                        cell.DataBindings.Add("Text", table, $"{dayIndex}");

                        string cellValue = row[dayIndex].ToString();
                    }
                }

                // ربط البيانات لـ Total Présent
                XRControl totalPresentCell = FindControl("xrTableCell35", true);
                if (totalPresentCell != null)
                {
                    totalPresentCell.DataBindings.Add("Text", table, "S/TOTAL");
                    totalPresentCell.BackColor = Color.Transparent;
                }
            }
        }

        private void cell_sp_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            cell.Text = cell.Text;

            string nomValue = GetCurrentColumnValue("POSTE")?.ToString();

            if (!string.IsNullOrEmpty(nomValue) && nomValue == "S/TOTAL" || !string.IsNullOrEmpty(nomValue) && nomValue == "Ecart")
            {
                cell_sp.BackColor = Color.Gainsboro;
                cell_name.BackColor = Color.Gainsboro;
                cell_FirstName.BackColor = Color.Gainsboro;
                cell_effec.BackColor = Color.Gainsboro;
                xrTableCell35.BackColor = Color.Gainsboro;
                #region
                cell_1.BackColor = Color.Gainsboro;
                cell_2.BackColor = Color.LightGray;
                cell_3.BackColor = Color.Gainsboro;
                cell_4.BackColor = Color.Gainsboro;
                cell_5.BackColor = Color.Gainsboro;
                cell_6.BackColor = Color.Gainsboro;
                cell_7.BackColor = Color.Gainsboro;
                cell_8.BackColor = Color.Gainsboro;
                cell_9.BackColor = Color.Gainsboro;
                cell_10.BackColor = Color.Gainsboro;
                cell_11.BackColor = Color.Gainsboro;
                cell_12.BackColor = Color.Gainsboro;
                cell_13.BackColor = Color.Gainsboro;
                cell_14.BackColor = Color.Gainsboro;
                cell_15.BackColor = Color.Gainsboro;
                cell_16.BackColor = Color.Gainsboro;
                cell_17.BackColor = Color.Gainsboro;
                cell_18.BackColor = Color.Gainsboro;
                cell_19.BackColor = Color.Gainsboro;
                cell_20.BackColor = Color.Gainsboro;
                cell_21.BackColor = Color.Gainsboro;
                cell_22.BackColor = Color.Gainsboro;
                cell_23.BackColor = Color.Gainsboro;
                cell_24.BackColor = Color.Gainsboro;
                cell_25.BackColor = Color.Gainsboro;
                cell_26.BackColor = Color.Gainsboro;
                cell_27.BackColor = Color.Gainsboro;
                cell_28.BackColor = Color.Gainsboro;
                cell_29.BackColor = Color.Gainsboro;
                cell_30.BackColor = Color.Gainsboro;
                cell_31.BackColor = Color.Gainsboro;
                #endregion
                cell_sp.Font = new Font(cell_sp.Font, FontStyle.Bold);
                cell_name.Font = new Font(cell_name.Font, FontStyle.Bold);
                cell_FirstName.Font = new Font(cell_FirstName.Font, FontStyle.Bold);
            }
            else
            {
                cell_sp.BackColor = Color.White;
                cell_name.BackColor = Color.White;
                cell_FirstName.BackColor = Color.White;
                cell_effec.BackColor = Color.White;
                xrTableCell35.BackColor = Color.White;

                #region
                cell_1.BackColor = Color.White;
                cell_2.BackColor = Color.White;
                cell_3.BackColor = Color.White;
                cell_4.BackColor = Color.White;
                cell_5.BackColor = Color.White;
                cell_6.BackColor = Color.White;
                cell_7.BackColor = Color.White;
                cell_8.BackColor = Color.White;
                cell_9.BackColor = Color.White;
                cell_10.BackColor = Color.White;
                cell_11.BackColor = Color.White;
                cell_12.BackColor = Color.White;
                cell_13.BackColor = Color.White;
                cell_14.BackColor = Color.White;
                cell_15.BackColor = Color.White;
                cell_16.BackColor = Color.White;
                cell_17.BackColor = Color.White;
                cell_18.BackColor = Color.White;
                cell_19.BackColor = Color.White;
                cell_20.BackColor = Color.White;
                cell_21.BackColor = Color.White;
                cell_22.BackColor = Color.White;
                cell_23.BackColor = Color.White;
                cell_24.BackColor = Color.White;
                cell_25.BackColor = Color.White;
                cell_26.BackColor = Color.White;
                cell_27.BackColor = Color.White;
                cell_28.BackColor = Color.White;
                cell_29.BackColor = Color.White;
                cell_30.BackColor = Color.White;
                cell_31.BackColor = Color.White;
                #endregion
                cell_sp.Font = new Font(cell_sp.Font, FontStyle.Regular);
                cell_name.Font = new Font(cell_name.Font, FontStyle.Regular);
                cell_FirstName.Font = new Font(cell_FirstName.Font, FontStyle.Regular);
            }
        }

        private void xrTableDetail_BeforePrint(object sender, CancelEventArgs e)
        {
        }

        private void cell_effec_BeforePrint(object sender, CancelEventArgs e)
        {
            //XRTableCell cell = (XRTableCell)sender;

            //// Set text alignment
            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            //// Configure cell merging
            //cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            //cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            //// Retrieve department and required employees
            //var department = GetCurrentColumnValue("POSTE")?.ToString(); // استخدام عمود "Poste" لتحديد القسم
            //var requiredEmployees = GetCurrentColumnValue("EFFC/CRT")?.ToString();

            //// Set the Tag property to department
            //if (!string.IsNullOrEmpty(department))
            //{
            //    cell.Tag = department;
            //}
            //else
            //{
            //    cell.Tag = null; // أو قيمة افتراضية
            //}

            //// Set the cell text to required employees
            //if (!string.IsNullOrEmpty(requiredEmployees))
            //{
            //    cell.Text = requiredEmployees;
            //}
            //else
            //{
            //    // إذا كانت الخلية فارغة، نتحقق من القسم ونعطيها نفس القيمة إذا كانت تابعة لنفس القسم
            //    if (cell.Tag != null)
            //    {
            //        cell.Text = " "; // نعطيها مسافة لضمان الدمج
            //    }
            //    else
            //    {
            //        cell.Text = string.Empty;
            //    }
            //}

            //XRTableCell cell = (XRTableCell)sender;

            //// Set text alignment
            //cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            //// Configure cell merging
            //cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            //cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            //// Retrieve department and required employees
            //var department = GetCurrentColumnValue("EFFC/CRT")?.ToString();
            //var requiredEmployees = GetCurrentColumnValue("EFFC/CRT")?.ToString();

            //// Set the Tag property to department
            //if (!string.IsNullOrEmpty(department))
            //{
            //    cell.Tag = department;
            //}
            //else
            //{
            //    cell.Tag = null; // or some default value
            //}

            //// Set the cell text to required employees
            //if (!string.IsNullOrEmpty(requiredEmployees))
            //{
            //    cell.Text = requiredEmployees;
            //}
            //else
            //{
            //    cell.Text = string.Empty;
            //}

            XRTableCell cell = (XRTableCell)sender;

            // تعيين محاذاة النص
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // استخراج القيم من الحقول
            var department = GetCurrentColumnValue("EFFC/CRT")?.ToString();
            var requiredEmployees = GetCurrentColumnValue("EFFC/CRT")?.ToString();

            // تعيين النص إذا لم يكن فارغًا
            if (!string.IsNullOrEmpty(requiredEmployees))
            {
                cell.Text = requiredEmployees;
                // تفعيل دمج الخلايا فقط إذا كان هناك نص
                cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
                cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;
                cell.Tag = department; // استخدام القيمة الفعلية للدمج
            }
            else
            {
                cell.Text = string.Empty;
                // إلغاء دمج الخلايا الفارغة
                //    cell.ProcessDuplicatesMode = ProcessDuplicatesMode.None;
                cell.Tag = Guid.NewGuid().ToString(); // قيمة فريدة لمنع الدمج
            }
        }

        private void cell_name_BeforePrint(object sender, CancelEventArgs e)
        {
        }

        private void cell_FirstName_BeforePrint(object sender, CancelEventArgs e)
        {
        }
    }
}