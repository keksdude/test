using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using test.XML;

namespace test
{
    public class Buffs
    {
        public delegate void BuffsHandler(Buff _buff);
        public event BuffsHandler BuffAppeared;
        public event BuffsHandler BuffVanished;

        private DataSet _buffs;
        private List<Buff> oldbuffs = new List<Buff>();
        private const int buffdigitscount = 8;


        public Buffs(IntPtr _processHandle)
        {
            _buffs = new DataSet(MemoryAddresses.MemoryDic["buffstart"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["buffstart"].Bytecount);

            _buffs.Changed += DataSet_Changed;
            
            MakeCurrentBuffList();
        }

        private void DataSet_Changed(DataSet sender)
        {
            this.MakeCurrentBuffList();
        }

        public void Delete()
        {
            this._buffs.Delete();
        }

        private void MakeCurrentBuffList()
        {
            List<Buff> nochangebuffs = new List<Buff>();
            List<Buff> newbuffs = new List<Buff>();

            String[] buffHexDatas = new String[_buffs.HexData.Length / buffdigitscount];
            for (int i = 0; i < _buffs.HexData.Length / buffdigitscount; i++)
            {
                String hexData = "";
                for(int j = 0; j < buffdigitscount; j++)
                {
                    hexData = hexData + _buffs.HexData[i * buffdigitscount + j];
                }
                buffHexDatas[i] = hexData;
            }



            foreach (String ds in buffHexDatas)
            {
                if (ds != "ffffffff" || ds != "00000000")
                {
                    Buff currentbuff = oldbuffs.Find(p => p.HexData == ds);
                    Buff checknuwbuff = newbuffs.Find(p => p.HexData == ds);
                    if (currentbuff != null && checknuwbuff == null)
                    {
                        nochangebuffs.Add(currentbuff);
                        oldbuffs.Remove(currentbuff);
                    }
                    else
                    {
                        Buff newbuff = new Buff(ds);
                        newbuffs.Add(newbuff);
                    }
                }
                else
                    break;
            }

            List<Buff> gonebuffs = oldbuffs;

            foreach (Buff b in gonebuffs)
            {
                if (BuffVanished != null)
                {
                    BuffVanished(b);
                }
            }

            foreach (Buff b in newbuffs)
            {

                if (BuffAppeared != null)
                {
                    BuffAppeared(b);
                }
            }

            
            oldbuffs = new List<Buff>();
            oldbuffs.AddRange(nochangebuffs);
            oldbuffs.AddRange(newbuffs);

            Console.WriteLine("");
        }

        public bool IsActive(string _buffHex)
        {
            foreach(Buff _b in oldbuffs)
            {
                if(_b.HexData == _buffHex)
                {
                    return true;
                }
            }
            return false;
        }
    }


    public class Buff : IEquatable<Buff>
    {
        public int Duration;
        public string HexData;
        public string Name;
        public BuffDBEntry buffDBEntry;


        public Buff(string hexData)
        {
            this.HexData = hexData;
            buffDBEntry = BuffDB.Name(hexData);
            this.Name = buffDBEntry.Name;
        }

        public bool Equals(Buff other)
        {
            if (other == null) return false;

            if (this.HexData == other.HexData)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return Name+":"+HexData;
        }
    }
}
