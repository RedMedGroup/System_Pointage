﻿using DevExpress.XtraEditors;
using DevExpress.XtraReports.Design;
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
    public partial class Frm_Fiche_Ajent : DevExpress.XtraEditors.XtraForm
    {
     

        DAL.Fiche_Agent agent;
        public Frm_Fiche_Ajent()
        {
            InitializeComponent();
            New();
        }
        public Frm_Fiche_Ajent(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DataClasses1DataContext())
            {
                agent = db.Fiche_Agents.Single(x => x.ID == id);
              //  GetData();
            }
            this.Text = string.Format(";;: {0}", agent.Name);
        }

        private void Frm_Fiche_Ajent_Load(object sender, EventArgs e)
        {
         
            dt_embouch.DateTime = DateTime.Now;
            using (var db = new DAL.DataClasses1DataContext())
            {
                var post = db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList();

                lkp_post.Properties.DataSource = post;
                lkp_post.Properties.DisplayMember = "Name";
                lkp_post.Properties.ValueMember = "ID";
                lkp_post.Properties.PopulateViewColumns();
                lkp_post.Properties.View.Columns["ID"].Visible = false;
                lkp_ScreanPoste.IntializeData(db.UserAccessProfilePostes.Select(x => new { x.ID, x.Name }).ToList());

            }
            lkp_post.EditValueChanged += Lkp_post_EditValueChanged;
            GetData();
        }

        private void Lkp_post_EditValueChanged(object sender, EventArgs e)
        {
            int selectedPostId = (int)lkp_post.EditValue;

            using (var db = new DAL.DataClasses1DataContext())
            {
                var postDetails = db.Fiche_Postes.FirstOrDefault(x => x.ID == selectedPostId);
                if (postDetails != null)
                {
                    txt_contra.Text = postDetails.Nembre_Contra.ToString();
                }

                int agentCount = db.Fiche_Agents.Count(x => x.ID_Post == selectedPostId );
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
            agent.ID_Post = Convert.ToInt32(lkp_post.EditValue);
            agent.Jour = Convert.ToInt32(spn_jour.EditValue);
            agent.Date_Embauche = dt_embouch.DateTime;
            agent.ScreenPosteD = Convert.ToInt32(lkp_ScreanPoste.EditValue);
            agent.Matricule=txt_matricule.Text;
            agent.Affecter=cmb_affecte.Text;
            if (cmb_statut.Text== "Actif")//
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
            lkp_post.EditValue = agent.ID_Post;
            spn_jour.EditValue= agent.Jour;
            dt_embouch.DateTime = agent.Date_Embauche;
            lkp_ScreanPoste.EditValue = agent.ScreenPosteD;
            txt_matricule.Text=agent.Matricule;
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
            agent = new DAL.Fiche_Agent();
            GetData();
            agent.Statut = true;
        }
        bool IsValidit()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ErrorText;
                return false;
            }
            if (lkp_post.Text.Trim() == string.Empty)
            {
                lkp_post.ErrorText = ErrorText;
                return false;
            }
            if (dt_embouch.DateTime == DateTime.MinValue) 
            {
                dt_embouch.ErrorText = ErrorText;
                return false;
            }
            if (spn_jour.Text.Trim() == string.Empty)
            {
                spn_jour.ErrorText = ErrorText;
                return false;
            }
            if (lkp_ScreanPoste.Text.Trim() == string.Empty)
            {
                lkp_ScreanPoste.ErrorText = ErrorText;
                return false;
            }
            if (cmb_statut.Text.Trim() == string.Empty)
            {
                cmb_statut.ErrorText = ErrorText;
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
                Name = "Frm_Fiche_Ajent", // اسم الشاشة
                IsNew = agent.ID == 0 // تحديد إذا كان الحفظ جديدًا
            };
            userLogAction.SaveAction(() =>
            {
               
                using (var db = new DAL.DataClasses1DataContext())
                {
                    bool VR=false;
                    if (agent.ID == 0)
                    {
                       
                        db.Fiche_Agents.InsertOnSubmit(agent);
                    }
                    else
                    {
                        db.Fiche_Agents.Attach(agent);VR = true;
                    }
                    SetData();
                    db.SubmitChanges();
                    userLogAction.PartID = agent.ID;
                    userLogAction.PartName = actionType == UserLogAction.ActionType.Add
                 ? $": {agent.Name} Ajouter-" 
                 : $": {agent.Name} Modifier-";
                }
                XtraMessageBox.Show("Enregistrer succés");
                // إعادة تهيئة البيانات
                New();
            },actionType);
           
        }

        //void Save()
        //{
        //    UserLogAction userLogAction = new UserLogAction();

        //    if (IsValidit() == false)
        //        return;
        //    if (!UserLogAction.CheckActionAuthorization(userLogAction.Name,agent.ID == 0 ? Master.Actions.Add : Master.Actions.Edit))
        //    {
        //        // إذا لم يكن لديه الصلاحيات، يتم إيقاف العملية
        //        return;
        //    }
        //    var db = new DAL.DataClasses1DataContext();
        //    bool isNew = agent.ID == 0;
        //    if (isNew/*agent.ID == 0*/)
        //    {
        //        db.Fiche_Agents.InsertOnSubmit(agent);
        //    }
        //    else
        //    {
        //        db.Fiche_Agents.Attach(agent);
        //    }
        //    SetData();
        //    db.SubmitChanges();
        //    New();
        //    XtraMessageBox.Show("Enregistrer succés");
        //    userLogAction.PartID = agent.ID; userLogAction.PartName = agent.Name;
        //}
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            var db = new DAL.DataClasses1DataContext();

            if (XtraMessageBox.Show(text: "Voulez-vous supprimer?", caption: "Confirmer la suppression", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var log = db.MVMAgentDetails.Where(x => x.ItemID == agent.ID).Count();
                if (log > 0)
                {
                    XtraMessageBox.Show(text: "Chambre ne peut pas etre supprimé ou il a été utilisé danns le systéme"
                        , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                    return;
                }
                db.Fiche_Agents.Attach(agent);
                db.Fiche_Agents.DeleteOnSubmit(agent);
                db.SubmitChanges();
                New();
            }
        }
    }
}