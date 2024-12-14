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

namespace System_Pointage.Form
{
    public partial class Frm_MVMAgentDetails : DevExpress.XtraEditors.XtraForm
    {
        public Frm_MVMAgentDetails()
        {
            InitializeComponent();
        }

        private void Frm_MVMAgentDetails_Load(object sender, EventArgs e)
        {
            RefrechData();
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
        }
        void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();
            var data = from mvm in db.MVMAgentDetails
                       join n in db.Fiche_Agents on mvm.ItemID equals n.ID into agents
                       from agent in agents.DefaultIfEmpty()
                       orderby mvm.ID // يمكنك تغيير هذا إلى الحقل الذي تريد الترتيب حسبه
                       select new
                       {mvm.ID,
                          /* mvm.ItemID,*/ // تأكد من أن هذا هو العمود الصحيح
                           //ID = agent != null ? agent.ID : (int?)null, // تأكد من أن هذا هو العمود الصحيح
                           Name = agent != null ? agent.Name : "N/A", // اسم افتراضي إذا لم يوجد تطابق
                           mvm.Date,
                           mvm.Statut,
                       };

            gridControl1.DataSource = data.ToList();
            gridControl1.Refresh();
            var count = data.Count();
        }

        private void btn_delet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] selectedRows = gridView1.GetSelectedRows();
            if (selectedRows.Length > 1)
            {
                MessageBox.Show("Veuillez sélectionner une seule ligne à supprimer.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            int selectedRowHandle = selectedRows[0];

            // احصل على البيانات من الصف المحدد
            var rowData = gridView1.GetRow(selectedRowHandle) as dynamic; // استخدم dynamic إذا كنت تعمل مع مصادر غير معرّفة بدقة

            if (rowData != null)
            {

                int id = rowData.ID;
                DateTime date = rowData.Date;
                string name = rowData.Name;
                // تأكيد الحذف
                DialogResult result = MessageBox.Show($"Voulez-vous vraiment supprimer la ligne sélectionnée ?\nNom : {name}\nDate : {date}", "Confirmation de suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (var db = new DAL.DataClasses1DataContext())
                    {
                        // حذف السجل من قاعدة البيانات
                        var recordToDelete = db.MVMAgentDetails.FirstOrDefault(mvm => mvm.Date == date && mvm.ID == id);
                        if (recordToDelete != null)
                        {
                            db.MVMAgentDetails.DeleteOnSubmit(recordToDelete);
                            db.SubmitChanges();

                            // تحديث البيانات
                            RefrechData();
                            MessageBox.Show("La suppression a été effectuée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("L'enregistrement spécifié n'a pas été trouvé dans la base de données.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}