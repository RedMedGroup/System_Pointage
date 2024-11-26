using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Heir : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.AgentStatus> activeAgentsList;

        public Frm_Heir()
        {
            InitializeComponent();
        }

        private void Frm_Heir_Load(object sender, EventArgs e)
        {
            RefrecheData();
            gridView1.GroupPanelText =" ";
            gridView1.RefreshData();
        }
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            activeAgentsList = agentDataService.GetAgentStatuses(userAccessPosteID, Master.MVMType.CR, isAdmin);

            gridControl1.DataSource = activeAgentsList;

            GridName();
        }

        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["Poste"].VisibleIndex = 2;
            gridView1.Columns["Affecter"].VisibleIndex = 3;
            gridView1.Columns["Date"].VisibleIndex = 4;
            gridView1.Columns["Jour"].VisibleIndex = 5;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 6;
            gridView1.Columns["DaysCount"].VisibleIndex = 7;
            gridView1.Columns["Statut"].VisibleIndex = 8;
            gridView1.Columns["Date"].Caption = "Date de rentrée";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
            gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
            gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
           
        }
    }
}