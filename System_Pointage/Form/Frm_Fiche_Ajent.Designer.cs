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
            this.txt_efectif = new DevExpress.XtraEditors.TextEdit();
            this.txt_contra = new DevExpress.XtraEditors.TextEdit();
            this.btn_valid = new System.Windows.Forms.Button();
            this.lkp_post = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.spn_jour = new DevExpress.XtraEditors.SpinEdit();
            this.dt_embouch = new DevExpress.XtraEditors.DateEdit();
            this.cmb_statut = new DevExpress.XtraEditors.ComboBoxEdit();
            this.gridLookUpEdit1 = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lkp_ScreanPoste = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanPoste.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_efectif
            // 
            this.txt_efectif.Location = new System.Drawing.Point(517, 86);
            this.txt_efectif.Margin = new System.Windows.Forms.Padding(4);
            this.txt_efectif.Name = "txt_efectif";
            this.txt_efectif.Properties.ReadOnly = true;
            this.txt_efectif.Size = new System.Drawing.Size(141, 26);
            this.txt_efectif.StyleController = this.layoutControl1;
            this.txt_efectif.TabIndex = 20;
            // 
            // txt_contra
            // 
            this.txt_contra.Location = new System.Drawing.Point(198, 86);
            this.txt_contra.Margin = new System.Windows.Forms.Padding(4);
            this.txt_contra.Name = "txt_contra";
            this.txt_contra.Properties.ReadOnly = true;
            this.txt_contra.Size = new System.Drawing.Size(141, 26);
            this.txt_contra.StyleController = this.layoutControl1;
            this.txt_contra.TabIndex = 19;
            // 
            // btn_valid
            // 
            this.btn_valid.Location = new System.Drawing.Point(12, 311);
            this.btn_valid.Name = "btn_valid";
            this.btn_valid.Size = new System.Drawing.Size(658, 30);
            this.btn_valid.TabIndex = 18;
            this.btn_valid.Text = "Valider";
            this.btn_valid.UseVisualStyleBackColor = true;
            this.btn_valid.Click += new System.EventHandler(this.btn_valid_Click);
            // 
            // lkp_post
            // 
            this.lkp_post.Location = new System.Drawing.Point(198, 56);
            this.lkp_post.Name = "lkp_post";
            this.lkp_post.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_post.Properties.NullText = "";
            this.lkp_post.Properties.PopupView = this.gridLookUpEdit1View;
            this.lkp_post.Size = new System.Drawing.Size(460, 26);
            this.lkp_post.StyleController = this.layoutControl1;
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
            this.txt_Name.Location = new System.Drawing.Point(198, 116);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(460, 26);
            this.txt_Name.StyleController = this.layoutControl1;
            this.txt_Name.TabIndex = 15;
            // 
            // spn_jour
            // 
            this.spn_jour.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spn_jour.Location = new System.Drawing.Point(517, 146);
            this.spn_jour.Margin = new System.Windows.Forms.Padding(4);
            this.spn_jour.Name = "spn_jour";
            this.spn_jour.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spn_jour.Size = new System.Drawing.Size(141, 28);
            this.spn_jour.StyleController = this.layoutControl1;
            this.spn_jour.TabIndex = 26;
            // 
            // dt_embouch
            // 
            this.dt_embouch.EditValue = null;
            this.dt_embouch.Location = new System.Drawing.Point(198, 146);
            this.dt_embouch.Margin = new System.Windows.Forms.Padding(4);
            this.dt_embouch.Name = "dt_embouch";
            this.dt_embouch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt_embouch.Size = new System.Drawing.Size(141, 26);
            this.dt_embouch.StyleController = this.layoutControl1;
            this.dt_embouch.TabIndex = 27;
            // 
            // cmb_statut
            // 
            this.cmb_statut.Location = new System.Drawing.Point(198, 178);
            this.cmb_statut.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_statut.Name = "cmb_statut";
            this.cmb_statut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb_statut.Properties.Items.AddRange(new object[] {
            "Actif",
            "Inactif"});
            this.cmb_statut.Size = new System.Drawing.Size(460, 26);
            this.cmb_statut.StyleController = this.layoutControl1;
            this.cmb_statut.TabIndex = 30;
            // 
            // gridLookUpEdit1
            // 
            this.gridLookUpEdit1.Location = new System.Drawing.Point(198, 238);
            this.gridLookUpEdit1.Name = "gridLookUpEdit1";
            this.gridLookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit1.Properties.NullText = "";
            this.gridLookUpEdit1.Properties.PopupView = this.gridView1;
            this.gridLookUpEdit1.Size = new System.Drawing.Size(460, 26);
            this.gridLookUpEdit1.StyleController = this.layoutControl1;
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
            // lkp_ScreanPoste
            // 
            this.lkp_ScreanPoste.Location = new System.Drawing.Point(198, 208);
            this.lkp_ScreanPoste.Name = "lkp_ScreanPoste";
            this.lkp_ScreanPoste.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_ScreanPoste.Properties.NullText = "";
            this.lkp_ScreanPoste.Properties.PopupView = this.gridView2;
            this.lkp_ScreanPoste.Size = new System.Drawing.Size(460, 26);
            this.lkp_ScreanPoste.StyleController = this.layoutControl1;
            this.lkp_ScreanPoste.TabIndex = 33;
            // 
            // gridView2
            // 
            this.gridView2.DetailHeight = 349;
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridLookUpEdit1);
            this.layoutControl1.Controls.Add(this.btn_valid);
            this.layoutControl1.Controls.Add(this.lkp_ScreanPoste);
            this.layoutControl1.Controls.Add(this.lkp_post);
            this.layoutControl1.Controls.Add(this.cmb_statut);
            this.layoutControl1.Controls.Add(this.txt_contra);
            this.layoutControl1.Controls.Add(this.txt_efectif);
            this.layoutControl1.Controls.Add(this.spn_jour);
            this.layoutControl1.Controls.Add(this.txt_Name);
            this.layoutControl1.Controls.Add(this.dt_embouch);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(682, 353);
            this.layoutControl1.TabIndex = 35;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem10,
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(682, 353);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lkp_post;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(638, 30);
            this.layoutControlItem1.Text = "Post :";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(162, 19);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 268);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(662, 31);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_contra;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(319, 30);
            this.layoutControlItem2.Text = "EFECTIF CONTRAT";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txt_efectif;
            this.layoutControlItem3.Location = new System.Drawing.Point(319, 30);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(319, 30);
            this.layoutControlItem3.Text = "N°actual";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txt_Name;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(638, 30);
            this.layoutControlItem4.Text = "Nom et Prénom :";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.dt_embouch;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 90);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(319, 32);
            this.layoutControlItem5.Text = "Date Embauche :";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.spn_jour;
            this.layoutControlItem6.Location = new System.Drawing.Point(319, 90);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(319, 32);
            this.layoutControlItem6.Text = "Systéme 4/4 :";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.cmb_statut;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 122);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(638, 30);
            this.layoutControlItem7.Text = "Statut :";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.lkp_ScreanPoste;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 152);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(638, 30);
            this.layoutControlItem8.Text = "Type de département :";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.gridLookUpEdit1;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 182);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(638, 30);
            this.layoutControlItem9.Text = "Affecter :";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(162, 19);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btn_valid;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 299);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(662, 34);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.layoutControlGroup1.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(662, 268);
            this.layoutControlGroup1.Text = "Fiche Agent";
            // 
            // Frm_Fiche_Ajent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 353);
            this.Controls.Add(this.layoutControl1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Frm_Fiche_Ajent.IconOptions.SvgImage")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Frm_Fiche_Ajent";
            this.Text = "Fiche_Agent";
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanPoste.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.TextEdit txt_efectif;
        private DevExpress.XtraEditors.TextEdit txt_contra;
        private System.Windows.Forms.Button btn_valid;
        private DevExpress.XtraEditors.GridLookUpEdit lkp_post;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.SpinEdit spn_jour;
        private DevExpress.XtraEditors.DateEdit dt_embouch;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_statut;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GridLookUpEdit lkp_ScreanPoste;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
    }
}