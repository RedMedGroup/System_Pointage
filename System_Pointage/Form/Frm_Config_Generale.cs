using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System_Pointage.Form
{
    public partial class Frm_Config_Generale : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Frm_Config_Generale()
        {
            InitializeComponent();
        }

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {
            // إنشاء مثيل من Form1
            report.Frm_Report_Config form1 = new report.Frm_Report_Config();

            // تعيين أسلوب الإغلاق للعرض
            form1.TopLevel = false;
            form1.FormBorderStyle = FormBorderStyle.None;
            form1.Dock = DockStyle.Fill;

            // إضافة Form1 إلى layoutControl1
            layoutControl1.Controls.Clear(); // مسح أي عناصر موجودة
            layoutControl1.Controls.Add(form1);

            // إظهار Form1
            form1.Show();
        }
    }
}
