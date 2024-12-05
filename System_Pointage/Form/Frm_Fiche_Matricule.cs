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
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Fiche_Matricule : DevExpress.XtraEditors.XtraForm
    {
        DAL.Fiche_Matricule agent;
        public Frm_Fiche_Matricule()
        {
            InitializeComponent();
            New();
        }
        public Frm_Fiche_Matricule(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DataClasses1DataContext())
            {
                agent = db.Fiche_Matricules.Single(x => x.ID == id);
                //  GetData();
            }
            this.Text = string.Format(";;: {0}", agent.Name);
        }
        private void Frm_Fiche_Matricule_Load(object sender, EventArgs e)
        {
            dt_embouch.DateTime = DateTime.Now;
            using (var db = new DAL.DataClasses1DataContext())
            {
                var post = db.Fiche_materiels.Select(x => new { x.ID, x.Name }).ToList();

                lkp_Materiels.Properties.DataSource = post;
                lkp_Materiels.Properties.DisplayMember = "Name";
                lkp_Materiels.Properties.ValueMember = "ID";
                lkp_Materiels.Properties.PopulateViewColumns();
                lkp_Materiels.Properties.View.Columns["ID"].Visible = false;

            }
            lkp_Materiels.EditValueChanged += Lkp_Materiels_EditValueChanged;
            GetData();
        }

        private void Lkp_Materiels_EditValueChanged(object sender, EventArgs e)
        {
            int selectedPostId = (int)lkp_Materiels.EditValue;

            using (var db = new DAL.DataClasses1DataContext())
            {
                var postDetails = db.Fiche_materiels.FirstOrDefault(x => x.ID == selectedPostId);
                if (postDetails != null)
                {
                    txt_contra.Text = postDetails.Nembre_Contra.ToString();
                }

                int agentCount = db.Fiche_Matricules.Count(x => x.ID_materiels == selectedPostId);
                txt_efectif.Text = agentCount.ToString();
            }
        }

        private void btn_valid_Click(object sender, EventArgs e)
        {
            Save();
        }
        void SetData()
        {
            agent.Name = txt_Name.Text;
            agent.ID_materiels = Convert.ToInt32(lkp_Materiels.EditValue);
            agent.Date_Embauche = dt_embouch.DateTime;
            agent.Matricule = txt_matricule.Text;
            agent.Affecter = cmb_affecte.Text;
            if (cmb_statut.Text == "Actif")//
            {
                agent.Statut = true;
            }
            else
            {
                agent.Statut = false;
            }
        }
        void GetData()
        {
            txt_Name.Text = agent.Name;
            lkp_Materiels.EditValue = agent.ID_materiels;
            dt_embouch.DateTime = agent.Date_Embauche;
            txt_matricule.Text = agent.Matricule;
            cmb_affecte.Text = agent.Affecter;
            if (agent.Statut == true)
            {
                cmb_statut.Text = "Actif";
            }
            else
            {
                cmb_statut.Text = "Inactif";
            }
        }
        void New()
        {
            agent = new DAL.Fiche_Matricule();
            GetData();
        }
        bool IsValidit()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText =Master.ErrorText;
                return false;
            }
            if (lkp_Materiels.Text.Trim() == string.Empty)
            {
                lkp_Materiels.ErrorText = Master.ErrorText;
                return false;
            }
            if (dt_embouch.DateTime == DateTime.MinValue)
            {
                dt_embouch.ErrorText = Master.ErrorText;
                return false;
            }
           
            if (cmb_statut.Text.Trim() == string.Empty)
            {
                cmb_statut.ErrorText = Master.ErrorText;
                return false;
            }
            var db = new DAL.DataClasses1DataContext();
            if (db.Fiche_Agents.Where(x => x.ID != agent.ID && x.Matricule.Trim() == txt_matricule.Text.Trim()).Count() > 0)
            {
                txt_matricule.ErrorText = ErrorTextadvance;
                return false;
            }
            return true;
        }
        public static string ErrorTextadvance
        {
            get
            {
                return "Nom enregistré";
            }
        }
        private void Save()
        {
            if (IsValidit() == false)
                return;
            UserLogAction.ActionType actionType = agent.ID == 0 ? UserLogAction.ActionType.Add : UserLogAction.ActionType.Edit;

            UserLogAction userLogAction = new UserLogAction
            {
                PartID = agent.ID,
                PartName = agent.Name,
                Name = "Frm_Fiche_Matricule", 
                IsNew = agent.ID == 0 // تحديد إذا كان الحفظ جديدًا
            };
            userLogAction.SaveAction(() =>
            {

                using (var db = new DAL.DataClasses1DataContext())
                {
                    bool VR = false;
                    if (agent.ID == 0)
                    {

                        db.Fiche_Matricules.InsertOnSubmit(agent);
                    }
                    else
                    {
                        db.Fiche_Matricules.Attach(agent); VR = true;
                    }
                    SetData();
                    db.SubmitChanges();
                    userLogAction.PartID = agent.ID;
                    userLogAction.PartName = actionType == UserLogAction.ActionType.Add
                 ? $": {agent.Name} Ajouter-"
                 : $": {agent.Name} Modifier-";
                }
                XtraMessageBox.Show("Enregistrer succés");
                New();
            }, actionType);

        }
    }
}