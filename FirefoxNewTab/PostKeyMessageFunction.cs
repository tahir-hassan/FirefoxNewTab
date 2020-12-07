using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace FirefoxNewTab
{
    public enum KeyMessage
    {
        KeyDown,
        KeyUp,
        KeyDownUp
    }

    public static class PostKeyMessageFunction
    {
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x0101;

        [DllImport("user32.dll")]
        private static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private static uint[] TranslateKeyMessage(KeyMessage message)
        {
            if (message == KeyMessage.KeyDown)
            {
                return new[] { WM_KEYDOWN };
            }
            else if (message == KeyMessage.KeyUp)
            {
                return new[] { WM_KEYUP };
            }
            else
            {
                return new[] { WM_KEYDOWN, WM_KEYUP };
            }
        }

        public static void PostKeyMessage(IntPtr windowHandle, KeyMessage message, System.Windows.Forms.Keys key)
        {
            foreach (var msg in TranslateKeyMessage(message))
            {
                PostMessage(windowHandle, msg, (IntPtr)key, (IntPtr)0x0);
            }
        }
    }
}
