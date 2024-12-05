namespace System_Pointage.Form
{
    partial class Frm_Fiche_Matricule
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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.cmb_affecte = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cmb_statut = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dt_embouch = new DevExpress.XtraEditors.DateEdit();
            this.txt_matricule = new DevExpress.XtraEditors.TextEdit();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.lkp_Materiels = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_contra = new DevExpress.XtraEditors.TextEdit();
            this.txt_efectif = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.btn_valid = new System.Windows.Forms.Button();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_affecte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_statut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_matricule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Materiels.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_contra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_efectif.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btn_valid);
            this.layoutControl1.Controls.Add(this.lkp_Materiels);
            this.layoutControl1.Controls.Add(this.txt_contra);
            this.layoutControl1.Controls.Add(this.txt_efectif);
            this.layoutControl1.Controls.Add(this.txt_Name);
            this.layoutControl1.Controls.Add(this.dt_embouch);
            this.layoutControl1.Controls.Add(this.cmb_statut);
            this.layoutControl1.Controls.Add(this.cmb_affecte);
            this.layoutControl1.Controls.Add(this.txt_matricule);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(898, 368);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlItem10});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(898, 368);
            this.Root.TextVisible = false;
            // 
            // cmb_affecte
            // 
            this.cmb_affecte.Location = new System.Drawing.Point(189, 275);
            this.cmb_affecte.Name = "cmb_affecte";
            this.cmb_affecte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_affecte.Properties.Items.AddRange(new object[] {
            "TFT",
            "TFW"});
            this.cmb_affecte.Properties.PopupSizeable = true;
            this.cmb_affecte.Size = new System.Drawing.Size(682, 26);
            this.cmb_affecte.StyleController = this.layoutControl1;
            this.cmb_affecte.TabIndex = 9;
            // 
            // cmb_statut
            // 
            this.cmb_statut.Location = new System.Drawing.Point(189, 239);
            this.cmb_statut.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_statut.Name = "cmb_statut";
            this.cmb_statut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_statut.Properties.Items.AddRange(new object[] {
            "Actif",
            "Inactif"});
            this.cmb_statut.Size = new System.Drawing.Size(682, 26);
            this.cmb_statut.StyleController = this.layoutControl1;
            this.cmb_statut.TabIndex = 8;
            // 
            // dt_embouch
            // 
            this.dt_embouch.EditValue = null;
            this.dt_embouch.Location = new System.Drawing.Point(189, 203);
            this.dt_embouch.Margin = new System.Windows.Forms.Padding(4);
            this.dt_embouch.Name = "dt_embouch";
            this.dt_embouch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Size = new System.Drawing.Size(682, 26);
            this.dt_embouch.StyleController = this.layoutControl1;
            this.dt_embouch.TabIndex = 6;
            // 
            // txt_matricule
            // 
            this.txt_matricule.Location = new System.Drawing.Point(189, 131);
            this.txt_matricule.Name = "txt_matricule";
            this.txt_matricule.Size = new System.Drawing.Size(682, 26);
            this.txt_matricule.StyleController = this.layoutControl1;
            this.txt_matricule.TabIndex = 4;
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(189, 167);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(682, 26);
            this.txt_Name.StyleController = this.layoutControl1;
            this.txt_Name.TabIndex = 5;
            // 
            // lkp_Materiels
            // 
            this.lkp_Materiels.Location = new System.Drawing.Point(189, 59);
            this.lkp_Materiels.Name = "lkp_Materiels";
            this.lkp_Materiels.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Materiels.Properties.NullText = "";
            this.lkp_Materiels.Properties.PopupView = this.gridLookUpEdit1View;
            this.lkp_Materiels.Size = new System.Drawing.Size(682, 26);
            this.lkp_Materiels.StyleController = this.layoutControl1;
            this.lkp_Materiels.TabIndex = 0;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.DetailHeight = 349;
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // txt_contra
            // 
            this.txt_contra.Location = new System.Drawing.Point(189, 95);
            this.txt_contra.Margin = new System.Windows.Forms.Padding(4);
            this.txt_contra.Name = "txt_contra";
            this.txt_contra.Properties.ReadOnly = true;
            this.txt_contra.Size = new System.Drawing.Size(255, 26);
            this.txt_contra.StyleController = this.layoutControl1;
            this.txt_contra.TabIndex = 2;
            // 
            // txt_efectif
            // 
            this.txt_efectif.Location = new System.Drawing.Point(616, 95);
            this.txt_efectif.Margin = new System.Windows.Forms.Padding(4);
            this.txt_efectif.Name = "txt_efectif";
            this.txt_efectif.Properties.ReadOnly = true;
            this.txt_efectif.Size = new System.Drawing.Size(255, 26);
            this.txt_efectif.StyleController = this.layoutControl1;
            this.txt_efectif.TabIndex = 3;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.layoutControlGroup2.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup2.CustomizationFormText = "Fiche Agent";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem9,
            this.layoutControlItem11});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 3;
            this.layoutControlGroup2.Size = new System.Drawing.Size(878, 308);
            this.layoutControlGroup2.Text = "Fiche Mateicule";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lkp_Materiels;
            this.layoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem1.CustomizationFormText = "Poste :";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem1.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem1.Text = "Materiels :";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_contra;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "EFECTIF CONTRAT :";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 36);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(427, 36);
            this.layoutControlItem2.Text = "EFECTIF CONTRAT :";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txt_efectif;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "N°actual";
            this.layoutControlItem3.Location = new System.Drawing.Point(427, 36);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem3.Size = new System.Drawing.Size(427, 36);
            this.layoutControlItem3.Text = "N°actual";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txt_Name;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "Nom et Prénom :";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 108);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem4.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem4.Text = "Nom de materiels :";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.dt_embouch;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "Date Embauche :";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem5.Text = "Date Embauche :";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cmb_statut;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "Statut :";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 180);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem7.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem7.Text = "Statut :";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.cmb_affecte;
            this.layoutControlItem9.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem9.CustomizationFormText = "Affecter :";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 216);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem9.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem9.Text = "Affecter :";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(150, 19);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.txt_matricule;
            this.layoutControlItem11.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem11.CustomizationFormText = "Nom et Prénom :";
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem11.Size = new System.Drawing.Size(854, 36);
            this.layoutControlItem11.Text = "Matricule :";
            this.layoutControlItem11.TextSize = new System.Drawing.Size(150, 19);
            // 
            // btn_valid
            // 
            this.btn_valid.Location = new System.Drawing.Point(12, 320);
            this.btn_valid.Name = "btn_valid";
            this.btn_valid.Size = new System.Drawing.Size(874, 36);
            this.btn_valid.TabIndex = 11;
            this.btn_valid.Text = "Valider";
            this.btn_valid.UseVisualStyleBackColor = true;
            this.btn_valid.Click += new System.EventHandler(this.btn_valid_Click);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_valid;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 308);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(878, 40);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // Frm_Fiche_Matricule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 368);
            this.Controls.Add(this.layoutControl1);
            this.Name = "Frm_Fiche_Matricule";
            this.Text = "Frm_Fiche_Matricule";
            this.Load += new System.EventHandler(this.Frm_Fiche_Matricule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_affecte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb_statut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_embouch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_matricule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Materiels.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_contra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_efectif.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.GridLookUpEdit lkp_Materiels;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.TextEdit txt_contra;
        private DevExpress.XtraEditors.TextEdit txt_efectif;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.DateEdit dt_embouch;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_statut;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_affecte;
        private DevExpress.XtraEditors.TextEdit txt_matricule;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private System.Windows.Forms.Button btn_valid;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
    }
}