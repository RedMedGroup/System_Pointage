using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System_Pointage.Form
{
    public partial class BackUp : DevExpress.XtraEditors.XtraForm
    {
        public BackUp()
        {
            InitializeComponent();
        }

        private void BackUp_Load(object sender, EventArgs e)
        {
            txt_Line.Click += Txt_Line_Click;
            txt_DataBase.Text = Properties.Settings.Default.Database_Globale;
            txt_Server.Text = Properties.Settings.Default.Server_Globale;
        }

        private void Txt_Line_Click(object sender, EventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = @"C:\";
            open.Title = "Browser";
            open.CheckFileExists = true;
            open.CheckPathExists = true;
            open.DefaultExt = "BAK";
            open.Filter = "text (*.bak)|*.bak";
            open.FilterIndex = 2;
            open.RestoreDirectory = true;
            open.ReadOnlyChecked = true;
            open.ShowReadOnly = true;

            if (open.ShowDialog() == DialogResult.OK)
            {
                txt_Line.Text = open.FileName;
            }
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            string servername = txt_Server.Text;
            string dbname = txt_DataBase.Text;

            SqlConnection con = new SqlConnection(@"Data Source =" + servername + ";Integrated Security =true; Initial Catalog =" + dbname + "");
            //SqlConnection con = new SqlConnection(@"Data Source=" + servername + ";Initial Catalog=" + dbname + ";User ID=sa;Password=123;");
            //SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-647FREP\SQL2014;Initial Catalog=RedMed_TFT;User ID=sa;Password=123;Integrated Security=False;");

            con.Open();//DESKTOP-647FREP\SQL2014RedMed_TFT
            string str = "USE master;";
            string str1 = "ALTER DATABASE " + dbname + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
            string str2 = "RESTORE DATABASE " + dbname + " FROM DISK = '" + txt_Line.Text + "' WITH REPLACE ";
            SqlCommand smd = new SqlCommand(str, con);
            SqlCommand smd1 = new SqlCommand(str1, con);
            SqlCommand smd2 = new SqlCommand(str2, con);
            smd.ExecuteNonQuery();
            smd1.ExecuteNonQuery();
            smd2.ExecuteNonQuery();
            XtraMessageBox.Show("Restauré avec succès");
            con.Close();
            Application.Exit();
            this.Hide();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (txt_Server.Text == "")
            {
                XtraMessageBox.Show("Veuillez entrer le serveur de connexion");
                return;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Sélectionnez le dossier de sauvegarde";

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                string backupFolder = Path.Combine(folderPath, "Sauvegardes");

                bool directoryExists = Directory.Exists(backupFolder);
                if (!directoryExists)
                {
                    Directory.CreateDirectory(backupFolder);
                    XtraMessageBox.Show("Le dossier des sauvegardes a été créé avec succès");
                    CreateBackup(backupFolder);
                }
                else
                {
                    CreateBackup(backupFolder);
                }
            }
        }
        private void CreateBackup(string backupFolder)
        {
            DateTime currentDate = DateTime.Now;
            string dateFormatted = currentDate.ToString("dd-MM-yyyy");

            string serverName = txt_Server.Text;
            string dbName = txt_DataBase.Text;

            string connectionString = @"Data Source=" + serverName + ";Integrated Security=true; Initial Catalog=" + dbName + ";";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                string backupPath = Path.Combine(backupFolder, dbName + "_" + dateFormatted + ".Bak");

                // تحقق مما إذا كان المسار متاحًا
                if (!Directory.Exists(backupFolder))
                {
                    XtraMessageBox.Show("Le répertoire pour la sauvegarde n'existe pas");
                    return;
                }

                string backupQuery = $"BACKUP DATABASE {dbName} TO DISK = '{backupPath}' WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', NAME = 'Full Backup of {dbName}';";

                SqlCommand backupCommand = new SqlCommand(backupQuery, connection);
                backupCommand.ExecuteNonQuery();

                XtraMessageBox.Show("Les sauvegardes ont été créées avec succès");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Une erreur s'est produite lors de la création des sauvegardes: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}