using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;

namespace System_Pointage.report
{
    public partial class rpt_penalite2 : DevExpress.XtraReports.UI.XtraReport
    {
        public string ReportTitle { get; set; }
        public rpt_penalite2()
        {
            InitializeComponent();
            BindData();
            cell_n.BeforePrint += Cell_n_BeforePrint;
        }
        int index = 1;
        private void Cell_n_BeforePrint(object sender, CancelEventArgs e)
        {
            cell_n.Text = (index++).ToString();
        }
        public void BindData()
        {
            
            cell_Department.DataBindings.Add("Text", this.DataSource, "Department");
            cell_effc.DataBindings.Add("Text", this.DataSource, "Nembre_Contra");
            cell_Totaljourcontra.DataBindings.Add("Text", this.DataSource, "CalculatedField3");
            cell_totaljourpresent.DataBindings.Add("Text", this.DataSource, "AttendanceDays");
            cell_jour_absent.DataBindings.Add("Text", this.DataSource, "CalculatedField5");
            cell_M_Penalite.DataBindings.Add("Text", this.DataSource, "M_Penalite");
            cell_TotalPenalties.DataBindings.Add("Text", this.DataSource, "TotalPenalties");

            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"));
            //lbl_date.Text = formattedDate;
        }
        public void BindAttendanceDataToCells()
        {
            XRLabel lblBase = (XRLabel)FindControl("lbl_base", true);
            if (lblBase != null)
            {
                lblBase.Text = this.ReportTitle; // تعيين النص إلى lbl_base
            }
        }
    }
}
