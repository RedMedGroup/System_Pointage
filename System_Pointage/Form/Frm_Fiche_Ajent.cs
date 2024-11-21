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
                GetData();
            }
            this.Text = string.Format(";;: {0}", agent.Name);
        }

        private void Frm_Fiche_Ajent_Load(object sender, EventArgs e)
        {
            using (var db = new DAL.DataClasses1DataContext())
            {
                var post = db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList();

                // تهيئة GridLookUpEdit بالبيانات
                lkp_post.Properties.DataSource = post;
                lkp_post.Properties.DisplayMember = "Name";
                lkp_post.Properties.ValueMember = "ID";
                lkp_post.Properties.PopulateViewColumns();
                lkp_post.Properties.View.Columns["ID"].Visible = false;
            }
            lkp_post.EditValueChanged += Lkp_post_EditValueChanged;

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
           // agent.Date_StopJob = dateEdit1.DateTime;
        }
        void GetData()
        {
            txt_Name.Text = agent.Name;
            lkp_post.EditValue = agent.ID_Post;
            spn_jour.EditValue= agent.Jour;
           // dateEdit1.DateTime = (DateTime)agent.Date_StopJob;
        }
        void New()
        {
            agent = new DAL.Fiche_Agent();
        }
        bool IsValidit()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.Text = ErrorText;
                return false;
            }
            if (lkp_post.Text.Trim() == string.Empty)
            {
                lkp_post.Text = ErrorText;
                return false;
            }
            return true;
        }
        void Save()
        {
            if (IsValidit() == false)
                return;
            var db = new DAL.DataClasses1DataContext();
            if (agent.ID == 0)
            {
                db.Fiche_Agents.InsertOnSubmit(agent);
            }
            else
            {
                db.Fiche_Agents.Attach(agent);
            }
            SetData();
            db.SubmitChanges();
            New();
            XtraMessageBox.Show("Enregistrer succés");
        }
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }
    }
}