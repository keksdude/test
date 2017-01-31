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
    public static class TaskDB
    {
        public static List<TaskDBEntry> _tasks;

        public static void Init()
        {
            _tasks = new List<TaskDBEntry>();
            string[] _files = Directory.GetFiles("tasks");
            if (_files != null)
            {
                foreach (string file in _files)
                {
                    TaskDBEntry _task;
                    try
                    {
                        _task = LoadDB(file);
                        _tasks.Add(_task);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public static void SaveTaskDBEntry(TaskDBEntry _task)
        {
            XmlSerializer serializer =
            new XmlSerializer(typeof(TaskDBEntry));
            TextWriter writer = new StreamWriter("tasks\\"+_task.Name+".xml");
            serializer.Serialize(writer, _task);
            writer.Close();
        }

        public static List<TaskDBEntry> GetTaskListByJob (int _job)
        {
            List<TaskDBEntry> _returnTasks = new List<TaskDBEntry>();
            foreach(TaskDBEntry _task in _tasks)
            {
                var elements = _task.Classes.Split(new[] { ',' },
                System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string items in elements)
                {
                    if (Convert.ToInt32(items) == _job || items == "0")
                    {
                        _returnTasks.Add(_task);
                        break;
                    }
                }
            }
            return _returnTasks;
        }


        private static TaskDBEntry LoadDB(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TaskDBEntry));
            // If the XML document has been altered with unknown 
            // nodes or attributes, handles them with the 
            // UnknownNode and UnknownAttribute events.
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            FileStream fs = new FileStream(filename, FileMode.Open);

            TaskDBEntry _task = (TaskDBEntry)serializer.Deserialize(fs);
            fs.Close();
            //_task.Name = new FileInfo(filename).Name;
            return _task;
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

    public class TaskDBEntry : IEquatable<TaskDBEntry>
    {
        [XmlElementAttribute]
        public string Name;
        public string Classes;
        public string ToolbarSlotHex;
        public TaskType TaskType;

        [XmlElementAttribute]
        public int SendDelay;
        public bool WithClick;
        public int ClickDelay;

        [XmlElementAttribute]
        public string BuffHex;
        public bool OnOccurence;

        [XmlElementAttribute]
        public string ToolbarSlotHex2;
        public int PauseBetweenSkills;

        public String TaskName
        {
            get { return this.Name; }
        }

        public TaskDBEntry()
        {

        }


        public bool Equals(TaskDBEntry other)
        {
            if (other == null) return false;

            if (this.Name == other.Name)
                return true;
            else
                return false;
        }
    }

    public enum TaskType
    {
        Hold, Toggle, Buff, 
    }
}
