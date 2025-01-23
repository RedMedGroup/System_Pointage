using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraRichEdit.Model;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                            lkp_FirstName.Properties.DataSource = columnNames;
                            lkp_Poste.Properties.DataSource = columnNames;
                            lkp_syst.Properties.DataSource = columnNames;
                            lkp_date.Properties.DataSource = columnNames;
                            lkp_departement.Properties.DataSource = columnNames;
                            lkp_affecter.Properties.DataSource = columnNames;
                            dt_P.Properties.DataSource = columnNames;
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

                            gridControl1.DataSource = dataTable;

                            gridView1.OptionsView.AllowCellMerge = true;

                            // تحديد الأعمدة التي يجب دمجها بناءً على أسماء الأعمدة
                            for (int i = 0; i < columnNames.Count; i++)
                            {
                                if (i < dataTable.Columns.Count)
                                {
                                    gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                                }
                            }
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
        int errorCount = 0;
        int correctCount = 0;
        int correctMvm = 0;
        public void Save()
        {

            if (!IsDataValide())
            {
                return;
            }


            DataTable table = (DataTable)gridControl1.DataSource;

            #region
            DataTable tableError = (DataTable)gridControl1.DataSource;
            // إنشاء DataTable للصفوف الخاطئة التي سيتم عرضها في GridView2
            DataTable errorTable = tableError.Clone();
            errorTable.Columns.Add("Observation", typeof(string));
         
            #endregion

            using (var db = new DAL.DataClasses1DataContext())
            {
                foreach (DataRow row in table.Rows)
                {
                    // تحقق من وجود اسم العمود في الجدول باستخدام lkp_Matricule.Text
                    string matriculeColumn = lkp_Matricule.Text;
                    string nameColumn = lkp_Name.Text;
                    string firstnameColumn=lkp_FirstName.Text;
                    string jourColumn = lkp_syst.Text;
                    string affecterColumn = lkp_affecter.Text;
                    string dateColumn = lkp_date.Text;
                    string datePresence = dt_P.Text;

                    string departementColumn = lkp_departement.Text;
                    string posteColumn = lkp_Poste.Text;

              
                    

                    if (table.Columns.Contains(matriculeColumn) && table.Columns.Contains(nameColumn) &&
                        table.Columns.Contains(posteColumn) && table.Columns.Contains(departementColumn) &&
                        table.Columns.Contains(affecterColumn))
                    {
                        string matricule = row[matriculeColumn]?.ToString() ?? string.Empty;
                        string name = row[nameColumn]?.ToString() ?? string.Empty;
                        string firstname = row[firstnameColumn]?.ToString() ?? string.Empty;
                        string posteName = row[posteColumn]?.ToString() ?? string.Empty;
                        string departementName = row[departementColumn]?.ToString() ?? string.Empty;
                        string affecter = row[affecterColumn]?.ToString() ?? string.Empty;
                        int jour = row[jourColumn] != DBNull.Value &&int.TryParse(row[jourColumn]?.ToString(), out int tempJour)? tempJour : 0;

                        string dateStr = row[dateColumn]?.ToString() ?? string.Empty;
                        string datePresent= row[datePresence]?.ToString() ?? string.Empty;
                        DateTime date;
                        DateTime dateP;
                        if (!DateTime.TryParse(dateStr, out date))
                        {
                            date = default(DateTime);
                        }
                        if (!DateTime.TryParse(datePresent, out dateP))
                        {
                            dateP = default(DateTime);
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = "تاريخ الحضور خطا";
                            errorTable.Rows.Add(errorRow);
                            correctMvm--;
                            continue;
                        }

                        var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);

                        if (poste == null)
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = "Poste non trouvée";
                            errorTable.Rows.Add(errorRow);
                            errorCount++;
                            //  errorTable.ImportRow(row); 
                            continue;
                        }
                        if (departement == null)
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = "département non trouvée";
                            errorTable.Rows.Add(errorRow);
                            errorCount++;
                            // errorTable.ImportRow(row);
                            continue;
                        }
                        var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
                        if (existingAgent != null)
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = "Le matricule existe déjà.";
                            errorTable.Rows.Add(errorRow);
                            errorCount++;
                            // errorTable.ImportRow(row);
                            continue;
                        }
                        if (poste != null && departement != null)
                        {
                            var agent = new DAL.Fiche_Agent
                            {
                                Matricule = matricule,
                                Name = name,
                                FirstName=firstname,
                                ID_Post = poste.ID,
                                Jour = jour,
                                Date_Embauche = date,// ?? default(DateTime),
                                ScreenPosteD = departement.ID,
                                Affecter = affecter,
                                Statut = true,
                            };
                            correctCount++;
                            db.Fiche_Agents.InsertOnSubmit(agent);
                            db.SubmitChanges();
                            var agentDetail = new DAL.MVMAgentDetail
                            {
                                ItemID = agent.ID,
                                Date =dateP,
                                Statut="P",
                            };
                            correctMvm++;
                            db.MVMAgentDetails.InsertOnSubmit(agentDetail);
                            db.SubmitChanges();
                        }
                    }
                }
               
                gridControl2.DataSource = errorTable;
                string message =
     $"{errorCount} erreurs détectées ❌\n" +
     $"{correctCount} lignes traitées avec succès ✅\n" +
     $"{correctMvm} personnes présentes enregistrées 👤";

                MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                errorCount = correctCount = correctMvm = 0;

            }
        

        Application.DoEvents();
        }

     
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
    }
}