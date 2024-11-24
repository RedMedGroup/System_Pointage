using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
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
    public partial class Frm_AccessPosteHeder : DevExpress.XtraEditors.XtraForm
    {
        DAL.UserAccessProfilePoste Aposte;
        DAL.UserAccessProfilePosteDetail AposteD;

        public Frm_AccessPosteHeder()
        {
            InitializeComponent();
            New();
        }

        private void Frm_AccessPosteHeder_Load(object sender, EventArgs e)
        {
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            RefrechDate();
        }
        void RefrechDate()
        {
            using(var db = new DAL.DataClasses1DataContext())
            {
                gridControl1.DataSource = db.Fiche_Postes;
            }
        }
        void SetData()
        {
            Aposte.Name = txt_Name.Text;
          
        }
        void GetData()
        {
            txt_Name.Text = Aposte.Name;
          
        }
        void New()
        {
            Aposte = new DAL.UserAccessProfilePoste();
            AposteD = new DAL.UserAccessProfilePosteDetail();

        }
        bool IsValidit()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.Text = ErrorText;
                return false;
            }
            
            return true;
        }
        void Save()
        {
            if (IsValidit() == false)
                return;

            using (var db = new DAL.DataClasses1DataContext())
            {
                // الخطوة 1: إنشاء أو تحديث UserAccessProfilePoste
                if (Aposte.ID == 0)
                {
                    db.UserAccessProfilePostes.InsertOnSubmit(Aposte);
                }
                else
                {
                    db.UserAccessProfilePostes.Attach(Aposte);
                }

                // تحديث البيانات من TextEdit
                SetData();
                db.SubmitChanges();
                // الخطوة 2: معالجة الأسطر المحددة في GridControl
                var selectedRows = gridView1.GetSelectedRows(); // جلب الصفوف المحددة
                var duplicatePosteIDs = new List<int>();
                var newDetails = new List<DAL.UserAccessProfilePosteDetail>();

                foreach (var rowHandle in selectedRows)
                {
                    var posteID = (int)gridView1.GetRowCellValue(rowHandle, "ID"); // جلب ID من العمود

                    // التحقق مما إذا كان ID_Name_Poste موجودًا مسبقًا مع ID_AccessPoste
                    bool exists = db.UserAccessProfilePosteDetails
                        .Any(detail => detail.ID_Name_Poste == posteID /*&& detail.ID_AccessPoste == Aposte.ID*/);

                    if (exists)
                    {
                        // إضافة الـ posteID إلى قائمة التكرارات
                        duplicatePosteIDs.Add(posteID);
                    }
                    else
                    {
                        // إضافة البيانات الجديدة إلى قائمة الإدخالات
                        var detail = new DAL.UserAccessProfilePosteDetail
                        {
                            ID_Name_Poste = posteID,
                            ID_AccessPoste = Aposte.ID // استخدام ID المحفوظ في الخطوة السابقة
                        };
                        newDetails.Add(detail);
                    }
                }

                // إدخال البيانات الجديدة فقط إذا لم تكن مكررة
                if (newDetails.Any())
                {
                    db.UserAccessProfilePosteDetails.InsertAllOnSubmit(newDetails);
                    db.SubmitChanges();
                }

                // عرض رسالة إذا كانت هناك تكرارات
                if (duplicatePosteIDs.Any())
                {
                    string duplicateMessage = $"Les postes suivants existent déjà : {string.Join(", ", duplicatePosteIDs)}";
                    XtraMessageBox.Show(duplicateMessage, "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    XtraMessageBox.Show("Enregistrer avec succès");
                }

                // إعادة التهيئة
                New();
            }
        }




        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }

        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
    }
}