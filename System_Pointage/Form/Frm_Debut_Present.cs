using DevExpress.XtraEditors;
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
    public partial class Frm_Debut_Present : DevExpress.XtraEditors.XtraForm
    {
        private AgentDataService _agentDataService;

        DAL.MVMAgentDetail mvm;
        public Frm_Debut_Present()
        {
            InitializeComponent();
            New();
        }

        private void Frm_Debut_Present_Load(object sender, EventArgs e)
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
            //using (var db = new DAL.DataClasses1DataContext())
            //{
            //    gridControl1.DataSource = db.Fiche_Agents;
            //    gridView1.Columns["Name"].Caption = "Nom et prénom";
            //    gridView1.Columns["ID_Post"].Caption = "Poste";
            //    gridView1.Columns["Jour"].Caption = "Systéme";
            //    //gridView1.Columns["AgentID"].Visible = false;
            //    gridView1.Columns["ID"].Visible = false;
            //    gridView1.Columns["ScreenPosteD"].Visible = false;
            //    //gridView1.Columns["AccessPosteID"].Visible = false;
            //    //gridView1.Columns["NamePosteDetail"].Visible = false;
            //}
        }
        void SetData()
        {
            mvm.Statut=txt_Statut.Text;
            mvm.Date = dateEdit1.DateTime;

        }
        void GetData()
        {
            txt_Statut.Text = mvm.Statut;
            dateEdit1.DateTime = mvm.Date;
        }
        void New()
        {
            mvm = new DAL.MVMAgentDetail();

        }
        bool IsValidit()
        {
            if (txt_Statut.Text.Trim() == string.Empty)
            {
                txt_Statut.Text = ErrorText;
                return false;
            }

            return true;
        }
        void Save()
        {
            if (IsValidit() == false)
                return;

            // استرداد الصفوف المختارة من gridView1
            var selectedRows = gridView1.GetSelectedRows();

            if (selectedRows.Length == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner au moins une ligne.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new DAL.DataClasses1DataContext())
            {
                // قائمة لتخزين القيم المكررة
                var duplicateAgents = new List<string>();

                foreach (var rowHandle in selectedRows)
                {
                    // استرداد الصف الحالي ككائن عام
                    var rowData = gridView1.GetRow(rowHandle);

                    if (rowData != null)
                    {
                        // الحصول على معرف الوكيل (ItemID) من البيانات المرتبطة
                        int agentID = (int)gridView1.GetRowCellValue(rowHandle, "ID"); // استخدم اسم العمود المناسب

                        // التحقق مما إذا كانت البيانات موجودة بالفعل في MVMAgentDetails
                        bool exists = db.MVMAgentDetails.Any(m =>
                            m.ItemID == agentID &&
                            m.Date == dateEdit1.DateTime &&
                            m.Statut == txt_Statut.Text);

                        if (exists)
                        {
                            // إضافة اسم الوكيل إلى قائمة القيم المكررة
                            string agentName = gridView1.GetRowCellValue(rowHandle, "Name").ToString(); // استخدم اسم العمود المناسب
                            duplicateAgents.Add(agentName);
                        }
                        else
                        {
                            // إعداد بيانات MVMAgentDetail جديدة
                            var mvm = new DAL.MVMAgentDetail
                            {
                                ItemID = agentID, // تعيين ItemID من ID
                                Statut = txt_Statut.Text,
                                Date = dateEdit1.DateTime
                            };

                            // إدخال السجل الجديد
                            db.MVMAgentDetails.InsertOnSubmit(mvm);
                        }
                    }
                }

                // حفظ التغييرات
                db.SubmitChanges();

                // عرض رسالة نجاح
                if (duplicateAgents.Count > 0)
                {
                    string duplicates = string.Join(", ", duplicateAgents);
                    XtraMessageBox.Show($"Les agents suivants existent déjà pour cette date et statut : {duplicates}",
                        "Duplication détectée", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("Enregistrer avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        //void Save()
        //{
        //    if (IsValidit() == false)
        //        return;

        //    // الخطوة 1: استرداد الصفوف المختارة من gridView1
        //    var selectedRows = gridView1.GetSelectedRows();
        //    using (var db = new DAL.DataClasses1DataContext())
        //    {
        //        foreach (var rowHandle in selectedRows)
        //        {
        //            // استرداد الكائن Fiche_Agent من الصف المختار
        //           var agent = gridView1.GetRow(rowHandle) as DAL.Fiche_Agent;

        //            if (agent != null)
        //            {
        //                // الخطوة 2: إعداد بيانات MVMAgentDetail جديدة
        //                var mvm = new DAL.MVMAgentDetail
        //                {
        //                    ItemID = agent.ID, // تعيين ItemID من ID في Fiche_Agent
        //                    Statut = txt_Statut.Text,
        //                    Date = dateEdit1.DateTime
        //                };

        //                // الخطوة 3: إدراج السجل في MVMAgentDetails
        //                db.MVMAgentDetails.InsertOnSubmit(mvm);
        //            }
        //        }

        //        // الخطوة 4: حفظ التغييرات في قاعدة البيانات
        //        db.SubmitChanges();
        //        XtraMessageBox.Show("Enregistrer succés");
        //    }
        //}
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
    }
}