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

namespace System_Pointage.Frm_List
{
    public partial class frm_User_MVM : DevExpress.XtraEditors.XtraForm
    {
        public frm_User_MVM()
        {
            InitializeComponent();
        }

        private void frm_User_MVM_Load(object sender, EventArgs e)
        {
            GetHistory();
        }
        public void GetHistory() //mvm user
        {
           
            using (var db = new DAL.DataClasses1DataContext())
            {
                var data = db.UserLogs;//.Where(x => x.PartID == Invoice.ID && (x.ScreenID == mainSecrinID || x.ScreenID == listScreenID));
                gridControl1.DataSource = (from d in data
                                           join u in db.Users on d.UserID equals u.ID
                                           select new
                                           {
                                               u.Name,
                                               d.ActionDate,
                                               Action = (d.ActionType == (byte)Classe.UserLogAction. ActionType.Add) ? "Ajouter" :
                                           (d.ActionType == (byte)Classe.UserLogAction.ActionType.Delete) ? "Supprimer" :
                                           (d.ActionType == (byte)Classe.UserLogAction.ActionType.Edit) ? "Modifier" :
                                           (d.ActionType == (byte)Classe.UserLogAction.ActionType.Print) ? "Imprimer" :
                                               (d.ActionType == (byte)Classe.UserLogAction.ActionType.Entrée) ? "Rentrée" :
                                                 (d.ActionType == (byte)Classe.UserLogAction.ActionType.Sortie) ? "Sortie" :
                                               (d.ActionType == (byte)Classe.UserLogAction.ActionType.Absent) ? "Absent" : "",
                                               d.PartName,
                                           }).ToList();
            }
            gridView1.Columns["Name"].Caption = "Nom d'utilisateur"; 
            gridView1.Columns["ActionDate"].Caption = "Date de l'action";
            gridView1.Columns["Action"].Caption = "Type d'action"; 
            gridView1.Columns["PartName"].Caption = "Rapport"; 
            gridView1.Columns["ActionDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            gridView1.Columns["ActionDate"].DisplayFormat.FormatString = "yyyy-MM-dd hh:mm:ss";
            //gridView1.RowStyle += GridView1_RowStyle;
            gridView1.RowCellStyle += GridView1_RowCellStyle;
        }

        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            var action = gridView1.GetRowCellValue(e.RowHandle, "Action");
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Ajouter")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Supprimer")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Modifier")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Imprimer")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Primary;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Rentrée")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Success;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Sortie")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Warning;
            }
            if (e.Column.FieldName == "Action")
            {
                if (action.ToString() == "Absent")
                    e.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Danger;
            }
        }
    }
    
}