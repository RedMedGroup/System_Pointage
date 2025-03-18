using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;
using FuzzySharp.SimilarityRatio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FuzzySharp;

namespace System_Pointage.Classe
{
    public class DataCorrection
    {

            private DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext();
        public (string CorrectedName, string CorrectedFirstName) CorrectAgentNameUsingTokenSetRatio(
    string matricule, string posteName, string name, string firstName)
        {
            // البحث عن الوكيل حسب Matricule و Poste
            var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
            var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
            int? idPost = poste?.ID; // استخراج معرف الوظيفة إذا وجد

            // التأكد من وجود Matricule و Poste في قاعدة البيانات
            if (existingAgent == null || poste == null)
            {
                return (name, firstName); // إذا لم يتم العثور، نرجع القيم كما هي
            }

            // جلب جميع الأسماء والألقاب معًا للبحث عن أفضل تطابق
            var allAgents = db.Fiche_Agents
                .Where(x => x.ID_Post == idPost) // البحث فقط في نفس القسم
                .Select(x => new
                {
                    FullName = $"{x.Name} {x.FirstName}", // الجمع بين الاسم واللقب
                    Name = x.Name,
                    FirstName = x.FirstName
                })
                .ToList();

            // البحث عن أقرب تطابق للاسم واللقب معًا
            var bestMatch = FuzzySharp.Process.ExtractOne(
                $"{name} {firstName}", // الاسم واللقب المدخلان
                allAgents.Select(x => x.FullName), // قائمة الأسماء والألقاب في قاعدة البيانات
                scorer: ScorerCache.Get<TokenSetScorer>(), // استخدام TokenSetScorer
                cutoff: 80 // عتبة التشابه 80%
            );

            // إذا تم العثور على تطابق
            if (bestMatch != null && bestMatch.Score >= 80)
            {
                var matchedAgent = allAgents[bestMatch.Index];
                return (matchedAgent.Name, matchedAgent.FirstName); // إرجاع الاسم واللقب المصححين
            }

            // إذا لم يتم العثور على تطابق
            return (name, firstName); // إرجاع القيم الأصلية
        }

        //public (string CorrectedName, string CorrectedFirstName) CorrectAgentNameUsingTokenSetRatio(
        //    string matricule, string posteName, string name, string firstName)
        //{



        //// البحث عن الوكيل حسب Matricule و Poste
        //var existingAgent = db.Fiche_Agents.FirstOrDefault(x => x.Matricule.Trim() == matricule.Trim());
        //    var poste = db.Fiche_Postes.FirstOrDefault(c => c.Name == posteName);
        //   int? idPost = poste?.ID; // استخراج معرف الوظيفة إذا وجد
        //                            // التأكد من وجود Matricule و Poste في قاعدة البيانات
        //if ( existingAgent == null || poste == null)
        //{
        //    return (name, firstName); // إذا لم يتم العثور، نرجع القيم كما هي
        //}

        //// جلب جميع الأسماء واللقب للبحث عن أفضل تطابق
        //var allNames = db.Fiche_Agents.Select(x => x.Name).Distinct().ToList();
        //    var allFirstNames = db.Fiche_Agents.Select(x => x.FirstName).Distinct().ToList();

        //// تصحيح الاسم
        //var bestNameMatch = FuzzySharp.Process.ExtractOne(name, allNames, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);
        //    if (bestNameMatch != null && bestNameMatch.Score >= 80)
        //    {
        //        name = bestNameMatch.Value;
        //    }

        //    // تصحيح اللقب
        //    var bestFirstNameMatch = FuzzySharp.Process.ExtractOne(firstName, allFirstNames, scorer: ScorerCache.Get<TokenSetScorer>(), cutoff: 80);
        //    if (bestFirstNameMatch != null && bestFirstNameMatch.Score >= 80)
        //    {
        //        firstName = bestFirstNameMatch.Value;
        //    }

        //return (name, firstName);
        //}
    }
}
