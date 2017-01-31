using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using test.XML;


namespace test
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BuffDB.SaveBuffDic();
            Properties.Settings.Default.Location = this.Location;
            Properties.Settings.Default.Save();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public bool AddCharWindow(CharWindow _charWindow)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<CharWindow, bool>)AddCharWindow, _charWindow); }

            flowLayoutPanel1.Controls.Add(_charWindow);
            flowLayoutPanel1.SetFlowBreak(_charWindow, true);
            return true;
        }

        public bool RemoveCharWindow(CharWindow _charWindow)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<CharWindow, bool>)RemoveCharWindow, _charWindow); }

            if (flowLayoutPanel1.Controls.Contains(_charWindow))
            {
                flowLayoutPanel1.Controls.Remove(_charWindow);
            }
            return true;
        }

        private GetPauseKey _getPauseKey;
        private void setPauseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _getPauseKey = new GetPauseKey();
            _getPauseKey.KeyDown += _getPauseKey_KeyDown;
            _getPauseKey.Show();
        }

        void _getPauseKey_KeyDown(object sender, KeyEventArgs e)
        {
            Properties.Settings.Default.PauseKey = e.KeyValue;
            Properties.Settings.Default.Save();
            _getPauseKey.Dispose();
            _getPauseKey = null;

        }

        private void GUI_LocationChanged(object sender, EventArgs e)
        {
           
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.Location;
        }
    }
}
