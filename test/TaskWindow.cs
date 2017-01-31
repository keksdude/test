using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class TaskWindow : UserControl
    {
        public TaskWindow()
        {
            InitializeComponent();
            this.Margin = new Padding(0,0,0,1);
        }

        public bool SetPause(bool _pause)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<bool, bool>)SetPause, _pause); }
            if (_pause)
            {
                labelPlay.BackColor = Color.Red;
            }
            else
            {
                labelPlay.BackColor = Color.LightGreen;
            }
            return true; // lesender Zugriff
        }

        public bool SetStatus(int _value)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetStatus, _value); }
            
            if (_value == 0)
            {
                flowLayoutPanel1.BackColor = Color.White;
            }
            else if(_value == 1)
            {
                flowLayoutPanel1.BackColor = Color.LightGreen;
            }
            else if(_value == 2)
            {
                flowLayoutPanel1.BackColor = Color.Red;
            }
            return true; // lesender Zugriff
        }

        public bool SetSkill(string _skill)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<string, bool>)SetSkill, _skill); }

            labelName.Text = _skill;
            return true;
        }
    }
}
