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

namespace System_Pointage.Frm_List
{
    public partial class Frm_MaterielsListe : DevExpress.XtraEditors.XtraForm
    {
        public Frm_MaterielsListe()
        {
            InitializeComponent();
        }

        private void Frm_MaterielsListe_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            RefrechData();
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);

            if (info.InRow || info.InRowCell)
            {
                // الحصول على قيمة ID
                int id = Convert.ToInt32(view.GetFocusedRowCellValue("ID"));
                OpenForm(id);
            }
        }
        public virtual void OpenForm(int id)
        {
            var frm = new Form.Frm_Fiche_Matricule(id);
            frm.ShowDialog();
        }
        void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();
            gridControl1.DataSource = db.Fiche_Matricules;
            gridView1.Columns["ID"].Visible = false;
            //gridView1.Columns["Name"].Caption = "Materiels";
            //gridView1.Columns["Nembre_Contra"].Caption = "CONTRAT";
            //gridView1.Columns["M_Penalite"].Caption = "Montant de pénalité";

        }
    }
}