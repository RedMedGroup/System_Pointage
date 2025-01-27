using DevExpress.XtraReports;
using DevExpress.XtraReports.Design;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.DAL;
using System_Pointage.Form;
using System_Pointage.report;

namespace System_Pointage.Classe
{
    public abstract class ReportManager
    {
        protected abstract string ReportName { get; }

        public void OpenReportDesigner()
        {
            // تحميل التقرير من قاعدة البيانات (إذا وجد)
            XtraReport report = LoadReportFromDatabase();

            // إذا لم يتم العثور على التقرير المعدل، استخدم التقرير الافتراضي
            if (report == null)
            {
                report = CreateDefaultReport();
            }

            // عرض التقرير في مصمم التقارير
            ReportDesignTool designTool = new ReportDesignTool(report);
            designTool.ShowDesignerDialog();

            // حفظ التقرير المعدل في قاعدة البيانات
            SaveReportToDatabase(report);
        }

        protected abstract XtraReport CreateDefaultReport();

        private XtraReport LoadReportFromDatabase()
        {
            using (var context = new DAL.DataClasses1DataContext()) // استبدل بسياق قاعدة البيانات الخاص بك
            {
                // استرجاع أحدث تقرير محفوظ
                var reportEntity = context.Reports
                    .OrderByDescending(r => r.ModifiedDate)
                    .FirstOrDefault(r => r.ReportName == ReportName);

                if (reportEntity != null && reportEntity.ReportData != null)
                {
                    // تحويل byte[] إلى XtraReport
                    using (MemoryStream ms = new MemoryStream(reportEntity.ReportData.ToArray())) // استخدام ToArray()
                    {
                        XtraReport report = new XtraReport();
                        report.LoadLayoutFromXml(ms); // تحميل التقرير من MemoryStream
                        return report;
                    }
                }
                else
                {
                    MessageBox.Show("Aucun rapport modifié trouvé dans la base de données.");
                    return null;
                }
            }
        }

        private void SaveReportToDatabase(XtraReport report)
        {
            // تحويل التقرير إلى byte[]
            byte[] reportData;
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms); // حفظ التقرير في MemoryStream
                reportData = ms.ToArray(); // تحويل التقرير إلى byte[]
            }

            // حفظ reportData في قاعدة البيانات
            using (var context = new DAL.DataClasses1DataContext()) // استبدل بسياق قاعدة البيانات الخاص بك
            {
                var oldReports = context.Reports
                    .Where(r => r.ReportName == ReportName)
                    .ToList();

                // حذف التقارير القديمة
                if (oldReports.Any())
                {
                    context.Reports.DeleteAllOnSubmit(oldReports); // حذف جميع التقارير القديمة
                    context.SubmitChanges(); // حفظ التغييرات
                }

                var reportEntity = new Report
                {
                    ReportName = ReportName, // اسم التقرير
                    ReportData = new System.Data.Linq.Binary(reportData), // تحويل byte[] إلى Binary
                    ModifiedDate = DateTime.Now // تاريخ ووقت التعديل
                };

                context.Reports.InsertOnSubmit(reportEntity); // إضافة التقرير إلى الجدول
                context.SubmitChanges(); // حفظ التغييرات
            }

            MessageBox.Show("Le rapport a été enregistré avec succès dans la base de données !");
        }

     

    }
  
    public class WorkDayReportManager : ReportManager
    {
        protected override string ReportName => "rpt_WorkDay";

        protected override XtraReport CreateDefaultReport()
        {
            return new rpt_WorkDay(); // إنشاء التقرير الافتراضي
        }
    }

    public class EtatJournalReportManager : ReportManager
    {
        protected override string ReportName => "rpt_DailyReport";

        protected override XtraReport CreateDefaultReport()
        {
            return new rpt_DailyReport(); // إنشاء التقرير الافتراضي
        }
    }

    public class EtatPointageReportManager : ReportManager
    {
        protected override string ReportName => "rpt_pointage2";

        protected override XtraReport CreateDefaultReport()
        {
            return new rpt_pointage2(); // إنشاء التقرير الافتراضي
        }
    }

    
    public class LoadModifiedReportClass//pour confirmer dans la base de donné copy de raport
    {
        private readonly DAL.DataClasses1DataContext _context;
        private readonly Frm_Pointage _parentForm; 

        // تعديل الـ Constructor لاستقبال الفورم
        public LoadModifiedReportClass(DAL.DataClasses1DataContext context, Frm_Pointage parentForm)
        {
            _context = context;
            _parentForm = parentForm;
        }

        public T LoadModifiedReport<T>(string reportName) where T : XtraReport, new()
        {
            var reportEntity = _context.Reports
                .OrderByDescending(r => r.ModifiedDate)
                .FirstOrDefault(r => r.ReportName == reportName);

            if (reportEntity != null && reportEntity.ReportData != null)
            {
                using (MemoryStream ms = new MemoryStream(reportEntity.ReportData.ToArray()))
                {
                    T report = new T();
                    report.LoadLayoutFromXml(ms);

                    // تمرير الفورم الصحيح إلى Initialize
                    if (report is rpt_DailyReport dailyReport)
                    {
                        dailyReport.Initialize(_parentForm);
                    }

                    return report;
                }
            }
            else
            {
                MessageBox.Show("Aucun rapport modifié trouvé dans la base de données, le rapport par défaut sera utilisé."); return null;
            }
        }
    }



}
