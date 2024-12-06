using DevExpress.XtraEditors.Controls;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace System_Pointage.report
{
    public partial class rpt_pointage_mensuel : DevExpress.XtraReports.UI.XtraReport
    {
        public List<DateTime> ListOfDays { get; set; }
        public rpt_pointage_mensuel()
        {
            InitializeComponent();
        }
        private TimeSpan dateRange;
        private int totalDays;
        private XRTable headerTable;
        private XRTableRow headerRow;
        private XRTable detailTable;
        private XRTableRow detailRow;
        private float availableWidth;
        private float columnWidth;
        public void AddDateColumnsToReport(XtraReport report, DateTime startDate, DateTime endDate)
        {
            dateRange = endDate - startDate;
            totalDays = dateRange.Days + 1;
            headerTable = (XRTable)FindControl("xrTableHeader", true);
            headerRow = headerTable.Rows[0];
            detailTable = (XRTable)FindControl("xrTableDetail", true);
            detailRow = detailTable.Rows[0];

            if (headerRow == null || detailRow == null)
            {
                throw new Exception("لم يتم العثور على جدول العناوين أو التفاصيل في التقرير.");
            }

            availableWidth = PageWidth - Margins.Left - Margins.Right;
            columnWidth = availableWidth / (totalDays + 3);


        }
        public void BindAttendanceDataToCells(DataTable table)
        {
            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                DataRow row = table.Rows[rowIndex];

                XRControl posteCell = FindControl("cell_sp", true);
                XRControl efectifCell = FindControl("cell_required", true);
                XRControl absenceCell = FindControl("cell_absence", true);
                XRControl pinaliteCell = FindControl("cell_penality", true);
                XRControl totalpinaliteCell = FindControl("cell_totalpinality", true);


                if (posteCell != null)
                {
                    cell_sp.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[POSTE]"));
                }
                if (efectifCell != null)
                {
                    cell_required.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[EFECTIF/CONTRAT]"));
                }
                //if (absenceCell != null)
                //{
                //    cell_required.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Total Absences]"));
                //}
                //if (pinaliteCell != null)
                //{
                //    cell_required.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[Total Absences]"));
                //}
                if (posteCell != null)
                {
                    posteCell.DataBindings.Add("Text", table, "POSTE");
                }
                if (efectifCell != null)
                {
                    efectifCell.DataBindings.Add("Text", table, "EFECTIF/CONTRAT");
                }
                if (absenceCell != null)
                {
                    absenceCell.DataBindings.Add("Text", table, "Total Absences");
                }
                if (pinaliteCell != null)
                {
                    pinaliteCell.DataBindings.Add("Text", table, "M_Penalite");
                }
                if (totalpinaliteCell != null)
                {
                    totalpinaliteCell.DataBindings.Add("Text", table, "Total Penalty");
                }
            }
           
            for (int dayIndex = 1; dayIndex <= 31; dayIndex++)
                {
                    string cellName = $"cell_{dayIndex}";
                    XRControl cell = FindControl(cellName, true);
                    if (cell != null)
                    {
                        cell.DataBindings.Add("Text", table, $"{dayIndex}");
                    }
                }
            }
        }
    }

