namespace System_Pointage.Form
{
    partial class Frm_FichePost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_FichePost));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btn_new = new DevExpress.XtraEditors.SimpleButton();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.txt_name = new DevExpress.XtraEditors.TextEdit();
            this.spn_contra = new DevExpress.XtraEditors.SpinEdit();
            this.spn_penalite = new DevExpress.XtraEditors.SpinEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_contra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_penalite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btn_new);
            this.layoutControl1.Controls.Add(this.btn_save);
            this.layoutControl1.Controls.Add(this.treeList1);
            this.layoutControl1.Controls.Add(this.txt_name);
            this.layoutControl1.Controls.Add(this.spn_contra);
            this.layoutControl1.Controls.Add(this.spn_penalite);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(472, 469);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btn_new
            // 
            this.btn_new.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_new.ImageOptions.SvgImage")));
            this.btn_new.Location = new System.Drawing.Point(238, 100);
            this.btn_new.Margin = new System.Windows.Forms.Padding(4);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(222, 52);
            this.btn_new.StyleController = this.layoutControl1;
            this.btn_new.TabIndex = 14;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_save.ImageOptions.SvgImage")));
            this.btn_save.Location = new System.Drawing.Point(12, 100);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(222, 52);
            this.btn_save.StyleController = this.layoutControl1;
            this.btn_save.TabIndex = 13;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // treeList1
            // 
            this.treeList1.FixedLineWidth = 3;
            this.treeList1.HorzScrollStep = 4;
            this.treeList1.Location = new System.Drawing.Point(12, 156);
            this.treeList1.Margin = new System.Windows.Forms.Padding(4);
            this.treeList1.MinWidth = 30;
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(448, 301);
            this.treeList1.TabIndex = 9;
            this.treeList1.TreeLevelWidth = 27;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(120, 11);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(341, 26);
            this.txt_name.StyleController = this.layoutControl1;
            this.txt_name.TabIndex = 4;
            // 
            // spn_contra
            // 
            this.spn_contra.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spn_contra.Location = new System.Drawing.Point(120, 39);
            this.spn_contra.Name = "spn_contra";
            this.spn_contra.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spn_contra.Size = new System.Drawing.Size(341, 28);
            this.spn_contra.StyleController = this.layoutControl1;
            this.spn_contra.TabIndex = 5;
            // 
            // spn_penalite
            // 
            this.spn_penalite.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spn_penalite.Location = new System.Drawing.Point(120, 69);
            this.spn_penalite.Name = "spn_penalite";
            this.spn_penalite.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spn_penalite.Size = new System.Drawing.Size(341, 28);
            this.spn_penalite.StyleController = this.layoutControl1;
            this.spn_penalite.TabIndex = 7;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem1,
            this.layoutControlItem6});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(472, 469);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_name;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "Nom de post";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlItem2.Size = new System.Drawing.Size(452, 28);
            this.layoutControlItem2.Text = "Nom de post";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(97, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.spn_contra;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "Efectif contrat";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 28);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlItem3.Size = new System.Drawing.Size(452, 30);
            this.layoutControlItem3.Text = "Efectif contrat";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(97, 19);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.spn_penalite;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "Pénalité";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 58);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(1, 1, 1, 1);
            this.layoutControlItem4.Size = new System.Drawing.Size(452, 30);
            this.layoutControlItem4.Text = "Pénalité";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(97, 19);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btn_new;
            this.layoutControlItem5.Location = new System.Drawing.Point(226, 88);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(226, 56);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeList1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 144);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(452, 305);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btn_save;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 88);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(226, 56);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // Frm_FichePost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 469);
            this.Controls.Add(this.layoutControl1);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Frm_FichePost.IconOptions.SvgImage")));
            this.MaximizeBox = false;
            this.Name = "Frm_FichePost";
            this.Text = "Fiche de Poste";
            this.Load += new System.EventHandler(this.Frm_FichePost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_contra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spn_penalite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton btn_new;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.TextEdit txt_name;
        private DevExpress.XtraEditors.SpinEdit spn_contra;
        private DevExpress.XtraEditors.SpinEdit spn_penalite;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
    }
}