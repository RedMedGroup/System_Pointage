using DevExpress.Utils.Extensions;
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
    public partial class Frm_OP_Materiels : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.MaterielsStatus> activeAgentsList;
        private BindingList<Models.MaterielsStatus> transferredAgentsList;
        public event EventHandler<BindingList<Models.AgentStatus>> AgentsTransferred;

        public Frm_OP_Materiels()
        {
            InitializeComponent();
        }

        private void Frm_OP_Materiels_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            gridView2.OptionsBehavior.Editable = true;
            transferredAgentsList = new BindingList<Models.MaterielsStatus>();
            gridControl2.DataSource = transferredAgentsList;

            RefrecheData();
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            var selectedRow = gridView1.GetFocusedRow() as Models.MaterielsStatus;

            if (selectedRow != null)
            {
                // نقل السطر إلى القائمة الثانية
                transferredAgentsList.Add(selectedRow);

                // إزالة السطر من القائمة الأولى
                activeAgentsList.Remove(selectedRow);
              //  SomeMethod();
            }
        }
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            // جلب البيانات
            activeAgentsList = agentDataService.GetMaterieltStatuses();


            //SomeMethod();

            //GridName();
        }
    }
}