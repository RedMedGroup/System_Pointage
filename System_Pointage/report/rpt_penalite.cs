using DevExpress.XtraReports.UI;
using Humanizer;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System_Pointage.Classe;

namespace System_Pointage.report
{
    public partial class rpt_penalite : DevExpress.XtraReports.UI.XtraReport
    {
        public string ReportTitle { get; set; }
        public decimal SumPinality { get; set; }

        public rpt_penalite()
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
            cell_TotalAbsences.DataBindings.Add("Text", this.DataSource, "TotalAbsences");
            cell_M_Penalite.DataBindings.Add("Text", this.DataSource, "M_Penalite");
            cell_TotalPenalties.DataBindings.Add("Text", this.DataSource, "TotalPenalties");
            cell_nP.DataBindings.Add("Text", this.DataSource, "Nombre du personnel absent");

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

        private void rpt_penalite_BeforePrint(object sender, CancelEventArgs e)
        {
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            if (isAdmin)
            {
                xrLabel4.Text = "Personnel(restauration/hébergement):";
            }
            else
            {
                if (userAccessPosteID == 1)
                {
                    xrLabel4.Text = "Personnel d'hébergement et de maintenance générale:";
                }
                else if (userAccessPosteID == 2)
                {
                    xrLabel4.Text = "Personnel de restauration:";
                }
             
            }
        }

        private void xrLabel7_BeforePrint(object sender, CancelEventArgs e)
        {
            string cellText = SumPinality.ToString();

            if (string.IsNullOrWhiteSpace(cellText))
            {
                ((DevExpress.XtraReports.UI.XRLabel)sender).Text = "Valeur vide";
                return;
            }

            // التحقق من القيمة إذا كانت رقمًا
            if (decimal.TryParse(cellText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal number))
            {
                // استخراج الجزء الصحيح من الرقم
                int integerPart = (int)Math.Floor(number);

                // تحويل الجزء الصحيح إلى نص باستخدام ToWords
                string integerTextInFrench = integerPart.ToWords(new CultureInfo("fr-FR"));

                // التعامل مع الجزء العشري إذا كان موجودًا
                int decimalPart = (int)((number - integerPart) * 100); // استخراج الخانات العشرية (خانتين)
                string decimalTextInFrench = decimalPart > 0 ? $" et {decimalPart.ToWords(new CultureInfo("fr-FR"))} centimes" : "";

                // دمج الجزء الصحيح مع الجزء العشري (إن وجد)
                ((DevExpress.XtraReports.UI.XRLabel)sender).Text = $"{integerTextInFrench}{decimalTextInFrench} Dinars Algériens";

            }
            else
            {
                ((DevExpress.XtraReports.UI.XRLabel)sender).Text = $"Valeur non valide: {cellText}";
            }
        }
    }
}
