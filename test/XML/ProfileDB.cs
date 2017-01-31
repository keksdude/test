using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace test.XML
{
    public static class ProfileDB
    {
        private static string _profilePath = "profiles";

        public static void Init()
        {
            if (!Directory.Exists(_profilePath))
            {
                Directory.CreateDirectory(_profilePath);
            }
        }

        public static Profile GetProfile(String _charName)
        {
            if (File.Exists(_profilePath+"\\"+_charName+".xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Profile));

                FileStream fs = new FileStream(_profilePath + "\\" + _charName + ".xml", FileMode.Open);
                Profile _profile = (Profile)serializer.Deserialize(fs);
                fs.Close();
                if (_profile.CharName != null && _profile.TaskDBNames != null)
                    return _profile;
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        public static void SaveProfile(Profile _profile)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(Profile));
            TextWriter writer = new StreamWriter(_profilePath + "\\" + _profile.CharName + ".xml");

            serializer.Serialize(writer, _profile);
            writer.Close();
        }
    }

    public class Profile : IEquatable<Profile>
    {
        [XmlElementAttribute]
        public string _charName;
        [XmlElementAttribute]
        private List<string> _taskDBNames;

        public Profile()
        {

        }

        public Profile(string _charName, List<string> _taskDBNames)
        {
            this._charName = _charName;
            this._taskDBNames = _taskDBNames;
        }

        public string CharName
        {
            get { return _charName; }
        }

        public List<string> TaskDBNames
        {
            get { return _taskDBNames; }
            set { _taskDBNames = value;  }
        }

        public bool Equals(Profile other)
        {
            if (other == null) return false;

            if (this._charName == other._charName)
                return true;
            else
                return false;
        }
    }
}
