namespace System_Pointage.Form
{
    partial class Frm_User
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_User));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.txt_UserName = new DevExpress.XtraEditors.TextEdit();
            this.txt_PWD = new DevExpress.XtraEditors.TextEdit();
            this.lkp_UserType = new DevExpress.XtraEditors.LookUpEdit();
            this.lkp_ScreanProfileID = new DevExpress.XtraEditors.LookUpEdit();
            this.toggleSwitch1 = new DevExpress.XtraEditors.ToggleSwitch();
            this.lkp_ScreanPoste = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btn_Save = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_PWD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_UserType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanProfileID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitch1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanPoste.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txt_Name);
            this.layoutControl1.Controls.Add(this.txt_UserName);
            this.layoutControl1.Controls.Add(this.txt_PWD);
            this.layoutControl1.Controls.Add(this.lkp_UserType);
            this.layoutControl1.Controls.Add(this.lkp_ScreanProfileID);
            this.layoutControl1.Controls.Add(this.toggleSwitch1);
            this.layoutControl1.Controls.Add(this.lkp_ScreanPoste);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 55);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(831, 311);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(193, 62);
            this.txt_Name.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(467, 26);
            this.txt_Name.StyleController = this.layoutControl1;
            this.txt_Name.TabIndex = 4;
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(193, 95);
            this.txt_UserName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(608, 26);
            this.txt_UserName.StyleController = this.layoutControl1;
            this.txt_UserName.TabIndex = 5;
            // 
            // txt_PWD
            // 
            this.txt_PWD.Location = new System.Drawing.Point(193, 127);
            this.txt_PWD.Margin = new System.Windows.Forms.Padding(4);
            this.txt_PWD.Name = "txt_PWD";
            this.txt_PWD.Properties.UseSystemPasswordChar = true;
            this.txt_PWD.Size = new System.Drawing.Size(608, 26);
            this.txt_PWD.StyleController = this.layoutControl1;
            this.txt_PWD.TabIndex = 6;
            // 
            // lkp_UserType
            // 
            this.lkp_UserType.Location = new System.Drawing.Point(193, 159);
            this.lkp_UserType.Margin = new System.Windows.Forms.Padding(4);
            this.lkp_UserType.Name = "lkp_UserType";
            this.lkp_UserType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_UserType.Properties.NullText = "";
            this.lkp_UserType.Size = new System.Drawing.Size(608, 26);
            this.lkp_UserType.StyleController = this.layoutControl1;
            this.lkp_UserType.TabIndex = 8;
            this.lkp_UserType.EditValueChanged += new System.EventHandler(this.lkp_UserType_EditValueChanged);
            // 
            // lkp_ScreanProfileID
            // 
            this.lkp_ScreanProfileID.Location = new System.Drawing.Point(193, 191);
            this.lkp_ScreanProfileID.Margin = new System.Windows.Forms.Padding(4);
            this.lkp_ScreanProfileID.Name = "lkp_ScreanProfileID";
            this.lkp_ScreanProfileID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_ScreanProfileID.Properties.NullText = "";
            this.lkp_ScreanProfileID.Size = new System.Drawing.Size(608, 26);
            this.lkp_ScreanProfileID.StyleController = this.layoutControl1;
            this.lkp_ScreanProfileID.TabIndex = 10;
            // 
            // toggleSwitch1
            // 
            this.toggleSwitch1.EditValue = true;
            this.toggleSwitch1.Location = new System.Drawing.Point(666, 62);
            this.toggleSwitch1.Margin = new System.Windows.Forms.Padding(4);
            this.toggleSwitch1.Name = "toggleSwitch1";
            this.toggleSwitch1.Properties.OffText = "In active";
            this.toggleSwitch1.Properties.OnText = "Active";
            this.toggleSwitch1.Size = new System.Drawing.Size(135, 27);
            this.toggleSwitch1.StyleController = this.layoutControl1;
            this.toggleSwitch1.TabIndex = 11;
            // 
            // lkp_ScreanPoste
            // 
            this.lkp_ScreanPoste.Location = new System.Drawing.Point(193, 223);
            this.lkp_ScreanPoste.Margin = new System.Windows.Forms.Padding(4);
            this.lkp_ScreanPoste.Name = "lkp_ScreanPoste";
            this.lkp_ScreanPoste.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_ScreanPoste.Properties.NullText = "";
            this.lkp_ScreanPoste.Size = new System.Drawing.Size(608, 26);
            this.lkp_ScreanPoste.StyleController = this.layoutControl1;
            this.lkp_ScreanPoste.TabIndex = 10;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(831, 311);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.BorderColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.layoutControlGroup2.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup2.CustomizationFormText = "معلومات المستخدم";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem7,
            this.layoutControlItem6,
            this.layoutControlItem4});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.OptionsItemText.TextToControlDistance = 3;
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(13, 13, 13, 13);
            this.layoutControlGroup2.Size = new System.Drawing.Size(811, 291);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup2.Text = "Info utilisateur";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txt_Name;
            this.layoutControlItem1.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem1.CustomizationFormText = "الاسم";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem1.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem1.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem1.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem1.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem1.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem1.Size = new System.Drawing.Size(636, 33);
            this.layoutControlItem1.Text = "Nome";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txt_UserName;
            this.layoutControlItem2.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem2.CustomizationFormText = "اسم الدخول";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 33);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem2.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem2.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem2.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem2.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem2.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem2.Size = new System.Drawing.Size(777, 32);
            this.layoutControlItem2.Text = "Nom de connexion";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txt_PWD;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem3.CustomizationFormText = "الرقم السري";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 65);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem3.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem3.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem3.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem3.Size = new System.Drawing.Size(777, 32);
            this.layoutControlItem3.Text = "Mot de passe";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lkp_UserType;
            this.layoutControlItem5.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem5.CustomizationFormText = "نوع الدخول";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 97);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem5.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem5.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem5.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem5.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem5.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem5.Size = new System.Drawing.Size(777, 32);
            this.layoutControlItem5.Text = "Type d\'entrée";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.lkp_ScreanProfileID;
            this.layoutControlItem7.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem7.CustomizationFormText = "نموذج الوصول";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 129);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem7.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem7.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem7.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem7.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem7.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem7.Size = new System.Drawing.Size(777, 32);
            this.layoutControlItem7.Text = "Modéle d\'acces";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.lkp_ScreanPoste;
            this.layoutControlItem6.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem6.CustomizationFormText = "نموذج الوصول";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 161);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.OptionsPrint.AppearanceItem.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem6.OptionsPrint.AppearanceItemControl.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem6.OptionsPrint.AppearanceItemText.Font = new System.Drawing.Font("Tahoma", 12F);
            this.layoutControlItem6.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem6.Size = new System.Drawing.Size(777, 64);
            this.layoutControlItem6.Text = "Type de département";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(151, 19);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.toggleSwitch1;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(636, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlItem4.Size = new System.Drawing.Size(141, 33);
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextVisible = false;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_Save,
            this.barButtonItem1});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 2;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_Save),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)});
            this.bar1.Text = "Tools";
            // 
            // btn_Save
            // 
            this.btn_Save.Caption = "Sauvegarder";
            this.btn_Save.Id = 0;
            this.btn_Save.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btn_Save.ImageOptions.SvgImage")));
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btn_Save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_Save_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Supprimer";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(831, 55);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 366);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(831, 20);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 55);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 311);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(831, 55);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 311);
            // 
            // Frm_User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 386);
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("Frm_User.IconOptions.SvgImage")));
            this.Name = "Frm_User";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajouter utilisateur";
            this.Load += new System.EventHandler(this.Frm_User_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_PWD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_UserType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanProfileID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toggleSwitch1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_ScreanPoste.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraEditors.TextEdit txt_Name;
        private DevExpress.XtraEditors.TextEdit txt_UserName;
        private DevExpress.XtraEditors.TextEdit txt_PWD;
        private DevExpress.XtraEditors.LookUpEdit lkp_UserType;
        private DevExpress.XtraEditors.LookUpEdit lkp_ScreanProfileID;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitch1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btn_Save;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.LookUpEdit lkp_ScreanPoste;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}