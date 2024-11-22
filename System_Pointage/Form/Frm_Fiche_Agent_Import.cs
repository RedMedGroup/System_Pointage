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

namespace System_Pointage.Form
{
    public partial class Frm_Fiche_Agent_Import : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Fiche_Agent_Import()
        {
            InitializeComponent();
        }

        private void Frm_Fiche_Agent_Import_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        public  void RefreshData()
        {
            using (var db = new DAL.DataClasses1DataContext())
            {
                lkp_Poste.IntializeData(db.Fiche_Postes.Select(x => new { x.ID, x.Name }).ToList());//base               
            }
        }
    }
}