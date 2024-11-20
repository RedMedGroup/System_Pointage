namespace System_Pointage.Form
{
    partial class Frm_LogIn
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
            this.btn_log = new System.Windows.Forms.Button();
            this.txt_UserName = new DevExpress.XtraEditors.TextEdit();
            this.txt_UserPWD = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserPWD.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_log
            // 
            this.btn_log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btn_log.FlatAppearance.BorderSize = 0;
            this.btn_log.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.btn_log.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_log.ForeColor = System.Drawing.Color.LightGray;
            this.btn_log.Location = new System.Drawing.Point(60, 284);
            this.btn_log.Name = "btn_log";
            this.btn_log.Size = new System.Drawing.Size(317, 31);
            this.btn_log.TabIndex = 10;
            this.btn_log.Text = "Connexion";
            this.btn_log.UseVisualStyleBackColor = false;
            this.btn_log.Click += new System.EventHandler(this.btn_log_Click);
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(94, 89);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(203, 20);
            this.txt_UserName.TabIndex = 11;
            // 
            // txt_UserPWD
            // 
            this.txt_UserPWD.Location = new System.Drawing.Point(94, 187);
            this.txt_UserPWD.Name = "txt_UserPWD";
            this.txt_UserPWD.Size = new System.Drawing.Size(218, 20);
            this.txt_UserPWD.TabIndex = 12;
            // 
            // Frm_LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 451);
            this.Controls.Add(this.txt_UserPWD);
            this.Controls.Add(this.txt_UserName);
            this.Controls.Add(this.btn_log);
            this.Name = "Frm_LogIn";
            this.Text = "Frm_LogIn";
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_UserPWD.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_log;
        private DevExpress.XtraEditors.TextEdit txt_UserName;
        private DevExpress.XtraEditors.TextEdit txt_UserPWD;
    }
}