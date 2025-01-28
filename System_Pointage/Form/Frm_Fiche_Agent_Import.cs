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
using FuzzySharp;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.Extractor;
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
                        bool hasError = false;
                        string observation = "";
                        //DataRow errorRow = errorTable.NewRow();
                        //errorRow.ItemArray = row.ItemArray;

                        if (!DateTime.TryParse(datePresent, out dateP))
                        {
                            dateP = default(DateTime);
                            //  DataRow errorRow = errorTable.NewRow();
                            // errorRow.ItemArray = row.ItemArray;
                            observation += "تاريخ الحضور خطا";
                          //  errorTable.Rows.Add(errorRow);
                        //    correctMvm--;
                            hasError = true;
                            //  continue;
                        }

                        var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);

                        if (poste == null)
                        {
                            //    DataRow errorRow = errorTable.NewRow();
                            //  errorRow.ItemArray = row.ItemArray;
                            observation += "Poste non trouvée";
                          //  errorTable.Rows.Add(errorRow);
                           // errorCount++;
                            hasError = true;
                           // continue;
                        }
                        if (departement == null)
                        {
                            //      DataRow errorRow = errorTable.NewRow();
                            //    errorRow.ItemArray = row.ItemArray;
                            observation += "département non trouvée";
                         //   errorTable.Rows.Add(errorRow);
                           // errorCount++;
                            hasError = true;
                         //   continue;
                        }
                        var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
                        if (existingAgent != null)
                        {
                            //   DataRow errorRow = errorTable.NewRow();
                            //   errorRow.ItemArray = row.ItemArray;
                            observation += "Le matricule existe déjà.";
                         //   errorTable.Rows.Add(errorRow);
                          //  errorCount++;
                            hasError = true;
                        //    continue;
                        }
                        if (hasError)
                        {
                            DataRow errorRow = errorTable.NewRow();
                            errorRow.ItemArray = row.ItemArray;
                            errorRow["Observation"] = observation; // إضافة الملاحظات المتراكمة
                            errorTable.Rows.Add(errorRow); // إضافة الصف إلى الجدول
                            errorCount++;
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
                                //Jour = jour,
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

        // الحصول على البيانات من gridControl2
        DataTable table = (DataTable)gridControl1.DataSource;

        DataTable tableError = (DataTable)gridControl1.DataSource;
        DataTable errorTable = tableError.Clone();
        errorTable.Columns.Add("Observation", typeof(string));

        using (var db = new DAL.DataClasses1DataContext())
        {
            var allPostes = db.Fiche_Postes.Select(p => p.Name).ToList();

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

                    var correctedPosteName = CorrectPosteUsingTokenSetRatio(posteName, allPostes);
                    if (correctedPosteName != null)
                    {
                        observation += $"Poste corrigé automatiquement: {correctedPosteName}; ";
                    }
                    else
                    {
                        observation += "Poste non trouvée et aucune correction trouvée; ";
                        hasError = true;
                    }

                    DataRow newRow = correctedDataTable.NewRow();
                    newRow.ItemArray = new object[]
                    {
                    matricule,          // Matricule
                    name,               // Name
                    firstname,          // Firstname
                    correctedPosteName, // Poste 
                    departementName,    // Département
                    affecter,           // Affecter
                    jour,               // Jour
                    observation         // Observation
                    };
                    correctedDataTable.Rows.Add(newRow);

                    if (hasError)
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

        string message =
            $"{errorCount} erreurs détectées ❌\n" +
            $"{correctCount} lignes traitées avec succès ✅\n" +
            $"{correctMvm} personnes présentes enregistrées 👤";

        MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
        errorCount = correctCount = correctMvm = 0;
    }

    public string CorrectPosteUsingTokenSetRatio(string posteName, List<string> allPostes)
    {
      
        var bestMatch = Process.ExtractOne(posteName, allPostes, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);

        if (bestMatch != null && bestMatch.Score >= 80) 
        {
            return bestMatch.Value;
        }

        return null; 
    }

    #region
    //   void Coreger()
    //   {
    //       DataTable table = (DataTable)gridControl2.DataSource;
    //       #region

    //       DataTable correctedDataTable = new DataTable();
    //       correctedDataTable.Columns.Add("Matricule", typeof(string));
    //       correctedDataTable.Columns.Add("Name", typeof(string));
    //       correctedDataTable.Columns.Add("Firstname", typeof(string));
    //       correctedDataTable.Columns.Add("Poste", typeof(string));
    //       correctedDataTable.Columns.Add("Département", typeof(string));
    //       correctedDataTable.Columns.Add("Affecter", typeof(string));
    //       correctedDataTable.Columns.Add("Jour", typeof(int));
    //       correctedDataTable.Columns.Add("Observation", typeof(string));
    //       #endregion
    //       #region
    //       DataTable tableError = (DataTable)gridControl1.DataSource;
    //       DataTable errorTable = tableError.Clone();
    //       errorTable.Columns.Add("Observation", typeof(string));

    //       #endregion

    //       using (var db = new DAL.DataClasses1DataContext())
    //       {
    //           foreach (DataRow row in table.Rows)
    //           {
    //               string matriculeColumn = lkp_Matricule.Text;
    //               string nameColumn = lkp_Name.Text;
    //               string firstnameColumn = lkp_FirstName.Text;
    //               string jourColumn = lkp_syst.Text;
    //               string affecterColumn = lkp_affecter.Text;
    //               string dateColumn = lkp_date.Text;
    //               string datePresence = dt_P.Text;

    //               string departementColumn = lkp_departement.Text;
    //               string posteColumn = lkp_Poste.Text;




    //               if (table.Columns.Contains(matriculeColumn) && table.Columns.Contains(nameColumn) &&
    //                   table.Columns.Contains(posteColumn) && table.Columns.Contains(departementColumn) &&
    //                   table.Columns.Contains(affecterColumn))
    //               {
    //                   string matricule = row[matriculeColumn]?.ToString() ?? string.Empty;
    //                   string name = row[nameColumn]?.ToString() ?? string.Empty;
    //                   string firstname = row[firstnameColumn]?.ToString() ?? string.Empty;
    //                   string posteName = row[posteColumn]?.ToString() ?? string.Empty;
    //                   string departementName = row[departementColumn]?.ToString() ?? string.Empty;
    //                   string affecter = row[affecterColumn]?.ToString() ?? string.Empty;
    //                   int jour = row[jourColumn] != DBNull.Value && int.TryParse(row[jourColumn]?.ToString(), out int tempJour) ? tempJour : 0;

    //                   string dateStr = row[dateColumn]?.ToString() ?? string.Empty;
    //                   string datePresent = row[datePresence]?.ToString() ?? string.Empty;
    //                   DateTime date;
    //                   DateTime dateP;
    //                   if (!DateTime.TryParse(dateStr, out date))
    //                   {
    //                       date = default(DateTime);
    //                   }
    //                   bool hasError = false;
    //                   string observation = "";
    //                   if (!DateTime.TryParse(datePresent, out dateP))
    //                   {
    //                       dateP = default(DateTime);                            
    //                       observation += "تاريخ الحضور خطا";                           
    //                       hasError = true;
    //                   }

    //                   var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
    //                   var departement = db.UserAccessProfilePostes.FirstOrDefault(d => d.Name == departementName);
    //                   string correctedPosteName = CorrectPoste(posteName);
    //                   if (correctedPosteName != null)
    //                   {
    //                       poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == correctedPosteName);
    //                       observation += $"Poste corrigé automatiquement: {correctedPosteName}; ";
    //                   }
    //                   else
    //                   {
    //                       observation += "Poste non trouvée et aucune correction trouvée; ";
    //                       hasError = true;
    //                   }
    //                   //if (departement == null)
    //                   //{
    //                   //    observation += "département non trouvée";                          
    //                   //    hasError = true;
    //                   //}
    //                   //var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
    //                   //if (existingAgent != null)
    //                   //{                    
    //                   //    observation += "Le matricule existe déjà.";                     
    //                   //    hasError = true;
    //                   //}
    //                   DataRow newRow = correctedDataTable.NewRow();
    //                   newRow.ItemArray = new object[]
    //                   {
    //               matricule,          // Matricule
    //               name,               // Name
    //               firstname,          // Firstname
    //               poste?.Name,        // Poste (تم تصحيحه إذا لزم الأمر)
    //               departementName,    // Département
    //               affecter,           // Affecter
    //               jour,               // Jour
    //               observation         // Observation
    //                   };
    //                   correctedDataTable.Rows.Add(newRow);

    //                   if (hasError)
    //                   {
    //                       DataRow errorRow = errorTable.NewRow();
    //                       errorRow.ItemArray = row.ItemArray;
    //                       errorRow["Observation"] = observation;
    //                       errorTable.Rows.Add(errorRow);
    //                       errorCount++;
    //                   }
    //                   else
    //                   {
    //                       correctCount++;
    //                   }

    //                   //gridControl3.DataSource = correctedDataTable;

    //                   //// تحديث الواجهة
    //                   //gridControl3.RefreshDataSource();

    //               }
    //               }
    //           }
    //       gridControl3.DataSource = correctedDataTable;
    //       gridControl3.RefreshDataSource();

    //       gridControl1.DataSource = errorTable;
    //       string message =
    //$"{errorCount} erreurs détectées ❌\n" +
    //$"{correctCount} lignes traitées avec succès ✅\n" +
    //$"{correctMvm} personnes présentes enregistrées 👤";

    //           MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
    //           errorCount = correctCount = correctMvm = 0;

    //   }
    #endregion
        private void btn_save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
        #region
        //public string CorrectPoste(string posteName)
        //{
        //    var db = new DAL.DataClasses1DataContext();
        //    // الحصول على جميع المناصب من الجدول Fiche_Postes
        //    var allPostes = db.Fiche_Postes.Select(p => p.Name).ToList();

        //    // إذا لم يكن هناك مناصب، إرجاع null
        //    if (allPostes.Count == 0)
        //        return null;

        //    // البحث عن أقرب تطابق باستخدام Levenshtein Distance
        //    string closestPoste = null;
        //    int minDistance = int.MaxValue;

        //    foreach (var poste in allPostes)
        //    {
        //        int distance = CalculateLevenshteinDistance(posteName, poste);
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            closestPoste = poste;
        //        }
        //    }

        //    // إذا كانت المسافة أقل من حد معين (مثل 3)، نعتبره تطابقًا
        //    if (minDistance <= 3) // يمكنك تعديل هذا الحد حسب الحاجة
        //        return closestPoste;

        //    // إذا لم يتم العثور على تطابق قريب، إرجاع null
        //    return null;
        //}

        //// دالة لحساب Levenshtein Distance
        //public int CalculateLevenshteinDistance(string s, string t)
        //{
        //    int n = s.Length;
        //    int m = t.Length;
        //    int[,] d = new int[n + 1, m + 1];

        //    if (n == 0)
        //        return m;
        //    if (m == 0)
        //        return n;

        //    for (int i = 0; i <= n; d[i, 0] = i++) { }
        //    for (int j = 0; j <= m; d[0, j] = j++) { }

        //    for (int i = 1; i <= n; i++)
        //    {
        //        for (int j = 1; j <= m; j++)
        //        {
        //            int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
        //            d[i, j] = Math.Min(
        //                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
        //                d[i - 1, j - 1] + cost);
        //        }
        //    }

        //    return d[n, m];
        //}
        #endregion
        private void btn_coriger_Click(object sender, EventArgs e)
        {
            Coreger();
        }

        private void btn_CorrectionA_Click(object sender, EventArgs e)
        {
            Coreger();
        }
    }
}