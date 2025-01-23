using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_FichePost : DevExpress.XtraEditors.XtraForm
    {
        DAL.Fiche_Poste un;
         
        public Frm_FichePost()
        {
            InitializeComponent();
            New();
        }

        private void Frm_FichePost_Load(object sender, EventArgs e)
        {
            RefrechData();
            treeList1.OptionsBehavior.Editable = false;
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;
            //treeList1.Columns[nameof(un.Name)].Caption = "Poste";
            //treeList1.Columns[nameof(un.Nembre_Contra)].Caption = "Efectif contra";
            //treeList1.Columns[nameof(un.M_Penalite)].Caption = "Pénalité";
        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(e.Node.GetValue("ID").ToString(), out id))
            {
                var db = new DAL.DataClasses1DataContext();
                un = db.Fiche_Postes.Single(x => x.ID == id);

                GetData();
                btn_save.Enabled = true;
            }
        }
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }
        public static string ErrorTextadvance
        {
            get
            {
                return "Nom déjà enregistré";
            }
        }
        bool IsValidit()
        {
            if (txt_name.Text.Trim() == string.Empty)
            {
                txt_name.ErrorText = ErrorText;
                return false;
            }
            if (lkp_ScreanPoste.Text.Trim() == string.Empty)
            {
                lkp_ScreanPoste.ErrorText = ErrorText;
                return false;
            }
            var db = new DAL.DataClasses1DataContext();
            var OL = db.Fiche_Postes.Where(x => x.Name.Trim() == txt_name.Text.Trim() && x.ID != un.ID);
            if (OL.Count() > 0)
            {
                txt_name.ErrorText = ErrorTextadvance;
                return false;
            }

            if (spn_contra.Value <= 0)
            {
                spn_contra.ErrorText = ErrorText;
                return false;
            }
            if (spn_penalite.Value <= 0)
            {
                spn_penalite.ErrorText = ErrorText;
                return false;
            }

            return true;
        }
        void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();
            lkp_ScreanPoste.IntializeData(db.UserAccessProfilePostes.Select(x => new { x.ID, x.Name }).ToList());
            treeList1.DataSource = db.Fiche_Postes;
            treeList1.ExpandAll();
            treeList1.Columns["Name"].Caption= "Poste";
            treeList1.Columns["Nembre_Contra"].Caption = "CONTRACTUEL(TFT)";
            treeList1.Columns["M_Penalite"].Caption = "Pénalité(TFT)";
            treeList1.Columns["EmployeeCount_tfw"].Visible =false;
            treeList1.Columns["Nembre_Contra_tfw"].Visible = false;
            treeList1.Columns["Departement"].Visible =false;

        }
        void New()
        {
            un = new DAL.Fiche_Poste();
        }
        void GetData()
        {
            txt_name.Text = un.Name;
            lkp_ScreanPoste.EditValue = un.Departement;
            spn_contra.EditValue = un.Nembre_Contra;
            spn_penalite.EditValue = un.M_Penalite;
        
        }
        void SetData()
        {
            un.Name = txt_name.Text;
            un.Departement = Convert.ToInt32(lkp_ScreanPoste.EditValue);
            un.Nembre_Contra = Convert.ToInt32(spn_contra.EditValue);
            un.M_Penalite = Convert.ToInt32(spn_penalite.EditValue);
             un.Nembre_Contra_tfw= 0;
            un.EmployeeCount_tfw = 0 ;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (IsValidit() == false)
                return;
            var db = new DAL.DataClasses1DataContext();
            if (un.ID == 0)
            {
                db.Fiche_Postes.InsertOnSubmit(un);
            }
            else
            {
                db.Fiche_Postes.Attach(un);
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