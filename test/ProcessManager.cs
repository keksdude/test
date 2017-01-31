using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace test
{
    public static class ProcessManager
    {
        static Timer _t = new Timer(1000);
        static Dictionary<int, Client> _processes = new Dictionary<int, Client>();
        
        public static void Init()
        {
            CheckProcesses(null, null);
            _t.Elapsed += CheckProcesses;
            _t.Start();
            Program.GUI.FormClosing += GUI_FormClosing;
        }

        static void GUI_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            foreach (KeyValuePair<int, Client> _client in _processes)
            {
                _client.Value.Delete();
            }
        }

        private static void CheckProcesses(object sender, ElapsedEventArgs e)
        {
            Process[] currentProcess = Process.GetProcessesByName("RoM");
            foreach (Process p in currentProcess)
            {
                if(!_processes.ContainsKey(p.Id))
                {
                    Client c = new Client(p);
                    _processes.Add(p.Id, c);
                    p.Exited += Process_Exited;
                }
                
            }
        }

        public static Client GetActiveClient()
        {
            int currentProcessId = GetActiveProcess().Id;
            if (_processes.ContainsKey(currentProcessId))
                return _processes[currentProcessId];
            else
                return null;
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            if (_processes.ContainsKey(((Process)sender).Id))
            {
                //destroy charwindow
                _processes[((Process)sender).Id].Delete();
                //_processes[((Process)sender).Id].
                _processes.Remove(((Process)sender).Id);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        private static Process GetActiveProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p;
        }
    }
}
