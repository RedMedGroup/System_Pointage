using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System_Pointage.Form;

namespace System_Pointage.report
{
    public partial class rpt_EtatJournal : DevExpress.XtraReports.UI.XtraReport
    {
        private Frm_PointageMateriels _frmPointage;

        public rpt_EtatJournal(Frm_PointageMateriels frmPointage)
        {
            InitializeComponent();
            _frmPointage = frmPointage;
            BindData();
            cell_Department.BeforePrint += Cell_Department_BeforePrint;
            cell_ncontra.BeforePrint += Cell_ncontra_BeforePrint;
            cell_n.BeforePrint += Cell_n_BeforePrint;
            Cell_PresentCount.BeforePrint += Cell_PresentCount_BeforePrint;
            cell_Ecart.BeforePrint += Cell_Ecart_BeforePrint;
            BeforePrint += Rpt_EtatJournal_BeforePrint;
        }
        private Dictionary<string, XRTableCell> lastCellByDepartment = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment = new Dictionary<string, object>();
        private Dictionary<string, XRTableCell> lastCellByDepartment3 = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment3 = new Dictionary<string, object>();
        private Dictionary<string, XRTableCell> lastCellByDepartment2 = new Dictionary<string, XRTableCell>();
        private Dictionary<string, object> lastValueByDepartment2 = new Dictionary<string, object>();
        private void Rpt_EtatJournal_BeforePrint(object sender, CancelEventArgs e)
        {
            lastCellByDepartment.Clear();
            lastValueByDepartment.Clear();
            lastCellByDepartment2.Clear();
            lastValueByDepartment2.Clear();
            lastCellByDepartment3.Clear();
            lastValueByDepartment3.Clear();
        }

        private void Cell_Ecart_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            // Set text alignment
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // Configure cell merging
            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            // Retrieve department and required employees
            var department = GetCurrentColumnValue("camion")?.ToString();
            var requiredEmployees = GetCurrentColumnValue("cell_Ecart")?.ToString();

            // Set the Tag property to department
            if (!string.IsNullOrEmpty(department))
            {
                cell.Tag = department;
            }
            else
            {
                cell.Tag = null; // or some default value
            }

            // Set the cell text to required employees
            if (!string.IsNullOrEmpty(requiredEmployees))
            {
                cell.Text = requiredEmployees;
            }
            else
            {
                cell.Text = string.Empty;
            }
        }

        private void Cell_PresentCount_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            // Set text alignment
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // Configure cell merging
            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            // Retrieve department and required employees
            var department = GetCurrentColumnValue("camion")?.ToString();
            var requiredEmployees = GetCurrentColumnValue("PresentCount")?.ToString();

            // Set the Tag property to department
            if (!string.IsNullOrEmpty(department))
            {
                cell.Tag = department;
            }
            else
            {
                cell.Tag = null; // or some default value
            }

            // Set the cell text to required employees
            if (!string.IsNullOrEmpty(requiredEmployees))
            {
                cell.Text = requiredEmployees;
            }
            else
            {
                cell.Text = string.Empty;
            }
        }

        int index = 0;
        private string currentDepartment = string.Empty;
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

        private void Cell_ncontra_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;

            // Set text alignment
            cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            // Configure cell merging
            cell.ProcessDuplicatesMode = ProcessDuplicatesMode.Merge;
            cell.ProcessDuplicatesTarget = ProcessDuplicatesTarget.Tag;

            // Retrieve department and required employees
            var department = GetCurrentColumnValue("camion")?.ToString();
            var requiredEmployees = GetCurrentColumnValue("RequiredEmployees")?.ToString();

            // Set the Tag property to department
            if (!string.IsNullOrEmpty(department))
            {
                cell.Tag = department;
            }
            else
            {
                cell.Tag = null; // or some default value
            }

            // Set the cell text to required employees
            if (!string.IsNullOrEmpty(requiredEmployees))
            {
                cell.Text = requiredEmployees;
            }
            else
            {
                cell.Text = string.Empty;
            }
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
            cell_Department.DataBindings.Add("Text", this.DataSource, "camion");
            cell_ncontra.DataBindings.Add("Text", this.DataSource, "RequiredEmployees");
            cell_M_Penalite.DataBindings.Add("Text", this.DataSource, "WorkerName");
            cell_TotalPenalties.DataBindings.Add("Text", this.DataSource, "Status");
            Cell_PresentCount.DataBindings.Add("Text", this.DataSource, "PresentCount");
            cell_Ecart.DataBindings.Add("Text", this.DataSource, "cell_Ecart");
            DateTime reportDate = _frmPointage.StartDate;
            string formattedDate = reportDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"));
            lbl_date.Text = formattedDate;
        }
    }
}
