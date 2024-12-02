using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace System_Pointage.Form
{
    public partial class frm_AccessProfile : DevExpress.XtraEditors.XtraForm
    {
        Classe.ScreensAccessProfile ins;
        DAL.UserAccessProfileName profile;
        DAL.User user;
        RepositoryItemCheckEdit repoChech;
        public frm_AccessProfile()
        {
            InitializeComponent();
            New();
           // PartID = profile.ID; PartName = profile.Name;
            GetDataList();
           
        }
        public frm_AccessProfile(int id)
        {
            InitializeComponent();
            using (var db = new DAL.DataClasses1DataContext())
            {
                profile = db.UserAccessProfileNames.SingleOrDefault(x => x.ID == id);
            }
            textEdit1.Text = profile.Name;
            //PartID = profile.ID;
            //PartName = profile.Name;
            GetDataList();

        }
        private void frm_AccessProfile_Load(object sender, EventArgs e)
        {
            treeList1.KeyFieldName = nameof(ins.ScreenID);
            treeList1.ParentFieldName = nameof(ins.ParentScreenID);
            treeList1.Columns[nameof(ins.CanPrint)].Visible = false;
            treeList1.Columns[nameof(ins.CanDelet)].Visible = false;
            treeList1.Columns[nameof(ins.SsvgImage)].Visible = false;
            treeList1.Columns[nameof(ins.ScreenName)].Visible = false;
            treeList1.Columns[nameof(ins.ScreenName)].OptionsColumn.AllowEdit = false;
            treeList1.Columns[nameof(ins.ScreenCaption)].OptionsColumn.AllowEdit = false;
            treeList1.Columns[nameof(ins.CanAdd)].Caption = "Valider";
            treeList1.Columns[nameof(ins.CanDelet)].Caption = "Supprimer";
            treeList1.Columns[nameof(ins.CanEdit)].Caption = "Modifier";
            treeList1.Columns[nameof(ins.CanOpen)].Caption = "Ouvert";
            treeList1.Columns[nameof(ins.CanPrint)].Caption = "Impression";
            treeList1.Columns[nameof(ins.CanShow)].Caption = "Visible";

            treeList1.BestFitColumns();
            repoChech = new RepositoryItemCheckEdit();
            repoChech.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgStar1;
            repoChech.CheckBoxOptions.SvgColorChecked = DXSkinColors.ForeColors.WindowText;
            repoChech.CheckBoxOptions.SvgColorUnchecked = DXSkinColors.ForeColors.Question;
            treeList1.Columns[nameof(ins.CanAdd)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanDelet)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanEdit)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanOpen)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanPrint)].ColumnEdit =
            treeList1.Columns[nameof(ins.CanShow)].ColumnEdit = repoChech;
            treeList1.CustomNodeCellEdit += TreeList1_CustomNodeCellEdit;
        }

        private void TreeList1_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if (e.Node.Id >= 0)
            {
                var row = treeList1.GetRow(e.Node.Id) as Classe.ScreensAccessProfile;
                if (row != null)
                {
                    if (e.Column.FieldName == nameof(ins.CanAdd) && row.Actions.Contains(Classe.Master.Actions.Add) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                    else if (e.Column.FieldName == nameof(ins.CanDelet) && row.Actions.Contains(Classe.Master.Actions.Delete) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                    else if (e.Column.FieldName == nameof(ins.CanEdit) && row.Actions.Contains(Classe.Master.Actions.Edit) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                    else if (e.Column.FieldName == nameof(ins.CanOpen) && row.Actions.Contains(Classe.Master.Actions.Open) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                    else if (e.Column.FieldName == nameof(ins.CanPrint) && row.Actions.Contains(Classe.Master.Actions.Print) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                    else if (e.Column.FieldName == nameof(ins.CanShow) && row.Actions.Contains(Classe.Master.Actions.Show) == false)
                    {
                        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItem();
                    }
                }
            }
        }
          void New()
        {
            profile = new DAL.UserAccessProfileName();
        }

        public void GetDataList()
        {
            List<Classe.ScreensAccessProfile> data;

            using (DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext())
            {
                data = (from s in Classe.Screens.GetScreens
                        from d in db.UserAccessProfileDetails
                        .Where(x => x.ProfileID == profile.ID && x.ScreenID == s.ScreenID).DefaultIfEmpty()
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
                        }).ToList();
            }
            treeList1.DataSource = data;
        }
          void Save()
        {
            if (textEdit1.Text.Trim() == string.Empty)
            {
                textEdit1.ErrorText = ErrorText;
                return;
            }
            var db = new DAL.DataClasses1DataContext();

            if (profile.ID == 0)
            {
                db.UserAccessProfileNames.InsertOnSubmit(profile);
            }
            else
            {
                db.UserAccessProfileNames.Attach(profile);
            }
            profile.Name = textEdit1.Text;
            db.SubmitChanges();
            db.UserAccessProfileDetails.DeleteAllOnSubmit(
                db.UserAccessProfileDetails.Where(x => x.ProfileID == profile.ID));
            db.SubmitChanges();

            var data = treeList1.DataSource as List<Classe.ScreensAccessProfile>;
            var dbData = data.Select(s => new DAL.UserAccessProfileDetail
            {
                CanAdd = s.CanAdd,
                CanDelete = s.CanDelet,
                CanEdit = s.CanEdit,
                CanOpen = s.CanOpen,
                CanPrint = s.CanPrint,
                CanShow = s.CanShow,
                ProfileID = profile.ID,
                ScreenID = s.ScreenID
            }).ToList();
            db.UserAccessProfileDetails.InsertAllOnSubmit(dbData);
            db.SubmitChanges();
            XtraMessageBox.Show("Enregistrer succès", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static string ErrorText
        {
            get
            {
                return "Ce champ est obligatoire";
            }
        }

        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }
    }
}