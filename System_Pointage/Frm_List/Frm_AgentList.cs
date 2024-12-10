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
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Card.ViewInfo;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.XtraExport.Helpers;

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
               // gridControl1.DataSource = data;
                if (data == null || !data.Any())
                {
                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("ID", typeof(int));
                    emptyTable.Columns.Add("AgentID", typeof(int));
                    emptyTable.Columns.Add("Matricule", typeof(string));
                    emptyTable.Columns.Add("Name", typeof(string));
                    emptyTable.Columns.Add("PostName", typeof(string));
                    emptyTable.Columns.Add("Affecter", typeof(bool));
                    emptyTable.Columns.Add("Jour", typeof(string));
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
                gridView1.GroupPanelText = " ";
                // تحديث التسميات
                gridView1.Columns["Name"].Caption = "Nom et prénom";
                gridView1.Columns["PostName"].Caption = "Poste";
                gridView1.Columns["Jour"].Caption = "Systéme";
                gridView1.Columns["AgentID"].Visible = false;
                gridView1.Columns["ID"].Visible = false;
                gridView1.Columns["ScreenPosteD"].Visible = false;
                gridView1.Columns["AccessPosteID"].Visible = false;
                gridView1.Columns["NamePosteDetail"].Visible = false;
                gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            }
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            // تحقق إذا كان العمود هو "Name" و إذا كانت الحالة في العمود "Status" هي false
            if (e.RowHandle >= 0 && e.Column.FieldName == "Name")
            {
                // استرجاع القيمة من عمود "Status"
                bool status = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "Statut"));

                // إذا كانت القيمة في عمود "Status" هي false
                if (!status)
                {
                    // تخصيص لون النص باللون الأحمر
                    e.Appearance.ForeColor = Color.Red;

                    // رسم النص مع السهم
                    string arrow = "→"; // السهم الذي تريده
                    e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);

                    // منع الرسم الافتراضي
                    e.Handled = true;
                }
            }
        }

        //private void GridView1_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // التحقق من أن sender هو CardView
        //        if (sender is CardView view)
        //        {
        //            // التحقق من أن e هو DXMouseEventArgs
        //            if (e is DXMouseEventArgs ea)
        //            {
        //                // استخدام CardHitInfo بدلاً من GridHitInfo
        //                CardHitInfo info = view.CalcHitInfo(ea.Location);

        //                if (info.InCard)
        //                {
        //                    // الحصول على قيمة الحقل "ID"
        //                    object idValue = view.GetFocusedRowCellValue("ID");
        //                    if (idValue != null && int.TryParse(idValue.ToString(), out int id))
        //                    {
        //                        OpenForm(id); // فتح النموذج بناءً على ID
        //                    }
        //                    else
        //                    {
        //                        MessageBox.Show("لا توجد قيمة صالحة لـ 'ID' في العنصر المحدد.");
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show("لم يتم النقر على عنصر صالح.");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("حدث خطأ: لم يتم توفير حدث ماوس صالح.");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("حدث خطأ: العنصر ليس من نوع CardView.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // التعامل مع الأخطاء غير المتوقعة
        //        MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}");
        //    }
        //}


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

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Frm_Fiche_Agent_Import frm = new Frm_Fiche_Agent_Import();
            frm.ShowDialog();
        }

        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.ShowPrintPreview();
        }
    }
}