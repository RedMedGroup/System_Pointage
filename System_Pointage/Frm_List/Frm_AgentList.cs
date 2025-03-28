﻿using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System_Pointage.Classe;
using System_Pointage.Form;

namespace System_Pointage.Frm_List
{
    public partial class Frm_AgentList : DevExpress.XtraEditors.XtraForm
    {
        private AgentDataService _agentDataService;

        public Frm_AgentList()
        {
            InitializeComponent();
        }

        private StatusStrip statusStrip = new StatusStrip();
        private ToolStripStatusLabel statusLabel = new ToolStripStatusLabel();

        private void Frm_AgentList_Load(object sender, EventArgs e)
        {
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);

            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;

            #region paramater gridview     ////////////////
            ///
            gridView1.OptionsView.ShowFooter = true;
            gridView1.FooterPanelHeight = 70;

            Color highPriority = Color.Green;
            Color normalPriority = Color.Orange;
            Color lowPriority = Color.Red;
            //int markWidth = 16;
            ///
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " ";

            #endregion paramater gridview     ////////////////

            gridView1.OptionsBehavior.Editable = false;

            using (var dbContext = new DAL.DataClasses1DataContext())
            {
                _agentDataService = new AgentDataService(dbContext);

                bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager || Master.User.UserType == (byte)Master.UserType.Guest;

                int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

                var data = _agentDataService.GetAgents(userAccessPosteID, isAdminOrManager);

                if (data == null || !data.Any())
                {
                    DataTable emptyTable = new DataTable();
                    emptyTable.Columns.Add("ID", typeof(int));
                    emptyTable.Columns.Add("AgentID", typeof(int));
                    emptyTable.Columns.Add("Matricule", typeof(string));
                    emptyTable.Columns.Add("Name", typeof(string));
                    emptyTable.Columns.Add("FirstName", typeof(string));
                    emptyTable.Columns.Add("PostName", typeof(string));
                    emptyTable.Columns.Add("Affecter", typeof(bool));
                    emptyTable.Columns.Add("Jour", typeof(string));
                    emptyTable.Columns.Add("Date_Embauche", typeof(DateTime));
                    emptyTable.Columns.Add("Statut", typeof(string));
                    emptyTable.Columns.Add("ScreenPosteD", typeof(int));
                    emptyTable.Columns.Add("AccessPosteID", typeof(int));
                    emptyTable.Columns.Add("NamePosteDetail", typeof(string));
                    emptyTable.Columns.Add("Statutmvm", typeof(string));
                    emptyTable.Columns.Add("PostID", typeof(string));

                    gridControl1.DataSource = emptyTable;
                }
                else
                {
                    gridControl1.DataSource = data;
                }

                gridView1.Columns["Name"].Caption = "Nom";
                gridView1.Columns["FirstName"].Caption = "Prénom";
                gridView1.Columns["Date_Embauche"].Caption = "Date Affectée";
                gridView1.Columns["Statut"].Caption = "Active/Inactive";
                gridView1.Columns["Statutmvm"].Caption = "Statut";
                gridView1.Columns["PostName"].Caption = "Poste";
                gridView1.Columns["AgentID"].Visible = false;
                gridView1.Columns["ID"].Visible = false;
                gridView1.Columns["ScreenPosteD"].Visible = false;
                gridView1.Columns["AccessPosteID"].Visible = false;
                gridView1.Columns["NamePosteDetail"].Visible = false;
                gridView1.Columns["PostID"].Visible = false;
                gridView1.Columns["Statutmvm"].Visible = false;

                gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            }
            gridView1.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.True;
            //gridView1.CustomDrawFooter += (s, args) =>
            //{ 
            //    int offset = 5;
            //    args.DefaultDraw();
            //    Color color = highPriority;
            //    Rectangle markRectangle;
            //    string priorityText = " - Pésent";
            //    for (int i = 0; i < 3; i++)
            //    {
            //        if (i == 1)
            //        {
            //            color = normalPriority;
            //            priorityText = " - Congé";
            //        }
            //        else if (i == 2)
            //        {
            //            color = lowPriority;
            //            priorityText = " - Absent";
            //        }
            //        markRectangle = new Rectangle(args.Bounds.X + offset, args.Bounds.Y + offset + (markWidth + offset) * i, markWidth, markWidth);
            //        args.Cache.FillEllipse(markRectangle.X, markRectangle.Y, markRectangle.Width, markRectangle.Height, color);
            //        args.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            //        args.Appearance.Options.UseTextOptions = true;
            //        args.Appearance.DrawString(args.Cache, priorityText, new Rectangle(markRectangle.Right + offset, markRectangle.Y, args.Bounds.Width, markRectangle.Height));
            //    }
            //};
            int presentCount = 0;
            int absentCount = 0;  
            int leaveCount = 0;   

            // حساب عدد النقاط
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string statutValue = gridView1.GetRowCellValue(i, "Statutmvm")?.ToString(); 

                if (statutValue == "P") // الحضور
                    presentCount++;
                else if (statutValue == "A") // الغياب
                    absentCount++;
                else if (statutValue == "CR") // العطل
                    leaveCount++;
            }

