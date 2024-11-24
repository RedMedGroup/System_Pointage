using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class SQL_Server_Config : DevExpress.XtraEditors.XtraForm
    {
        private string STR____;
        private bool IsOk_;
        private TextBox txtCon_Setring;
        public SQL_Server_Config()
        {
            InitializeComponent();
            txtCon_Setring = new TextBox();

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "Caramel";
        }

        private void SQL_Server_Config_Load(object sender, EventArgs e)
        {
            if (ComboBoxEdit2 != null)
            {
                ComboBoxEdit2.Properties.Items.Add("Windows Authentication");
                ComboBoxEdit2.Properties.Items.Add("SQL Server Authentication");
            }
            else
            {
                MessageBox.Show("ComboBoxEdit2 n'est pas initialisé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static List<string> GetDatabaseNames(string connectionString)
        {
            List<string> databaseNames = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // استعلام SQL لاسترجاع أسماء قواعد البيانات
                    using (SqlCommand command = new SqlCommand("SELECT name FROM sys.databases", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                databaseNames.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // معالجة الأخطاء (يمكنك إضافة كود لعرض رسالة خطأ هنا)
                    Console.WriteLine("Error retrieving database names: " + ex.Message);
                }
            }

            return databaseNames;
        }
        private void LoadDatabases()
        {
            string serverInstance = ComboBoxEdit1.Text.Trim(); // اسم الخادم من ComboBox
            string connectionString = $"Data Source={serverInstance};Integrated Security=True;"; // سلسلة الاتصال باستخدام الأمان المدمج
                                                                                                 // أو إذا كنت تستخدم SQL Server Authentication
                                                                                                 // string connectionString = $"Data Source={serverInstance};User ID=your_username;Password=your_password;";

            List<string> databaseNames = DatabaseHelper.GetDatabaseNames(connectionString);

            // إضافة أسماء قواعد البيانات إلى ComboBoxEdit3
            ComboBoxEdit3.Properties.Items.Clear(); // امسح العناصر السابقة
            foreach (var dbName in databaseNames)
            {
                ComboBoxEdit3.Properties.Items.Add(dbName);
            }

            // تفريغ النص في ComboBoxEdit3 بعد تحديث العناصر
            ComboBoxEdit3.Text = "";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ComboBoxEdit1.Text))
            {
                // قم بتفريغ العناصر السابقة في ComboBoxEdit3
                ComboBoxEdit3.Properties.Items.Clear();

                // الحصول على اسم الخادم من ComboBoxEdit1
                string serverInstance = ComboBoxEdit1.Text.Trim();

                // بناء سلسلة الاتصال
                string connectionString = $"Data Source={serverInstance};Integrated Security=True;"; // استخدام الأمان المدمج
                                                                                                     // إذا كنت تستخدم SQL Server Authentication، يمكنك استخدام الكود التالي:
                                                                                                     // string connectionString = $"Data Source={serverInstance};User ID=your_username;Password=your_password;";

                // استدعاء دالة GetDatabaseNames للحصول على أسماء قواعد البيانات
                var databaseNames = DatabaseHelper.GetDatabaseNames(connectionString);

                // إضافة أسماء قواعد البيانات إلى ComboBoxEdit3
                foreach (var dbName in databaseNames)
                {
                    ComboBoxEdit3.Properties.Items.Add(dbName);
                }

                // تفريغ النص في ComboBoxEdit3 بعد تحديث العناصر
                ComboBoxEdit3.Text = "";
            }
        }
        private void Fill_Combo_Database()
        {
            try
            {
                ComboBoxEdit3.Properties.Items.Clear();
                DataSet DS = new DataSet();
                using (SqlConnection SqlConnection1 = new SqlConnection("Data Source=" + ComboBoxEdit1.Text + ";Initial Catalog=tempdb;Integrated Security=SSPI;"))
                {
                    string str = "SELECT DISTINCT name FROM master.dbo.sysdatabases WHERE name NOT IN('master','msdb','tempdb' ,'model') AND has_dbaccess(name) = 1 ORDER BY name";
                    SqlDataAdapter ADP = new SqlDataAdapter(str, SqlConnection1);
                    DS.Clear();
                    ADP.Fill(DS);
                    ComboBoxEdit3.Properties.Items.Clear();

                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        ComboBoxEdit3.Properties.Items.Add(DS.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                    ADP.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void checker__()
        {
            IsOk_ = true;
            if (ComboBoxEdit1.Text == "")
            {
                MessageBox.Show("يجب إختيار اسم السيرفر\nمن فضلك أعد المحاولة مرة أخرى", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxEdit1.Focus();
                IsOk_ = false;
                return;
            }

            if (ComboBoxEdit2.SelectedIndex == -1)
            {
                MessageBox.Show("يجب إختيار طريقة الولوج للسيرفر\nمن فضلك أعد المحاولة مرة أخرى", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ComboBoxEdit2.Focus();
                IsOk_ = false;
                return;
            }

            if (ComboBoxEdit2.SelectedIndex == 1)
            {
                if (TextEdit1.Text.Length == 0)
                {
                    MessageBox.Show("يجب كتابة اسم المستخدم\nمن فضلك أعد المحاولة مرة أخرى", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextEdit1.Focus();
                    IsOk_ = false;
                    return;
                }

                if (TextEdit2.Text.Length == 0)
                {
                    MessageBox.Show("يجب كتابة  كلمة المرور\nمن فضلك أعد المحاولة مرة أخرى", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TextEdit2.Focus();
                    IsOk_ = false;
                    return;
                }
            }

            IsOk_ = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            checker__();
            if (IsOk_)
            {
                using (SqlConnection SqlConn = new SqlConnection())
                {
                    if (ComboBoxEdit2.SelectedIndex == 0)
                    {
                        SqlConn.ConnectionString = "Data Source=" + ComboBoxEdit1.Text.Trim() + ";Initial Catalog=master;Integrated Security=True";
                    }
                    else if (ComboBoxEdit2.SelectedIndex == 1)
                    {
                        SqlConn.ConnectionString = "Data Source=" + ComboBoxEdit1.Text.Trim() + ";Initial Catalog=master;User ID=" + TextEdit1.Text.Trim() + ";Password=" + TextEdit2.Text;
                    }

                    try
                    {
                        SqlConn.Open();
                        MessageBox.Show("تم الأتصال بالسيرفر بنجاح", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception)
                    {
                        XtraMessageBox.Show("فشل الأتصال بالسيرفر\nمن فضلك أعد المحاولة مرة أخرى", "رسالة تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ComboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextEdit1.ReadOnly = ComboBoxEdit2.SelectedIndex == 0;
            TextEdit2.ReadOnly = ComboBoxEdit2.SelectedIndex == 0;
        }

        private void ComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDatabases();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (ComboBoxEdit2.Text == "SQL Server Authentication")
            {
                if (TextEdit1.Text == "" || TextEdit2.Text == "")
                {
                    XtraMessageBox.Show("الرجاء الإلتزام بتعبئة الحقول الإجبارية", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TextEdit1.BackColor = Color.Red;
                    TextEdit2.BackColor = Color.Red;
                    return;
                }
            }


            Properties.Settings.Default.Server_Globale = ComboBoxEdit1.Text.Trim();
            Properties.Settings.Default.Database_Globale = ComboBoxEdit3.Text.Trim();
            Properties.Settings.Default.Mode_Globale = ComboBoxEdit2.SelectedIndex == 0 ? "Windows" : "SQL";
            Properties.Settings.Default.Utilisateur_Globale = TextEdit1.Text.Trim();
            Properties.Settings.Default.Password_Globale = TextEdit2.Text.Trim();
            if (ComboBoxEdit1 != null && ComboBoxEdit3 != null && TextEdit1 != null && TextEdit2 != null)
            {
                string dataSource = ComboBoxEdit1.Text;
                string initialCatalog = ComboBoxEdit3.Text;
                string userId = TextEdit1.Text;
                string password = TextEdit2.Text;

                if (ComboBoxEdit2.Text == "Windows Authentication")
                {
                    txtCon_Setring.Text = "Data Source=" + ComboBoxEdit1.Text + "; Initial Catalog=" + ComboBoxEdit3.Text + "; Integrated Security=True";
                }
                else if (ComboBoxEdit2.Text == "SQL Server Authentication")
                {
                    if (!string.IsNullOrEmpty(dataSource) && !string.IsNullOrEmpty(initialCatalog) && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
                    {
                        txtCon_Setring.Text = $"Data Source={dataSource}; Initial Catalog={initialCatalog}; User ID={userId}; Password={password};";
                        //txtCon_Setring.Text = "Data Source=" + ComboBoxEdit1.Text + "; Initial Catalog=" + ComboBoxEdit3.Text + "; User ID=" + TextEdit1.Text + "; Password=" + TextEdit2.Text + ";";
                    }
                    else
                    {
                        MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            Properties.Settings.Default.CON_STRING = txtCon_Setring.Text;
            Properties.Settings.Default.Save();

            XtraMessageBox.Show("Server = " + Properties.Settings.Default.Server_Globale + " قاعدة البيانات = " + Properties.Settings.Default.Database_Globale + " اسم المستخدم = " + Properties.Settings.Default.Utilisateur_Globale + " كلمة المرور = " + Properties.Settings.Default.Password_Globale);
            XtraMessageBox.Show("تمت بنجاح عملية حفظ بيانات الإتصال بإعدادات المشروع", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Restart();
        }
    }
}