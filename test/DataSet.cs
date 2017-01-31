using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class DataSet
    {
        public delegate void DataSetEventHandler(DataSet sender);
        public event DataSetEventHandler Changed;

        IntPtr processHandle;

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private System.Timers.Timer timer1;
        

        private int address;
        private byte[] data;
        private int datalength;

        public DataSet(int address, IntPtr processHandle, int checkInterval, int datalength)
        {
            this.processHandle = processHandle;
            this.address = address;
            this.datalength = datalength;

            int bytesRead = 0;
            byte[] buffer = new byte[datalength]; //'Hello World!' takes 12*2 bytes because of Unicode 
            ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
            data = buffer;

            timer1 = new System.Timers.Timer();
            timer1.Elapsed += timer1_Tick;
            timer1.Interval = checkInterval; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[datalength]; //'Hello World!' takes 12*2 bytes because of Unicode 
            ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);
            if (!data.SequenceEqual(buffer))
            {
                data = buffer;
                if (Changed != null)
                    Changed(this);
            }
        }

        public void Delete()
        {
            timer1.Stop();
            timer1 = null;
        }

        public byte[] Data
        {
            set { this.data = value; }
            get { return data; }
        }

        public override string ToString()
        {
            return HexData;
        }

        public int ToInt()
        {
            return Convert.ToInt32(data);
        }

        public string HexAdress
        {
            get { return address.ToString("X"); }
        }

        public string HexData
        {
            get
            {
                StringBuilder hex = new StringBuilder(data.Length * 2);
                foreach (byte b in data)
                    hex.AppendFormat("{0:x2}", b);
                return hex.ToString();
            }
         }

        public string EncodedString
        {
            get {
                string text = Encoding.Default.GetString(data);
                text = text.Replace("\n","");
                text = text.Replace("\0", "");
                return text; 
            
            }
        }

        public int EncodedInt
        {
            get
            {
                //string s = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
                int number = BitConverter.ToInt32(data,0);
                return number;
            }
        }

        public string DataToString
        {
            get {
                string output = "";
                foreach(byte b in data)
                {
                    output = output + b;
                }
                return output; 
            }
        }
    }
}
