using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using test.XML;

namespace test
{
    public class Toolbar
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        private DataSet[] dataSets;
        private ToolbarSlot[] _toolbar;
        private Dictionary<int, String> items;

        public Toolbar(IntPtr processHandle)
        {
            dataSets = new DataSet[36];
            _toolbar = new ToolbarSlot[36];
            items = new Dictionary<int, string>();
            items.Add(1, "Blessing");

            int j = 1;
            int k = 1;
            for (int i = 0; i < 36; i++)
            {
                dataSets[i] = new DataSet(MemoryAddresses.MemoryDic["toolbarstart"].Address + (i * 7), processHandle, 1000, MemoryAddresses.MemoryDic["toolbarstart"].Bytecount);
                _toolbar[i] = new ToolbarSlot(dataSets[i]);
                _toolbar[i].KeyInt = KeySetting.KeyCodesOnToolbar[i];

                
                if (i == 9 || i == 18 || i == 27)
                {
                    j++; k = 1;
                }
                //Console.WriteLine(j +":" + k+" "+_toolbar[i].ToolbarSkillHex);
                k++;
            }

            
        }

        public ToolbarSlot GetToolbarSlot(string _toolbarSkillHex)
        {
            foreach(ToolbarSlot slot in _toolbar)
            {
                if(slot.ToolbarSkillHex == _toolbarSkillHex)
                {
                    return slot;
                }
            }

            return null;
        }
    }

    public class ToolbarSlot
    {
        public delegate void ChangedEventHandler(ToolbarSlot sender);
        public event ChangedEventHandler Changed;

        private DataSet dataSet;
        public string ToolbarSkillHex; //Toolbar Slot ID from memory
        public string OldToolbarSkillHex;
        public int KeyInt; //KeySetting Map ID

        public ToolbarSlot(DataSet dataSet)
        {
            this.dataSet = dataSet;
            dataSet.Changed += dataSet_Changed;
            dataSet_Changed(dataSet);
        }

        void dataSet_Changed(DataSet sender)
        {
            OldToolbarSkillHex = ToolbarSkillHex;
            ToolbarSkillHex = sender.HexData;
            if(this.Changed != null)
            {
                Changed(this);
            }
        }
    }
}
