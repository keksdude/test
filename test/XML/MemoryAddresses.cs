using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace test.XML
{
    public static class MemoryAddresses
    {
        private static Dictionary<string, MemoryAddress> memoryDic;
        

        public static Dictionary<string, MemoryAddress> MemoryDic
        {
            get { return memoryDic; }
        }

        public static void Reload(string filename)
        {
            LoadMemoryDic(filename);
        }

        public static void Init(string filename)
        {
            if(File.Exists(filename))
            {
                //LoadMemoryDic(filename);
                CreateMemoryDic(filename);
            }
            else
            {
                CreateMemoryDic(filename);
            }
        }

        private static void CreateMemoryDic(string filename)
        {
            // Creates an instance of the XmlSerializer class;
            // specifies the type of object to serialize.
            
            /* ROM
            memoryDic = new Dictionary<string, MemoryAddress>();
            memoryDic.Add("hp", new MemoryAddress(0xE4CAF4, 4));
            memoryDic.Add("name", new MemoryAddress(0xE4D768, 8));
            memoryDic.Add("maxhp", new MemoryAddress(0xE4CAF8, 4));
            memoryDic.Add("sp", new MemoryAddress(0xE4CAFC, 4));
            memoryDic.Add("maxsp", new MemoryAddress(0xE4CB00, 4));
            memoryDic.Add("job", new MemoryAddress(0xE4CAF4, 4));
            memoryDic.Add("blvl", new MemoryAddress(0xE491AC, 1));
            memoryDic.Add("jlvl", new MemoryAddress(0xE49218, 1)); //0xE4CAF4
            memoryDic.Add("x", new MemoryAddress(0xE4CAF4, 4));
            memoryDic.Add("y", new MemoryAddress(0xE4CAF4, 4));

            memoryDic.Add("buffstart", new MemoryAddress(0xE4CF54, 64*10));
            memoryDic.Add("toolbarstart", new MemoryAddress(0xE4CAF4, 4));
             * 
             */
            memoryDic = new Dictionary<string, MemoryAddress>();
            memoryDic.Add("hp", new MemoryAddress(0xE4CAF4, 4));
            memoryDic.Add("name", new MemoryAddress(0xE4D768, 8));
            memoryDic.Add("maxhp", new MemoryAddress(0xE4CAF8, 4));
            memoryDic.Add("sp", new MemoryAddress(0xE4CAFC, 4));
            memoryDic.Add("maxsp", new MemoryAddress(0xE4CB00, 4));
            memoryDic.Add("job", new MemoryAddress(0xE49204, 4));
            memoryDic.Add("blvl", new MemoryAddress(0xE4920C, 4));
            memoryDic.Add("jlvl", new MemoryAddress(0xE49218, 4)); //0xE4CAF4
            memoryDic.Add("x", new MemoryAddress(0xE4CAF4, 4));
            memoryDic.Add("y", new MemoryAddress(0xE4CAF4, 4));

            memoryDic.Add("buffstart", new MemoryAddress(0xE4CF54, 64 * 10));
            memoryDic.Add("toolbarstart", new MemoryAddress(0xE48464, 7));

            memoryDic.Add("exp", new MemoryAddress(0xE49208, 7));
            memoryDic.Add("ingame", new MemoryAddress(0xBC4E00, 4));

            XmlSerializer serializer =
            new XmlSerializer(typeof(List<MemoryAddress>));
            TextWriter writer = new StreamWriter(filename);

            List<MemoryAddress> list = new List<MemoryAddress>();
            foreach(KeyValuePair<string,MemoryAddress> ma in memoryDic)
            {
                ma.Value.Name = ma.Key;
                list.Add(ma.Value);
            }

            serializer.Serialize(writer, list);
            writer.Close();
        }

        private static void LoadMemoryDic(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<MemoryAddress>));
            // If the XML document has been altered with unknown 
            // nodes or attributes, handles them with the 
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = new FileStream(filename, FileMode.Open);

            if (memoryDic == null)
                memoryDic = new Dictionary<string, MemoryAddress>();
            else
                memoryDic.Clear();


            List<MemoryAddress> list = (List<MemoryAddress>)serializer.Deserialize(fs);
            foreach(MemoryAddress ma in list)
            {
                memoryDic.Add(ma.Name, ma);
            }
        }

        static void serializer_UnknownNode
    (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        static void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }



    public class MemoryAddress
    {
        [XmlElementAttribute(IsNullable = false)]
        public string Name;
        public int Bytecount;
        public int Address;

        public MemoryAddress(int address, int bytecount)
        {
            this.Address = address;
            this.Bytecount = bytecount;
        }

        public MemoryAddress()
        {
            this.Name = "";
            this.Address = 0;
            this.Bytecount = 0;
        }
    }
}
