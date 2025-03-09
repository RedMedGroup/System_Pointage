using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System_Pointage.Frm_List;

namespace System_Pointage.Form
{
    public partial class Frm_Config_Generale : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Frm_Config_Generale()
        {
            InitializeComponent();
        }
        private void OpenForm(XtraForm form)
        {
            // التحقق من وجود فورم مفتوح مسبقًا وإغلاقه
            foreach (Control ctrl in layoutControl1.Controls)
            {
                if (ctrl is XtraForm existingForm)
                {
                    existingForm.Close();
                }
            }

            layoutControl1.Controls.Clear(); // مسح جميع العناصر من الحاوية

            // إعداد الفورم الجديد
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            layoutControl1.Controls.Add(form);
            form.Show();
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            OpenForm(new report.Frm_Report_Config());
            //report.Frm_Report_Config form1 = new report.Frm_Report_Config();

            //form1.TopLevel = false;
            //form1.FormBorderStyle = FormBorderStyle.None;
            //form1.Dock = DockStyle.Fill;
            //layoutControl1.Controls.Clear(); 
            //layoutControl1.Controls.Add(form1);
            //form1.Show();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {
            OpenForm(new Frm_Config_JourTravail());
        }

        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            OpenForm(new BackUp());
        }

        private void accordionControlElement4_Click(object sender, EventArgs e)
        {
            OpenForm(new Frm_Activation_List());
           
        }
    }
}
