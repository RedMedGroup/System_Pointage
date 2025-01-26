namespace System_Pointage.report
{
    partial class Frm_Report_Config
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Report_Config));
            this.btn_rpt_28J = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btn_EtatJournal = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btn_rpt_pointage = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btn_rpt_28J
            // 
            this.btn_rpt_28J.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_rpt_28J.ImageOptions.Image")));
            this.btn_rpt_28J.Location = new System.Drawing.Point(362, 72);
            this.btn_rpt_28J.Name = "btn_rpt_28J";
            this.btn_rpt_28J.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btn_rpt_28J.Size = new System.Drawing.Size(38, 32);
            this.btn_rpt_28J.TabIndex = 0;
            this.btn_rpt_28J.Click += new System.EventHandler(this.btn_rpt_28J_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(23, 80);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(319, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Rapport des Agents avec plus de 28 jours de travail";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(21, 34);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Etat journalier";
            // 
            // btn_EtatJournal
            // 
            this.btn_EtatJournal.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btn_EtatJournal.ImageOptions.Image")));
            this.btn_EtatJournal.Location = new System.Drawing.Point(132, 26);
            this.btn_EtatJournal.Name = "btn_EtatJournal";
            this.btn_EtatJournal.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btn_EtatJournal.Size = new System.Drawing.Size(38, 32);
            this.btn_EtatJournal.TabIndex = 2;
            this.btn_EtatJournal.Click += new System.EventHandler(this.btn_EtatJournal_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(21, 135);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(129, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Rapport de Pointage";
            // 
            // btn_rpt_pointage
            // 
            this.btn_rpt_pointage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            this.btn_rpt_pointage.Location = new System.Drawing.Point(167, 127);
            this.btn_rpt_pointage.Name = "btn_rpt_pointage";
            this.btn_rpt_pointage.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.btn_rpt_pointage.Size = new System.Drawing.Size(38, 32);
            this.btn_rpt_pointage.TabIndex = 4;
            this.btn_rpt_pointage.Click += new System.EventHandler(this.btn_rpt_pointage_Click);
            // 
            // Frm_Report_Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 342);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btn_rpt_pointage);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.btn_EtatJournal);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btn_rpt_28J);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Report_Config";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_Report_Config";
            this.Load += new System.EventHandler(this.Frm_Report_Config_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btn_rpt_28J;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btn_EtatJournal;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btn_rpt_pointage;
    }
}