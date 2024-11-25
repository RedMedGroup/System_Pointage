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

            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Value;

            cell.Text = cell.Text;
            //XRTableCell currentCell = (XRTableCell)sender;
            //string currentDepartment = GetCurrentColumnValue("Department")?.ToString();
            //string currentEcartValue = GetCurrentColumnValue("RequiredEmployees")?.ToString();

            //// تحقق إذا كانت القيم الحالية فارغة
            //if (string.IsNullOrEmpty(currentDepartment) || string.IsNullOrEmpty(currentEcartValue))
            //{
            //    return;
            //}

            //// تحقق مما إذا كانت هناك خلية سابقة بنفس القسم
            //if (lastCellByDepartment2.ContainsKey(currentDepartment) &&
            //    lastValueByDepartment2[currentDepartment]?.ToString() == currentEcartValue)
            //{
            //    XRTableCell lastCell = lastCellByDepartment2[currentDepartment];

            //    // زيادة قيمة RowSpan للخلية السابقة
            //    lastCell.RowSpan = lastCell.RowSpan == 0 ? 2 : lastCell.RowSpan + 1;

            //    // تعيين الخلية الحالية كغير مرئية
            //    currentCell.Visible = false;
            //}
            //else
            //{
            //    // إذا كانت القيم مختلفة، تحديث القيم للخلية الحالية
            //    lastCellByDepartment2[currentDepartment] = currentCell;
            //    lastValueByDepartment2[currentDepartment] = currentEcartValue;

            //    // اجعل الخلية الحالية مرئية وأعد ضبط RowSpan
            //    currentCell.Visible = true;
            //    currentCell.RowSpan = 1;
            //}
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
