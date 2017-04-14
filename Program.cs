using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TataBuilder
{
    static class Program
    {
        public const string APP_NAME                        = "Tata Builder";
        public const string DOC_EXTENSION                   = "ttb";    // abbreviation of Tata Builder
        public const string PACKAGE_EXTENSION               = "tpk";    // abbreviation of Tata Package

        public const int SCENE_THUMBNAIL_OUTER_WIDTH        = 137;
        public const int SCENE_THUMBNAIL_OUTER_HEIGHT       = 104;
        public const int SCENE_THUMBNAIL_WIDTH              = 131;
        public const int SCENE_THUMBNAIL_HEIGHT             = 98;

        public const string DRAG_SCENE_PREFIX               = "TataScene:";

        public const float  DEFAULT_ZOOM                    = 0.75F;    // default zoom
        public const int    BOOK_WIDTH                      = 1024;     // width of book
        public const int    BOOK_HEIGHT                     = 768;      // height of book

        public const bool   NAVBUTTON_STRETCH               = true;     // indicate whether nav button will be stretched
        public const int    NAVBUTTON_WIDTH                 = 100;      // width of back/next button
        public const int    NAVBUTTON_HEIGHT                = 80;       // height of back/next button

        // Default Events
        public const string DEFAULT_EVENT_UNDEFINED         = "Undefined";
        public const string DEFAULT_EVENT_ENTER             = "Enter";
        public const string DEFAULT_EVENT_AUTOPLAY          = "AutoPlay";
        public const string DEFAULT_EVENT_TOUCH             = "Touch";
        public const string DEFAULT_EVENT_DRAGGING          = "Dragging";
        public const string DEFAULT_EVENT_DROP              = "Drop";
        public const string DEFAULT_EVENT_PUZZLE_SUCCESS    = "PuzzleSuccess";
        public const string DEFAULT_EVENT_PUZZLE_FAIL       = "PuzzleFail";

        // Default States
        public const string DEFAULT_STATE_DEFAULT = "Default";

        public const string NONE_ITEM                       = "(None)";

        public const string AUTHOR_SERVER                   = "http://author.tataland.com";
        public const string URL_LOGIN                       = AUTHOR_SERVER + "/index.php?r=site/loginCGI";
        public const string URL_LOGOUT                      = AUTHOR_SERVER + "/index.php?r=site/logoutCGI";
        public const string URL_CHECK_READY                 = AUTHOR_SERVER + "/index.php?r=books/checkReadyForUpload";
        public const string URL_UPLOAD_BINARY               = AUTHOR_SERVER + "/index.php?r=books/uploadBinary";

        public static CookieAwareWebClient webClient = null;

        public static bool signed = false;
        public static string author = "";

        private static PrivateFontCollection pfc = new PrivateFontCollection();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // http client to connect with author site.
            webClient = new CookieAwareWebClient();
            signed = false;

            // load fonts
            loadFonts();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMainContainer());
        }

        public static string getVersion()
        {
            Assembly executingAssembly;

            AssemblyName executingAssemblyName;

            Version executingAssemblyVersion;

            executingAssembly = Assembly.GetEntryAssembly();

            executingAssemblyName = executingAssembly.GetName();
            executingAssemblyVersion = executingAssemblyName.Version;
            string assemblyVersion = executingAssemblyVersion.ToString(4);

            return assemblyVersion;
        }

        public static string getWorkspaceLocation()
        {
            string regValue = Properties.Settings.Default.WorkspaceLocation;
            if (!string.IsNullOrEmpty(regValue)) 
                return regValue;

            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public static void setWorkspaceLocation(string location)
        {
            Properties.Settings.Default.WorkspaceLocation = location;
            Properties.Settings.Default.Save();
        }

        public static string[] getFontFamilies()
        {
            List<string> ret = new List<string>();
            foreach (FontFamily font in pfc.Families) {
                ret.Add(font.Name);
            }

            return ret.ToArray();
        }

        public static FontFamily findFontFamily(string familyName)
        {
            foreach (FontFamily family in pfc.Families) {
                if (String.Equals(family.Name, familyName, StringComparison.OrdinalIgnoreCase)) {
                    return family;
                }
            }

            return null;
        }

        private static void loadFonts()
        {
            addFontToCollection(Properties.Resources.aller);
            addFontToCollection(Properties.Resources.ara_hamah_1964_r);
            addFontToCollection(Properties.Resources.ara_jozoor);
            addFontToCollection(Properties.Resources.arial);
            addFontToCollection(Properties.Resources.kufyan_arabic);
            addFontToCollection(Properties.Resources.lato);
            addFontToCollection(Properties.Resources.love_ya_like_a_sister);
            addFontToCollection(Properties.Resources.nurkholis);
            addFontToCollection(Properties.Resources.open_sans);
            addFontToCollection(Properties.Resources.quicksand);
            addFontToCollection(Properties.Resources.roboto);
        }

        private static void addFontToCollection(byte[] font)
        {
            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(font.Length);

            // copy the bytes to the unsafe memory block
            Marshal.Copy(font, 0, data, font.Length);

            // pass the font to the font collection
            pfc.AddMemoryFont(data, font.Length);

            // free up the unsafe memory
            Marshal.FreeCoTaskMem(data);
        }
    }
}
