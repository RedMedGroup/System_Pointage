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
using DevExpress.ClipboardSource.SpreadsheetML;
using ClosedXML.Excel;
using System_Pointage.Classe;
using OfficeOpenXml;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;

namespace System_Pointage.Form
{
    public partial class Frm_MVM_Import_XLSX : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public Frm_MVM_Import_XLSX()
        {
            InitializeComponent();
        }

        private void accordionContentContainer2_Click(object sender, EventArgs e)
        {

        }

  
        public static string ErrorText
        {
            get
            {
                return "Sélectionnez le nom de colonne";
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
            }

            return (NumberOfErrors == 0);
        }
      
        int errorCount = 0;
        int correctCount = 0;
        int correctSuc = 0;
        #region
        void Coreger()
        {
            DataTable correctedDataTable = new DataTable();
            correctedDataTable.Columns.Add("Matricule", typeof(string));
            correctedDataTable.Columns.Add("Name", typeof(string));
            correctedDataTable.Columns.Add("Firstname", typeof(string));
            correctedDataTable.Columns.Add("Poste", typeof(string));
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

               
                foreach (DataRow row in table.Rows)
                {
                    string matriculeColumn = "Matricule"; // اسم عمود Matricule
                    string nameColumn = "Nom";         // اسم عمود Name
                    string firstnameColumn = "Prénom"; // اسم عمود Firstname
                    string datePresenceColumn = "DatePresence"; // اسم عمود DatePresence
                    string posteColumn = "Poste";


                    if (table.Columns.Contains(matriculeColumn) && table.Columns.Contains(nameColumn) )
                    {
                        string matricule = row[matriculeColumn]?.ToString() ?? string.Empty;
                        string name = row[nameColumn]?.ToString() ?? string.Empty;
                        string firstname = row[firstnameColumn]?.ToString() ?? string.Empty;
                        string posteName = row[posteColumn]?.ToString() ?? string.Empty;
                     //   string datePresent = row[datePresenceColumn]?.ToString() ?? string.Empty;
                        DateTime date;
                        DateTime dateP;
                       
                        bool hasError = false;
                        string observation = "";
                        //if (!DateTime.TryParse(datePresent, out dateP))
                        //{
                        //    dateP = default(DateTime);
                        //    observation += "Date de présence incorrecte";
                        //    hasError = true;
                        //}

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
                        //int? idPost = poste?.ID;
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
                        var agent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule == matricule && x.Name == name && x.FirstName == firstname/*&& x.ID_Post== idPost*/);
                        string mat = agent?.Matricule ?? string.Empty; 
                        var dataCorrection = new DataCorrection();
                        string correctedName = name;
                        string correctedFirstName = firstname;
                       
                        // إذا لم يتم العثور على العميل، نقوم بمحاولة تصحيح الاسم واللقب
                        if (agent == null)
                        {
                            var correctionResult = dataCorrection.CorrectAgentNameUsingTokenSetRatio(matricule, posteName, name, firstname);
                            correctedName = correctionResult.CorrectedName;
                            correctedFirstName = correctionResult.CorrectedFirstName;

                            if (correctedName != name  )
                            {
                                row[nameColumn] = correctedName;
                                row[firstnameColumn] = correctedFirstName;
                                observation += $"Nom corrigé automatiquement: {correctedName}; ";
                                correctSuc++;
                            }
                            else if(correctedFirstName != firstname)
                            {
                                row[nameColumn] = correctedName;
                                row[firstnameColumn] = correctedFirstName;
                                observation += $" Prénom corrigé automatiquement: {correctedFirstName}; ";
                                correctSuc++;
                            }
                       
                            //else
                            //{
                            //    observation += "Agent non trouvé et aucune correction trouvée; ";
                            //    hasError = true;
                            //}
                        }
                      
                        agent = db.Fiche_Agents.FirstOrDefault(x =>  x.Name == correctedName && x.FirstName == correctedFirstName);
                        var poste2 = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
                        int? idPost = poste2?.ID;

                        if (agent != null && agent.ID_Post != idPost)
                        {
                            // التحقق مما إذا كان الصف معمول له Check في GridView
                            bool isChecked = row["Selectionné"] != DBNull.Value && Convert.ToBoolean(row["Selectionné"]);
                            if (!isChecked)
                            {
                                if (agent.ID_Post != idPost)
                                {
                                    // جلب اسم الـ Post الصحيح من جدول Fiche_Postes
                                    var correctPoste = db.Fiche_Postes.FirstOrDefault(p => p.ID == agent.ID_Post)?.Name;

                                    if (!string.IsNullOrEmpty(correctPoste))
                                    {
                                        row[posteColumn] = correctPoste; // تصحيح اسم Post في الجدول
                                        observation += $"Poste corrigé : {correctPoste}; ";
                                        hasError = true;
                                        correctSuc++;
                                    }
                                }
                            }
                           
                        }
                        if (agent != null && agent.Matricule != mat)
                        {
                            // البحث عن جميع السجلات التي تحتوي على Matricule المطلوب
                            var allAgentsWithMatricule = db.Fiche_Agents
                                .Where(x => x.Name == agent.Name&&x.FirstName==agent.FirstName)
                                .ToList(); // جلب جميع النتائج

                            if (allAgentsWithMatricule.Count == 1) // إذا وجدت نتيجة واحدة فقط
                            {
                                var correctAgent = allAgentsWithMatricule.First();
                                string correctMat = correctAgent.Matricule;

                                if (!string.IsNullOrEmpty(correctMat))
                                {
                                    row[matriculeColumn] = correctMat; // تصحيح Matricule في الجدول
                                    observation += $"Matricule corrigé : {correctMat}; ";
                                    hasError = true;
                                    correctSuc++;
                                }
                            }
                        }

                        //if (agent != null && agent.Matricule != mat)
                        //{
                        //    if ( agent.Matricule != mat)
                        //    {
                        //        var correctMat = db.Fiche_Agents.FirstOrDefault(x => x.Matricule == agent.Matricule)?.Matricule;
                        //        if (!string.IsNullOrEmpty(correctMat))
                        //        {
                        //            row[matriculeColumn] = correctMat; // تصحيح اسم Post في الجدول
                        //            observation += $"mat corrigé : {correctMat}; ";
                        //            hasError = true;
                        //            correctSuc++;
                        //        }
                        //    }
                        //}
                        // إضافة السطر إلى updatedTable إذا لم يكن هناك خطأ أو تم تصحيحه
                        if (!hasError || (poste != null) ||matricule!=null)
                        {
                            DataRow updatedRow = updatedTable.NewRow();
                            updatedRow.ItemArray = row.ItemArray;
                            updatedRow["Observation"] = observation;
                            updatedTable.Rows.Add(updatedRow);
                        }

                        // إضافة السطر إلى correctedDataTable إذا تم تصحيح Matricule أو Poste أو Département
                        if (originalMatricule != correctedMatricule ||
                            (poste == null && CorrectPosteUsingTokenSetRatio(posteName, allPostes) != null)
                            || name != correctedName ||firstname != correctedFirstName
                            || (agent != null && agent.ID_Post != idPost)  
                           || (agent != null && agent.Matricule != mat))  
                        {
                            DataRow correctedRow = correctedDataTable.NewRow();
                            correctedRow.ItemArray = new object[]
                            {
                             matricule,          // Matricule
                             name,               // Name
                             firstname,          // Firstname
                             row[posteColumn],   // Poste 
                             observation         // Observation
                            };
                            correctedDataTable.Rows.Add(correctedRow);
                        }

                        if (hasError && (poste == null))
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

            gridControl16.DataSource = correctedDataTable;
            gridControl16.RefreshDataSource();

            gridControl4.DataSource = errorTable;

            gridControl6.DataSource = updatedTable;
            gridControl6.RefreshDataSource();

            // عرض رسالة النتيجة
            string message =
                $"{errorCount} erreurs détectées ❌\n" +
                $"{correctCount} lignes traitées avec succès ✅\n" +
                $"{correctSuc} Erreurs qui ont été corrigées ✅";
            MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            errorCount = correctCount = correctSuc = 0;
        }
        #endregion
        public string CorrectPosteUsingTokenSetRatio(string posteName, List<string> allPostes)
        {
            var bestMatch = Process.ExtractOne(posteName, allPostes, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);

            if (bestMatch != null && bestMatch.Score >= 80)
            {
                return bestMatch.Value;
            }

            return null;
        }

        private void btn_save_ItemClick(object sender, ItemClickEventArgs e)
        {
          //  gridControl4.DataSource = null; gridControl6.DataSource = null;
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (!IsDataValide())
            {
                return;
            }

            DataTable table = (DataTable)gridControl6.DataSource;
            #region
            DataTable tableError = (DataTable)gridControl6.DataSource;
            DataTable errorTable = tableError.Clone();
            #endregion


            using (var db = new DAL.DataClasses1DataContext())
            {
                foreach (DataRow row in table.Rows)
                {
                    string matriculeColumn = lkp_Matricule.Text;
                    string nameColumn = lkp_Name.Text;
                    string firstnameColumn = lkp_FirstName.Text;
                    string datePresence = dt_P.Text;
                    string dateConger = dt_CR.Text;

                    string datePresence2 = dt_P2.Text;
                    string dateConger2 = dt_CR2.Text;

                    string posteColumn = lkp_Poste.Text;

                    if (table.Columns.Contains(matriculeColumn) && table.Columns.Contains(nameColumn) &&
                        table.Columns.Contains(posteColumn))
                    {
                        string matricule = row[matriculeColumn]?.ToString() ?? string.Empty;
                        string name = row[nameColumn]?.ToString() ?? string.Empty;
                        string firstname = row[firstnameColumn]?.ToString() ?? string.Empty;
                        string posteName = row[posteColumn]?.ToString() ?? string.Empty;

                        string datePresent = row[datePresence]?.ToString() ?? string.Empty;
                        string dateCongert = row[dateConger]?.ToString()?.Trim() ?? string.Empty;
                        string datePresent2 = row[datePresence2]?.ToString() ?? string.Empty;
                        string dateCongert2 = row[dateConger2]?.ToString()?.Trim() ?? string.Empty;

                        DateTime dateP;
                        DateTime dateCR;
                        DateTime dateP2;
                        DateTime dateCR2;

                        bool isValidDate = DateTime.TryParse(dateCongert, out dateCR);
                        bool isValidDate2 = DateTime.TryParse(datePresent2, out dateP2);
                        bool isValidDate3 = DateTime.TryParse(dateCongert2, out dateCR2);

                        if (!DateTime.TryParse(datePresent, out dateP))
                        {
                            dateP = DateTime.MinValue; // تعيين تاريخ افتراضي
                        }
                       

                        bool hasError = false;
                        string observation = "";
                        var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
                        if (existingAgent == null)
                        {
                            observation += "Le matricule ne pas existe ,.";
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
                        var agent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule == matricule && x.Name == name && x.FirstName == firstname);

                        // ✅ تجاوز السجل إذا كان datePresent فارغًا
                        if (dateP == DateTime.MinValue)
                        {
                            continue;
                        }
                        var agentDetail = new DAL.MVMAgentDetail
                        {
                            ItemID = agent.ID,
                            Date = dateP,
                            Statut = "P",
                        };
                        db.MVMAgentDetails.InsertOnSubmit(agentDetail);
                        db.SubmitChanges();
                        if (!string.IsNullOrWhiteSpace(dateCongert) && isValidDate)
                        {
                            var agentDetail2 = new DAL.MVMAgentDetail
                            {
                                ItemID = agent.ID,
                                Date = dateCR,
                                Statut = "CR",
                            };
                            db.MVMAgentDetails.InsertOnSubmit(agentDetail2);
                            db.SubmitChanges();
                        }
                        if (!string.IsNullOrWhiteSpace(datePresent2) && isValidDate2)
                        {
                            var agentDetail2 = new DAL.MVMAgentDetail
                            {
                                ItemID = agent.ID,
                                Date = dateP2,
                                Statut = "P",
                            };
                            db.MVMAgentDetails.InsertOnSubmit(agentDetail2);
                            db.SubmitChanges();
                        }
                        if (!string.IsNullOrWhiteSpace(dateCongert2) && isValidDate3)
                        {
                            var agentDetail2 = new DAL.MVMAgentDetail
                            {
                                ItemID = agent.ID,
                                Date = dateCR2,
                                Statut = "CR",
                            };
                            db.MVMAgentDetails.InsertOnSubmit(agentDetail2);
                            db.SubmitChanges();
                        }
                    }
                }
            }
            gridControl4.DataSource = errorTable;
            string message =
             $"{errorCount} erreurs détectées ❌\n" +
             $"{correctCount} lignes traitées avec succès ✅\n";
            MessageBox.Show(message, "Résultat du traitement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.DoEvents();
        }
        private DataTable ReadFromGridControl()
        {
            DataTable dataTable = new DataTable();

            // الحصول على المخطط العمودي من GridView
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                dataTable.Columns.Add(gridView1.Columns[i].FieldName);
            }

            // الحصول على البيانات من GridView
            for (int rowIndex = 0; rowIndex < gridView1.DataRowCount; rowIndex++)
            {
                DataRow row = dataTable.NewRow();
                for (int colIndex = 0; colIndex < gridView1.Columns.Count; colIndex++)
                {
                    row[colIndex] = gridView1.GetRowCellValue(rowIndex, gridView1.Columns[colIndex]);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void btn_convert_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //string filePath = openFileDialog.FileName;


                    txt_lien.Text = openFileDialog.FileName;

                    using (var workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        var worksheet = workbook.Worksheet(1); // الورقة الأولى
                        DataTable summaryTable = new DataTable();

                        // إضافة الأعمدة للجدول المختصر
                        summaryTable.Columns.Add("Matricule", typeof(string));
                        summaryTable.Columns.Add("Nom", typeof(string));
                        summaryTable.Columns.Add("Prénom", typeof(string));
                        summaryTable.Columns.Add("Poste", typeof(string));
                        summaryTable.Columns.Add("Premier Jour Présence", typeof(string));
                        summaryTable.Columns.Add("Premier Jour Absence", typeof(string));
                        summaryTable.Columns.Add("Retour de Congé", typeof(string));
                        summaryTable.Columns.Add("Premier Jour Absence Après Présence", typeof(string)); // عمود جديد
                        summaryTable.Columns.Add("Selectionné", typeof(bool));

                        var rows = worksheet.RowsUsed().Skip(1); // تخطي الصف الأول (العناوين)
                        string lastValidPoste = ""; // حفظ آخر قيمة غير فارغة للـ Poste

                        foreach (var row in rows)
                        {
                            string post = row.Cell(1).Value.ToString();
                            if (string.IsNullOrEmpty(post))
                            {
                                post = lastValidPoste;
                            }
                            else
                            {
                                lastValidPoste = post; // تحديث آخر قيمة غير فارغة
                            }
                            if (post.Contains("Total des jours de présence") || post.Contains("Ecart (Nbre des jours d'absence)"))
                                continue;

                            DataRow dataRow = summaryTable.NewRow();

                            // استخراج البيانات الأساسية
                            dataRow["Matricule"] = row.Cell(2).Value.ToString(); // N° = Matricule
                            dataRow["Nom"] = row.Cell(3).Value.ToString();
                            dataRow["Prénom"] = row.Cell(4).Value.ToString();
                            dataRow["Poste"] = post;

                            // معالجة أيام الحضور والغياب
                            string premierJourPresence = "";
                            string premierJourAbsence = "";
                            string retourConge = "";
                            string premierJourAbsenceApresPresence = "";
                            bool estPresent = false; // حالة أول حضور
                            bool estAbsent = false; // حالة أول عطلة
                            bool retourTrouvé = false; // مؤشر لمعرفة هل حدث حضور بعد العطلة

                            for (int col = 6; col <= 33; col++) // الأعمدة من 6 إلى 33 تمثل أيام الشهر
                            {
                                string valeur = row.Cell(col).Value.ToString();
                                string dateCorrespondante = $"{col - 5:00}/02/2025"; // تحويل العمود إلى تاريخ

                                if (valeur == "1" && !estPresent)
                                {
                                    premierJourPresence = dateCorrespondante;
                                    estPresent = true;

                                }
                                else if (valeur == "0" && estPresent && !estAbsent)
                                {
                                    premierJourAbsence = dateCorrespondante;
                                    estAbsent = true;
                                }
                                else if (valeur == "1" && estAbsent && retourConge == "")
                                {
                                    retourConge = dateCorrespondante;
                                    retourTrouvé = true; // تم العثور على العودة من الإجازة
                                }
                                else if (valeur == "0" && retourTrouvé && premierJourAbsenceApresPresence == "")
                                {
                                    premierJourAbsenceApresPresence = dateCorrespondante;
                                    break; // بمجرد العثور على أول غياب بعد العودة، نخرج من اللوب
                                }
                            }

                            dataRow["Premier Jour Présence"] = premierJourPresence;
                            dataRow["Premier Jour Absence"] = premierJourAbsence;
                            dataRow["Retour de Congé"] = retourConge;
                            dataRow["Premier Jour Absence Après Présence"] = premierJourAbsenceApresPresence;

                            summaryTable.Rows.Add(dataRow);
                        }

                        // تعيين مصدر البيانات لـ GridControl
                        gridControl1.DataSource = summaryTable;
                        gridView1.BestFitColumns();

                        // إنشاء محرر CheckEdit وربطه بالعمود
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();
                        gridControl1.RepositoryItems.Add(checkEdit);
                        gridView1.Columns["Selectionné"].ColumnEdit = checkEdit;
                        gridView1.Columns["Selectionné"].Caption = "Sélectionné";
                        gridView1.Columns["Selectionné"].Width = 50;

                        excelData = ReadFromGridControl();

                    }
                }
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            gridView1.ShowPrintPreview();

        }
        #region Les Doublon
        private DataTable excelData;
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (excelData == null || excelData.Rows.Count == 0)
            {
                MessageBox.Show("No data loaded. Please load an Excel file first.");
                return;
            }

            // التحقق من وجود العمودين "Nom" و "Prenom" في الجدول
            if (!excelData.Columns.Contains("Nom") || !excelData.Columns.Contains("Prénom"))
            {
                MessageBox.Show("Columns 'Nom' and 'Prenom' do not exist in the data.");
                return;
            }

            try
            {
                // العثور على التكرارات بناءً على الاسم واللقب معًا
                var duplicateRows = excelData.AsEnumerable()
                    .Where(row => !string.IsNullOrWhiteSpace(row["Nom"].ToString()) &&
                                  !string.IsNullOrWhiteSpace(row["Prénom"].ToString()))
                    .GroupBy(row => new { Nom = row["Nom"].ToString(), Prenom = row["Prénom"].ToString() })
                    .Where(group => group.Count() > 1)
                    .SelectMany(group => group)
                    .ToList();

                var duplicateRowsMat = excelData.AsEnumerable()
                   .Where(row => !string.IsNullOrWhiteSpace(row["Matricule"].ToString()))
                   .GroupBy(row => new { Nom = row["Matricule"].ToString() })
                   .Where(group => group.Count() > 1)
                   .SelectMany(group => group)
                   .ToList();

              
                // إذا كانت هناك صفوف مكررة، نعرضها في DataGridView
                if (duplicateRows.Any())
                {
                    DataTable duplicateDataTable = duplicateRows.CopyToDataTable();
                   // duplicateDataTable.Columns.Add("Selectionné", typeof(bool));
                    gridControl2.DataSource = duplicateDataTable;
                    ConfigureCheckEdit(gridView2, gridControl2, "Selectionné");
                    AddColumnsIfNotExist(duplicateDataTable, "Supprimer");
                    ConfigureDeleteButton(gridView2, gridControl2, gridView1, "Supprimer");
                    gridView2.CellValueChanged += GridView_CellValueChanged;
                    MessageBox.Show("Duplicates found!");
                }
                else
                {
                    MessageBox.Show("No duplicates found.");
                }
                if (duplicateRowsMat.Any())
                {
                    DataTable duplicateDataTableMat = duplicateRowsMat.CopyToDataTable();
                 //   duplicateDataTableMat.Columns.Add("Selectionné", typeof(bool));
                    gridControl3.DataSource = duplicateDataTableMat;
                    ConfigureCheckEdit(gridView3, gridControl3, "Selectionné");
                    AddColumnsIfNotExist(duplicateDataTableMat, "Supprimer");
                    ConfigureDeleteButton(gridView3, gridControl3, gridView1, "Supprimer");
                    gridView3.CellValueChanged += GridView_CellValueChanged;

                    MessageBox.Show("Duplicates found Matricule!");
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigureCheckEdit(GridView gridView, GridControl gridControl, string columnName)
        {
            RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();
            gridControl.RepositoryItems.Add(checkEdit);
            gridView.Columns[columnName].ColumnEdit = checkEdit;
            gridView.Columns[columnName].Caption = "Sélectionné";
            gridView.Columns[columnName].Width = 50;
        }
        private void GridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            GridView currentGridView = sender as GridView;
            if (e.Column.FieldName == "Selectionné")
            {
                string nom = currentGridView.GetRowCellValue(e.RowHandle, "Nom").ToString();
                string prenom = currentGridView.GetRowCellValue(e.RowHandle, "Prénom").ToString();

                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string nom1 = gridView1.GetRowCellValue(i, "Nom").ToString();
                    string prenom1 = gridView1.GetRowCellValue(i, "Prénom").ToString();

                    if (nom == nom1 && prenom == prenom1)
                    {
                        gridView1.SetRowCellValue(i, gridView1.Columns["Selectionné"], e.Value);
                    }
                }
            }
        }
        private void ConfigureDeleteButton(GridView gridView, GridControl gridControl, GridView gridView1, string columnName)
        {
            if (gridView.Columns[columnName] == null)
            {
                GridColumn deleteColumn = new GridColumn
                {
                    FieldName = columnName,
                    Caption = "Supprimer",
                    Visible = true,
                    Width = 80
                };

                gridView.Columns.Add(deleteColumn);
            }

            RepositoryItemButtonEdit deleteButton = new RepositoryItemButtonEdit();
            deleteButton.Buttons[0].Caption = "🗑️";
            deleteButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            deleteButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;

            deleteButton.ButtonClick += (sender, e) =>
            {
                int rowHandle = gridView.FocusedRowHandle;
                if (rowHandle >= 0)
                {
                    // استخراج بيانات الصف المحذوف
                    string nom = gridView.GetRowCellValue(rowHandle, "Nom")?.ToString();
                    string prenom = gridView.GetRowCellValue(rowHandle, "Prénom")?.ToString();
                    string matricule = gridView.GetRowCellValue(rowHandle, "Matricule")?.ToString();

                    // البحث عن الصف المطابق في gridView1 وحذفه
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        string nom1 = gridView1.GetRowCellValue(i, "Nom")?.ToString();
                        string prenom1 = gridView1.GetRowCellValue(i, "Prénom")?.ToString();
                        string matricule1 = gridView1.GetRowCellValue(i, "Matricule")?.ToString();

                        if (nom == nom1 && prenom == prenom1 && matricule == matricule1)
                        {
                            gridView1.DeleteRow(i);
                            break;
                        }
                    }

                    // حذف الصف من gridView2 أو gridView3 حيث تم النقر على الزر
                    gridView.DeleteRow(rowHandle);
                }
            };

            gridControl.RepositoryItems.Add(deleteButton);
            gridView.Columns[columnName].ColumnEdit = deleteButton;
        }


        private void AddColumnsIfNotExist(DataTable table, string columnName)
        {
            if (!table.Columns.Contains(columnName))
            {
                if (columnName == "Selectionné")
                    table.Columns.Add(columnName, typeof(bool));
                else if (columnName == "Supprimer")
                    table.Columns.Add(columnName, typeof(string));
            }
        }
        //private DataTable ReadExcelFile(string filePath)
        //{
        //    try
        //    {
        //        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        //        using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
        //        {
        //            var worksheet = package.Workbook.Worksheets[0]; // اختر الورقة الأولى
        //            DataTable dataTable = new DataTable();

        //            // قراءة الأعمدة (العناوين)
        //            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //            {
        //                dataTable.Columns.Add(worksheet.Cells[1, col].Text);
        //            }

        //            // قراءة البيانات (الأسطر)
        //            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        //            {
        //                DataRow dataRow = dataTable.NewRow();
        //                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //                {
        //                    dataRow[col - 1] = worksheet.Cells[row, col].Text;
        //                }
        //                dataTable.Rows.Add(dataRow);
        //            }

        //            return dataTable;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error reading Excel file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return null;
        //    }
        //}
        #endregion

        private void btn_exl_Click_1(object sender, EventArgs e)
        {
            #region
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
                            dt_P.Properties.DataSource = columnNames;
                            dt_CR.Properties.DataSource = columnNames;
                            dt_P2.Properties.DataSource = columnNames;
                            dt_CR2.Properties.DataSource = columnNames;
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

                            gridControl6.DataSource = dataTable;

                            //  gridView1.OptionsView.AllowCellMerge = true;

                            // تحديد الأعمدة التي يجب دمجها بناءً على أسماء الأعمدة
                            //for (int i = 0; i < columnNames.Count; i++)
                            //{
                            //    if (i < dataTable.Columns.Count)
                            //    {
                            //        gridView1.Columns[i].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                            //    }
                            //}
                            gridView6.BestFitColumns();
                        }
                    }
                }
            }
            #endregion
        }

        private void btn_Correct_Click(object sender, EventArgs e)
        {
            //if (!IsDataValide())
            //{
            //    return;
            //}
            btn_save.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btn_Correction.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            Coreger();
        }
    }
}
