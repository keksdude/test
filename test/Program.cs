using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.XML;

namespace test
{
    static class Program
    {
        static public GUI GUI;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MemoryAddresses.Init("memory_addresses.xml");
            BuffDB.Init("buffdb.xml");
            Dics.Init();
            GlobalKeyboard.Init();
            KeySetting.Init();
            TaskDB.Init();
            ProfileDB.Init();
            GUI = new GUI();
            ProcessManager.Init();
            Application.Run(GUI);
        }
    }
}
