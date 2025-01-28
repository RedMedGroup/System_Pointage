using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;
using System_Pointage.DAL;

namespace System_Pointage.report
{
    public partial class Frm_Report_Config : DevExpress.XtraEditors.XtraForm
    {

        public Frm_Report_Config()
        {
            InitializeComponent();
        }
        private void btn_rpt_28J_Click(object sender, EventArgs e)
        {
            var reportManager = new WorkDayReportManager();
            reportManager.OpenReportDesigner();
        }   

        private void btn_EtatJournal_Click(object sender, EventArgs e)
        {
            var reportManager = new EtatJournalReportManager();
            reportManager.OpenReportDesigner();
        }

        private void Frm_Report_Config_Load(object sender, EventArgs e)
        {
           
        }

        private void btn_rpt_pointage_Click(object sender, EventArgs e)
        {
            var reportManager = new EtatPointageReportManager();
            reportManager.OpenReportDesigner();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            var reportManager = new EtatPénalitesReportManager();
            reportManager.OpenReportDesigner();
        }
    }
}