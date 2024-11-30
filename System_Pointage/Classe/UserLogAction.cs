using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System_Pointage.Classe
{
    public class UserLogAction
    {
        public string PartName;
        public int PartID;
        public string Name;
       public string screenName;
        public bool IsNew { get; set; } = true; // تحديد إذا كان الحفظ جديدًا أم تعديلًا

        public enum ActionType
        {
            Add,
            Edit,
            Delete,
            Print,
        }
        //public void InsertUserAction(ActionType actionType)
        //{
        //    InsertUserAction(actionType, this.PartID, this.PartName, this.Name);
        //}
        public void InsertUserAction(ActionType actionType)
        {
            // التحقق من القيم قبل الاستدعاء
            if (this.PartID == 0)
                throw new ArgumentException("PartID must be a valid ID and cannot be 0.");
            if (string.IsNullOrEmpty(this.PartName))
                throw new ArgumentException("PartName cannot be null or empty.");
            if (string.IsNullOrEmpty(this.Name))
                throw new ArgumentException("ScreenName (Name) cannot be null or empty.");

            // استدعاء النسخة العامة مع تمرير القيم
            InsertUserAction(actionType, this.PartID, this.PartName, this.Name);
        }

        public static void InsertUserAction(ActionType actionType, int partID, string partName, string screenName)
        {
            int screenID = GetScreenID(screenName);
            // الحصول على ScreenID بناءً على ScreenName
           

            // إنشاء اتصال بقاعدة البيانات
            using (var db = new DAL.DataClasses1DataContext())
            {
                // إعداد كائن السجل
                var userLog = new DAL.UserLog()
                {
                    UserID = Master.User.ID,
                    PartID = partID,
                    PartName = partName ?? throw new ArgumentNullException(nameof(partName), "PartName cannot be null."),
                    ActionType = (byte)actionType,
                    ActionDate = DateTime.Now,
                    ScreenID = screenID,
                };

                // إدخال السجل في الجدول
                db.UserLogs.InsertOnSubmit(userLog);

                // حفظ التغييرات
                db.SubmitChanges();
            }
        }
        public static int GetScreenID(string screenName)
        {
            int screenID = 0;

            // الحصول على الـ ScreenID من كلاس Screens
            var screen = Classe.Screens.GetScreens.SingleOrDefault(x => x.ScreenName == screenName);
            if (screen != null)
            {
                screenID = screen.ScreenID;
            }
            else
            {
                // معالجة الحالة إذا لم يتم العثور على الشاشة
                throw new ArgumentException($"Screen with name '{screenName}' was not found.");
            }

            return screenID;
        }

        public static bool CheckActionAuthorization(string formName, Master.Actions actions, DAL.User user = null)
        {
            // التحقق من صلاحيات المستخدم
            if (user == null)
                user = Master.User;

            if (user.UserType == (byte)Master.UserType.Admin)
                return true;

            var screen = Master.ScreensAccesses.SingleOrDefault(x => x.ScreenName == formName);
            if (screen != null)
            {
                bool flag = false;
                switch (actions)
                {
                    case Master.Actions.Add:
                        flag = screen.CanAdd;
                        break;
                    case Master.Actions.Edit:
                        flag = screen.CanEdit;
                        break;
                    case Master.Actions.Delete:
                        flag = screen.CanDelet;
                        break;
                    case Master.Actions.Print:
                        flag = screen.CanPrint;
                        break;
                    default:
                        flag = false;
                        break;
                }

                return flag;

            }

            // إذا لم يكن لديه صلاحيات
            XtraMessageBox.Show(
                text: "Vous n'avez pas accés",
                caption: "",
                icon: MessageBoxIcon.Error,
                buttons: MessageBoxButtons.OK
            );

            return false;
        }
        public virtual void SaveAction(Action additionalSaveLogic)
        {
            // التحقق من الصلاحيات
            if (!CheckActionAuthorization(this.Name, IsNew ? Master.Actions.Add : Master.Actions.Edit))
            {
                return;
            }

            // تنفيذ إجراء الحفظ الخاص
            additionalSaveLogic?.Invoke();

            // تسجيل الإجراء
            InsertUserAction(IsNew ? ActionType.Add : ActionType.Edit);

            // تحديث الحالة
            IsNew = false;

            // رسالة نجاح
            XtraMessageBox.Show("Enregistrer succès", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //public void SaveAction()
        //{
        //    if (CheckActionAuthorization(this.Name, IsNew ? Master.Actions.Add : Master.Actions.Edit))
        //    {
        //        if (IsNew == false) { InsertUserAction(ActionType.Add); }
        //        else { InsertUserAction(ActionType.Edit); IsNew = false; }
        //    }
        //}
    }
}
