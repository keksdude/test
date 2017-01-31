using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using test.XML;

namespace test.Task
{
    public class TaskToggle : TaskHold
    {
        private bool _toggle = true;

        public TaskToggle(TaskDBEntry _taskDBEntry, Character _parentChar, String _skillHex, int _sendDelay, bool _withClick, int _clickDelay)
            : base(_taskDBEntry, _parentChar, _skillHex, _sendDelay, _withClick, _clickDelay)
        {

        }


        public override void GlobalKeyboard_KeyUp(KeyboardEventArgs _args)
        {

        }

        public override void GlobalKeyboard_KeyDown(KeyboardEventArgs _args)
        {
            if (!TaskPause && base._parentChar.Toolbar.GetToolbarSlot(_skillHex) != null && _args.KeyCode == base._parentChar.Toolbar.GetToolbarSlot(base._skillHex).KeyInt && this._parentChar._parentClient == ProcessManager.GetActiveClient())
            {
                _args.Handled = true;
                if (_toggle)
                {
                    this.Start();
                    _toggle = false;
                }
                else
                {
                    _toggle = true;
                    this.Stop();
                }
            }
        }
    }
}
