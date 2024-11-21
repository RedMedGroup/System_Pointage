using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System_Pointage.Form;

namespace System_Pointage.Classe
{
    public class ScreensAccessProfile
    {
        public static int MaxID = 1;
        public ScreensAccessProfile(string name, ScreensAccessProfile parent = null)
        {
            ScreenName = name;
            ScreenID = MaxID++;
            if (parent != null)
                ParentScreenID = parent.ScreenID;
            else ParentScreenID = 0;
            Actions = new List<Master.Actions>()
            {
                Master.Actions.Add,
                 Master.Actions.Edit,
                Master.Actions.Delete,
                Master.Actions.Print,
                Master.Actions.Show,
                Master.Actions.Open,
            };
        }
        public int ScreenID { get; set; }
        public int ParentScreenID { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCaption { get; set; }
        public virtual DxImageUri SsvgImage { get; set; }
        public bool CanShow { get; set; }
        public bool CanOpen { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelet { get; set; }
        public bool CanPrint { get; set; }
        public List<Master.Actions> Actions { get; set; }


    }

    public static class Screens
    {
        public static ScreensAccessProfile Fiche = new ScreensAccessProfile("elm_Fiche")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "Fiche"
        };
        public static ScreensAccessProfile Resident = new ScreensAccessProfile(nameof(Frm_Fiche_Ajent), Fiche)
        {
            ScreenCaption = "Fiche Agent",
            SsvgImage = new DxImageUri("data:image/png;base64,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")
        };
        public static ScreensAccessProfile ReceptionDonné = new ScreensAccessProfile("elm_Reseptiond")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "Opérations"
        };
        
