using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using test.XML;

namespace test.Task
{
    public class TaskHold : TaskBase
    {
        //initial parameter
        public String _skillHex;
        private int _sendDelay;
        private bool _withClick;
        private int _clickDelay;

        //runtime variables
        private Thread _sendThread;


        public TaskHold(TaskDBEntry _taskDBEntry, Character _parentChar, String _skillHex, int _sendDelay, bool _withClick, int _clickDelay)
            : base(_taskDBEntry, _parentChar)
        {
            this._skillHex = _skillHex;
            this._sendDelay = _sendDelay;
            this._withClick = _withClick;
            this._clickDelay = _clickDelay;

            AddTrigger();
        }


        public void AddTrigger()
        {
            GlobalKeyboard.KeyDown += GlobalKeyboard_KeyDown;
            GlobalKeyboard.KeyUp += GlobalKeyboard_KeyUp;
        }

        public virtual void GlobalKeyboard_KeyUp(KeyboardEventArgs _args)
        {
            if (!TaskPause && base._parentChar.Toolbar.GetToolbarSlot(_skillHex) != null && _args.KeyCode == base._parentChar.Toolbar.GetToolbarSlot(_skillHex).KeyInt && this._parentChar._parentClient == ProcessManager.GetActiveClient())
            {
                _args.Handled = true;
                this.Stop();
            }
        }

        public virtual void GlobalKeyboard_KeyDown(KeyboardEventArgs _args)
        {
            if (!TaskPause && base._parentChar.Toolbar.GetToolbarSlot(_skillHex) != null && _args.KeyCode == base._parentChar.Toolbar.GetToolbarSlot(_skillHex).KeyInt && this._parentChar._parentClient == ProcessManager.GetActiveClient())
            {
                _args.Handled = true;
                this.Start();
            }
        }

        public override void Delete()
        {
            base.Delete();
            RemoveTrigger();
        }

        public override void Start()
        {
            if (!base.TaskPause)
            {
                base.Start();
                if (_sendThread == null || _sendThread.ThreadState == ThreadState.Stopped)
                {
                    _sendThread = new Thread(SendKeys);
                    _sendThread.IsBackground = true;
                    _sendThread.Name = "taskholdthread";
                    _runThread = true;
                    _sendThread.Start();
                }
            }
        }

        public override void Stop()
        {
            base.Stop();
            _runThread = false;
        }

        public void RemoveTrigger()
        {
            GlobalKeyboard.KeyDown -= GlobalKeyboard_KeyDown;
            GlobalKeyboard.KeyUp -= GlobalKeyboard_KeyUp;
        }

        public bool _runThread;
        private void SendKeys()
        {
            while (_runThread)
            {
                if (base._parentChar.Toolbar.GetToolbarSlot(_skillHex) != null)
                {
                    //Console.WriteLine("Sending SendKeyUntilStop");
                    if (!base.SendKeyDown(base._parentChar.Toolbar.GetToolbarSlot(_skillHex).KeyInt, _withClick))
                    {
                        _runThread = false;
                    }
                    base.SendKeyUp();
                    if (_withClick)
                    {
                        Thread.Sleep(_clickDelay);
                        if (!DoMouseClick(_withClick))
                        {
                            _runThread = false;
                        }
                        Thread.Sleep(_sendDelay - _clickDelay);
                    }
                    else
                    {
                        Thread.Sleep(_sendDelay);
                    }
                }
            }
        }
    }
}
