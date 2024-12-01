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
    public partial class frm_User_List : DevExpress.XtraEditors.XtraForm
    {
        DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext();
        DAL.User us;
        public frm_User_List()
        {
            InitializeComponent();
        }

        private void frm_User_List_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            RefrechData();
            gridView1.Columns[nameof(us.ID)].Visible = false;


            gridView1.Columns[nameof(us.Name)].Caption = "Nom";
            gridView1.Columns[nameof(us.UserName)].Caption = "Nom d'utilisateur";
            gridView1.Columns[nameof(us.Password)].Caption = "Mot de passe ";
            gridView1.Columns[nameof(us.ScreenProfileID)].Caption = "Modèle d'accès";
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

        public  void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();
            var data = from us in db.Users
                       join c in db.UserAccessProfileNames on us.ScreenProfileID equals c.ID
                       select new
                       {
                           us.ID,
                           us.Name,
                           us.Password,
                           us.UserName,
                           ScreenProfileID = c.Name,
                           us.IsActive

                       };
            gridControl1.DataSource = data;
        }
        public  void OpenForm(int id)
        {
            var frm = new Frm_User(id);
            Frm_Main.OpenForm(frm, true);
        }
        public  void New()
        {
            frm_AccessProfile frm = new frm_AccessProfile();
            Frm_Main.OpenForm(frm, true);
        }
    }
}