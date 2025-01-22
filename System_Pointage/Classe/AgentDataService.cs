using DevExpress.XtraPivotGrid.Data;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System_Pointage.Classe.Models;

namespace System_Pointage.Classe
{
    public class AgentDataService
    {
        public readonly DAL.DataClasses1DataContext _context;
        public AgentDataService()
        {
            _context = new DAL.DataClasses1DataContext();
        }
        public bool DoesAgentRecordExist(string matricule, DateTime date)
        {
            var agentID = _context.Fiche_Agents.FirstOrDefault(f => f.Matricule == matricule)?.ID;

            if (agentID.HasValue)
            {
                return _context.MVMAgentDetails
                               .Any(d => d.ItemID == agentID.Value && d.Date == date);
            }

            return false;
        }
        public bool DoesMaterielRecordExist(string matricule, DateTime date)
        {
            var agentID = _context.Fiche_Matricules.FirstOrDefault(f => f.Matricule == matricule)?.ID;

            if (agentID.HasValue)
            {
                return _context.MVMmaterielsDetails
                               .Any(d => d.ItemID == agentID.Value && d.Date == date);
            }

            return false;
        }
        public BindingList<Models.AgentStatus> GetAgentStatuses(int? userAccessPosteID, Master.MVMType type, bool isAdmin ,int? idAttentListe = null, bool fetchNullOnly = false, string formName = null)
        {
            var agentsQueryF = _context.MVMAgentDetails.AsQueryable();
            IEnumerable<dynamic> agentsQuery = Enumerable.Empty<dynamic>();
            // تحديد الشرط بناءً على fetchNullOnly
            if (fetchNullOnly)
            {
                agentsQueryF = agentsQueryF.Where(x => x.ID_Attent_Liste == null);
            }
            else
            {
                agentsQueryF = agentsQueryF.Where(x => x.ID_Attent_Liste == idAttentListe);
            }
            //if (formName == "Frm_Heir" /*|| formName == "Frm_WorkDays"*/)
            //{
                #region
   //             agentsQuery = agentsQueryF
   //.GroupBy(agent => agent.ItemID)
   //.ToList() // تحميل البيانات إلى الذاكرة
   //.Select(g =>
   //{
   //    var orderedRecords = g.OrderBy(x => x.Date).ToList();

   //    // البحث عن أول حالة "P" بعد "CR"
   //    bool foundCR = false; // هل وجدنا "CR"
   //    foreach (var record in orderedRecords)
   //    {
   //        if (record.Statut == "CR")
   //        {
   //            foundCR = true; // وجدنا عطلة
   //        }
   //        else if (record.Statut == "P" && foundCR)
   //        {
   //            return record; // أول حضور بعد عطلة
   //        }
   //    }
   //    foreach (var record in orderedRecords)
   //    {
   //        if (record.Statut == "CR")
   //        {
   //            return record;// وجدنا عطلة
   //        }
   //    }
   //    // إذا لم يتم العثور على الحضور بعد "CR"، ارجع إلى أول سجل
   //    return orderedRecords.FirstOrDefault();
   //})
   //.Where(agent => agent != null)
   //.Join(_context.Fiche_Agents.Where(x => x.Statut == true),
   //agent => agent.ItemID,
   //worker => worker.ID,
   //(agent, worker) => new { agent, worker })
   //.Join(_context.Fiche_Postes,
   //ma => ma.worker.ID_Post,
   //poste => poste.ID,
   //(ma, poste) => new
   //{
   //    ma.worker,
   //    ma.agent,
   //    poste.Name,
   //    ma.worker.Jour,
   //});
                #endregion



            //}


            if (formName == "frm_op" || formName == "Frm_WorkDays" || formName == "Frm_Heir" )
            {
                // استعلام أساسي
                 agentsQuery = agentsQueryF
                    .GroupBy(agent => agent.ItemID)
                    .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                    .Where(agent => agent != null)
                    .Join(_context.Fiche_Agents.Where(x => x.Statut == true),
                        agent => agent.ItemID,
                        worker => worker.ID,
                        (agent, worker) => new { agent, worker })
                    .Join(_context.Fiche_Postes,
                        ma => ma.worker.ID_Post,
                        poste => poste.ID,
                        (ma, poste) => new
                        {
                            ma.worker,
                            ma.agent,
                            poste.Name,
                            ma.worker.Jour,
                        });
            }


            // تطبيق الفلترة بناءً على userAccessPosteID إذا لم يكن المستخدم أدمن
            if (!isAdmin && userAccessPosteID.HasValue)
            {
                agentsQuery = agentsQuery.Where(ma => ma.worker.ScreenPosteD == userAccessPosteID.Value);
            }

        

            // تصفية حسب النوع
            switch (type)
            {

                case Master.MVMType.P:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "CR" || ma.agent.Statut == "A");
                    break;

                case Master.MVMType.A:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "P" /*|| ma.agent.Statut == "CR"*/);
                    break;

