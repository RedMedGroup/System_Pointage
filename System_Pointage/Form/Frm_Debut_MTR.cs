using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class Frm_Debut_MTR : DevExpress.XtraEditors.XtraForm
    {
        private AgentDataService _agentDataService;

        DAL.MVMmaterielsDetail mvm;
        public Frm_Debut_MTR()
        {
            InitializeComponent();
            New();
        }

        private void Frm_Debut_MTR_Load(object sender, EventArgs e)
        {
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            RefrechDate();
        }
        void RefrechDate()
        {
            using (var dbContext = new DAL.DataClasses1DataContext())
            {
                _agentDataService = new AgentDataService(dbContext);

                // التحقق من نوع المستخدم
                bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;
                int? userAccessPosteID = isAdmin ? null : Master.User.IDAccessPoste;

                // استدعاء الخدمة للحصول على البيانات
                //var data = _agentDataService.GetAgents(userAccessPosteID, isAdmin);
                var data = _agentDataService.GetAgentsWithoutMovementMateriels(userAccessPosteID, isAdmin);


                if (data == null || !data.Any())
                {
                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("ID", typeof(int));
                    emptyTable.Columns.Add("AgentID", typeof(int));
                    emptyTable.Columns.Add("Matricule", typeof(string));
                    emptyTable.Columns.Add("Name", typeof(string));
                    emptyTable.Columns.Add("PostName", typeof(string));
                    emptyTable.Columns.Add("Affecter", typeof(bool));
                    emptyTable.Columns.Add("Date_Embauche", typeof(DateTime));
                    emptyTable.Columns.Add("Statut", typeof(string));
                    emptyTable.Columns.Add("ScreenPosteD", typeof(int));
                    emptyTable.Columns.Add("AccessPosteID", typeof(int));
                    emptyTable.Columns.Add("NamePosteDetail", typeof(string));

                    gridControl1.DataSource = emptyTable;
                }
                else
                {
                    gridControl1.DataSource = data;
                }

                // تحديث التسميات
                gridView1.Columns["Name"].Caption = "Nom et prénom";
                gridView1.Columns["PostName"].Caption = "Poste";
                gridView1.Columns["AgentID"].Visible = false;
                gridView1.Columns["ID"].Visible = false;
                gridView1.Columns["ScreenPosteD"].Visible = false;
                //gridView1.Columns["AccessPosteD"].Visible = false;
                //gridView1.Columns["NamePosteDetail"].Visible = false;
            }

        }
        void SetData()
        {
            mvm.Statut = txt_Statut.Text;
            mvm.Date = dateEdit1.DateTime;

        }
        void GetData()
        {
            txt_Statut.Text = mvm.Statut;
            dateEdit1.DateTime = mvm.Date;
        }
        void New()
        {
            mvm = new DAL.MVMmaterielsDetail();

        }
        bool IsValidit()
        {
            if (txt_Statut.Text.Trim() == string.Empty)
            {
                txt_Statut.Text =Master.ErrorText;
                return false;
            }

            return true;
        }
    }
}