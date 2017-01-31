using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.XML;

namespace test
{
    public class Client
    {
        public delegate void ClientEventHandler(DataSet sender);
        public event ClientEventHandler ClientExited;

        private Process _process;
        public Process Process
        {
            get { return _process; }
        }

        public Character _char;

        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_WM_WRITE = 0x0020;
        DataSet _dsIngame;
        DataSet _dsClientOnline;

        OfflineWindow _offlineWindow = new OfflineWindow();

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);


        public Client(Process _process)
        {
            this._process = _process;
            IntPtr _processHandle = OpenProcess(PROCESS_WM_READ + PROCESS_WM_WRITE, false, _process.Id);
            _dsIngame = new DataSet(MemoryAddresses.MemoryDic["ingame"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["ingame"].Bytecount);
            _dsIngame.Changed += DsIngame_Changed;

            DsIngame_Changed(_dsIngame);
        }

        private void DsIngame_Changed(DataSet sender)
        {
            if (_dsIngame.EncodedInt == 1)
            {
                _char = new Character(this);
            }
            else
            {
                if (_char != null)
                {
                    _char.Delete();
                }
            } 
        }

        public void Delete()
        {
            if (_char != null)
            {
                _char.Delete();
                _char = null;
                
            }
        }
    }
}
