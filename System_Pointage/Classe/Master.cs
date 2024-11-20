using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Pointage.Classe
{
    public static class Master
    {
        public class ValueAndID
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public static List<ValueAndID> ReceptionTypesList = new List<ValueAndID>() {
                new ValueAndID() { ID = (int)MVMType.P, Name="Présent" },
                new ValueAndID() { ID = (int)MVMType.A, Name="Absent" },
                new ValueAndID() { ID = (int)MVMType.CR, Name="Congée" },
                };
 
        public enum MVMType
        {
            P = SourceTypes.Présent,
            A = SourceTypes.Absent,
            CR = SourceTypes.Congée,
        }
        public enum SourceTypes
        {
            Présent,
            Absent,
            Congée,
        }
        public enum UserType
        {
            Admin = 1,
            User,
        }
        public enum Actions
        {
            Show = 1,
            Open,
            Add,
            Edit,
            Delete,
            Print,
        }
        private static List<ScreensAccessProfile> _screensAccesses;
        public static List<ScreensAccessProfile> ScreensAccesses
        {
            get
            {             
                return _screensAccesses;
            }
        }
        private static DAL.User _user;
        public static DAL.User User { get { return _user; } }

        public static void SetUser(DAL.User user)
        {
            _user = user;
            using (DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext())
            {
                _screensAccesses = (from s in Classe.Screens.GetScreens
                                    from d in db.UserAccessProfileDetails
                                    .Where(x => x.ProfileID == user.ScreenProfileID && x.ScreenID == s.ScreenID).DefaultIfEmpty()
                                    select new Classe.ScreensAccessProfile(s.ScreenName)
                                    {
                                        Actions = s.Actions,
                                        CanAdd = (d == null) ? true : d.CanAdd,
                                        CanDelet = (d == null) ? true : d.CanDelete,
                                        CanEdit = (d == null) ? true : d.CanEdit,
                                        CanOpen = (d == null) ? true : d.CanOpen,
                                        CanPrint = (d == null) ? true : d.CanPrint,
                                        CanShow = (d == null) ? true : d.CanShow,
                                        ScreenName = s.ScreenName,
                                        ScreenCaption = s.ScreenCaption,
                                        ScreenID = s.ScreenID,
                                        ParentScreenID = s.ParentScreenID,
                                        SsvgImage = s.SsvgImage,
                                    }).ToList();
            }
        }
       
    }
}