                case Master.MVMType.CR:
                    if (formName == "Frm_Heir"/*|| formName == "Frm_WorkDays"*/)
                        agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "CR" || ma.agent.Statut == "P");
                    else if ( formName == "Frm_WorkDays")
                        agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "P" );
                    else// frm_operation partant/
                        agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "A" || ma.agent.Statut == "P");

                    break;
                case Master.MVMType.P_A_CR:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "P" || ma.agent.Statut == "CR" ||  ma.agent.Statut == "A");
                    break;
                default:
                    throw new NotImplementedException();
            }

            // تحويل البيانات إلى الكلاس AgentStatus
            var agents = agentsQuery
                .Select(ma => new Models.AgentStatus
                {
                    Name = ma.worker.Name,
                    Date = ma.agent.Date,
                    Statut = ma.agent.Statut,
                    Poste = ma.Name,
                    screenPosteD =ma.worker.ScreenPosteD ??0,
                    Jour = ma.Jour,
                    CalculatedDate = Convert.ToDateTime(ma.agent.Date).AddDays(ma.Jour),
                    Matricule = ma.worker.Matricule,
                    Affecter = ma.worker.Affecter,
                    DaysCount = ma.agent.Statut == "P"
                  ? (DateTime.Now - ma.agent.Date).Days 
                 : ma.agent.Statut == "CR"
                   ? (DateTime.Now - ma.agent.Date).Days 
                  : 0, // في الحالات الأخرى، القيمة 0
                    Difference = 0
                })
                .ToList();
            foreach (var agent in agents)
            {
                agent.Difference = agent.DaysCount - agent.Jour;
            }
            return new BindingList<Models.AgentStatus>(agents);
        }

        #region ////////////////////////  Agent liste
        private readonly DAL.DataClasses1DataContext _dbContext;

        public AgentDataService(DAL.DataClasses1DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<object> GetAgents(int? userAccessPosteID, bool isAdmin)
        {
            // الخطوة 1: جلب جميع السجلات من Fiche_Agents وربطها بالجدول المناسب
            var query = from agent in _dbContext.Fiche_Agents
                        join post in _dbContext.Fiche_Postes on agent.ID_Post equals post.ID
                        join userAccess in _dbContext.UserAccessProfilePostes on agent.ScreenPosteD equals userAccess.ID
                        join accessDetails in _dbContext.UserAccessProfilePosteDetails on userAccess.ID equals accessDetails.ID_AccessPoste

                        join lastMovement in (
                       from mv in _dbContext.MVMAgentDetails
                       group mv by (int)mv.ItemID into g // تحويل ItemID إلى int إذا لزم الأمر
                       select new
                       {
                           ItemID = g.Key,
                           LastDate = g.Max(x => x.Date)
                       }
                   ) on agent.ID equals lastMovement.ItemID into mvGroup
                        from mv in mvGroup.DefaultIfEmpty()
                        join lastStatut in _dbContext.MVMAgentDetails
                            on new { ItemID = (int)mv.ItemID, LastDate = mv.LastDate }
                            equals new { ItemID = (int)lastStatut.ItemID, LastDate = lastStatut.Date } into statutGroup
                        from statut in statutGroup.DefaultIfEmpty()

                        select new
                        {
                            agent.ID,
                            AgentID = agent.ID,
                            agent.Matricule,
                            Name = agent.Name,
                            FirstName = agent.FirstName,
                            PostID=post.ID.ToString(),// new
                            PostName = post.Name,
                            agent.Affecter,
                            agent.Jour,
                            agent.Date_Embauche,
                            agent.Statut,
                            Statutmvm = statut != null ? statut.Statut : "Aucun mouvement enregistré",
                            ScreenPosteD = agent.ScreenPosteD,
                            AccessPosteID = accessDetails.ID_AccessPoste,
                            NamePosteDetail = accessDetails.ID_Name_Poste
                        };

            // الخطوة 2: إزالة التكرار باستخدام GroupBy
            var groupedQuery = query
                .GroupBy(q => new { q.AgentID, q.Name, q.PostName, q.ScreenPosteD })
                .Select(g => g.First());

            // الخطوة 3: تطبيق الفلترة بناءً على حالة المستخدم
            if (!isAdmin) // إذا لم يكن المستخدم Admin
            {
                if (userAccessPosteID.HasValue)
                {
                    groupedQuery = groupedQuery.Where(q => q.AccessPosteID == userAccessPosteID.Value);
                }
                else
                {
                    return new List<object>(); // إرجاع قائمة فارغة إذا لم يتم تمرير userAccessPosteID
                }
            }

            // الخطوة 4: إرجاع القائمة
            return groupedQuery.ToList<object>();
        }
        #endregion


        #region ////////////////////////   agent liste 2
        public List<object> GetAgentsWithoutMovement(int? userAccessPosteID, bool isAdmin)
        {
            // الخطوة 1: جلب جميع السجلات من Fiche_Agents وربطها بالجداول الأخرى
            var query = from agent in _dbContext.Fiche_Agents
                        join post in _dbContext.Fiche_Postes on agent.ID_Post equals post.ID
                        join userAccess in _dbContext.UserAccessProfilePostes on agent.ScreenPosteD equals userAccess.ID
                        join accessDetails in _dbContext.UserAccessProfilePosteDetails on userAccess.ID equals accessDetails.ID_AccessPoste
                        select new
                        {
                            agent.ID,
                            AgentID = agent.ID,
                            agent.Matricule,
                            Name = agent.Name,
                            PostName = post.Name,
                            agent.Affecter,
                            agent.Jour,
                            agent.Date_Embauche,
                            agent.Statut,
                            ScreenPosteD = agent.ScreenPosteD,
                            AccessPosteID = accessDetails.ID_AccessPoste,
                            NamePosteDetail = accessDetails.ID_Name_Poste
                        };

            // الخطوة 2: إزالة التكرار باستخدام GroupBy
            var groupedQuery = query
                .GroupBy(q => new { q.AgentID, q.Name, q.PostName, q.ScreenPosteD })
                .Select(g => g.First());

            // الخطوة 3: تصفية الأشخاص الذين ليست لديهم حركة في MVMAgentDetails
            var agentsWithoutMovements = groupedQuery.Where(agent =>
                !_dbContext.MVMAgentDetails.Any(movement => movement.ItemID == agent.AgentID));

            // الخطوة 4: تطبيق الفلترة بناءً على حالة المستخدم
            if (!isAdmin) // إذا لم يكن المستخدم Admin
            {
                if (userAccessPosteID.HasValue)
                {
                    agentsWithoutMovements = agentsWithoutMovements.Where(q => q.AccessPosteID == userAccessPosteID.Value);
                }
                else
                {
                    return new List<object>(); // إرجاع قائمة فارغة إذا لم يتم تمرير userAccessPosteID
                }
            }

            // الخطوة 5: إرجاع القائمة
            return agentsWithoutMovements.ToList<object>();
        }
        #endregion

        #region materiels roulon
        public BindingList<Models.MaterielsStatus> GetAgentMateriels(int? userAccessPosteID, bool isAdmin)
        {
            var agentsQueryF = _context.MVMmaterielsDetails.AsQueryable();
            IEnumerable<dynamic> agentsQuery = Enumerable.Empty<dynamic>();
         
                agentsQueryF = agentsQueryF/*.Where(x => x.ID_Attent_Liste == null)*/;
            

          
                // استعلام أساسي
                agentsQuery = agentsQueryF
                   .GroupBy(agent => agent.ItemID)
                   .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                   .Where(agent => agent != null)
                   .Join(_context.Fiche_Matricules.Where(x => x.Statut == true),
                       agent => agent.ItemID,
                       worker => worker.ID,
                       (agent, worker) => new { agent, worker })
                   .Join(_context.Fiche_materiels,
                       ma => ma.worker.ID_materiels,
                       poste => poste.ID,
                       (ma, poste) => new
                       {
                           ma.worker,
                           ma.agent,
                           poste.Name,
                       });
            


            // تطبيق الفلترة بناءً على userAccessPosteID إذا لم يكن المستخدم أدمن
            if (!isAdmin && userAccessPosteID.HasValue)
            {
                agentsQuery = agentsQuery.Where(ma => ma.worker.ScreenPosteD == userAccessPosteID.Value);
            }


            agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "A" || ma.agent.Statut == "P");


            // تحويل البيانات إلى الكلاس AgentStatus
            var agents = agentsQuery
                .Select(ma => new Models.MaterielsStatus
                {
                    Name = ma.worker.Name,
                    Date = ma.agent.Date,
                    Statut = ma.agent.Statut,
                    Materiel = ma.Name,
                    screenPosteD = ma.worker.ScreenPosteD ?? 0,
                 //   CalculatedDate = Convert.ToDateTime(ma.agent.Date).AddDays(ma.Jour),
                    Matricule = ma.worker.Matricule,
                    Affecter = ma.worker.Affecter,
                    DaysCount = ma.agent.Statut == "P"
                  ? (DateTime.Now - ma.agent.Date).Days
                 : ma.agent.Statut == "CR"
                   ? (DateTime.Now - ma.agent.Date).Days
                  : 0, 
                    
                })
                .ToList();
            
            return new BindingList<Models.MaterielsStatus>(agents);
        }
        #endregion
        #region ////////////////////////   agent liste 2
        public List<object> GetAgentsWithoutMovementMateriels(int? userAccessPosteID, bool isAdmin)
        {
            // الخطوة 1: جلب جميع السجلات من Fiche_Agents وربطها بالجداول الأخرى
            var query = from agent in _dbContext.Fiche_Matricules
                        join post in _dbContext.Fiche_materiels on agent.ID_materiels equals post.ID
                        //join userAccess in _dbContext.UserAccessProfilePostes on agent.ScreenPosteD equals userAccess.ID
                        //join accessDetails in _dbContext.UserAccessProfilePosteDetails on userAccess.ID equals accessDetails.ID_AccessPoste
                        select new
                        {
                            agent.ID,
                            AgentID = agent.ID,
                            agent.Matricule,
                            Name = agent.Name,
                            PostName = post.Name,
                            agent.Affecter,
                            agent.Date_Embauche,
                            agent.Statut,
                            ScreenPosteD = agent.ScreenPosteD,
                            //AccessPosteID = accessDetails.ID_AccessPoste,
                            //NamePosteDetail = accessDetails.ID_Name_Poste
                        };

            // الخطوة 2: إزالة التكرار باستخدام GroupBy
            var groupedQuery = query
                .GroupBy(q => new { q.AgentID, q.Name, q.PostName, q.ScreenPosteD })
                .Select(g => g.First());

            // الخطوة 3: تصفية الأشخاص الذين ليست لديهم حركة في MVMAgentDetails
            var agentsWithoutMovements = groupedQuery.Where(agent =>
                !_dbContext.MVMmaterielsDetails.Any(movement => movement.ItemID == agent.AgentID));

            // الخطوة 4: تطبيق الفلترة بناءً على حالة المستخدم
            if (!isAdmin) // إذا لم يكن المستخدم Admin
            {
                if (userAccessPosteID.HasValue)
                {
                    agentsWithoutMovements = agentsWithoutMovements;/*.Where(q => q.AccessPosteID == userAccessPosteID.Value);*/
                }
                else
                {
                    return new List<object>(); // إرجاع قائمة فارغة إذا لم يتم تمرير userAccessPosteID
                }
            }

            // الخطوة 5: إرجاع القائمة
            return agentsWithoutMovements.ToList<object>();
        }
        #endregion

        #region
    
        #endregion
    }
}
