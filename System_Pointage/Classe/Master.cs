using DevExpress.Utils;
using DevExpress.XtraEditors;
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
        public static List<ValueAndID> UserTypeList = new List<ValueAndID>() {
                new ValueAndID() { ID = (int)UserType.Admin, Name="Admin" },
            new ValueAndID() { ID = (int)UserType.User, Name="Utilisateur"} };
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
        public static void IntializeData(this GridLookUpEdit lkp, object dataSource)
        {
            lkp.IntializeData(dataSource, "Name", "ID");

        }
        public static void IntializeData(this GridLookUpEdit lkp, object dataSource, string displayMember, string valuMember)
        {
            lkp.Properties.DataSource = dataSource;
            lkp.Properties.DisplayMember = displayMember;
            lkp.Properties.ValueMember = valuMember;
            lkp.Properties.NullText = "";
            lkp.Properties.ValidateOnEnterKey = true;
            lkp.Properties.AllowNullInput = DefaultBoolean.False;
            lkp.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
            lkp.Properties.ImmediatePopup = true;
            var PartIDView = lkp.Properties.View;
            //PartIDView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus;
            PartIDView.OptionsSelection.UseIndicatorForSelection = true;
            PartIDView.OptionsView.ShowAutoFilterRow = true;
            //PartIDView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            PartIDView.PopulateColumns(lkp.Properties.DataSource);
        }
        public static void IntializeData(this LookUpEdit lkp, object dataSource)
        {
            lkp.IntializeData(dataSource, "Name", "ID");

        }
        public static void IntializeData(this LookUpEdit lkp, object dataSource, string displayMember, string valuMember)
        {
            lkp.Properties.DataSource = dataSource;
            lkp.Properties.DisplayMember = displayMember;
            lkp.Properties.ValueMember = valuMember;
            lkp.Properties.Columns.Clear();
            lkp.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo()
            {
                FieldName = displayMember,
            });
            lkp.Properties.ShowHeader = false;
            //lkp.Properties.PopulateColumns();
            //lkp.Properties.Columns[valuMember].Visible = false;
            lkp.Properties.NullText = "";
        }
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }
    }
}
