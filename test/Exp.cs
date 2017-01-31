using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.XML;

namespace test
{
    public class Exp
    {
        public event EventHandler ExpPerHourChanged;
        public delegate void EventHandler(int _expPerHour);

        DataSet exp;
        int xp;
        int lastxp;
        
        public double ExpPerHour;

        bool resetFirst = true;

        KeyValuePair<DateTime, int> firstxp;

        public void ResetExp()
        {
            firstxp = new KeyValuePair<DateTime, int>(DateTime.Now, xp);
            CalculateExp();
        }

        public Exp(IntPtr _processHandle)
        {
            exp = new DataSet(MemoryAddresses.MemoryDic["exp"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["exp"].Bytecount);
            exp.Changed += exp_Changed;
        }

        void exp_Changed(DataSet sender)
        {
            
            lastxp = xp;
            xp = sender.EncodedInt;
            if (resetFirst)
            {
                firstxp = new KeyValuePair<DateTime, int>(DateTime.Now, xp);
                resetFirst = false;
            }

            CalculateExp();
        }

        void CalculateExp()
        {
            DateTime now = DateTime.Now;

            TimeSpan ts = now - firstxp.Key;
            double seconds = ts.TotalSeconds;
            double diffexp = xp - firstxp.Value;
            double ExpPerHour = diffexp / seconds * 3600;
            int iExpPerHour = 0;
            try
            {
                iExpPerHour = Convert.ToInt32(ExpPerHour);
            }
            catch(Exception ex)
            {

            }

            if(ExpPerHourChanged != null)
            {
                ExpPerHourChanged(iExpPerHour);
            }
        }
    }
}