            // رسم الفوتر
            gridView1.CustomDrawFooter += (s, args) =>
            {
                int offset = 5;
                int markWidth= 10; // عرض النقطة
                args.DefaultDraw();

                // تعريف الألوان
                Color presentColor = Color.Green;   
                Color absentColor = Color.Red;      
                Color leaveColor = Color.Orange;    

                // رسم النقاط والنصوص
                Rectangle markRectangle;

                // الحضور
                markRectangle = new Rectangle(args.Bounds.X + offset, args.Bounds.Y + offset, markWidth, markWidth);
                args.Cache.FillEllipse(markRectangle.X, markRectangle.Y, markRectangle.Width, markRectangle.Height, presentColor);
                args.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
                args.Appearance.Options.UseTextOptions = true;
                args.Appearance.DrawString(args.Cache, $" - Présent : {presentCount}", new Rectangle(markRectangle.Right + offset, markRectangle.Y, args.Bounds.Width, markRectangle.Height));

                // العطل
                markRectangle = new Rectangle(args.Bounds.X + offset, args.Bounds.Y + offset + (markWidth + offset) * 1, markWidth, markWidth);
                args.Cache.FillEllipse(markRectangle.X, markRectangle.Y, markRectangle.Width, markRectangle.Height, leaveColor);
                args.Appearance.DrawString(args.Cache, $" - Congé : {leaveCount}", new Rectangle(markRectangle.Right + offset, markRectangle.Y, args.Bounds.Width, markRectangle.Height));

                // الغياب
                markRectangle = new Rectangle(args.Bounds.X + offset, args.Bounds.Y + offset + (markWidth + offset) * 2, markWidth, markWidth);
                args.Cache.FillEllipse(markRectangle.X, markRectangle.Y, markRectangle.Width, markRectangle.Height, absentColor);
                args.Appearance.DrawString(args.Cache, $" - Absent : {absentCount}", new Rectangle(markRectangle.Right + offset, markRectangle.Y, args.Bounds.Width, markRectangle.Height));
            };
        }
        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int currentRow = gridView1.FocusedRowHandle >= 0 ? gridView1.FocusedRowHandle + 1 : 0;
            int totalRows = gridView1.RowCount;

            statusLabel.Text = $"Enregistrement {currentRow} sur {totalRows}";
            statusLabel.Font = new Font(statusLabel.Font, FontStyle.Bold);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusStrip.Items.Add(new ToolStripStatusLabel() { Spring = true });
            statusStrip.Items.Add(statusLabel);
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Column.FieldName == "Name")
            {
                bool status = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "Statut"));

                if (!status)
                {
                    e.Appearance.ForeColor = Color.Red;

                    string arrow = "→";
                    e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);

                    e.Handled = true;
                }
            }

            e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            e.Appearance.Options.UseTextOptions = true;
            e.DefaultDraw();
            if (e.Column.FieldName == "Matricule")
            {
                string statutValue = gridView1.GetRowCellValue(e.RowHandle, "Statutmvm")?.ToString(); // جلب قيمة "Statutmvm"

                if (statutValue == "P") // إذا كانت "P"، ارسم نقطة خضراء
                {
                    Color color = Color.Green;
                    int markWidth = 10;
                    int offset = (e.Bounds.Height - markWidth) / 2; // ضبط الموضع في وسط الخلية

                    e.Cache.FillEllipse(e.Bounds.X + 5, e.Bounds.Y + offset, markWidth, markWidth, color);
                }
                else if (statutValue == "CR")
                {
                    Color color = Color.Orange;
                    int markWidth = 10;
                    int offset = (e.Bounds.Height - markWidth) / 2; // ضبط الموضع في وسط الخلية

                    e.Cache.FillEllipse(e.Bounds.X + 5, e.Bounds.Y + offset, markWidth, markWidth, color);
                }
                else if (statutValue == "A")
                {
                    Color color = Color.Red;
                    int markWidth = 10;
                    int offset = (e.Bounds.Height - markWidth) / 2; // ضبط الموضع في وسط الخلية

                    e.Cache.FillEllipse(e.Bounds.X + 5, e.Bounds.Y + offset, markWidth, markWidth, color);
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

        //private void GridView1_DoubleClick(object sender, EventArgs e)
        //{
        //    DXMouseEventArgs ea = e as DXMouseEventArgs;
        //    GridView view = sender as GridView;
        //    GridHitInfo info = view.CalcHitInfo(ea.Location);

        //    if (info.InRow || info.InRowCell)
        //    {
        //        // الحصول على قيمة ID
        //        int id = Convert.ToInt32(view.GetFocusedRowCellValue("ID"));
        //        OpenForm(id);
        //    }
        //}
        //public virtual void OpenForm(int id)
        //{
        //    var frm = new Form.Frm_Fiche_Ajent(id);
        //    frm.ShowDialog();
        //}

        private void btn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Master.User.UserType == (byte)Master.UserType.Guest|| Master.User.UserType == (byte)Master.UserType.Manager|| Master.User.UserType == (byte)Master.UserType.User)
            {
                XtraMessageBox.Show("Vous n'avez pas l'autorisation d'accéder à ce rapport.",
                     "Autorisations insuffisantes",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }
            Frm_Import_XLSX frm = new Frm_Import_XLSX();
            frm.ShowDialog();
        }

        private void btn_print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridView1.ShowPrintPreview();
        }
    }
}