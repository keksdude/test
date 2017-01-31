using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace test
{
    public static class KeySetting
    {
        //private string path = "C:\\Spiele\\Rise of Midgard\\savedata\\UserKeys.lua";
        private static string _roPath;
        private static string _userKeysPath = "\\savedata\\UserKeys.lua";
        private static int[] _keyCodesOnToolbar = new int[36];

        private static string[] rowsstring = new string[36];

        public static void Init()
        {
            if (Properties.Settings.Default.RoPath == "" || !File.Exists(Properties.Settings.Default.RoPath + _userKeysPath))
            {
                ChooseFolder();
                Properties.Settings.Default.RoPath = _roPath;
                Properties.Settings.Default.Save();
            }
            else
            {
                _roPath = Properties.Settings.Default.RoPath;
            }

            string[] lines = System.IO.File.ReadAllLines(_roPath + _userKeysPath);
            for (int i = 1; i < 37; i++)
            {
                string[] line = lines[i].Split(new char[] { ' ' });
                _keyCodesOnToolbar[i - 1] = Convert.ToInt32(line[9]);
            }
        }

        public static int[] KeyCodesOnToolbar
        {
            get { return _keyCodesOnToolbar; }
        }

        private static void ChooseFolder()
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _roPath = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}

/*
 *             keyMap.Add(49, "1");
            keyMap.Add(50,"2");
            keyMap.Add(51,"3");
            keyMap.Add(52,"4");
            keyMap.Add(53,"5");
            keyMap.Add(54,"6");
            keyMap.Add(55,"7");
            keyMap.Add(56,"8");
            keyMap.Add(57,"9");

            keyMap.Add(81,"q");
            keyMap.Add(87,"w");
            keyMap.Add(69,"e");
            keyMap.Add(82,"r");
            keyMap.Add(84,"t");
            keyMap.Add(90,"z");
            keyMap.Add(85,"u");
            keyMap.Add(73,"i");
            keyMap.Add(79,"o");

            keyMap.Add(65,"a");
            keyMap.Add(83,"s");
            keyMap.Add(68,"d");
            keyMap.Add(70,"f");
            keyMap.Add(71,"g");
            keyMap.Add(72,"h");
            keyMap.Add(74,"j");
            keyMap.Add(75,"k");
            keyMap.Add(76,"l");

            keyMap.Add(89,"y");
            keyMap.Add(88,"x");
            keyMap.Add(67,"c");
            keyMap.Add(86,"v");
            keyMap.Add(66,"b");
            keyMap.Add(78,"n");
            keyMap.Add(77,"m");
            keyMap.Add(188,",");
            keyMap.Add(190,".");
 */
