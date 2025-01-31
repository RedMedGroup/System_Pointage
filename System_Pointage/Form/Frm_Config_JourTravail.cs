using System;
using System.Linq;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Config_JourTravail : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Config_JourTravail()
        {
            InitializeComponent();
            GetData();
        }

        private void Frm_Config_JourTravail_Load(object sender, EventArgs e)
        {

        }
        void GetData()
        {
            DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext();
            DAL.Config_Jour info = db.Config_Jours.FirstOrDefault();
            if (info == null) return;

            spn_sys.EditValue = info.Jour;
        }

        private void btn_valider_Click(object sender, EventArgs e)
        {
            if (spn_sys.EditValue == null || string.IsNullOrWhiteSpace(spn_sys.EditValue.ToString()))
            {
                spn_sys.ErrorText = Master.ErrorText;
                return;
            }
            if (!Master.ConfirmSave())
                return;
            DAL.DataClasses1DataContext db = new DAL.DataClasses1DataContext();
            DAL.Config_Jour info = db.Config_Jours.FirstOrDefault();

            if (info == null)
            {
                info = new DAL.Config_Jour();
                db.Config_Jours.InsertOnSubmit(info);
            }
            info.Jour = Convert.ToInt32(spn_sys.EditValue);
            db.SubmitChanges();
            Master.MessageBox();
        }
    }
}