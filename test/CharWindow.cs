using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.XML;

namespace test
{
    public partial class CharWindow : UserControl
    {
        delegate void SetTextCallback(string text);
        private BackgroundWorker backgroundWorker1;

        public Panel hpPanel;
        public Panel spPanel;

        private int hp, maxhp, sp, maxsp, hppercent, sppercent;

        public CharWindow()
        {
            InitializeComponent();
            hpPanel = new Panel();
            hpPanel.Width = panel1.Width;
            hpPanel.Height = panel1.Height;
            hpPanel.Location = panel1.Location;
            hpPanel.BackColor = Color.Green;

            spPanel = new Panel();
            spPanel.Width = panel2.Width;
            spPanel.Height = panel2.Height;
            spPanel.Location = panel2.Location;
            spPanel.BackColor = Color.Blue;
        }

        private void CharWindow_Load(object sender, EventArgs e)
        {
            

            this.Controls.Add(hpPanel);
            this.Controls.Add(spPanel);

            hpPanel.BringToFront();
            spPanel.BringToFront();
        }

        private void CalculateHpBar()
        {
            if (maxhp != 0 && hp != 0)
                hpPanel.Width = (int)(((double)hp / (double)maxhp) * panel1.Width);
        }

        private void CalculateSpBar()
        {
            if (maxsp != 0 && sp != 0)
                spPanel.Width = (int)(((double)sp / (double)maxsp) * panel2.Width);
        }

        public bool AddTaskWindow(TaskWindow _taskWindow)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<TaskWindow, bool>)AddTaskWindow, _taskWindow); }
            flowLayoutPanel1.Controls.Add(_taskWindow);
            flowLayoutPanel1.SetFlowBreak(_taskWindow, true);
            return true; // lesender Zugriff
        }

        public bool RemoveTaskWindow(TaskWindow _taskWindow)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<TaskWindow, bool>)RemoveTaskWindow, _taskWindow); }
            flowLayoutPanel1.Controls.Remove(_taskWindow);
            return true; // lesender Zugriff
        }

        public bool SetName(string _name)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<string, bool>)SetName, _name); }
            labelName.Text = _name;
            return true; // lesender Zugriff
        }

        public bool SetJLvl(string _jLvl)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<string, bool>)SetJob, _jLvl); }
            labelJoblevel.Text = _jLvl;
            return true; // lesender Zugriff
        }

        public bool SetBLvl(string _bLvl)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<string, bool>)SetBLvl, _bLvl); }
            labelLevel.Text = _bLvl;
            return true; // lesender Zugriff
        }

        public bool SetPause(bool _pause)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<bool, bool>)SetPause, _pause); }
            if (_pause)
            {
                flowLayoutPanel1.BackColor = Color.DarkGray;
            }
            else
            {
                flowLayoutPanel1.BackColor = Color.White;
            }
            return true; // lesender Zugriff
        }

        public bool SetJob(string _job)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<string, bool>)SetJob, _job); }
            labelJob.Text = _job;
            return true; // lesender Zugriff
        }

        public bool SetExp(int _exp)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetExp, _exp); }
            labelExp.Text = string.Format("{0:#,##0}", _exp);
            return true; // lesender Zugriff
        }

        public bool SetHp(int _value)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetHp, _value); }
            hp = _value; // schreibender Zugriff
            CalculateHpBar();
            return true; // lesender Zugriff
        }

        public bool SetMaxHp(int _value)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetMaxHp, _value); }
            maxhp = _value; // schreibender Zugriff
            CalculateHpBar();
            return true; // lesender Zugriff
        }

        public bool SetSp(int _value)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetSp, _value); }
            sp = _value; // schreibender Zugriff
            CalculateSpBar();
            return true; // lesender Zugriff
        }

        public bool SetMaxSp(int _value)
        {
            if (this.InvokeRequired)
            { return (bool)this.Invoke((Func<int, bool>)SetMaxSp, _value); }
            maxsp = _value; // schreibender Zugriff
            CalculateSpBar();
            return true; // lesender Zugriff
        }

        public void SetTasks(List<TaskDBEntry> _tasks)
        {
            this.comboBoxTasks.DisplayMember = "TaskName";
            this.comboBoxTasks.DataSource = _tasks;
        }
    }
}
