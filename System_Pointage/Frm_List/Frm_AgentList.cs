using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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
using System_Pointage.Form;
using System_Pointage.Classe;

namespace System_Pointage.Frm_List
{
    public partial class Frm_AgentList : DevExpress.XtraEditors.XtraForm
    {
        private AgentDataService _agentDataService;
        public Frm_AgentList()
        {
            InitializeComponent();
        }

        private void Frm_AgentList_Load(object sender, EventArgs e)
        {
            gridView1.DoubleClick += GridView1_DoubleClick;
            gridView1.OptionsBehavior.Editable = false;

            using (var dbContext = new DAL.DataClasses1DataContext())
            {
                _agentDataService = new AgentDataService(dbContext);

                // التحقق من نوع المستخدم
                bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;
                int? userAccessPosteID = isAdmin ? null : Master.User.IDAccessPoste;

                // استدعاء الخدمة للحصول على البيانات
                var data = _agentDataService.GetAgents(userAccessPosteID, isAdmin);

                // تعيين البيانات إلى الجدول
                gridControl1.DataSource = data;
             //   gridView1.Columns["ID"].Visible = false;

                // تحديث التسميات
                gridView1.Columns["Name"].Caption = "Nom et prénom";
                gridView1.Columns["PostName"].Caption = "Poste";
                gridView1.Columns["Jour"].Caption = "Systéme";
                gridView1.Columns["AgentID"].Visible = false;
                gridView1.Columns["ID"].Visible = false;
                gridView1.Columns["ScreenPosteD"].Visible = false;
                gridView1.Columns["AccessPosteID"].Visible = false;
                gridView1.Columns["NamePosteDetail"].Visible = false;
            }
        }
        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);

            if (info.InRow || info.InRowCell)
            {
                // الحصول على قيمة ID
                int id = Convert.ToInt32(view.GetFocusedRowCellValue("ID"));
                OpenForm(id);
            }
        }
        public virtual void OpenForm(int id)
        {
            var frm = new Form.Frm_Fiche_Ajent(id);
            frm.ShowDialog();
        }
        //void RefrechData(int? userAccessPosteID)
        //{
        //    using (var db = new DAL.DataClasses1DataContext())
        //    {
        //        // استعلام أساسي لجميع البيانات
        //        var query = from ag in db.Fiche_Agents
        //                    join post in db.Fiche_Postes on ag.ID_Post equals post.ID
        //                    join userAccess in db.UserAccessProfilePostes on ag.ScreenPosteD equals userAccess.ID
        //                    join accessDetails in db.UserAccessProfilePosteDetails on userAccess.ID equals accessDetails.ID_AccessPoste
        //                    select new
        //                    {
        //                        ag.ID,
        //                        Name = ag.Name,
        //                        Post = post.Name,                             
        //                        AccessPosteID = accessDetails.ID_AccessPoste
        //                    };

        //        // إذا كان المستخدم Admin، عرض الجميع
        //        if (Master.User.UserType == (byte)Master.UserType.Admin)
        //        {
        //            query = query.Distinct(); // عرض الجميع دون تصفية
        //        }
        //        else
        //        {
        //            // المستخدم العادي: تطبيق التصفية
        //            if (userAccessPosteID.HasValue)
        //            {
        //                query = query.Where(q => q.AccessPosteID == userAccessPosteID.Value);
        //            }
        //            else
        //            {
        //                query = query.Where(q => false); // إذا لم يتم تمرير معرف القسم
        //            }
        //        }

        //        // جلب البيانات مع إزالة التكرار
        //        var data = query.Distinct().ToList();

        //        // تعيين البيانات لمصدر بيانات الجدول
        //        gridControl1.DataSource = data;

        //        // إخفاء عمود ID
        //        if (gridView1.Columns["ID"] != null)
        //        {
        //            gridView1.Columns["ID"].Visible = false;
        //        }
        //    }
        //}





        //void RefrechData()
        //{
        //    var db = new DAL.DataClasses1DataContext();
        //    var data = from ag in db.Fiche_Agents
        //               join post in db.Fiche_Postes on ag.ID_Post equals post.ID
        //               select new
        //               {
        //                   ag.ID,
        //                   Name = ag.Name,
        //                   Post = post.Name,
        //               };
        //    gridControl1.DataSource = data;
        //    gridView1.Columns["ID"].Visible = false;
        //}
    }
}