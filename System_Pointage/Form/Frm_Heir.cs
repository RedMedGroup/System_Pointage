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
            gridView1.Appearance.GroupPanel.Font = new Font("Tahoma", 10, FontStyle.Bold);
            gridView1.GroupPanelText = "Liste des rentrants";
            gridView1.OptionsBehavior.Editable = false;

            gridView2.Appearance.HeaderPanel.ForeColor = Color.Black;
            gridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gridView2.Appearance.HeaderPanel.Font = new Font(gridView2.Appearance.HeaderPanel.Font, FontStyle.Bold);
            gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            gridView2.Appearance.GroupPanel.Font = new Font("Tahoma", 10, FontStyle.Bold);
            gridView2.GroupPanelText = "Liste des partants";
            gridView2.OptionsBehavior.Editable = false;
            #endregion

            RefrecheData();
            gridView1.RefreshData();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            gridView2.CustomDrawCell += GridView2_CustomDrawCell;
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
                //if (e.Column.FieldName == "Name")
                //{
                //    statut = gridView1.GetRowCellValue(e.RowHandle, "Statut")?.ToString();
                //}
                //else if (e.Column.FieldName == "NameCR")
                //{
                //    statut = gridView1.GetRowCellValue(e.RowHandle, "StatutCR")?.ToString();
                //}
                if (!string.IsNullOrEmpty(statut))
                {
                    //if (statut == "CR")
                    //{
                    //    e.Appearance.ForeColor = Color.Red;
                    //    string arrow = "→";
                    //    e.Appearance.Font = new Font("Arial", 10);
                    //    e.Appearance.DrawString(e.Cache, arrow + " " + e.CellValue.ToString(), e.Bounds);
                    //}
                     if (statut == "P")
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

            gridControl1.DataSource = activeAgentsList.Where(x=>x.Statut=="P");
            gridControl2.DataSource = activeAgentsList.Where(x => x.Statut == "CR");

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
            gridView1.Columns["Statut"].Visible = false;
            gridView1.Columns["screenPosteD"].Visible = false;
            gridView1.Columns["Date"].Caption = "Date rentrant";
            gridView1.Columns["CalculatedDate"].Visible = false;
            gridView1.Columns["DaysCount"].Visible = false;
            gridView1.Columns["Statutmvm"].Visible = false;

            gridView2.Columns["Name"].Caption = "Nom";
            gridView2.Columns["FirstName"].Caption = "Prénom";
            gridView2.Columns["Jour"].Visible = false;
            gridView2.Columns["Matricule"].VisibleIndex = 0;
            gridView2.Columns["Name"].VisibleIndex = 1;
            gridView2.Columns["FirstName"].VisibleIndex = 2;
            gridView2.Columns["Poste"].VisibleIndex = 3;
            gridView2.Columns["Affecter"].VisibleIndex = 4;
            gridView2.Columns["Jour"].Visible = false;
            gridView2.Columns["CalculatedDate"].Visible = false;
            gridView2.Columns["DaysCount"].Visible = false;
            gridView2.Columns["Difference"].Visible = false;
            gridView2.Columns["Statut"].Visible = false;
            gridView2.Columns["screenPosteD"].Visible = false;
            gridView2.Columns["Date"].Caption = "Date partent";
            gridView2.Columns["CalculatedDate"].Visible = false;
            gridView2.Columns["DaysCount"].Visible = false;
            gridView2.Columns["Statutmvm"].Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            #region
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
            DataTable table2 = new DataTable();
            table.Columns.Add("Matricule", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("Poste", typeof(string));
            table.Columns.Add("Statut", typeof(string));
            table2.Columns.Add("Matricule", typeof(string));
            table2.Columns.Add("Name", typeof(string));
            table2.Columns.Add("FirstName", typeof(string));
            table2.Columns.Add("Poste", typeof(string));
            table2.Columns.Add("Statut", typeof(string));

            // إضافة البيانات إلى الجدول
            int maxRows = Math.Max(listP.Count, listCR.Count);
            for (int i = 0; i < maxRows; i++)
            {
                var row = table.NewRow();
                var row2 = table2.NewRow();
                //  P
                if (i < listP.Count)
                {
                    row["Matricule"] = listP[i].Matricule;
                    row["Name"] = listP[i].Name;
                    row["FirstName"] = listP[i].FirstName;
                    row["Poste"] = listP[i].Poste;
                    row["Statut"] = listP[i].Statut;
                }
                else
                {
                    row["Matricule"] = string.Empty;
                    row["Name"] = string.Empty;
                    row["FirstName"] = string.Empty;
                    row["Poste"] = string.Empty;
                    row["Statut"] = string.Empty;
                }

                // CR
                if (i < listCR.Count)
                {
                    row2["Matricule"] = listCR[i].Matricule;
                    row2["Name"] = listCR[i].Name;
                    row2["FirstName"] = listCR[i].FirstName;
                    row2["Poste"] = listCR[i].Poste;
                    row2["Statut"] = listCR[i].Statut;
                }
                else
                {
                    row2["Matricule"] = string.Empty;
                    row2["Name"] = string.Empty;
                    row2["FirstName"] = string.Empty;
                    row2["Poste"] = string.Empty;
                    row2["Statut"] = string.Empty;
                }

                table.Rows.Add(row);
                table2.Rows.Add(row2);
            }

            gridControl1.DataSource = table;
            gridControl2.DataSource = table2;
            CustomizeGridView();
            gridView1.CustomDrawCell += GridView1_CustomDrawCell;
            gridView2.CustomDrawCell += GridView2_CustomDrawCell;
            #endregion
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

        private void GridView2_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string statut = null;
            if (e.Column.FieldName == "Name")
            {
                statut = gridView2.GetRowCellValue(e.RowHandle, "Statut")?.ToString();
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
               
                e.Handled = true;
            }
        }

        private void CustomizeGridView()
        {
            gridView1.Columns.Clear();
            gridView2.Columns.Clear();
            gridView1.Columns.AddVisible("Matricule", "Matricule ");
            gridView1.Columns.AddVisible("Name", "Nom");
            gridView1.Columns.AddVisible("FirstName", "Prénom ");
            gridView1.Columns.AddVisible("Poste", "Poste ");
            gridView2.Columns.AddVisible("Matricule", "Matricule ");
            gridView2.Columns.AddVisible("Name", "Nom");
            gridView2.Columns.AddVisible("FirstName", "Prénom");
            gridView2.Columns.AddVisible("Poste", "Poste ");

            gridView1.Columns["Matricule"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["Name"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["FirstName"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");
            gridView1.Columns["Poste"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#E8F5E9");

            gridView2.Columns["Matricule"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView2.Columns["Name"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView2.Columns["FirstName"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
            gridView2.Columns["Poste"].AppearanceCell.BackColor = ColorTranslator.FromHtml("#FBE9E7");
        }
        private void btn_liste_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            gridControl2.DataSource = null;
            gridView1.Columns.Clear();
            gridView2.Columns.Clear();
            RefrecheData();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            gridControl1.ShowRibbonPrintPreview();
        }



    }
}