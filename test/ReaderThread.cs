using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace test
{
    class ReaderThread
    {
        // ro constants
        const int NAME1 = 0xDEECA8;
        //const int NAME1 = 0x9EECA8;
        const int NAME2 = 0xA461A0;
        const int NAME3 = 0xA4D768;

        String name1;
        String name2;
        String name3;

        const int PROCESS_WM_READ = 0x0010;
        IntPtr processHandle;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private System.Timers.Timer timer1;
        public ReaderThread(Process process)
        {
            processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
            
           

            timer1 = new System.Timers.Timer();
            timer1.Elapsed += timer1_Tick;
            timer1.Interval = 2000; // in miliseconds

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[8]; //'Hello World!' takes 12*2 bytes because of Unicode 
            ReadProcessMemory((int)processHandle, NAME1, buffer, buffer.Length, ref bytesRead);
            String[] names = new String[10];
            names[0] = Encoding.UTF32.GetString(buffer);
            names[1] = Encoding.ASCII.GetString(buffer);
            names[2] = Encoding.BigEndianUnicode.GetString(buffer);
            names[3] = Encoding.Default.GetString(buffer);
            names[4] = Encoding.UTF7.GetString(buffer);
            names[5] = Encoding.UTF8.GetString(buffer);
        }
        
        public void Start()
        {
            timer1.Start();
            while (!_shouldStop)
            {
                Console.WriteLine("worker thread: working...");
            }
            timer1.Stop();
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}
