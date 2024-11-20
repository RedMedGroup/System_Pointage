using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }
        public static string NamUser;
        public static int IDUser;
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
        private void btn_log_Click(object sender, EventArgs e)
        {
            Login();
        }
    }
}