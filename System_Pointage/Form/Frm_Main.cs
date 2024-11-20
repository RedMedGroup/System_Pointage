using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System_Pointage.Classe;

namespace System_Pointage.Form
{
    public partial class Frm_Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static Frm_Main _instance;
        public static Frm_Main Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Frm_Main();
                return _instance;
            }
        }
        public Frm_Main()
        {
            InitializeComponent();
            ribbon.ItemClick += Ribbon_ItemClick;
        }

        private void Ribbon_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tag = e.Item.Tag as string;
            if (tag != string.Empty)
            {
                OpenFormByName(tag);
            }
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            var screens = Master.ScreensAccesses.Where(x => x.CanShow == true || Master.User.UserType == (byte)Master.UserType.Admin);

            screens.Where(s => s.ParentScreenID == 0).ToList().ForEach(s =>
            {
                RibbonPage elm = new RibbonPage()
                {
                    Text = s.ScreenCaption,
                    Tag = s.ScreenName,
                    Name = s.ScreenName,
                    KeyTip = ElementStyle.Item.ToString(),
                };
                elm.Appearance.Font = new Font("Arial", 10, FontStyle.Bold);
                ribbon.Pages.Add(elm);
                Addribon(elm, s.ScreenID);
            });
        }
        void Addribon(RibbonPage parent, int parentID)
        {

            var screens = Master.ScreensAccesses.Where(x => x.CanShow == true || Master.User.UserType == (byte)Master.UserType.Admin);
            screens.Where(s => s.ParentScreenID == parentID).ToList().ForEach(s =>
            {
                RibbonPageGroup elm = new RibbonPageGroup()
                {
                    Tag = s.ScreenName,
                    KeyTip = ElementStyle.Item.ToString(),
                    AllowTextClipping = false,
                };
                BarButtonItem btn = new BarButtonItem();
                if (s != null && s.SsvgImage != null && !string.IsNullOrEmpty(s.SsvgImage.Uri))
                {
                    string base64String = s.SsvgImage.Uri.Replace("data:image/png;base64,", "");

                    var salesImage = Convert.FromBase64String(base64String);
                    var salesBitmap = new Bitmap(new MemoryStream(salesImage));

                    if (btn.Tag == null || btn.Tag.ToString() != s.ScreenName)
                    {
                        btn = new BarButtonItem()
                        {
                            Tag = s.ScreenName,
                            Name = s.ScreenName,
                            Caption = s.ScreenCaption,
                            ImageUri = s.SsvgImage,
                            LargeGlyph = salesBitmap,
                        };
                        btn.Appearance.Font = new Font("Arial", 13, FontStyle.Bold);
                        parent.Groups.Add(elm);
                        elm.ItemLinks.Add(btn);
                    }
                }
            });

        }
        public static void OpenFormByName(string name)
        {
            XtraForm frm = null;
            switch (name)
            {
                case "Frm_MVM_Operation_P":
                    frm = new Frm_MVM_Operation(Classe.Master.MVMType.P);
                    break;
                case "Frm_MVM_Operation_A":
                    frm = new Frm_MVM_Operation(Classe.Master.MVMType.A);
                    break;
                case "Frm_MVM_Operation_CR":
                    frm = new Frm_MVM_Operation(Classe.Master.MVMType.CR);
                    break;
            }
            var open = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == name);
            if (open != null)
            {
                frm = Activator.CreateInstance(open) as XtraForm;
                if (Application.OpenForms[frm.Name] != null)
                {
                    frm = (XtraForm)Application.OpenForms[frm.Name];
                }
                else
                {
                    // frm.Show();
                }

                frm.BringToFront();

            }
            if (frm != null)
            {
                frm.Name = name;
                OpenForm(frm);
            }
        }
        public static void OpenForm(XtraForm frm, bool OpenInDialog = false)
        {
            if (Master.User.UserType == (byte)Master.UserType.Admin)
            {
                frm.Show();
                return;
            }
            var screen = Master.ScreensAccesses.SingleOrDefault(x => x.ScreenName == frm.Name);
            if (screen != null)
            {
                if (screen.CanOpen == true)
                {
                    if (OpenInDialog)
                        frm.ShowDialog();
                    else
                        frm.Show();
                    return;
                }
                else
                {
                    XtraMessageBox.Show(
                                         text: "Vous n'avez pas les autorisations d'accès",
                                         caption: "",
                                         icon: MessageBoxIcon.Error,
                                         buttons: MessageBoxButtons.OK
                                         );
                    return;
                }
            }
        }
    }
}