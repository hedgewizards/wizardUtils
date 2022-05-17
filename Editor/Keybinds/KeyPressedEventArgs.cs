using System;
using UnityEngine;

namespace WizardUtils
{
    public class KeyPressedEventArgs : EventArgs
    {
        Event e;

        public KeyCode KeyCode => e.keyCode;
        public bool Shift => e.shift;
        public bool Consumed { get; private set; } = false;

        public KeyPressedEventArgs(Event e)
        {
            this.e = e;
        }

        public void Use()
        {
            Consumed = true;
        }
    }
}