        public static ScreensAccessProfile Pointage = new ScreensAccessProfile(nameof(Frm_Pointage), ReceptionDonné)
        {
            ScreenCaption = "Pointage",
            SsvgImage = new DxImageUri("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAACXBIWXMAAAsTAAALEwEAmpwYAAAFv0lEQVR4nO3Z" +
                "W1ATZxQA4LUznU5bn+x02oe+dqad6UtfbN/6oiYhoIhmN5cl2QSStlrrOHWqJsGgaAlj7WWYtgreSrXWy9TSEECihpvE0GotFgWVi5LdgK0ahIghl9PZJdlkc+EimA0znJkzsz" +
                "vk4Xz8Of+ef4MgC7EQ8y8UpoE36ETma0g23X0R05NbMAP5CNNTXtRAmSWmocXIfApU78rB9GQvZqAgLl2onlIiCCxCMjmkRte7mJ5sSgKIS9KJ6gffRzItcre6XkEN1LeYgQx" +
                "MjWAxIVRPVklMQ6/zXT+i08HzmJHciBlIz/QB3EQN5ChmIIuFG26+wAtCqneLUQPV/bSA+Fy9qZNMP6LoXstcAdZu6YWsj5phGWGB9CoAFhFlAPklXpAaB58agG4bgJxPnLC" +
                "MqGEQyzW1/EDUZQCEOQiKYg9gxpkgSMjddA1WFNQzABoi2mgH0Xd16YcUXAyA6qtxBsOASv0gM92fEpG3uRuE2nNhgAWE686BaG8DCA5amEw7RHcHgE519TgQZQEWpNz9BK" +
                "RFQwkAydZ+EK9rYwH0amSZ7CyAd4iOzl4AosoHanNoAmSm+2cUMKMbUL0LcjZchuXqcB+orSD6vBGEFTUJCF4hcscA6PpDzLX2ryAQ5TQo2j/RPrBA1qd2EJXXJwXwDhEct" +
                "EDOqZbo6twB0Nj9oPrSz2BogOBDG4jKzqUsPvv4BSZ5hWQdtTEFRO4Lbo5NXPcDECd8E31QmRwgqqqHNfUdgDtJwNspkNg6QfrzwHu8QLS9AUAvdLEQ4ZFawBpvsvdJV+F" +
                "QDeRWO0FxaYABxKbCQYH8jKtRUnl3SVoh8RkpNhVk5almkLX2JQASQM1UQH7SvR8xwXNpgaz8tQ2IzmG2cNXV/yDnRFMCRHzUxqzcVID4lDdQD7HjLmzODZiRehstIq9z" +
                "Cj1Uw12Z8C4WC8m/PAQKp2vGEJxOJwXy38lO2THyrVkDdDp4SWqkfsP0VIgeRyKF0g0qjIFIW3pAmwTC9FPPOODtJCicM8coHCTkHrnin50CYJHUMDgS+6TWtD5hi1Xf" +
                "GOEUnn3cnrJnmO25y5u00VOl5Ew3ZFVMbN+zhtDPBLxkhDMcKr6/D4VX/ZwiaUSyZk+2MRB/ewBvHUwJkDX0QXZFyxw+MMMQJksBFMUPo3NUEQX5Rz2g7Q5OfH36Q8z" +
                "XKxWksGecC+oDUF/0At7mjgJaBiD3cDsIDiSOMXMHCaeq1A/S7f+yIOnOQSCso0xhk23Hop/Ogvxif8LqFN7wgfysC/KOdYCwsu4ZjTBJIJxpN+ZwJdtzj9M/qZ4rUz" +
                "2HBOmEKHd52Wt85yNO/+AVD6DwWgBEFfWw2nyNLVTxBwXiY+fZ+zzLn6Dp9vILWa62Qt7mLlCbg+HxPQjy2MPVdjegRpK5jv2va/uCnMKFh638QpaFx/M1W3oA3zkc0z8" +
                "+kG6/xzlUsSvidHFQedYrk440grke8yeD0MdXOuP/jpd4QbzewXwmttCVp1s5GFXHg/RBVGZ/cKYQequOfCY69jckH/vTtiLbHr2q3OVtVJeFZgUp7PEDev5GdDv+sQ6w" +
                "plvpg0Qif8fjD5S7n5ARiHi9Y0yy7e7YdCEzHfsFz/oojO8Y3pz32fV9kXvM6N43HciquLFfmWLsF/B5pp8OZLpjv4BXSOR1UCTNiZC1tn84Y7+spTfp2C/gE6Iwja5Qf" +
                "uEbmqpH1Nfjxv5f7JkFiQReMmpUlY77IpAVmrPjmg6fP1mzR14DZSSEDvpHT7zksSXr41aLZJ19MV7lfpmwjVZrbwVD3P4IgrT59rQgwkprCMmUIE4Pv0k0jV2NH/cnh9" +
                "SAuLyxR7yn/R0k00JdPSLTXPZ5poKIfrCNrNp7qQDJ9FDVjpQUdvrZ/mER++sD2V9fesbvteY4Ck56lmjOP67V3g6GBAesIC5vduR843gNma9RYPEszd3btpTvOhZiIZDE" +
                "+B+zsXFZlqc5kgAAAABJRU5ErkJggg==")
        };
        public static ScreensAccessProfile MVMp = new ScreensAccessProfile("Frm_MVM_Operation_P", ReceptionDonné)
        {
            ScreenCaption = "Ajouter des participants",
            SsvgImage = new DxImageUri("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAD/ElEQVR4nO2aX4gTRxzHhxak1z+KlFK8atXLTHJVEUuyG2sPqnAWBH2ocNC+XEGw0Fr0RfFxLzNX/1CEin0RQYovhQNfPEHQB0GvmclxlD7oo225UqVeM7PqzaRvW34bV3xKLrtmzV7mA18CB5dkv8n+sr/vfhGyWCwWS2u2fv/gjfdP+qtBeaY2Es8fGj7xKJ+jfhGUZ3KEMH80FFX78aQ/tsb7+3WUdTZ5wQrCVBBT02gqeBVlmeETj95OYADoB5RlCp7akMQATNUiyjJD39W3JDNA/o6yTK5S/yihATWUZXDF353wFLja7jXKvLHTFSZwhG44wkiXm/su1/ccru+6Qs+4Qt9wuZl2uZlyuL7kCHPeFeasw/WEw83xMtdHykJ/5Qoz7tbMWKmq95W5GSkLXXRqTzaXZhtDO2b04Mhtf3XHBhCq9icaglRebPcajtA3wYA0FMMAOZ7QgNNL+fR71gDM5NfJDFDHeuXTj2VArqJ2JbsOkF+2NsDM9bQBm7xgBabqcVwD8pX6npYG8MWDPW0AQKi6EteAwqRfQi345GbwJkzypvRhmOaRXGG+gKkeqVRtjEaC2QFTPlI07SOV+ZN3YepHQkHwCopLnslDcQ3YwOR6lHVyno/jGrAsNkKLxWLpe9aemR8oeAuDOVrf3Mz95Dim8ghhagIzeR7iL8zUDKHqLqFSYir3LhvTCt7CYMdrMJP31nt/vIaWA3hS7otzDYCp9NBygDA1EXMTNBCfo6yDqbqaYB2+0uq5t916/E7XlyBupmKlQRGYqvsJDPBRDwQijtB/FqvmYxQHEvfgm7rT6rldoQ+ltQo73PyWvgFUXWtjwMkUDfgrdQMwkxdaGxCsdLm5k9JpYNI3gLb/KXRFY6MrzMMODqQB0fmz2JybaYjLo6gcYnKIyKN4HMISiMW3V4OBl2CAOoCyDkl0CvifoqxDEg3Bfz9A/WxA4fTCWy/7/VssFoslaSKEm6EI5ALTze1QNp4+zsHfCFWXCJOnwpgsvKXuj0J8Bv+Lsg5hSvdtSQogVM33bUkKIEz+2s2SFCwwL2jba0C36OmCNOUKfSpcjGpmDO4gw0KE4kCYvN6tktSLOvilqji3uKZjAzBTP3erJJXmwYffktpi5/crMFPnulWSSt0ArifSi8VZqKO91BGC8KRzAyry226VpEqzZp0jzD8pGvCgcwNo/fNulaSeRePczIaN0FDmcjjFuf4JWqEONz/CRAdFfaIS19883w51avqzqD/k1HQJpn7xl/+GoS+0varfg/sCxTm5CqVdly20KUllAkzVh/1dkmL1dX1dklp7Zn5gCT93EvoB0BVo9gb80eHJh51fdFgsFovFgtLkfyiN146Fg5BVAAAAAElFTkSuQmCC")
        }; 
        public static ScreensAccessProfile MVMa = new ScreensAccessProfile("Frm_MVM_Operation_A", ReceptionDonné)
        {
            ScreenCaption = "Ajouter des absents",
            SsvgImage = new DxImageUri("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAD/ElEQVR4nO2aX4gTRxzHhxak1z+KlFK8atXLTHJVEUuyG2sPqnAWBH2ocNC+XEGw0Fr0RfFxLzNX/1CEin0RQYovhQNfPEHQB0GvmclxlD7oo225UqVeM7PqzaRvW34bV3xKLrtmzV7mA18CB5dkv8n+sr/vfhGyWCwWS2u2fv/gjfdP+qtBeaY2Es8fGj7xKJ+jfhGUZ3KEMH80FFX78aQ/tsb7+3WUdTZ5wQrCVBBT02gqeBVlmeETj95OYADoB5RlCp7akMQATNUiyjJD39W3JDNA/o6yTK5S/yihATWUZXDF353wFLja7jXKvLHTFSZwhG44wkiXm/su1/ccru+6Qs+4Qt9wuZl2uZlyuL7kCHPeFeasw/WEw83xMtdHykJ/5Qoz7tbMWKmq95W5GSkLXXRqTzaXZhtDO2b04Mhtf3XHBhCq9icaglRebPcajtA3wYA0FMMAOZ7QgNNL+fR71gDM5NfJDFDHeuXTj2VArqJ2JbsOkF+2NsDM9bQBm7xgBabqcVwD8pX6npYG8MWDPW0AQKi6EteAwqRfQi345GbwJkzypvRhmOaRXGG+gKkeqVRtjEaC2QFTPlI07SOV+ZN3YepHQkHwCopLnslDcQ3YwOR6lHVyno/jGrAsNkKLxWLpe9aemR8oeAuDOVrf3Mz95Dim8ghhagIzeR7iL8zUDKHqLqFSYir3LhvTCt7CYMdrMJP31nt/vIaWA3hS7otzDYCp9NBygDA1EXMTNBCfo6yDqbqaYB2+0uq5t916/E7XlyBupmKlQRGYqvsJDPBRDwQijtB/FqvmYxQHEvfgm7rT6rldoQ+ltQo73PyWvgFUXWtjwMkUDfgrdQMwkxdaGxCsdLm5k9JpYNI3gLb/KXRFY6MrzMMODqQB0fmz2JybaYjLo6gcYnKIyKN4HMISiMW3V4OBl2CAOoCyDkl0CvifoqxDEg3Bfz9A/WxA4fTCWy/7/VssFoslaSKEm6EI5ALTze1QNp4+zsHfCFWXCJOnwpgsvKXuj0J8Bv+Lsg5hSvdtSQogVM33bUkKIEz+2s2SFCwwL2jba0C36OmCNOUKfSpcjGpmDO4gw0KE4kCYvN6tktSLOvilqji3uKZjAzBTP3erJJXmwYffktpi5/crMFPnulWSSt0ArifSi8VZqKO91BGC8KRzAyry226VpEqzZp0jzD8pGvCgcwNo/fNulaSeRePczIaN0FDmcjjFuf4JWqEONz/CRAdFfaIS19883w51avqzqD/k1HQJpn7xl/+GoS+0varfg/sCxTm5CqVdly20KUllAkzVh/1dkmL1dX1dklp7Zn5gCT93EvoB0BVo9gb80eHJh51fdFgsFovFgtLkfyiN146Fg5BVAAAAAElFTkSuQmCC")
        };
        public static ScreensAccessProfile MVMcr = new ScreensAccessProfile("Frm_MVM_Operation_CR", ReceptionDonné)
        {
            ScreenCaption = "Ajouter des congés",
            SsvgImage = new DxImageUri("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAAD/ElEQVR4nO2aX4gTRxzHhxak1z+KlFK8atXLTHJVEUuyG2sPqnAWBH2ocNC+XEGw0Fr0RfFxLzNX/1CEin0RQYovhQNfPEHQB0GvmclxlD7oo225UqVeM7PqzaRvW34bV3xKLrtmzV7mA18CB5dkv8n+sr/vfhGyWCwWS2u2fv/gjfdP+qtBeaY2Es8fGj7xKJ+jfhGUZ3KEMH80FFX78aQ/tsb7+3WUdTZ5wQrCVBBT02gqeBVlmeETj95OYADoB5RlCp7akMQATNUiyjJD39W3JDNA/o6yTK5S/yihATWUZXDF353wFLja7jXKvLHTFSZwhG44wkiXm/su1/ccru+6Qs+4Qt9wuZl2uZlyuL7kCHPeFeasw/WEw83xMtdHykJ/5Qoz7tbMWKmq95W5GSkLXXRqTzaXZhtDO2b04Mhtf3XHBhCq9icaglRebPcajtA3wYA0FMMAOZ7QgNNL+fR71gDM5NfJDFDHeuXTj2VArqJ2JbsOkF+2NsDM9bQBm7xgBabqcVwD8pX6npYG8MWDPW0AQKi6EteAwqRfQi345GbwJkzypvRhmOaRXGG+gKkeqVRtjEaC2QFTPlI07SOV+ZN3YepHQkHwCopLnslDcQ3YwOR6lHVyno/jGrAsNkKLxWLpe9aemR8oeAuDOVrf3Mz95Dim8ghhagIzeR7iL8zUDKHqLqFSYir3LhvTCt7CYMdrMJP31nt/vIaWA3hS7otzDYCp9NBygDA1EXMTNBCfo6yDqbqaYB2+0uq5t916/E7XlyBupmKlQRGYqvsJDPBRDwQijtB/FqvmYxQHEvfgm7rT6rldoQ+ltQo73PyWvgFUXWtjwMkUDfgrdQMwkxdaGxCsdLm5k9JpYNI3gLb/KXRFY6MrzMMODqQB0fmz2JybaYjLo6gcYnKIyKN4HMISiMW3V4OBl2CAOoCyDkl0CvifoqxDEg3Bfz9A/WxA4fTCWy/7/VssFoslaSKEm6EI5ALTze1QNp4+zsHfCFWXCJOnwpgsvKXuj0J8Bv+Lsg5hSvdtSQogVM33bUkKIEz+2s2SFCwwL2jba0C36OmCNOUKfSpcjGpmDO4gw0KE4kCYvN6tktSLOvilqji3uKZjAzBTP3erJJXmwYffktpi5/crMFPnulWSSt0ArifSi8VZqKO91BGC8KRzAyry226VpEqzZp0jzD8pGvCgcwNo/fNulaSeRePczIaN0FDmcjjFuf4JWqEONz/CRAdFfaIS19883w51avqzqD/k1HQJpn7xl/+GoS+0varfg/sCxTm5CqVdly20KUllAkzVh/1dkmL1dX1dklp7Zn5gCT93EvoB0BVo9gb80eHJh51fdFgsFovFgtLkfyiN146Fg5BVAAAAAElFTkSuQmCC")
        };
        public static List<ScreensAccessProfile> GetScreens
        {
            get
            {
                Type t = typeof(Screens);
                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);

                var list = new List<ScreensAccessProfile>();
                foreach (var item in fields)
                {
                    var obj = item.GetValue(null);
                    if (obj != null && obj.GetType() == typeof(ScreensAccessProfile))
                        list.Add((ScreensAccessProfile)obj);
                }
                return list;
            }
        }

        private static byte[] ImageToByteArray(Image imageIn)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png); 
                return ms.ToArray();
            }
        }
    }
}
