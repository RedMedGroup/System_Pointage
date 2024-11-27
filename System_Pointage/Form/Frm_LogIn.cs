using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System_Pointage.Form
{
    public partial class Frm_LogIn : DevExpress.XtraEditors.XtraForm
    {
        public Frm_LogIn()
        {
            InitializeComponent();
            CheckForUpdates();
        }
        public static string NamUser;
        public static int IDUser;
        public static int _IDAccessPoste;
        void Login()
        {
            using (var db = new DAL.DataClasses1DataContext())
            {
                var userName = txt_UserName.Text;
                var passWord = txt_UserPWD.Text;

                var user = db.Users.SingleOrDefault(x => x.UserName == userName);
                if (user == null)
                {
                    XtraMessageBox.Show(
                         text: "Le nom d'utilisateur ou le mot de passe est incorrect",
                        caption: "",
                        icon: MessageBoxIcon.Error,
                        buttons: MessageBoxButtons.OK
                        );
                    return;
                }
                else
                {
                    if (user.IsActive == false)
                    {
                        XtraMessageBox.Show(
                      text: "Ce compte a été désactivé veuillez contacter l'administrateur",
                      caption: "",
                      icon: MessageBoxIcon.Error,
                      buttons: MessageBoxButtons.OK);
                        return;
                    }
                    var passWordHash = user.Password;
                    // var hasher = new Liphsoft.Crypto.Argon2.PasswordHasher();
                    // if (hasher.Verify(passWordHash, passWord))
                    if (txt_UserPWD.Text == user.Password && txt_UserName.Text == user.UserName)
                    {
                        this.Hide();
                        //SplashScreenManager.ShowForm(typeof(SplashScreen1));
                        // StartScreen1.Threading.Thread.Sleep(500);
                        //   System.Threading.Thread.Sleep(100);


                        // put loading over here
                        Classe.Master.SetUser(user);
                        NamUser = user.Name;
                        IDUser = user.ID;
                       // _IDAccessPoste = user.IDAccessPoste;
                       //Type t = typeof(Classe.Session);
                       //  var propertys = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                       //  var p = Class.Session.ProductsView.ToList();
                       //foreach (var item in propertys)
                       //{
                       //    var obj = item.GetValue(null);
                       //}                        ///////////
                        Frm_Main.Instance.Show();
                        // this.Close();
                        //SplashScreenManager.CloseForm();
                        return;

                        ///////////////////////////
                    }
                    else
                        goto LogInFaild;
                }

            }

        LogInFaild:
            XtraMessageBox.Show(
                      text: "Le nom d'utilisateur ou le mot de passe est incorrect",
                      caption: "",
                      icon: MessageBoxIcon.Error,
                      buttons: MessageBoxButtons.OK
                      );
            return;
        }
   

        private void Frm_LogIn_Load(object sender, EventArgs e)
        {

        }
        public async Task CheckForUpdates()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string apiUrl = "https://api.github.com/repos/RedMedGroup/System_Pointage/releases/latest";

            string currentVersion = "1.0.0.1";

            using (WebClient webClient = new WebClient())
            {
                // إعداد User-Agent مطلوب لطلب GitHub API
                webClient.Headers.Add("User-Agent", "request");

                // إرسال طلب API والحصول على البيانات بصيغة JSON
                string json = webClient.DownloadString(apiUrl);
                JObject release = JObject.Parse(json);

                // استخراج رقم الإصدار الأخير من JSON
                string latestVersion = release["tag_name"].ToString();

                if (latestVersion != currentVersion)
                {
                    if (MessageBox.Show("Voulez-vous installer la mise à jour ?", "Une nouvelle mise à jour est disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // استخراج رابط تحميل التحديث من JSON
                        string downloadUrl = release["assets"][0]["browser_download_url"].ToString();

                        // تحميل التحديث وتشغيله
                        string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Sonatrach_Pointage_New.exe");
                        webClient.DownloadFile(downloadUrl, tempPath);
                        Process.Start(tempPath);
                        System.Windows.Forms.Application.Exit();
                    }
                }
                //else
                //{
                //    MessageBox.Show("التطبيق محدث بالفعل.", "لا توجد تحديثات متاحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
        }

        private void checkboxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxShowPass.Checked)
            {
                txt_UserPWD.PasswordChar = '\0';
            }
            else
            {
                txt_UserPWD.PasswordChar = '*';

            }
        }

        private void btn_log_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            SQL_Server_Config frm = new SQL_Server_Config();
            frm.ShowDialog();
        }
    }
}