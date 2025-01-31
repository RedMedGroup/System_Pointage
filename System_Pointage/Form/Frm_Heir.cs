using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System_Pointage.Classe;

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
            #region paramater gridview     ////////////////
            gridView1.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView1.Appearance.HeaderPanel.Font = new Font(gridView1.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.GroupPanelText = " ";
            #endregion
            gridView1.OptionsBehavior.Editable = false;
            RefrecheData();
            gridView1.GroupPanelText = " ";
            gridView1.RefreshData();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
        }

        private void GridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0 && (e.Column.FieldName == "Name" || e.RowHandle >= 0 && e.Column.FieldName == "NameP" || e.RowHandle >= 0 && e.Column.FieldName == "NameCR"))
            {
                string statut = null;
                if (e.Column.FieldName == "Name")
                {
                    statut = gridView1.GetRowCellValue(e.RowHandle, "Statut")?.ToString();
                }
                if (e.Column.FieldName == "NameP")
                {
                    statut = gridView1.GetRowCellValue(e.RowHandle, "StatutP")?.ToString();
                }
                else if (e.Column.FieldName == "NameCR")
                {
                    statut = gridView1.GetRowCellValue(e.RowHandle, "StatutCR")?.ToString();
                }
                if (!string.IsNullOrEmpty(statut))
                {
                    if (statut == "CR")
                    {
                        e.Appearance.ForeColor = Color.Red;
                        string arrow = "→";
                        e.Appearance.Font = new Font("Arial", 10);
                        e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);
                    }
                    else if (statut == "P")
                    {
                        e.Appearance.ForeColor = Color.Green;
                        string arrow = "←";
                        e.Appearance.Font = new Font("Arial", 10);
                        e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);
                    }
                    e.Handled = true;
                }
            }
        }

        int selectedID;
        private void RefrecheData()
        {
            var agentDataService = new AgentDataService();

            // التحقق مما إذا كان المستخدم أدمن أو مانجر
            bool isAdminOrManager = Master.User.UserType == (byte)Master.UserType.Admin || Master.User.UserType == (byte)Master.UserType.Manager;

            // إذا لم يكن أدمن أو مانجر، استخدم userAccessPosteID
            int? userAccessPosteID = isAdminOrManager ? null : (int?)Master.User.IDAccessPoste;

            activeAgentsList = agentDataService.GetAgentStatuses(userAccessPosteID, Master.MVMType.CR, isAdminOrManager, selectedID, true, "Frm_Heir");

            gridControl1.DataSource = activeAgentsList;

            GridName();
        }

        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom";
            gridView1.Columns["FirstName"].Caption = "Prénom";
            gridView1.Columns["Jour"].Visible = false;
            gridView1.Columns["Matricule"].VisibleIndex = 0;
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["FirstName"].VisibleIndex = 2;
            gridView1.Columns["Poste"].VisibleIndex = 3;
            gridView1.Columns["Affecter"].VisibleIndex = 4;
            gridView1.Columns["Jour"].Visible = false;
            gridView1.Columns["CalculatedDate"].Visible = false;
            gridView1.Columns["DaysCount"].Visible = false;
            gridView1.Columns["Difference"].Visible = false;
            gridView1.Columns["Statut"].VisibleIndex = 9;
            gridView1.Columns["screenPosteD"].Visible = false;
            gridView1.Columns["Date"].Visible = false;
            gridView1.Columns["CalculatedDate"].Visible = false;
            gridView1.Columns["DaysCount"].Visible = false;
            gridView1.Columns["Statutmvm"].Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue == null)
            {
                MessageBox.Show("Veuillez sélectionner une date pour la recherche.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime selectedDate = dateEdit1.DateTime.Date;

            var filteredList = activeAgentsList
                .Where(agent => agent.Date.Date == selectedDate)
                .ToList();

            var listP = filteredList.Where(agent => agent.Statut == "P").ToList();
            var listCR = filteredList.Where(agent => agent.Statut == "CR").ToList();

            DataTable table = new DataTable();
            table.Columns.Add("MatriculeP", typeof(string));
            table.Columns.Add("NameP", typeof(string));
            table.Columns.Add("FirstNameP", typeof(string));
            table.Columns.Add("PosteP", typeof(string));
            table.Columns.Add("StatutP", typeof(string));
            table.Columns.Add("MatriculeCR", typeof(string));
            table.Columns.Add("NameCR", typeof(string));
            table.Columns.Add("FirstNameCR", typeof(string));
            table.Columns.Add("PosteCR", typeof(string));
            table.Columns.Add("StatutCR", typeof(string));

            // إضافة البيانات إلى الجدول
            int maxRows = Math.Max(listP.Count, listCR.Count);
            for (int i = 0; i < maxRows; i++)
            {
                var row = table.NewRow();

                //  P
                if (i < listP.Count)
                {
                    row["MatriculeP"] = listP[i].Matricule;
                    row["NameP"] = listP[i].Name;
                    row["FirstNameP"] = listP[i].FirstName;
                    row["PosteP"] = listP[i].Poste;
                    row["StatutP"] = listP[i].Statut;
                }
                else
                {
                    row["MatriculeP"] = string.Empty;
                    row["NameP"] = string.Empty;
                    row["FirstNameP"] = string.Empty;
                    row["PosteP"] = string.Empty;
                    row["StatutP"] = string.Empty;
                }

                // CR
                if (i < listCR.Count)
                {
                    row["MatriculeCR"] = listCR[i].Matricule;
                    row["NameCR"] = listCR[i].Name;
                    row["FirstNameCR"] = listCR[i].FirstName;
                    row["PosteCR"] = listCR[i].Poste;
                    row["StatutCR"] = listCR[i].Statut;
                }
                else
                {
                    row["MatriculeCR"] = string.Empty;
                    row["NameCR"] = string.Empty;
                    row["FirstNameCR"] = string.Empty;
                    row["PosteCR"] = string.Empty;
                    row["StatutCR"] = string.Empty;
                }

                table.Rows.Add(row);
            }

            gridControl1.DataSource = table;

            CustomizeGridView();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            //if (dateEdit1.EditValue == null)
            //{
            //    MessageBox.Show("Veuillez sélectionner une date pour la recherche.", "Avertissement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (comboBoxEdit1.Text.Trim() == string.Empty)
            //{
            //    comboBoxEdit1.ErrorText =Classe.Master.ErrorText;
            //    return;
            //}
            //// الحصول على التاريخ المدخل
            //DateTime selectedDate = dateEdit1.DateTime.Date;
            //var statut=comboBoxEdit1.Text;
            //// تصفية البيانات بناءً على التاريخ
            //var filteredList = activeAgentsList
            //    .Where(agent => agent.Date.Date == selectedDate)
            //    .ToList();
            //if (statut == "CR")
            //{
            //    filteredList = filteredList.Where(agent => agent.Statut == "CR").ToList();
            //}
            //else if (statut == "P")
            //{
            //    filteredList = filteredList.Where(agent => agent.Statut == "P").ToList();
            //}
            //// التحقق إذا لم تكن هناك نتائج مطابقة
            //if (filteredList.Count == 0)
            //{
            //    MessageBox.Show("Aucun résultat correspondant à la date sélectionnée.", "Résultats de la recherche", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            //gridControl1.DataSource = new BindingList<Models.AgentStatus>(filteredList);
        }
        private void CustomizeGridView()
        {
            gridView1.Columns.Clear();

            gridView1.Columns.AddVisible("MatriculeP", "Matricule (P)");
            gridView1.Columns.AddVisible("NameP", "Nom (P)");
            gridView1.Columns.AddVisible("FirstNameP", "Prénom (P)");
            gridView1.Columns.AddVisible("PosteP", "Poste (P)");
            gridView1.Columns.AddVisible("MatriculeCR", "Matricule (CR)");
            gridView1.Columns.AddVisible("NameCR", "Nom (CR)");
            gridView1.Columns.AddVisible("FirstNameCR", "Prénom (CR)");
            gridView1.Columns.AddVisible("PosteCR", "Poste (CR)");

            gridView1.Columns["MatriculeP"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["NameP"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["FirstNameP"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["PosteP"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");

            gridView1.Columns["MatriculeCR"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView1.Columns["NameCR"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView1.Columns["FirstNameCR"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView1.Columns["PosteCR"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
        }
        private void btn_liste_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();
            RefrecheData();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }



    }
}