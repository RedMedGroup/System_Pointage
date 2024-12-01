using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Form;

namespace System_Pointage.Frm_List
{
    public partial class Frm_Liste_AccessProfile : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Liste_AccessProfile()
        {
            InitializeComponent();
        }

        private void Frm_Liste_AccessProfile_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            var db = new DAL.DataClasses1DataContext();
            gridControl1.DataSource=db.UserAccessProfileNames;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                OpenForm(Convert.ToInt32(view.GetFocusedRowCellValue("ID")));
            }
        }
        public void OpenForm(int id)
        {
            var frm = new frm_AccessProfile(id);
            Frm_Main.OpenForm(frm, true);
        }
    }
}