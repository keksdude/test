using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace test.XML
{
    class BuffDB
    {
        private static Dictionary<string,BuffDBEntry> buffDic;
        private static string savedfilename;

        public static void Init(string filename)
        {
            savedfilename = filename;
            if (File.Exists(filename))
            {
                LoadBuffDB(filename);
            }
            else
            {
                buffDic = new Dictionary<string,BuffDBEntry>();
            }
        }

        public static BuffDBEntry Name(string hexdata)
        {
            if (buffDic.ContainsKey(hexdata))
                return buffDic[hexdata];
            else
            {
                BuffDBEntry newBuff = new BuffDBEntry("", hexdata);
                buffDic.Add(hexdata, newBuff);
                return newBuff;
            }
        }

        public static void SaveBuffDic()
        {
            XmlSerializer serializer =
            new XmlSerializer(typeof(List<BuffDBEntry>));
            TextWriter writer = new StreamWriter(savedfilename);

            List<BuffDBEntry> list = new List<BuffDBEntry>();
            foreach (KeyValuePair<string,BuffDBEntry> be in buffDic)
            {
                be.Value.HexData = be.Key;
                list.Add(be.Value);
            }

            serializer.Serialize(writer, list);
            writer.Close();
        }

        private static void LoadBuffDB(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<BuffDBEntry>));
            // If the XML document has been altered with unknown 
            // nodes or attributes, handles them with the 
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = new FileStream(filename, FileMode.Open);

            if (buffDic == null)
                buffDic = new Dictionary<string, BuffDBEntry>();
            else
                buffDic.Clear();

            //try
            //{
            List<BuffDBEntry> list = (List<BuffDBEntry>)serializer.Deserialize(fs);
            fs.Close();
            foreach (BuffDBEntry ma in list)
            {
                buffDic.Add(ma.HexData, ma);
            }

            //}
            //catch(Exception ex)
            //{
             //   Console.WriteLine("buffdb xml read error");
             //   fs.Close();

            //}
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

    public class BuffDBEntry : IEquatable<BuffDBEntry>
    {
        public string Name;
        public string HexData;

        public BuffDBEntry(string name, string data)
        {
            this.Name = name;
            this.HexData = data;
        }

        public BuffDBEntry()
        {
            this.Name = "";
            this.HexData = "";
        }

        public bool Equals(BuffDBEntry other)
        {
            if (other == null) return false;

            if (this.HexData == other.HexData)
                return true;
            else
                return false;
        }
    }
}
