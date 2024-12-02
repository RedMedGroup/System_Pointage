using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;
using static System_Pointage.Classe.Master;

namespace System_Pointage.Form
{
    public partial class Frm_User : DevExpress.XtraEditors.XtraForm
    {
        DAL.User user;
        public Frm_User()
        {
            InitializeComponent();
            New();
        }
        public Frm_User(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DataClasses1DataContext())
            {
                user = db.Users.Single(x => x.ID == id);
            }
            GetData();
            this.Text = string.Format("Données utilisateur   :" + user.Name);
        }
        private void Frm_User_Load(object sender, EventArgs e)
        {
            RefreshData();
            layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }
        #region user
        public  void RefreshData()
        {
            using (var db = new DAL.DataClasses1DataContext())
            {
                lkp_ScreanProfileID.IntializeData(db.UserAccessProfileNames.Select(x => new { x.ID, x.Name }).ToList());
                lkp_ScreanPoste.IntializeData(db.UserAccessProfilePostes.Select(x => new { x.ID,x.Name }).ToList());
                lkp_UserType.IntializeData(Master.UserTypeList);
            }
        }
        #endregion
        public  void New()
        {
            user = new DAL.User();
            user.IsActive = true;
        }
        public  void GetData()
        {
            //////////user
            txt_Name.Text = user.Name;
            txt_UserName.Text = user.UserName;
            txt_PWD.Text = user.Password;

            lkp_UserType.EditValue = user.UserType;
            lkp_ScreanProfileID.EditValue = user.ScreenProfileID;
            lkp_ScreanPoste.EditValue = user.IDAccessPoste;
            toggleSwitch1.IsOn = user.IsActive;
            ////////
        }
        public  void SetData()
        {
            user.Name = txt_Name.Text;
            user.Password = txt_PWD.Text;
            user.UserName = txt_UserName.Text.Trim();
            user.UserType = Convert.ToByte(lkp_UserType.EditValue);
            user.ScreenProfileID = Convert.ToInt32(lkp_ScreanProfileID.EditValue);
            user.IDAccessPoste= Convert.ToInt32(lkp_ScreanPoste.EditValue);
            user.IsActive = toggleSwitch1.IsOn;
        }
        bool IsValidit()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ErrorText;
                return false;
            }
            if (txt_UserName.Text.Trim() == string.Empty)
            {
                txt_UserName.ErrorText = ErrorText;
                return false;
            }
            if (txt_PWD.Text.Trim() == string.Empty)
            {
                txt_PWD.Text = ErrorText;
                return false;
            }
            if (lkp_UserType.Text.Trim() == string.Empty)
            {
                lkp_UserType.ErrorText = ErrorText;
                return false;
            }
            if (lkp_UserType.EditValue is int userTypeId && (UserType)userTypeId == UserType.User)
            {
                if (lkp_ScreanProfileID.Text.Trim() == string.Empty)
                {
                    lkp_ScreanProfileID.ErrorText = ErrorText;
                    return false;
                }
                if (lkp_ScreanPoste.Text.Trim() == string.Empty)
                {
                    lkp_ScreanPoste.ErrorText = ErrorText;
                    return false;
                }
            }
           
            return true;
        }
        public  void Save()
        {
            if (IsValidit() == false)
                return;

            using (var dbc = new DAL.DataClasses1DataContext())
            {
                if (user.ID == 0)
                {
                    dbc.Users.InsertOnSubmit(user);
                }
                else
                {
                    dbc.Users.Attach(user);
                }
                SetData();
                dbc.SubmitChanges();
            }
        }
        //public static string ErrorText
        //{
        //    get
        //    {
        //        return "Ce champ est obligatoire";
        //    }
        //}
        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void lkp_UserType_EditValueChanged(object sender, EventArgs e)
        {
            if (lkp_UserType.EditValue is int userTypeId && (UserType)userTypeId == UserType.User)
            {
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
    }
}