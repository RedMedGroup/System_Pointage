using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
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
using static System_Pointage.Classe.Master;

namespace System_Pointage.Frm_List
{
    public partial class Frm_Activation_List : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Activation_List()
        {
            InitializeComponent();
            gridView1.OptionsBehavior.Editable = true;
  

        }

        private void Frm_Activation_List_Load(object sender, EventArgs e)
        {
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " Les appareils autorisés à se connecter à la base de données";
            gridView1.OptionsFind.AllowFindPanel = false;
            #endregion
            var db = new DAL.DataClasses1DataContext();
            //gridControl1.DataSource = db.activations;
            RefrechData();
        }

        void RefrechData()
        {
            var db = new DAL.DataClasses1DataContext();

            // جلب البيانات وتحويلها إلى قائمة
            var data = db.activations.ToList();

            // تعيين البيانات إلى GridControl
            gridControl1.DataSource = data;

            // التأكد من أن العمود "Statut" موجود
            if (gridView1.Columns["Statut"] != null)
            {
                // إنشاء محرر ToggleSwitch
                RepositoryItemToggleSwitch toggleSwitch = new RepositoryItemToggleSwitch
                {
                    AutoHeight = false,
                    OffText = "Off",
                    OnText = "On"
                };

                if (!gridControl1.RepositoryItems.Contains(toggleSwitch))
                    gridControl1.RepositoryItems.Add(toggleSwitch);

                gridView1.Columns["Statut"].ColumnEdit = toggleSwitch;
                gridView1.Columns["Statut"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["Statut"].OptionsColumn.ReadOnly = false;
            }

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView1.Columns)
            {
                if (col.FieldName != "Statut")
                {
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.ReadOnly = true;
                }
            }

            gridView1.CellValueChanged -= GridView1_CellValueChanged;
            gridView1.CellValueChanged += GridView1_CellValueChanged;
        }
        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Statut") 
            {
                var db = new DAL.DataClasses1DataContext();
                int id = (int)gridView1.GetRowCellValue(e.RowHandle, "ID"); 
                bool newStatus = (bool)e.Value; 

                var activation = db.activations.FirstOrDefault(x => x.ID == id);
                if (activation != null)
                {
                    activation.Statut = newStatus;
                    db.SubmitChanges(); 
                }
            }
        }
    }
}