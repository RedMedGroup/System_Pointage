using DevExpress.XtraBars;
using ExcelDataReader;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.SimilarityRatio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FuzzySharp;

namespace System_Pointage.Form
{
    public partial class Frm_Import_XLSX : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Frm_Import_XLSX()
        {
            InitializeComponent();
            btn_save.Visibility=DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void Frm_Import_XLSX_Load(object sender, EventArgs e)
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
        public bool IsDataValide()
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
        int correctSuc = 0;

        void Coreger()
        {
            DataTable correctedDataTable = new DataTable();
            correctedDataTable.Columns.Add("Matricule", typeof(string));
            correctedDataTable.Columns.Add("Name", typeof(string));
            correctedDataTable.Columns.Add("Firstname", typeof(string));
            correctedDataTable.Columns.Add("Poste", typeof(string));
            correctedDataTable.Columns.Add("Département", typeof(string));
            correctedDataTable.Columns.Add("Affecter", typeof(string));
            correctedDataTable.Columns.Add("Jour", typeof(int));
            correctedDataTable.Columns.Add("Observation", typeof(string));

            DataTable table = (DataTable)gridControl1.DataSource;

            DataTable updatedTable = table.Clone();
            updatedTable.Columns.Add("Observation", typeof(string));

            DataTable tableError = (DataTable)gridControl1.DataSource;
            DataTable errorTable = tableError.Clone();
            errorTable.Columns.Add("Observation", typeof(string));

            using (var db = new DAL.DataClasses1DataContext())
            {
                var allPostes = db.Fiche_Postes.Select(p => p.Name).ToList();

                var allDepartements = db.UserAccessProfilePostes.Select(d => d.Name).ToList();

                foreach (DataRow row in table.Rows)
                {
                    string matriculeColumn = lkp_Matricule.Text;
                    string nameColumn = lkp_Name.Text;
                    string firstnameColumn = lkp_FirstName.Text;
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
                        int jour = row[jourColumn] != DBNull.Value && int.TryParse(row[jourColumn]?.ToString(), out int tempJour) ? tempJour : 0;

                        string dateStr = row[dateColumn]?.ToString() ?? string.Empty;
                        string datePresent = row[datePresence]?.ToString() ?? string.Empty;
                        DateTime date;
                        DateTime dateP;
                        if (!DateTime.TryParse(dateStr, out date))
                        {
                            date = default(DateTime);
                        }
                        bool hasError = false;
                        string observation = "";
                        if (!DateTime.TryParse(datePresent, out dateP))
                        {
                            dateP = default(DateTime);
                            observation += "تاريخ الحضور خطا";
                            hasError = true;
                        }

                        string originalMatricule = matricule;
                        string correctedMatricule = matricule.Replace(" ", "");
                        if (originalMatricule != correctedMatricule)
                        {
                            matricule = correctedMatricule;
                            row[matriculeColumn] = correctedMatricule;
                            observation += $"Matricule corrigé automatiquement: {correctedMatricule}; ";
                            correctSuc++;
                        }

                        var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        if (poste == null) 
                        {
                            var correctedPosteName = CorrectPosteUsingTokenSetRatio(posteName, allPostes);
                            if (correctedPosteName != null)
                            {
                                row[posteColumn] = correctedPosteName;
                                observation += $"Poste corrigé automatiquement: {correctedPosteName}; ";
                                correctSuc++;
                            }
                            else 
                            {
                                observation += "Poste non trouvée et aucune correction trouvée; ";
                                hasError = true;
                            }
                        }

                        var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);
                        if (departement == null) 
                        {
                            var correctedDepartementName = CorrectDepartementUsingTokenSetRatio(departementName, allDepartements);
                            if (correctedDepartementName != null) 
                            {
                                row[departementColumn] = correctedDepartementName;
                                observation += $"Département corrigé automatiquement: {correctedDepartementName}; ";
                                correctSuc++;
                            }
                            else 
                            {
                                observation += "Département non trouvé et aucune correction trouvée; ";
                                hasError = true;
                            }
                        }

                        // إضافة السطر إلى updatedTable إذا لم يكن هناك خطأ أو تم تصحيحه
                        if (!hasError || (poste != null && departement != null))
                        {
                            DataRow updatedRow = updatedTable.NewRow();
                            updatedRow.ItemArray = row.ItemArray;
                            updatedRow["Observation"] = observation;
                            updatedTable.Rows.Add(updatedRow);
                        }

                        // إضافة السطر إلى correctedDataTable إذا تم تصحيح Matricule أو Poste أو Département
                        if (originalMatricule != correctedMatricule ||
                            (poste == null && CorrectPosteUsingTokenSetRatio(posteName, allPostes) != null) ||
                            (departement == null && CorrectDepartementUsingTokenSetRatio(departementName, allDepartements) != null))
                        {
                            DataRow correctedRow = correctedDataTable.NewRow();
                            correctedRow.ItemArray = new object[]
                            {
                        matricule,          // Matricule
                        name,               // Name
                        firstname,          // Firstname
                        row[posteColumn],   // Poste 
                        row[departementColumn], // Département
                        affecter,           // Affecter
                        jour,               // Jour
                        observation         // Observation
                            };
                            correctedDataTable.Rows.Add(correctedRow);
                        }

                        if (hasError && (poste == null || departement == null))
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = observation;
                            errorTable.Rows.Add(errorRow);
                            errorCount++;
                        }
                        else
                        {
                            correctCount++;
                        }
                    }
                }
            }

            gridControl3.DataSource = correctedDataTable;
            gridControl3.RefreshDataSource();

            gridControl2.DataSource = errorTable;

            gridControl1.DataSource = updatedTable;
            gridControl1.RefreshDataSource();

            // عرض رسالة النتيجة
            string message =
                $"{errorCount} erreurs détectées ❌\n" +
                $"{correctCount} lignes traitées avec succès ✅\n" +
                $"{correctSuc} Erreurs qui ont été corrigées ✅";
            MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            errorCount = correctCount = correctSuc = 0;
        }

        //  Token Set Ratio
        public string CorrectPosteUsingTokenSetRatio(string posteName, List<string> allPostes)
        {
            var bestMatch = Process.ExtractOne(posteName, allPostes, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);

            if (bestMatch != null && bestMatch.Score >= 80) 
            {
                return bestMatch.Value; 
            }

            return null; 
        }

        //  Token Set Ratio
        public string CorrectDepartementUsingTokenSetRatio(string departementName, List<string> allDepartements)
        {
            var bestMatch = Process.ExtractOne(departementName, allDepartements, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);

            if (bestMatch != null && bestMatch.Score >= 80) 
            {
                return bestMatch.Value; 
            }

            return null; 
        }

        private void btn_Correction_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!IsDataValide())
            {
                return;
            }
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btn_Correction.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            Coreger();
        }

        private void btn_save_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl2.DataSource=null;gridControl3.DataSource=null;
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (!IsDataValide())
            {
                return;
            }

            DataTable table = (DataTable)gridControl1.DataSource;
            #region
            DataTable tableError = (DataTable)gridControl1.DataSource;
            DataTable errorTable = tableError.Clone();
            #endregion


            using (var db = new DAL.DataClasses1DataContext())
            {
                foreach (DataRow row in table.Rows)
                {
                    string matriculeColumn = lkp_Matricule.Text;
                    string nameColumn = lkp_Name.Text;
                    string firstnameColumn = lkp_FirstName.Text;
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
                        int jour = row[jourColumn] != DBNull.Value && int.TryParse(row[jourColumn]?.ToString(), out int tempJour) ? tempJour : 0;

                        string dateStr = row[dateColumn]?.ToString() ?? string.Empty;
                        string datePresent = row[datePresence]?.ToString() ?? string.Empty;
                        DateTime date;
                        DateTime dateP;
                        if (!DateTime.TryParse(dateStr, out date))
                        {
                            date = default(DateTime);
                        }
                        if (!DateTime.TryParse(datePresent, out dateP))
                        {
                            dateP = default(DateTime);
                        }

                      

                        bool hasError = false;
                        string observation = "";
                        var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
                        if (existingAgent != null)
                        {
                            observation += "Le matricule existe déjà.";
                            hasError = true;
                        }
                        if (hasError)
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = observation; 
                            errorTable.Rows.Add(errorRow); 
                            errorCount++;
                            continue;
                        }
                        var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);
                        var agent = new DAL.Fiche_Agent
                            {
                                Matricule = matricule,
                                Name = name,
                                FirstName = firstname,
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
                                Date = dateP,
                                Statut = "P",
                            };
                            db.MVMAgentDetails.InsertOnSubmit(agentDetail);
                            db.SubmitChanges();

                    }
                }
            }
            gridControl2.DataSource = errorTable;
            string message =
             $"{errorCount} erreurs détectées ❌\n" +
             $"{correctCount} lignes traitées avec succès ✅\n";
            MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.DoEvents();
        }
    }
}
