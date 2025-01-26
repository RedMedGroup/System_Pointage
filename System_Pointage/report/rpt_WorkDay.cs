using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System_Pointage.Classe;

namespace System_Pointage.report
{
    public partial class rpt_WorkDay : DevExpress.XtraReports.UI.XtraReport
    {
   
        public rpt_WorkDay()
        {
            InitializeComponent();
            cell_n.BeforePrint += Cell_n_BeforePrint;
        }
        int index = 1;
        private void Cell_n_BeforePrint(object sender, CancelEventArgs e)
        {
            cell_n.Text = (index++).ToString();
        }

        public void LoadData(IEnumerable<Models.AgentStatus> data)
        {
            // تعيين مصدر البيانات للتقرير
            this.DataSource = data;

            // ربط الحقول في التقرير بالبيانات
            cell_nom.DataBindings.Add("Text", null, "Name");
            cell_Matricule.DataBindings.Add("Text", null, "Matricule");
            cell_poste.DataBindings.Add("Text", null, "Poste");
            cell_date.DataBindings.Add("Text", null, "CalculatedDate", "{0:dd/MM/yyyy}"); 
            cell_jour.DataBindings.Add("Text", null, "DaysCount");

            string formattedDate = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr-FR"));
            lbl_date.Text = formattedDate;
        }
    }
}
