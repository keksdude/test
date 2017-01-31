using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.XML;

namespace test.Task
{
    public abstract class TaskBase
    {
        public bool TaskPause = false;
        public bool ClientPause = false;
        public bool TaskPauseTemp = false;

        public TaskWindow _taskWindow;
        private TaskDBEntry _taskDBEntry;
        public Character _parentChar;

        public TaskBase(TaskDBEntry _taskDBEntry, Character _parentChar)
        {
            this._taskDBEntry = _taskDBEntry;
            this._parentChar = _parentChar;

            _taskWindow = new TaskWindow();
            _taskWindow.labelPlay.Click += PauseTaskFromGui;
            _taskWindow.labelRemove.Click += RemoveTask;
            _taskWindow.SetPause(TaskPause);
            _taskWindow.SetSkill(_taskDBEntry.Name);
            _taskWindow.SetStatus(0);

            _parentChar.CharWindow.AddTaskWindow(_taskWindow);
        }

        private void RemoveTask(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PauseTaskFromGui(object sender, EventArgs e)
        {
            if (!ClientPause)
            ToggleTaskPause();
        }


        public void ToggleClientPause()
        {
            if (ClientPause)
                ClientPause = false;
            else
                ClientPause = true;

            if (ClientPause)
            {
                TaskPauseTemp = TaskPause;
                TaskPause = ClientPause;
                _taskWindow.SetPause(ClientPause);
            }
            else
            {
                TaskPause = TaskPauseTemp;
                _taskWindow.SetPause(TaskPause);
            }
        }

        public void ToggleTaskPause()
        {
            if (TaskPause)
                TaskPause = false;
            else
                TaskPause = true;

            _taskWindow.SetPause(TaskPause);
        }

        public bool IsRunning
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual void Start()
        {
            _taskWindow.SetStatus(1);
        }

        public virtual void Stop()
        {
            _taskWindow.SetStatus(0);
        }

        public virtual void Delete()
        {
            _parentChar.CharWindow.RemoveTaskWindow(_taskWindow);
        }


        public TaskWindow TaskWindow
        {
            get { throw new NotImplementedException(); }
        }

        public Character ParentChar
        {
            get { throw new NotImplementedException(); }
        }

        public TaskDBEntry TaskDBEntry
        {
            get { return _taskDBEntry; }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageCallback(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, IntPtr lpCallBack, IntPtr dwData);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_MOVE = 0x0001;

        public bool SendKeyDown(int _keyCode, bool _foregroundNeeded)
        {
            if (_foregroundNeeded && GetForegroundWindow() != _parentChar.ParentClient.Process.MainWindowHandle)
            {
                return false;
            }
            else
            {
                const uint WM_KEYDOWN = 0x100;
                IntPtr result1 = SendMessage(_parentChar.ParentClient.Process.MainWindowHandle, WM_KEYDOWN, (IntPtr)_keyCode, (IntPtr)0);
                return true;
            }
        }

        public void SendKeyUp()
        {
            const uint WM_KEYUP = 0x101;
            IntPtr result1 = SendMessage(_parentChar.ParentClient.Process.MainWindowHandle, WM_KEYUP, (IntPtr)0, (IntPtr)0);
        }

        int xoffset = 1;
        public bool DoMouseClick(bool _foregroundNeeded)
        {
            if (_foregroundNeeded && GetForegroundWindow() != _parentChar.ParentClient.Process.MainWindowHandle)
            {
                return false;
            }
            else
            {
                Cursor.Position = new Point(Cursor.Position.X + xoffset, Cursor.Position.Y);
                int X = Cursor.Position.X;
                int Y = Cursor.Position.Y;

                //move to coordinates

                //Cursor.Position = pt;

                //perform click            
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                xoffset = xoffset * -1;
                return true;
            }
        }
    }
}
