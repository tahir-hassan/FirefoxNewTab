using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FirefoxNewTab
{
    public static class RegisterHotKeyFunction
    {
        public static void RegisterHotKey(IntPtr handle, int id, KeyModifier modifier, Keys key)
        {
            User32.RegisterHotKey(handle, id, (int)modifier, key.GetHashCode());
        }
    }
}
