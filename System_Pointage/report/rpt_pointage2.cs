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
        //public string ReportTitle { get; set; }

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
                //if (lblBase != null)
                //{
                //    lblBase.Text = this.ReportTitle; // تعيين النص إلى lbl_base
                //}
                if (posteCell != null)
                {
                    posteCell.DataBindings.Add("Text", table, "POSTE"); // ربط POSTE بالبيانات
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
        }

        private void xrTableDetail_BeforePrint(object sender, CancelEventArgs e)
        {
            
        }

        private void cell_effec_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            // Set text alignment
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // Configure cell merging
            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            // Retrieve department and required employees
            var department = GetCurrentColumnValue("POSTE")?.ToString(); // استخدام عمود "Poste" لتحديد القسم
            var requiredEmployees = GetCurrentColumnValue("EFFC/CRT")?.ToString();

            // Set the Tag property to department
            if (!string.IsNullOrEmpty(department))
            {
                cell.Tag = department;
            }
            else
            {
                cell.Tag = null; // أو قيمة افتراضية
            }

            // Set the cell text to required employees
            if (!string.IsNullOrEmpty(requiredEmployees))
            {
                cell.Text = requiredEmployees;
            }
            else
            {
                // إذا كانت الخلية فارغة، نتحقق من القسم ونعطيها نفس القيمة إذا كانت تابعة لنفس القسم
                if (cell.Tag != null)
                {
                    cell.Text = " "; // نعطيها مسافة لضمان الدمج
                }
                else
                {
                    cell.Text = string.Empty;
                }
            }

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
       

        }

        private void cell_name_BeforePrint(object sender, CancelEventArgs e)
        {
           
        }

        private void cell_FirstName_BeforePrint(object sender, CancelEventArgs e)
        {
            
        }
    }
}
