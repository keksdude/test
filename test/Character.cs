using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using test.Task;
using test.XML;

namespace test
{
    public class Character
    {
        public delegate void AliveEventHandler();
        public event AliveEventHandler Alive;

        public Client _parentClient;
        private int _hp, _sp, _maxHp, _maxSp, _weight, _maxWeight, _bLvl, _jLvl,
            _x, _y, _job;
        private string _name;
        private bool _alive,_loggedIn;
        private Toolbar _toolbar;
        private Buffs _buffs;
        private Exp _exp;

        private IntPtr _processHandle;
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_WM_WRITE = 0x0020;

        private CharWindow _charWindow;

        private List<TaskBase> _tasks = new List<TaskBase>();
        private List<TaskDBEntry> _availableDBTasks = new List<TaskDBEntry>();

        public Character(Client _parentClient)
        {
            this._parentClient = _parentClient;
            _processHandle = OpenProcess(PROCESS_WM_READ + PROCESS_WM_WRITE, false, _parentClient.Process.Id);

            DataSet _dsName = new DataSet(MemoryAddresses.MemoryDic["name"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["name"].Bytecount);
            DataSet _dsHp = new DataSet(MemoryAddresses.MemoryDic["hp"].Address, _processHandle, 100, MemoryAddresses.MemoryDic["hp"].Bytecount);
            DataSet _dsMaxHp = new DataSet(MemoryAddresses.MemoryDic["maxhp"].Address, _processHandle, 100, MemoryAddresses.MemoryDic["maxhp"].Bytecount);
            DataSet _dsSp = new DataSet(MemoryAddresses.MemoryDic["sp"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["sp"].Bytecount);
            DataSet _dsMaxSp = new DataSet(MemoryAddresses.MemoryDic["maxsp"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["maxsp"].Bytecount);
            DataSet _dsJob = new DataSet(MemoryAddresses.MemoryDic["job"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["job"].Bytecount);
            DataSet _dsBLvl = new DataSet(MemoryAddresses.MemoryDic["blvl"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["blvl"].Bytecount);
            DataSet _dsJLvl = new DataSet(MemoryAddresses.MemoryDic["jlvl"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["jlvl"].Bytecount);
            DataSet _dsX = new DataSet(MemoryAddresses.MemoryDic["x"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["x"].Bytecount);
            DataSet _dsY = new DataSet(MemoryAddresses.MemoryDic["y"].Address, _processHandle, 1000, MemoryAddresses.MemoryDic["y"].Bytecount);


            _buffs = new Buffs(_processHandle);
            _toolbar = new Toolbar(_processHandle);
            _exp = new Exp(_processHandle);

            _dsName.Changed += DsNameChange;
            _dsJob.Changed += DsJob_Changed;
            _dsHp.Changed += DsHp_Changed;
            _dsMaxHp.Changed += DsMaxHp_Changed;
            _dsSp.Changed += DsSp_Changed;
            _dsMaxSp.Changed += DsMaxSp_Changed;

            _dsX.Changed += DsX_Changed;
            _dsY.Changed += DsY_Changed;
            _dsBLvl.Changed += DsBLvl_Changed;
            _dsJLvl.Changed += DsJlvl_Changed;

            _exp.ExpPerHourChanged += ExpPerHour_Changed;

            _charWindow = new CharWindow();
            _charWindow.buttonResetExp.Click += CharWindowResetExp_Click;
            _charWindow.buttonAddTask.Click += CharWindowAddTask_Click;

            

            //Program.GUI.Shown += GUI_Shown;

            DsNameChange(_dsName); 
            DsJob_Changed(_dsJob); 
            DsHp_Changed(_dsHp); 
            DsMaxHp_Changed(_dsMaxHp); 
            DsSp_Changed(_dsSp);
            DsMaxSp_Changed(_dsMaxSp);
            DsBLvl_Changed(_dsBLvl);
            DsJlvl_Changed(_dsJLvl);

            _availableDBTasks = TaskDB.GetTaskListByJob(_job);
            List<TaskDBEntry> _toAddDBTasks = new List<TaskDBEntry>();
            Profile _loadedProfile = ProfileDB.GetProfile(this._name);

            if(_loadedProfile != null)
            {
                foreach (string _string in _loadedProfile.TaskDBNames)
                {
                    foreach(TaskDBEntry _dbtask in  _availableDBTasks)
                    {
                        if(_dbtask.Name == _string)
                        {
                            _toAddDBTasks.Add(_dbtask);
                        }

                    }
                }

                foreach (TaskDBEntry _dbtask in _toAddDBTasks)
                {
                    _availableDBTasks.Remove(_dbtask);
                    this.AddTask(_dbtask);
                }
            }

            
            _charWindow.comboBoxTasks.DataSource = _availableDBTasks;
            _charWindow.comboBoxTasks.DisplayMember = "TaskName";

            Program.GUI.AddCharWindow(_charWindow);
        }

        public void Delete()
        {
            List<string> _toSave = new List<string>();
            foreach(TaskBase _t in _tasks)
            {
                _toSave.Add(_t.TaskDBEntry.Name);
                _t.Delete();
                
            }
            _buffs = null;
            _toolbar = null;

            Program.GUI.RemoveCharWindow(_charWindow);
            Profile _profile = new Profile(this._name, _toSave);
            ProfileDB.SaveProfile(_profile);
        }

        private bool ClientPause = false;
        public void PauseClient()
        {
            if (ClientPause)
                ClientPause = false;
            else
                ClientPause = true;

            foreach(TaskBase _t in _tasks)
            {
                _t.ToggleClientPause();
            }

            _charWindow.SetPause(ClientPause);
        }

        private void DsJlvl_Changed(DataSet sender)
        {
            _jLvl = sender.EncodedInt;
            _charWindow.SetJLvl("" + _jLvl);
        }

        private void DsBLvl_Changed(DataSet sender)
        {
            _bLvl = sender.EncodedInt;
            _charWindow.SetBLvl("" + _bLvl);

            _exp.ResetExp();
        }

        private void DsY_Changed(DataSet sender)
        {
            _y = sender.EncodedInt;
        }

        private void DsX_Changed(DataSet sender)
        {
            _x = sender.EncodedInt;
        }

        private void DsMaxSp_Changed(DataSet sender)
        {
            _maxSp = sender.EncodedInt;
            _charWindow.SetMaxSp(_maxSp);
        }

        private void DsSp_Changed(DataSet sender)
        {
            _sp = sender.EncodedInt;
            _charWindow.SetSp(_sp);
        }

        private void DsMaxHp_Changed(DataSet sender)
        {
            _maxHp = sender.EncodedInt;
            _charWindow.SetMaxHp(_maxHp);
        }

        private void DsHp_Changed(DataSet sender)
        {
            _hp = sender.EncodedInt;
            _charWindow.SetHp(_hp);

            //Check _alive status, sends DoSingle to all tasks in _tasksOnAlive
            if (_hp == 1)
            {
                _alive = false;
            }
            else
            {
                if (!_alive)
                {
                    _alive = true;

                    if (Alive != null)
                        Alive();
                }
            }
        }

        private void CharWindowResetExp_Click(object sender, EventArgs e)
        {
            _exp.ResetExp();
        }

        private void CharWindowAddTask_Click(object sender, EventArgs e)
        {
            if (_charWindow.comboBoxTasks.SelectedItem != null)
            {
                AddTask((TaskDBEntry)_charWindow.comboBoxTasks.SelectedItem);
                _availableDBTasks.Remove((TaskDBEntry)_charWindow.comboBoxTasks.SelectedItem);
                _charWindow.comboBoxTasks.DataSource = null;
                _charWindow.comboBoxTasks.DataSource = _availableDBTasks;
                _charWindow.comboBoxTasks.DisplayMember = "TaskName";
            }
        }

        private void AddTask(TaskDBEntry _dbTask)
        {
            //Task _task = new Task(_dbTask, _dbTask.ToolbarSlotHex, _dbTask.BuffHex, _dbTask.OnOccurence, _dbTask.TaskType, _dbTask.SendDelay, _dbTask.WithClick, _dbTask.ClickDelay, this, _dbTask.ToolbarSlotHex2, _dbTask.PauseBetweenSkills);
            switch(_dbTask.TaskType)
            {
                case TaskType.Hold:
                    {
                        TaskHold _task = new TaskHold(_dbTask, this, _dbTask.ToolbarSlotHex, _dbTask.SendDelay, _dbTask.WithClick, _dbTask.ClickDelay);
                        _tasks.Add(_task);
                        break;
                    }
                case TaskType.Buff:
                    {
                        TaskBuff _task = new TaskBuff(_dbTask, this, _dbTask.ToolbarSlotHex, _dbTask.BuffHex, _dbTask.SendDelay, _dbTask.WithClick, _dbTask.ClickDelay, _dbTask.OnOccurence);
                        _tasks.Add(_task);
                        break;
                    }
                case TaskType.Toggle:
                    {
                        TaskToggle _task = new TaskToggle(_dbTask, this, _dbTask.ToolbarSlotHex, _dbTask.SendDelay, _dbTask.WithClick, _dbTask.ClickDelay);
                        _tasks.Add(_task);
                        break;
                    }
            }
        }

        public void RemoveTask(TaskBase _task)
        {
            
            _tasks.Remove(_task);
            _charWindow.flowLayoutPanel1.Controls.Remove(_task.TaskWindow);
            _availableDBTasks.Add(_task.TaskDBEntry);
            _charWindow.comboBoxTasks.DataSource = null;
            _charWindow.comboBoxTasks.DataSource = _availableDBTasks;
            _charWindow.comboBoxTasks.DisplayMember = "TaskName";
        }


        public Buffs Buffs
        {
            get { return _buffs; }
        }

        public Toolbar Toolbar
        {
            get { return _toolbar; }
        }

        public Client ParentClient
        {
            get { return _parentClient; }
        }

        public CharWindow CharWindow
        {
            get { return _charWindow; }
        }

        private void ExpPerHour_Changed(int _expPerHour)
        {
            _charWindow.SetExp(_expPerHour);
        }

        private void DsNameChange(DataSet sender)
        {
            _name = sender.EncodedString;
            _charWindow.SetName(_name);
        }

        private void DsJob_Changed(DataSet sender)
        {
            _job = sender.EncodedInt;
            if(Dics.Jobs.ContainsKey(_job))
            {
                _charWindow.SetJob(Dics.Jobs[_job]);
            }
            
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    }
}
