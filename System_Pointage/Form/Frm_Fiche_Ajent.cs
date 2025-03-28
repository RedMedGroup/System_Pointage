﻿using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Fiche_Ajent : DevExpress.XtraEditors.XtraForm
    {
        private AgentDataService _agentDataService;
        DAL.Fiche_Agent agent;
        public Frm_Fiche_Ajent()
        {
            InitializeComponent();
            //  layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; //btn_Delete
            this.Size = new System.Drawing.Size(505, 220);
            New();
        }
        //public Frm_Fiche_Ajent(int id)
        //{
        //    InitializeComponent();
        //   // layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; //btn_Delete
        //    using (var db = new DAL.DataClasses1DataContext())
        //    {
        //        agent = db.Fiche_Agents.Single(x => x.ID == id);
        //    }
        //    this.Text = string.Format(";;: {0}", agent.Name);
        //}

        private void Frm_Fiche_Ajent_Load(object sender, EventArgs e)
        {
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.GroupPanelText = " ";
            #endregion


            layoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; //grid
            layoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;// btn ajouter 2
            emptySpaceItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            emptySpaceItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            MasqueColumns();
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;

            dt_embouch.DateTime = DateTime.Now;
            using (var db = new DAL.DataClasses1DataContext())
            {
                var post = db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList();
                lkp_post.IntializeData(db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList());
                lkp_post.Properties.View.Columns["Name"].Caption = "Poste/Fonction";
                lkp_post.Properties.PopulateViewColumns();
                //lkp_post.Properties.DataSource = post;
                //lkp_post.Properties.DisplayMember = "Name";
                //lkp_post.Properties.ValueMember = "ID";

                lkp_post.Properties.View.Columns["ID"].Visible = false;
                lkp_ScreanPoste.IntializeData(db.UserAccessProfilePostes.Select(x => new { x.ID, x.Name }).ToList());
            }
            lkp_post.EditValueChanged += Lkp_post_EditValueChanged;
            GetData();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;

        }
        bool change;
        private void Lkp_post_EditValueChanged(object sender, EventArgs e)
        {
            var selectedPost = lkp_post.GetSelectedDataRow();
            if(change==false)
            {

          
            if (selectedPost != null)
            {
                var postData = (dynamic)selectedPost;
                labelControl1.Text = postData.Name; // 
            }
            MasqueColumns();
            layoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;//button ajouter
            layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;// btn update
            layoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;// btn ajouter 2
            layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;// btn ajouter 2
            layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            emptySpaceItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            emptySpaceItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            int selectedPostId = (int)lkp_post.EditValue;

            using (var db = new DAL.DataClasses1DataContext())
            {
                var postDetails = db.Fiche_Postes.FirstOrDefault(x => x.ID == selectedPostId);
                if (postDetails != null)
                {
                    txt_contra.Text = postDetails.Nembre_Contra.ToString();
                }

                int agentCount = db.Fiche_Agents.Count(x => x.ID_Post == selectedPostId && x.Statut == true);
                txt_efectif.Text = agentCount.ToString();

                if (int.TryParse(txt_contra.Text, out int contraValue) &&
              int.TryParse(txt_efectif.Text, out int efectifValue))
                {
                    int ecartValue = efectifValue - contraValue * 2;

                    txt_ecart.Text = (ecartValue >= 0 ? "+" : "") + ecartValue.ToString();
                }
            }


            layoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            this.Size = new System.Drawing.Size(1208, 395);
            gridView1.OptionsBehavior.Editable = false;

            using (var dbContext = new DAL.DataClasses1DataContext())
            {
                var agentDataService = new AgentDataService();
                _agentDataService = new AgentDataService(dbContext);

                bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;
                int? userAccessPosteID = isAdmin ? null : Master.User.IDAccessPoste;

                var data = _agentDataService.GetAgents(userAccessPosteID, isAdmin)
                    .Select(x =>
                    {
                        int agentId = ((dynamic)x).ID;
                        string statutmvm = ((dynamic)x).Statutmvm != null ? ((dynamic)x).Statutmvm.ToString() : "Aucun mouvement enregistré";


                        return new Models.AgentStatus
                        {
                            screenPosteD = agentId,
                            Name = ((dynamic)x).Name.ToString(),
                            FirstName = ((dynamic)x).FirstName.ToString(),
                            Poste = ((dynamic)x).PostID?.ToString() ?? string.Empty,
                            Matricule = ((dynamic)x).Matricule.ToString(),
                            Affecter = ((dynamic)x).Affecter.ToString(),
                            //Jour = ((dynamic)x).Jour,
                            Date = (DateTime)((dynamic)x).Date_Embauche,
                            Statut = ((dynamic)x).Statut ? "Active" : "Inactive",
                            Statutmvm = statutmvm
                        };
                    }).Where(x => x.Poste == selectedPostId.ToString()).ToList();

                if (data == null || !data.Any())
                {
                    XtraMessageBox.Show("Aucune donnée trouvée");
                    gridControl1.DataSource = null;
                }
                else
                {
                    gridControl1.DataSource = data;
                }
                gridView1.GroupPanelText = " ";
                if (data == null || !data.Any())
                {
                    return;
                }
                gridView1.Columns["Name"].Caption = "Nom";
                gridView1.Columns["FirstName"].Caption = "Prénom";
                gridView1.Columns["Date"].Caption = "Date Affectée";
                gridView1.Columns["Jour"].Caption = "Systéme";
                gridView1.Columns["Statut"].Caption = "Active/Inactive";
                gridView1.Columns["Statutmvm"].Caption = "Statut";

                gridView1.Columns["Poste"].Visible = false;
                gridView1.Columns["screenPosteD"].Visible = false;
                gridView1.Columns["CalculatedDate"].Visible = false;
                gridView1.Columns["DaysCount"].Visible = false;
                gridView1.Columns["Difference"].Visible = false;
                gridView1.Columns["Matricule"].VisibleIndex = 1;
                gridView1.Columns["Name"].VisibleIndex = 2;
                gridView1.Columns["FirstName"].VisibleIndex = 3;
                gridView1.Columns["Date"].VisibleIndex = 4;
                gridView1.Columns["Jour"].VisibleIndex = 5;
                gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            }
            }
        }
        void MasqueColumns()
        {

            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem15.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

        }
        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string status = gridView1.GetRowCellValue(e.RowHandle, "Statut")?.ToString();
            if (status == "Inactive")
            {
                e.Appearance.ForeColor = Color.Red;
                string arrow = "→";
                e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds); // رسم النص مع السهم
                e.Handled = true;
            }
        }

        private void btn_valid_Click(object sender, EventArgs e)
        {
            Save();
        }
        void SetData()
        {
            agent.Name = txt_Name.Text;
            agent.FirstName = txt_FirstName.Text;
            agent.ID_Post = Convert.ToInt32(lkp_post.EditValue);
            // agent.Jour = Convert.ToInt32(spn_jour.EditValue);
            agent.Date_Embauche = dt_embouch.DateTime;
            agent.ScreenPosteD = Convert.ToInt32(lkp_ScreanPoste.EditValue);
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
            txt_FirstName.Text = agent.FirstName;
            lkp_post.EditValue = agent.ID_Post;
            // spn_jour.EditValue= agent.Jour;
            dt_embouch.DateTime = agent.Date_Embauche;
            lkp_ScreanPoste.EditValue = agent.ScreenPosteD;
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
            if (txt_FirstName.Text.Trim() == string.Empty)
            {
                txt_FirstName.ErrorText = ErrorText;
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
            //if (spn_jour.Text.Trim() == string.Empty)
            //{
            //    spn_jour.ErrorText = ErrorText;
            //    return false;
            //}
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
        int AgentID;
        private void Save()
        {
            change = false;
            if (IsValidit() == false)
                return;
            if (!Master.ConfirmSave())
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
                    bool VR = false;
                    if (agent.ID == 0)
                    {

                        db.Fiche_Agents.InsertOnSubmit(agent);
                        SetData();
                        db.SubmitChanges();
                        AgentID = agent.ID;
                        SavePrésent();
                    }
                    else
                    {
                        db.Fiche_Agents.Attach(agent); VR = true;
                        SetData();
                        db.SubmitChanges();
                    }

                    userLogAction.PartID = agent.ID;
                    userLogAction.PartName = actionType == UserLogAction.ActionType.Add
                 ? $": {agent.Name} Ajouter-"
                 : $": {agent.Name} Modifier-";
                }
                Master.MessageBox();
                // New();
                SaveEmptyTextEdite();
            }, actionType);

        }

        void SaveEmptyTextEdite()
        {
            agent = new DAL.Fiche_Agent();
            txt_Name.Text = agent.Name;
            txt_FirstName.Text = agent.FirstName;
            // spn_jour.EditValue = agent.Jour;
            dt_embouch.DateTime = agent.Date_Embauche;
            lkp_ScreanPoste.EditValue = agent.ScreenPosteD;
            txt_matricule.Text = agent.Matricule;
            cmb_affecte.Text = agent.Affecter;
            Lkp_post_EditValueChanged(this, EventArgs.Empty);
        }
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }

        private void btn_Ajouter_Click(object sender, EventArgs e)
        {
            AfichierTextEdite();
        }
        void AfichierTextEdite()
        {
            //layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            // layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem15.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            layoutControlItem14.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;//button ajouter
            layoutControlItem16.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;// btn update
            layoutControlItem17.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;// btn ajouter 2

        }
        private void btn_Update_Click(object sender, EventArgs e)
        {
            change = true;
            var selectedRows = gridView1.GetSelectedRows();

            if (selectedRows.Length != 1)
            {
                string message = selectedRows.Length == 0
                    ? "Veuillez sélectionner une ligne."
                    : "Veuillez sélectionner une seule ligne.";

                XtraMessageBox.Show(message, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AfichierTextEdite();
            int id = Convert.ToInt32(gridView1.GetRowCellValue(selectedRows[0], "screenPosteD"));

            using (var db = new DAL.DataClasses1DataContext())
            {
                // استرداد الكائن من قاعدة البيانات
                agent = db.Fiche_Agents.Single(x => x.ID == id);

                // تعبئة الحقول بالمعلومات
                txt_Name.Text = agent.Name;
                txt_FirstName.Text = agent.FirstName;
                lkp_post.EditValue = agent.ID_Post;
                // spn_jour.EditValue = agent.Jour;
                dt_embouch.DateTime = agent.Date_Embauche;
                lkp_ScreanPoste.EditValue = agent.ScreenPosteD;
                txt_matricule.Text = agent.Matricule;
                cmb_affecte.Text = agent.Affecter;
                cmb_statut.Text = agent.Statut ? "Actif" : "Inactif";

                //  cmb_statut.Text = agent.Statut.GetValueOrDefault(false) ? "Actif" : "Inactif";

            }
        }

        private void btn_Delete2_Click(object sender, EventArgs e)
        {
            var selectedRows = gridView1.GetSelectedRows();

            if (selectedRows.Length != 1)
            {
                string message = selectedRows.Length == 0
                    ? "Veuillez sélectionner une ligne."
                    : "Veuillez sélectionner une seule ligne.";

                XtraMessageBox.Show(message, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(gridView1.GetRowCellValue(selectedRows[0], "screenPosteD"));

            var db = new DAL.DataClasses1DataContext();

            if (XtraMessageBox.Show(text: "Voulez-vous supprimer?", caption: "Confirmer la suppression", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var log = db.MVMAgentDetails.Where(x => x.ItemID == id).Count();
                if (log > 0)
                {
                    XtraMessageBox.Show(text: "La gent ne peut pas etre supprimé ou il a été utilisé danns le systéme"
                        , caption: "", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                    return;
                }
                var agentToDelete = db.Fiche_Agents.SingleOrDefault(x => x.ID == id);
                if (agentToDelete != null)
                {
                    db.Fiche_Agents.Attach(agent);
                    db.Fiche_Agents.DeleteOnSubmit(agentToDelete);
                    db.SubmitChanges();
                    Lkp_post_EditValueChanged(this, EventArgs.Empty);
                }
                else
                {
                    XtraMessageBox.Show("L'agent spécifié n'existe pas.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        void SavePrésent()
        {

            XtraForm dialog = new XtraForm
            {
                Text = "Sélectionnez la date de présence",
                Size = new System.Drawing.Size(300, 180),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
            };



            DateEdit dateEdit = new DateEdit
            {
                Text = "Date Présent",
                Location = new System.Drawing.Point(20, 50),
                AutoSize = true
            };
            dateEdit.DateTime = DateTime.Now;
            dialog.Controls.Add(dateEdit);

            Button button = new Button
            {
                Text = "sauvegarder",
                Location = new System.Drawing.Point(100, 80)
            };
            button.Click += (s, args) =>
            {
                if (dateEdit.DateTime == DateTime.MinValue)
                {
                    dateEdit.ErrorText = ErrorText;
                    return;
                }
                using (var db = new DAL.DataClasses1DataContext())
                {
                    var mvm = new DAL.MVMAgentDetail
                    {
                        ItemID = AgentID,
                        Statut = "P",
                        Date = dateEdit.DateTime
                    };
                    db.MVMAgentDetails.InsertOnSubmit(mvm);
                    db.SubmitChanges();
                }
                dialog.Close();
            };
            dialog.Controls.Add(button);

            dialog.ShowDialog();
        }
    }
}