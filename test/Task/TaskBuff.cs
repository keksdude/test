using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using test.XML;

namespace test.Task
{
    public class TaskBuff : TaskBase
    {
        //initial parameter
        private String _skillHex;
        private String _buffHex;
        private int _sendDelay;
        private bool _withClick;
        private int _clickDelay;
        private bool _onAppearence;

        //runtime variables
        private Thread _sendThread;


        public TaskBuff(TaskDBEntry _taskDBEntry, Character _parentChar, String _skillHex, String _buffHex, int _sendDelay, bool _withClick, int _clickDelay, bool _onAppearence)
            : base(_taskDBEntry, _parentChar)
        {
            this._skillHex = _skillHex;
            this._buffHex = _buffHex;
            this._sendDelay = _sendDelay;
            this._withClick = _withClick;
            this._clickDelay = _clickDelay;
            this._onAppearence = _onAppearence;



            AddTrigger();
        }


        public void AddTrigger()
        {
            if(_onAppearence)
                this._parentChar.Buffs.BuffAppeared += Buffs_BuffAppeared;
            else
                this._parentChar.Buffs.BuffVanished += Buffs_BuffVanished;
        }

        void Buffs_BuffVanished(Buff _buff)
        {
            if (_buff.HexData == _buffHex)
            {
                this.Start();
            }
        }

        void Buffs_BuffAppeared(Buff _buff)
        {
            if(_buff.HexData == _buffHex)
            {
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
                    _sendThread.Name = "taskbuff";
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
            if (this._parentChar.Buffs != null)
            {
                if (_onAppearence)
                    this._parentChar.Buffs.BuffAppeared -= Buffs_BuffAppeared;
                else
                    this._parentChar.Buffs.BuffVanished -= Buffs_BuffVanished;
            }
        }

        public bool _runThread;
        private void SendKeys()
        {
            while (_runThread)
            {
                Thread.Sleep(100);
                bool _check = _parentChar.Buffs.IsActive(this._buffHex);
                if (!_check && _onAppearence)
                {
                    base._taskWindow.SetStatus(2);
                    Stop();
                }
                else if (_check && !_onAppearence)
                {
                    _taskWindow.SetStatus(2);
                    Stop();
                }
                else
                {
                    _taskWindow.SetStatus(1);
                }

                if (_runThread)
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

                    Thread.Sleep(100);
                    _check = _parentChar.Buffs.IsActive(this._buffHex);
                    if (!_check && _onAppearence)
                    {
                        base._taskWindow.SetStatus(2);
                        Stop();
                    }
                    else if (_check && !_onAppearence)
                    {
                        _taskWindow.SetStatus(2);
                        Stop();
                    }
                    else
                    {
                        _taskWindow.SetStatus(1);
                    }
                }
            }
        }

        /*sendKeyDown(_keyCode1);
                sendKeyUp();
                Thread.Sleep(_pauseBetweenSkills);
                sendKeyDown(_keyCode2);
                sendKeyUp();
                Thread.Sleep(_sendDelay);*/
    }
}
