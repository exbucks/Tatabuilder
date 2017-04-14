using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TtbAdmin
{
    class Program
    {
        public const string APP_FILE = "TataBuilder.exe";
        public const string APP_NAME = "Tata Builder";
        public const string DOC_EXTENSION = "ttb";    // abbreviation of Tata Builder
        public const string PACKAGE_EXTENSION = "tpk";    // abbreviation of Tata Package

        static void Main(string[] args)
        {
            // register tata doc file and package file to windows explorer
            registerFileExtensions();

        }
        
        public static void registerFileExtensions()
        {
            if (UacHelper.IsProcessElevated) 
            {
                string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string builder = Path.Combine(Path.GetDirectoryName(path), APP_FILE);

                SetAssociation("." + Program.DOC_EXTENSION, "TataBuilderProject", builder, "Tata Builder Project File");
                SetAssociation("." + Program.PACKAGE_EXTENSION, "TataBuilderPackage", builder, "Tata Builder Package File");
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            RegistryKey BaseKey;
            RegistryKey OpenMethod;
            RegistryKey Shell;
            RegistryKey CurrentUser;

            BaseKey = Registry.ClassesRoot.CreateSubKey(Extension);
            BaseKey.SetValue("", KeyName);

            OpenMethod = Registry.ClassesRoot.CreateSubKey(KeyName);
            OpenMethod.SetValue("", FileDescription);
            OpenMethod.CreateSubKey("DefaultIcon").SetValue("", "\"" + OpenWith + "\",0");
            Shell = OpenMethod.CreateSubKey("Shell");
            Shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
            BaseKey.Close();
            OpenMethod.Close();
            Shell.Close();

            // Delete the key instead of trying to change it
            CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + Extension, true);
            if (CurrentUser != null)
            {
                CurrentUser.DeleteSubKey("UserChoice", false);
                CurrentUser.Close();
            }

            // Tell explorer the file association has been changed
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
