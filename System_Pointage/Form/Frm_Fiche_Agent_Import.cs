using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Fiche_Agent_Import : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Fiche_Agent_Import()
        {
            InitializeComponent();
        }

        private void Frm_Fiche_Agent_Import_Load(object sender, EventArgs e)
        {
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.GroupPanelText = " ";
            #endregion
        }
        public  void RefreshData()
        {
            //using (var db = new DAL.DataClasses1DataContext())
            //{
            //    lkp_Poste.IntializeData(db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList());//base               
            //}
        }
        private void btn_exl_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txt_lien.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            // تخطي الصفوف الفارغة حتى نجد صف يحتوي على بيانات
                            bool validRowFound = false;
                            DataTable dataTable = new DataTable();
                            var columnNames = new List<string>();

                            while (!validRowFound && reader.Read())
                            {
                                // التحقق من وجود قيمة غير فارغة في أي عمود من السطر
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (reader.GetValue(i) != null && !string.IsNullOrWhiteSpace(reader.GetValue(i).ToString()))
                                    {
                                        validRowFound = true;
                                        break;
                                    }
                                }

                                // إذا كان الصف صالحًا، قم بإعداد أسماء الأعمدة
                                if (validRowFound)
                                {
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        columnNames.Add(reader.GetValue(i)?.ToString() ?? $"Column{i + 1}");
                                        dataTable.Columns.Add(columnNames[i]);
                                    }
                                }
                            }

                            // Set the data source for lookups
                            lkp_Matricule.Properties.DataSource = columnNames;
                            lkp_Name.Properties.DataSource = columnNames;
                            lkp_Poste.Properties.DataSource = columnNames;
                            lkp_syst.Properties.DataSource = columnNames;
                            lkp_date.Properties.DataSource = columnNames;
                            lkp_departement.Properties.DataSource = columnNames;
                            lkp_affecter.Properties.DataSource = columnNames;
                            // قراءة بقية البيانات وإضافتها كصفوف إلى DataTable
                            while (reader.Read())
                            {
                                DataRow dataRow = dataTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    object value = reader.GetValue(i);
                                    dataRow[i] = value != null ? value.ToString() : ""; // تعيين القيمة الفارغة كـ ""
                                }
                                dataTable.Rows.Add(dataRow);
                            }

                            // ربط DataTable بالـ gridControl1 لعرض البيانات
                            gridControl1.DataSource = dataTable;

                            // إعداد خاصية دمج الخلايا للـ GridView
                            gridView1.OptionsView.AllowCellMerge = true;

                            // تحديد الأعمدة التي يجب دمجها بناءً على أسماء الأعمدة
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                if (i < dataTable.Columns.Count)
                                {
                                    gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                                }
                            }
                            // ضبط تنسيق الأعمدة
                            gridView1.BestFitColumns();
                        }
                    }
                }
            }
        }
        public  bool IsDataValide()
        {
            int NumberOfErrors = 0;
            using (var db = new DAL.DataClasses1DataContext())
            {
                if (string.IsNullOrWhiteSpace(lkp_Matricule.Text))
                {
                    NumberOfErrors += 1;
                    lkp_Matricule.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_Name.Text))
                {
                    NumberOfErrors += 1;
                    lkp_Name.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_Poste.Text))
                {
                    NumberOfErrors += 1;
                    lkp_Poste.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_syst.Text))
                {
                    NumberOfErrors += 1;
                    lkp_syst.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_date.Text))
                {
                    NumberOfErrors += 1;
                    lkp_date.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_departement.Text))
                {
                    NumberOfErrors += 1;
                    lkp_departement.ErrorText = ErrorText;
                }
                if (string.IsNullOrWhiteSpace(lkp_affecter.Text))
                {
                    NumberOfErrors += 1;
                    lkp_affecter.ErrorText = ErrorText;
                }
                
            }

            return (NumberOfErrors == 0);
        }
        public static string ErrorText
        {
            get
            {
                return "Sélectionnez le nom de colonne";
            }
        }
        public void Save()
        {

            if (!IsDataValide())
            {
                return;
            }


            DataTable table = (DataTable)gridControl1.DataSource;


            using (var db = new DAL.DataClasses1DataContext())
            {
                foreach (DataRow row in table.Rows)
                {
                    // تحقق من وجود اسم العمود في الجدول باستخدام lkp_Matricule.Text
                    string matriculeColumn = lkp_Matricule.Text;
                    string nameColumn = lkp_Name.Text;
                    string posteColumn = lkp_Poste.Text;
                    string jourColumn = lkp_syst.Text;

                    string departementColumn = lkp_departement.Text;
                    string affecterColumn = lkp_affecter.Text;
                    string dateColumn = lkp_date.Text;

                    if (table.Columns.Contains(matriculeColumn) && table.Columns.Contains(nameColumn) &&
                        table.Columns.Contains(posteColumn) && table.Columns.Contains(departementColumn) &&
                        table.Columns.Contains(affecterColumn))
                    {
                        string matricule = row[matriculeColumn]?.ToString() ?? string.Empty;
                        string name = row[nameColumn]?.ToString() ?? string.Empty;
                        string posteName = row[posteColumn]?.ToString() ?? string.Empty;
                        string departementName = row[departementColumn]?.ToString() ?? string.Empty;
                        string affecter = row[affecterColumn]?.ToString() ?? string.Empty;
                        int jour = row[jourColumn] != DBNull.Value &&
               int.TryParse(row[jourColumn]?.ToString(), out int tempJour)
               ? tempJour
               : 0;
                        // string date = row[dateColumn]?.ToString() ?? string.Empty;
                        string dateStr = row[dateColumn]?.ToString() ?? string.Empty;
                        DateTime date;
                        bool isValidDate = DateTime.TryParse(dateStr, out date);

                        // إذا كان التاريخ غير صالح، استخدم قيمة افتراضية
                        if (!isValidDate)
                        {
                            date = DateTime.MinValue;  // يمكن استبدال DateTime.MinValue بتاريخ افتراضي آخر إذا لزم الأمر
                        }

                        // تحقق من وجود السجل المرتبط بـ poste و departement
                        var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);

                        if (poste != null && departement != null)
                        {
                            // إعداد كائن Fiche_Agent
                            var agent = new DAL.Fiche_Agent
                            {
                                Matricule = matricule,
                                Name = name,
                                ID_Post = poste.ID,
                                Jour = jour,
                                Date_Embauche = date,// ?? default(DateTime),
                                ScreenPosteD = departement.ID,
                                Affecter = affecter,
                                Statut = true,
                            };

                            db.Fiche_Agents.InsertOnSubmit(agent);
                        }
                    }
                }
                db.SubmitChanges();
                XtraMessageBox.Show("Enregistrer succés");
            }
        

        Application.DoEvents();
        }
        

        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
    }
}