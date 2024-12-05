using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System_Pointage.Form
{
    public partial class Fiche_Materiels : DevExpress.XtraEditors.XtraForm
    {
        DAL.Fiche_materiel un;
        public Fiche_Materiels()
        {
            InitializeComponent();
            New();
        }

        private void Fiche_Materiels_Load(object sender, EventArgs e)
        {
            RefrechData();
            treeList1.OptionsBehavior.Editable = false;
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;
            treeList1.Columns[nameof(un.Name)].Caption = "Poste";
            treeList1.Columns[nameof(un.Nembre_Contra)].Caption = "Efectif contra";
            treeList1.Columns[nameof(un.M_Penalite)].Caption = "Pénalité";
        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(e.Node.GetValue("ID").ToString(), out id))
            {
                var db = new DAL.DataClasses1DataContext();
                un = db.Fiche_materiels.SingleOrDefault(x => x.ID == id);

                if (un != null) // تحقق من أن العنصر موجود
                {
                    GetData();
                }
            }
        }
        bool IsValidit()
        {
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText =Classe.Master. ErrorText;
                return false;
            }
            var db = new DAL.DataClasses1DataContext();
            var OL = db.Fiche_Postes.Where(x => x.Name.Trim() == txt_name.Text.Trim() && x.ID != un.ID);
            if (OL.Count() > 0)
            {
                txt_name.ErrorText = Classe.Master.ErrorTextadvance;
                return false;
            }

            if (spn_contra.Value <= 0)
            {
                spn_contra.ErrorText = Classe.Master.ErrorText;
                return false;
            }
            if (spn_penalite.Value <= 0)
            {
                spn_penalite.ErrorText = Classe.Master.ErrorText;
                return false;
            }

            return true;
        }
        void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();

            treeList1.DataSource = db.Fiche_materiels;
            treeList1.ExpandAll();
        }
        void New()
        {
            un = new DAL.Fiche_materiel();
        }
        void GetData()
        {
            txt_name.Text = un.Name;
            spn_contra.EditValue = un.Nembre_Contra;
            spn_penalite.EditValue = un.M_Penalite;
        }
        void SetData()
        {
            un.Name = txt_name.Text;
            un.Nembre_Contra = Convert.ToInt32(spn_contra.EditValue);
            un.M_Penalite = Convert.ToInt32(spn_penalite.EditValue);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (IsValidit() == false)
                return;
            var db = new DAL.DataClasses1DataContext();
            if (un.ID == 0)
            {
                db.Fiche_materiels.InsertOnSubmit(un);
            }
            else
            {
                db.Fiche_materiels.Attach(un);
            }
            SetData();
            db.SubmitChanges();
            RefrechData();
            New();
            btn_save.Enabled = false;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            New();
            btn_save.Enabled = true;
            txt_name.Text = ""; spn_contra.EditValue = 0; spn_penalite.EditValue = 0;
        }
    }
}