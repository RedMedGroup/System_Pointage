using DevExpress.Xpo.DB;
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
using System_Pointage.DAL;

namespace System_Pointage.Form
{
    public partial class Frm_Heir : DevExpress.XtraEditors.XtraForm
    {
        private BindingList<Models.AgentStatus> activeAgentsList;
    

        public Frm_Heir()
        {
            InitializeComponent();
        }

        private void Frm_Heir_Load(object sender, EventArgs e)
        {
            RefrecheData();
            gridView1.GroupPanelText =" ";
            gridView1.RefreshData();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && e.Column.FieldName == "Name")
            {
                // استرجاع القيمة من عمود "Statut"
                string statut = gridView1.GetRowCellValue(e.RowHandle, "Statut").ToString();

                // تخصيص لون النص بناءً على القيمة في عمود "Statut"
                if (statut == "CR")
                {
                    // تخصيص لون النص باللون الأحمر
                    e.Appearance.ForeColor = Color.Red;

                    // رسم النص مع السهم
                    string arrow = "→";
                    e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);
                }
                else if (statut == "P")
                {
                    // تخصيص لون النص باللون الأخضر
                    e.Appearance.ForeColor = Color.Green;

                    // رسم النص مع السهم المعاكس
                    string arrow = "←";
                    e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);
                }

                // منع الرسم الافتراضي
                e.Handled = true;
            }
        }

        int selectedID;
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن
            bool isAdmin = Master.User.UserType == (byte)Master.UserType.Admin;

            // إذا لم يكن أدمن، استخدم userAccessPosteID
            int? userAccessPosteID = isAdmin ? null : (int?)Master.User.IDAccessPoste;

            activeAgentsList = agentDataService.GetAgentStatuses(userAccessPosteID, Master.MVMType.CR, isAdmin,selectedID, true, "Frm_Heir");

            gridControl1.DataSource = activeAgentsList;

            GridName();
        }

        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["Poste"].VisibleIndex = 2;
            gridView1.Columns["Affecter"].VisibleIndex = 3;
            gridView1.Columns["Date"].VisibleIndex = 4;
            gridView1.Columns["Jour"].VisibleIndex = 5;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 6;
            gridView1.Columns["DaysCount"].VisibleIndex = 7;
            gridView1.Columns["Difference"].VisibleIndex = 8;
            gridView1.Columns["Statut"].VisibleIndex = 9;
            gridView1.Columns["screenPosteD"].Visible = false;
            gridView1.Columns["Date"].Caption = "Date de rentrée";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue Congé";
          //  gridView1.Columns["DaysCount"].Caption = "Nombre de jours de présent";
            gridView1.Columns["DaysCount"].Caption = "Nombre de jours de travail";
           
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // التحقق من تحديد تاريخ في DateEdit
            if (dateEdit1.EditValue == null)
            {
                MessageBox.Show("Veuillez sélectionner une date pour la recherche.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBoxEdit1.Text.Trim() == string.Empty)
            {
                comboBoxEdit1.ErrorText =Classe.Master.ErrorText;
                return;
            }
            // الحصول على التاريخ المدخل
            DateTime selectedDate = dateEdit1.DateTime.Date;
            var statut=comboBoxEdit1.Text;
            // تصفية البيانات بناءً على التاريخ
            var filteredList = activeAgentsList
                .Where(agent => agent.Date.Date == selectedDate)
                .ToList();
            if (statut == "CR")
            {
                filteredList = filteredList.Where(agent => agent.Statut == "CR").ToList();
            }
            else if (statut == "P")
            {
                filteredList = filteredList.Where(agent => agent.Statut == "P").ToList();
            }
            // التحقق إذا لم تكن هناك نتائج مطابقة
            if (filteredList.Count == 0)
            {
                MessageBox.Show("Aucun résultat correspondant à la date sélectionnée.", "Résultats de la recherche", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            gridControl1.DataSource = new BindingList<Models.AgentStatus>(filteredList);
        }

        private void btn_liste_Click(object sender, EventArgs e)
        {
            RefrecheData();
        }
    }
}