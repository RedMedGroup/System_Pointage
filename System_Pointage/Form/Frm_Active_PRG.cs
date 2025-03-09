using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Active_PRG : DevExpress.XtraEditors.XtraForm
    {
        DAL.activation activation;
        public Frm_Active_PRG()
        {
            InitializeComponent(); New();
        }

        private void Frm_Active_PRG_Load(object sender, EventArgs e)
        {
            var db = new DAL.DataClasses1DataContext();

            var existingActivation = db.activations
                .FirstOrDefault(x => x.DISCK_HD.Trim() == Get_DISCK_HD().Trim());

            if (existingActivation != null) 
            {
                if (existingActivation.Statut == true) 
                { 
                    this.Close();
                    Thread th = new Thread(OpenLoginForm);
                    th.SetApartmentState(ApartmentState.STA);
                    th.Start();
                }
                else
                {
                    XtraMessageBox.Show("Une demande d'activation a été envoyée pour cet appareil. Veuillez patienter pendant la vérification de votre demande.",
                      "Demande d'activation en cours",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information);
                    this.Close();
                }
            }


        }
        public void OpenLoginForm()
        {
            Application.Run(new Frm_LogIn());
        }
        void New()
        {
            activation = new DAL.activation();
        }
        public string Get_DISCK_HD()
        {
            string SerialNumber = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM win32_DiskDrive");
            foreach (ManagementObject info in searcher.Get())
            {
                SerialNumber = (info["SerialNumber"].ToString());
            }
            string HD_serial = SerialNumber;
            string mytext = HD_serial;
            char[] myChars = mytext.ToCharArray();
            HD_serial = "";
            foreach (char ch in myChars)
            {
                if (char.IsDigit(ch))
                    HD_serial += (ch);
            }
            return HD_serial;
        }
        static string GetCpuName()
        {
            string cpuName = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                cpuName = obj["Name"].ToString();
                break; // نأخذ أول قيمة فقط
            }

            return cpuName;
        }
        public string GetComputerName()
        {
            return Environment.MachineName;
        }
        public static string GetConnectedNetworkName()
        {
            foreach (NetworkInterface network in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (network.OperationalStatus == OperationalStatus.Up &&
                    network.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    return "Ethernet";
                }
            }

            string ssid = "Hors ligne";
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh wlan show interfaces",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Match match = Regex.Match(output, @"SSID\s*:\s*(.+)");
            if (match.Success)
            {
                ssid = match.Groups[1].Value.Trim();
            }

            return ssid;
        }
        
        bool IsValidit()
        {       
            var db = new DAL.DataClasses1DataContext();
            if (db.activations.Where(x => x.ID != activation.ID && x.DISCK_HD.Trim() == Get_DISCK_HD().Trim()).Count() > 0)
            {
                XtraMessageBox.Show("tm");
                return false;
            }
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = Master.ErrorText;
                return false;
            }
            return true;
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (IsValidit() == false)
                return;
            var db = new DAL.DataClasses1DataContext();
            if (activation.ID == 0)
            {
                db.activations.InsertOnSubmit(activation);
                activation.Poste = txt_Name.Text;
                activation.ComputerName= GetComputerName();
                activation.NetworkName=GetConnectedNetworkName();
                activation.CpuName=GetCpuName();
                activation.DISCK_HD= Get_DISCK_HD();
                activation.Date = DateTime.Now;
                activation.Statut=false;
                db.SubmitChanges();

                XtraMessageBox.Show("Une demande d'activation a été envoyée pour cet appareil. Veuillez patienter pendant la vérification de votre demande.",
                  "Demande d'activation en cours",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {
            SQL_Server_Config frm = new SQL_Server_Config();
            frm.ShowDialog();
        }
    }
}