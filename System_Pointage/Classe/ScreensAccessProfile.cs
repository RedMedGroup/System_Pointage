using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        
        public static ScreensAccessProfile ReceptionDonné = new ScreensAccessProfile("elm_Reseptiond")
        {
            Actions = new List<Master.Actions>() { Master.Actions.Show },
            ScreenCaption = "Opérations"
        };
        
        public static ScreensAccessProfile Pointage = new ScreensAccessProfile(nameof(Form1), ReceptionDonné)
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
