using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.SimilarityRatio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;
using System.Data;
using System_Pointage.DAL;

namespace System_Pointage.Classe
{
    public class DataCorrectionHelper
    {
        private DAL.DataClasses1DataContext db;

        public DataCorrectionHelper(DAL.DataClasses1DataContext dbContext)
        {
            db = dbContext;
        }
        public bool IsNameValid(string name, string firstname)
        {
            // استخدام LINQ للتحقق من وجود الاسم واللقب في الجدول المناسب
            var isValid = db.Fiche_Agents.Any(e => e.Name == name && e.FirstName == firstname);
            return isValid;
        }
        public bool IsNameValidInDatabase(string name, string firstname)
        {
            // إنشاء سياق قاعدة البيانات
            var dbContext = new DAL.DataClasses1DataContext();

            // إنشاء كائن من DataCorrectionHelper
            var dataCorrectionHelper = new DataCorrectionHelper(dbContext);

            // التحقق من صحة الاسم واللقب
            return dataCorrectionHelper.IsNameValid(name, firstname);
        }
        // تصحيح Poste
        public string CorrectPosteUsingTokenSetRatio(string posteName, List<string> allPostes)
        {
            var bestMatch = Process.ExtractOne(posteName, allPostes, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);

            if (bestMatch != null && bestMatch.Score >= 80)
            {
                return bestMatch.Value;
            }

            return null;
        }
        public (string CorrectedPoste, string Observation) CorrectPoste(string posteName, List<string> allPostes)
        {
            string observation = "";
            string correctedPoste = posteName;

            var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
            if (poste == null)
            {
                correctedPoste = CorrectPosteUsingTokenSetRatio(posteName, allPostes);
                if (!string.IsNullOrEmpty(correctedPoste))
                {
                    observation += $"Poste corrigé automatiquement: {correctedPoste}; ";
                }
                else
                {
                    observation += "Poste non trouvée et aucune correction trouvée; ";
                }
            }

            return (correctedPoste, observation);
        }

        // تصحيح الاسم واللقب
        public (string CorrectedName, string CorrectedFirstName, string Observation, bool HasError) CorrectNameAndFirstName(
        string matricule, string posteName, string name, string firstname)
        {
            string observation = "";
            string correctedName = name;
            string correctedFirstName = firstname;
            bool hasError = false;

            var dataCorrection = new DataCorrection();

            // البحث عن أقرب تطابق للاسم واللقب معًا
            var correctionResult = dataCorrection.CorrectAgentNameUsingTokenSetRatio(matricule, posteName, name, firstname);

            // تحديث القيم المصححة
            correctedName = correctionResult.CorrectedName;
            correctedFirstName = correctionResult.CorrectedFirstName;

            // التحقق مما إذا كان الاسم أو اللقب قد تم تصحيحه
            bool nameCorrected = correctedName != name;
            bool firstNameCorrected = correctedFirstName != firstname;

            if (nameCorrected || firstNameCorrected)
            {
                if (nameCorrected)
                {
                    observation += $"Nom corrigé automatiquement: {correctedName}; ";
                }

                if (firstNameCorrected)
                {
                    observation += $"Prénom corrigé automatiquement: {correctedFirstName}; ";
                }
            }
            else
            {
                // التحقق من صحة الاسم واللقب في قاعدة البيانات
                bool isNameValid = IsNameValidInDatabase(name, firstname);

                if (!isNameValid)
                {
                    observation += "Aucune correction trouvée pour le nom et le prénom; ";
                    hasError = true;
                }
               
            }

            return (correctedName, correctedFirstName, observation, hasError);
        }
        //public bool IsNameValidInDatabase(string name, string firstname)
        //{
        //    // قم بتنفيذ الاستعلام إلى قاعدة البيانات للتحقق من صحة الاسم واللقب
        //    // هذا مثال بسيط، يجب استبداله بالاستعلام الفعلي إلى قاعدة البيانات
        //    var dataCorrection = new DataCorrection();
        //    return dataCorrection.IsNameValid(name, firstname);
        //}

        // تصحيح Poste بناءً على ID_Post
        public (string CorrectedPoste, string Observation) CorrectPosteBasedOnIDPost(
            Fiche_Agent agent, int? idPost, DataRow row, string posteColumn)
        {
            string observation = "";
            string correctedPoste = row[posteColumn]?.ToString();

            if (agent != null && agent.ID_Post != idPost)
            {
                var correctPoste = db.Fiche_Postes.FirstOrDefault(p => p.ID == agent.ID_Post)?.Name;

                if (!string.IsNullOrEmpty(correctPoste))
                {
                    correctedPoste = correctPoste;
                    observation += $"Poste corrigé : {correctPoste}; ";
                }
            }

            return (correctedPoste, observation);
        }

        // تصحيح Matricule
        public (string CorrectedMatricule, string Observation) CorrectMatricule(
       string name, string firstname, string posteName, DataRow row, string matriculeColumn)
        {
            string observation = "";
            string correctedMatricule = row[matriculeColumn]?.ToString();

            // البحث عن جميع الأشخاص الذين لديهم نفس الاسم واللقب والقسم
            var allAgentsWithSameNameAndPoste = db.Fiche_Agents
                .Where(x => x.Name == name && x.FirstName == firstname && x.ID_Post == db.Fiche_Postes
                    .Where(p => p.Name == posteName)
                    .Select(p => (int?)p.ID) // تحويل النتيجة إلى Nullable<int>
                    .FirstOrDefault())
                .ToList();

            if (allAgentsWithSameNameAndPoste.Count == 1) // إذا وجد شخص واحد فقط
            {
                var correctAgent = allAgentsWithSameNameAndPoste.First();
                string correctMatricule = correctAgent.Matricule;

                // التحقق مما إذا كان Matricule في Grid خاطئًا
                if (correctedMatricule != correctMatricule)
                {
                    correctedMatricule = correctMatricule;
                    observation += $"Matricule corrigé : {correctedMatricule}; ";
                }
                // إذا كان Matricule صحيحًا، لا نضيف أي ملاحظة
            }
            else if (allAgentsWithSameNameAndPoste.Count > 1) // إذا وجد أكثر من شخص
            {
                observation += "Plusieurs agents avec le même nom, prénom et poste trouvés; aucune correction appliquée; ";
            }
            else // إذا لم يتم العثور على أي شخص
            {
                observation += "Aucun agent correspondant trouvé; ";
            }

            return (correctedMatricule, observation);
        }
    }
}

