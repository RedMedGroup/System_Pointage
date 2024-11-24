using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Pointage.Classe
{
    public class AgentDataService
    {
        private readonly DAL.DataClasses1DataContext _context;

        public AgentDataService()
        {
            _context = new DAL.DataClasses1DataContext();
        }

        public BindingList<Models.AgentStatus> GetAgentStatuses(int? userAccessPosteID, Master.MVMType type, bool isAdmin)
        {
        
            // استعلام أساسي
            var agentsQuery = _context.MVMAgentDetails
                .GroupBy(agent => agent.ItemID)
                .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
                .Where(agent => agent != null)
                .Join(_context.Fiche_Agents,
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
                        ma.worker.Jour
                    });

            // تطبيق الفلترة بناءً على userAccessPosteID إذا لم يكن المستخدم أدمن
            if (!isAdmin && userAccessPosteID.HasValue)
            {
                agentsQuery = agentsQuery.Where(ma => ma.worker.ScreenPosteD == userAccessPosteID.Value);
            }

        

            // تصفية حسب النوع
            switch (type)
            {

                case Master.MVMType.P:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "A" || ma.agent.Statut == "CR");
                    break;

                case Master.MVMType.A:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "P" /*|| ma.agent.Statut == "CR"*/);
                    break;

                case Master.MVMType.CR:
                    agentsQuery = agentsQuery.Where(ma => ma.agent.Statut == "A" || ma.agent.Statut == "P");
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
                    Jour = ma.Jour.GetValueOrDefault(),
                    CalculatedDate = Convert.ToDateTime(ma.agent.Date).AddDays(ma.Jour.GetValueOrDefault())
                })
                .ToList();

            return new BindingList<Models.AgentStatus>(agents);
        }
        #region ////////////////////////
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
                        select new
                        {
                            agent.ID,
                           AgentID = agent.ID,
                            Name = agent.Name,
                            PostName = post.Name,
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


        //#region ////////////////////////
        //private readonly DAL.DataClasses1DataContext _dbContext;

        //public AgentDataService(DAL.DataClasses1DataContext dbContext)
        //{
        //    _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        //}

        //public List<object> GetAgents(int? userAccessPosteID, bool isAdmin)
        //{
        //    // الخطوة 1: جلب جميع السجلات من Fiche_Agents وFiche_Postes دون أي شروط
        //    var query = from agent in _dbContext.Fiche_Agents
        //                join post in _dbContext.Fiche_Postes on agent.ID_Post equals post.ID
        //                select new
        //                {
        //                    AgentID = agent.ID,
        //                    Name = agent.Name,
        //                    PostName = post.Name,
        //                    ScreenPosteD = agent.ScreenPosteD
        //                };

        //    // الخطوة 2: تطبيق الفلترة بناءً على حالة المستخدم
        //    if (!isAdmin) // إذا لم يكن المستخدم Admin
        //    {
        //        if (userAccessPosteID.HasValue)
        //        {
        //            query = query.Where(q => q.ScreenPosteD == userAccessPosteID.Value);
        //        }
        //        else
        //        {
        //            return new List<object>(); // إرجاع قائمة فارغة إذا لم يتم تمرير userAccessPosteID
        //        }
        //    }

        //    // الخطوة 3: إرجاع القائمة بعد الفلترة
        //    return query.Distinct().ToList<object>();
        //}
        //#endregion



    }
}
