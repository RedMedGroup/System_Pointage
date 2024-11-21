using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit.Model;
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
using static System_Pointage.Classe.Models;

namespace System_Pointage.Form
{
    public partial class Frm_Prevu : DevExpress.XtraEditors.XtraForm
    {
        Master.MVMType Type;
        private BindingList<Models.AgentStatus> transferredAgentsList;       

        public Frm_Prevu(Master.MVMType _Type)
        {
            InitializeComponent();
            Type = _Type;
        }
 
        private void Frm_Prevu_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime=DateTime.Now;
            dateEdit1.EditValueChanged += DateEdit1_EditValueChanged;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            transferredAgentsList = new BindingList<Models.AgentStatus>();
        }
        private void DateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            
        }
        private void LoadEligibleEmployees(DateTime selectedDate)
        {
            switch (Type)
            {
                case Master.MVMType.P:
                    using (var context = new DAL.DataClasses1DataContext())
                    {
                        // استرجاع أحدث سجل لكل عامل
                        var latestStatusForEachEmployee = context.MVMAgentDetails
                            .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
                            .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
                            .Where(m => m.Statut == "CR") // التأكد من أن الحالة "P"
                            .ToList();

                        // الربط بين الجداول بعد التحقق من الحالة
                        var eligibleEmployees = latestStatusForEachEmployee
                            .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
                                  m => m.ItemID,
                                  agent => agent.ID,
                                  (m, agent) => new { m, agent })
                                     .Join(context.Fiche_Postes, // الربط مع جدول Fiche_Poste
                  ma => ma.agent.ID_Post,
                  poste => poste.ID,
                  (ma, poste) => new
                  {
                      ma.agent.Name,
                      ma.m.Date,
                      ma.m.Statut,
                      ma.agent.Jour,
                      PosteName = poste.Name,
                      CalculatedDate = ma.m.Date.AddDays(ma.agent.Jour.GetValueOrDefault()) // حساب التاريخ
                  }) // اختيار Name من Fiche_Agents
                            .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
                            .ToList();

                        // إنشاء BindingList جديدة
                        transferredAgentsList = new BindingList<Models.AgentStatus>(
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Name = x.Name,
                                Date = x.Date,
                                Statut = x.Statut,
                                Jour = x.Jour ?? 0,
                                Poste = x.PosteName,
                                CalculatedDate = x.CalculatedDate
                            }).ToList()
                        );

                        // تعيين البيانات إلى GridControl
                        gridControl1.DataSource = transferredAgentsList;
                        GridName();
                    }
                    break;
                case Master.MVMType.CR:
                    using (var context = new DAL.DataClasses1DataContext())
                    {
                        // استرجاع أحدث سجل لكل عامل
                        var latestStatusForEachEmployee = context.MVMAgentDetails
                            .GroupBy(m => m.ItemID) // تجميع حسب معرف العامل
                            .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault()) // الحصول على أحدث سجل لكل عامل
                            .Where(m => m.Statut == "P") // التأكد من أن الحالة "P"
                            .ToList();

                        // الربط بين الجداول بعد التحقق من الحالة
                        var eligibleEmployees = latestStatusForEachEmployee
                            .Join(context.Fiche_Agents, // الربط بين Fiche_Agents و MVMAgentDetails
                                  m => m.ItemID,
                                  agent => agent.ID,
                                  (m, agent) => new { agent.Name, m.Date, m.Statut, agent.Jour,
                                      CalculatedDate = m.Date.AddDays(agent.Jour ?? 0)
                                  }) // اختيار Name من Fiche_Agents
                            .Where(x => (selectedDate - x.Date).TotalDays >= x.Jour) // تحقق من الأيام بين تاريخ العمل والتاريخ المختار
                            .ToList();

                        // إنشاء BindingList جديدة
                        transferredAgentsList = new BindingList<Models.AgentStatus>(
                            eligibleEmployees.Select(x => new Models.AgentStatus
                            {
                                Name = x.Name,
                                Date = x.Date,
                                Statut = x.Statut,
                                Jour = x.Jour ?? 0,
                                CalculatedDate = x.CalculatedDate
                            }).ToList()
                        );

                        // تعيين البيانات إلى GridControl
                        gridControl1.DataSource = transferredAgentsList;
                        GridName();
                    }
                    break;
                default:
                    break;
                   
            }
        }
        public void GridName()
        {
            gridView1.Columns["Name"].Caption = "Nom et prénom";
            gridView1.Columns["Date"].Caption = "Date de début";
            gridView1.Columns["Jour"].Caption = "Systéme";
            gridView1.Columns["CalculatedDate"].Caption = "Date prévue";
            gridView1.Columns["Name"].VisibleIndex = 1;
            gridView1.Columns["Poste"].VisibleIndex = 2;
            gridView1.Columns["Date"].VisibleIndex = 3;
            gridView1.Columns["Jour"].VisibleIndex = 4;
            gridView1.Columns["CalculatedDate"].VisibleIndex = 5;
            gridView1.Columns["Statut"].VisibleIndex =6;
        }
        private void btn_prevu_Click(object sender, EventArgs e)
        {
            if (dateEdit1.EditValue is DateTime selectedDate)
            {
                LoadEligibleEmployees(selectedDate);
            }

        }
        private void btn_envoyer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var selectedAgents = gridView1.GetSelectedRows()
    .Select(rowHandle => gridView1.GetRow(rowHandle) as Models.AgentStatus)
    .Where(agent => agent != null)
    .ToList();
            // تحويل List إلى BindingList
            var selectedAgentsBindingList = new BindingList<Models.AgentStatus>(selectedAgents);

            if (selectedAgentsBindingList.Count > 0)
            {
                // الحصول على الفورم الأصلية (Frm_MVM_Operation)
                var mainForm = this.Owner as Frm_MVM_Operation;
                if (mainForm != null)
                {
                    // التحقق من التكرارات
                    var duplicateAgents = selectedAgentsBindingList
                        .Where(agent => mainForm.IsAgentExists(agent))
                        .ToList();

                    if (duplicateAgents.Any())
                    {
                        // عرض رسالة تحذيرية
                        string duplicateNames = string.Join(", ", duplicateAgents.Select(a => a.Name));
                        MessageBox.Show(
                            $"العناصر التالية مكررة بالفعل في القائمة: {duplicateNames}",
                            "تحذير",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                    else
                    {
                        // استدعاء الطريقة لإضافة البيانات إلى Frm_MVM_Operation
                        mainForm.AddTransferredAgents(selectedAgentsBindingList);

                        // إغلاق الفورم الحالية
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("لا توجد بيانات لإرسالها.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //if (transferredAgentsList.Count > 0)
            //{
            //    // الحصول على الفورم الأصلية (Frm_MVM_Operation)
            //    var mainForm = this.Owner as Frm_MVM_Operation;
            //    if (mainForm != null)
            //    {
            //        // التحقق من التكرارات
            //        var duplicateAgents = transferredAgentsList
            //            .Where(agent => mainForm.IsAgentExists(agent))
            //            .ToList();

            //        if (duplicateAgents.Any())
            //        {
            //            // عرض رسالة تحذيرية
            //            string duplicateNames = string.Join(", ", duplicateAgents.Select(a => a.Name));
            //            MessageBox.Show(
            //                $"العناصر التالية مكررة بالفعل في القائمة: {duplicateNames}",
            //                "تحذير",
            //                MessageBoxButtons.OK,
            //                MessageBoxIcon.Warning
            //            );
            //        }
            //        else
            //        {
            //            // استدعاء الطريقة لإضافة البيانات إلى Frm_MVM_Operation
            //            mainForm.AddTransferredAgents(transferredAgentsList);

            //            // إغلاق الفورم الحالية
            //            this.Close();
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("لا توجد بيانات لإرسالها.", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }
    }
}