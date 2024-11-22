namespace System_Pointage.Form
{
    partial class Frm_Fiche_Ajent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Fiche_Ajent));
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txt_efectif = new DevExpress.XtraEditors.TextEdit();
            this.txt_contra = new DevExpress.XtraEditors.TextEdit();
            this.btn_valid = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lkp_post = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.spn_jour = new DevExpress.XtraEditors.SpinEdit();
            this.dt_embouch = new DevExpress.XtraEditors.DateEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.cmb_statut = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.gridLookUpEdit1 = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.txt_efectif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_contra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_post.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_jour.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_statut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(376, 180);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(70, 19);
            this.labelControl4.TabIndex = 22;
            this.labelControl4.Text = "N°actual";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(36, 180);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(153, 19);
            this.labelControl3.TabIndex = 21;
            this.labelControl3.Text = "EFECTIF CONTRAT";
            // 
            // txt_efectif
            // 
            this.txt_efectif.Location = new System.Drawing.Point(475, 181);
            this.txt_efectif.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_efectif.Name = "txt_efectif";
            this.txt_efectif.Properties.ReadOnly = true;
            this.txt_efectif.Size = new System.Drawing.Size(160, 26);
            this.txt_efectif.TabIndex = 20;
            // 
            // txt_contra
            // 
            this.txt_contra.Location = new System.Drawing.Point(203, 177);
            this.txt_contra.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_contra.Name = "txt_contra";
            this.txt_contra.Properties.ReadOnly = true;
            this.txt_contra.Size = new System.Drawing.Size(118, 26);
            this.txt_contra.TabIndex = 19;
            // 
            // btn_valid
            // 
            this.btn_valid.Location = new System.Drawing.Point(279, 488);
            this.btn_valid.Name = "btn_valid";
            this.btn_valid.Size = new System.Drawing.Size(201, 31);
            this.btn_valid.TabIndex = 18;
            this.btn_valid.Text = "Valider";
            this.btn_valid.UseVisualStyleBackColor = true;
            this.btn_valid.Click += new System.EventHandler(this.btn_valid_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(36, 239);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(138, 19);
            this.labelControl1.TabIndex = 17;
            this.labelControl1.Text = "Nom et Prénom :";
            // 
            // lkp_post
            // 
            this.lkp_post.Location = new System.Drawing.Point(203, 117);
            this.lkp_post.Name = "lkp_post";
            this.lkp_post.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_post.Properties.NullText = "";
            this.lkp_post.Properties.PopupView = this.gridLookUpEdit1View;
            this.lkp_post.Size = new System.Drawing.Size(370, 26);
            this.lkp_post.TabIndex = 16;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.DetailHeight = 349;
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(195, 236);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(440, 26);
            this.txt_Name.TabIndex = 15;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(36, 121);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(47, 19);
            this.labelControl2.TabIndex = 23;
            this.labelControl2.Text = "Post :";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(376, 292);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(114, 19);
            this.labelControl5.TabIndex = 25;
            this.labelControl5.Text = "Systéme 4/4 :";
            // 
            // spn_jour
            // 
            this.spn_jour.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spn_jour.Location = new System.Drawing.Point(523, 288);
            this.spn_jour.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spn_jour.Name = "spn_jour";
            this.spn_jour.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spn_jour.Size = new System.Drawing.Size(112, 28);
            this.spn_jour.TabIndex = 26;
            // 
            // dt_embouch
            // 
            this.dt_embouch.EditValue = null;
            this.dt_embouch.Location = new System.Drawing.Point(195, 288);
            this.dt_embouch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dt_embouch.Name = "dt_embouch";
            this.dt_embouch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Size = new System.Drawing.Size(174, 26);
            this.dt_embouch.TabIndex = 27;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(36, 292);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(138, 19);
            this.labelControl6.TabIndex = 28;
            this.labelControl6.Text = "Date Embauche :";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(36, 344);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(62, 19);
            this.labelControl7.TabIndex = 29;
            this.labelControl7.Text = "Statut :";
            // 
            // cmb_statut
            // 
            this.cmb_statut.Location = new System.Drawing.Point(195, 341);
            this.cmb_statut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_statut.Name = "cmb_statut";
            this.cmb_statut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_statut.Properties.Items.AddRange(new object[] {
            "Actif",
            "Inactif"});
            this.cmb_statut.Size = new System.Drawing.Size(150, 26);
            this.cmb_statut.TabIndex = 30;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(36, 407);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(77, 19);
            this.labelControl8.TabIndex = 32;
            this.labelControl8.Text = "Affecter :";
            // 
            // gridLookUpEdit1
            // 
            this.gridLookUpEdit1.Location = new System.Drawing.Point(203, 403);
            this.gridLookUpEdit1.Name = "gridLookUpEdit1";
            this.gridLookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit1.Properties.NullText = "";
            this.gridLookUpEdit1.Properties.PopupView = this.gridView1;
            this.gridLookUpEdit1.Size = new System.Drawing.Size(370, 26);
            this.gridLookUpEdit1.TabIndex = 31;
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 349;
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // Frm_Fiche_Ajent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 550);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.gridLookUpEdit1);
            this.Controls.Add(this.cmb_statut);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.dt_embouch);
            this.Controls.Add(this.spn_jour);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txt_efectif);
            this.Controls.Add(this.txt_contra);
            this.Controls.Add(this.btn_valid);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lkp_post);
            this.Controls.Add(this.txt_Name);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Frm_Fiche_Ajent.IconOptions.SvgImage")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Frm_Fiche_Ajent";
            this.Text = "Fiche_Ajent";
            this.Load += new System.EventHandler(this.Frm_Fiche_Ajent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_efectif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_contra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_post.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_jour.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_statut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txt_efectif;
        private DevExpress.XtraEditors.TextEdit txt_contra;
        private System.Windows.Forms.Button btn_valid;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GridLookUpEdit lkp_post;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit spn_jour;
        private DevExpress.XtraEditors.DateEdit dt_embouch;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_statut;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}