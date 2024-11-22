using DevExpress.XtraReports.UI;
using System;
using System.Collections;
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
