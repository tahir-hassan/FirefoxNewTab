using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

/*

TAHIR: read README.md for the link to the StackOverflow link to set a Global hook using C#

*/

namespace FirefoxNewTab
{
    using static User32;
    using static RegisterHotKeyFunction;
    using static PostKeyMessageFunction;
    using static GetProcessFunction;

    public static class GlobalHook
    {
        private static int _hookHandle = 0;

        private static string KeyRepresentation(bool ctrlDown, bool shiftDown, bool altDown, Keys numkey)
        {
            string bracket(bool apply, string parent, string inner) => apply ? $"{parent}({inner})" : inner;

            return bracket(ctrlDown, "Ctrl", bracket(shiftDown, "Shift", bracket(altDown, "Alt", numkey.ToString())));
        }

        // https://forums.codeguru.com/showthread.php?503595-GetKeyState-function-in-C
        [Flags]
        private enum KeyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }
        //https://forums.codeguru.com/showthread.php?503595-GetKeyState-function-in-C
        private static KeyStates GetKeyState2(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            Debug.WriteLine($"{key}: {retVal}");

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        private static bool IsDown(Keys key)
        {
            return GetKeyState((int)key) < 0;
        }

        private static IntPtr KbHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = (KbLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KbLLHookStruct));

                bool ctrlDown = IsDown(Keys.LControlKey);
                bool shiftDown = IsDown(Keys.LShiftKey);
                bool altDown = IsDown(Keys.LMenu);
                var key = (Keys)hookStruct.vkCode;
                var isNumpadKey = key >= Keys.NumPad0 && key <= Keys.NumPad9;
                if (isNumpadKey && (ctrlDown || shiftDown || altDown))
                {
                    Debug.WriteLine(KeyRepresentation(ctrlDown, shiftDown, altDown, key));
                    return new IntPtr(1);
                }
            }

            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }


        public static void SetHook()
        {
            // Set system-wide hook.
            _hookHandle = SetWindowsHookEx(
                WH_KEYBOARD_LL,
                KbHookProc,
                (IntPtr)0,
                0);
        }

        public static void Unhook()
        {
            UnhookWindowsHookEx(_hookHandle);
        }
    }

    public partial class MainForm : Form
    {
        private static bool isUsingGlobalHook = true;
        

        private NotifyIcon notifyIcon;
        const int HOTKEY_ID = 0;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                var handle = GetForegroundWindow();
                var process = GetProcess(handle);

                if (process.ProcessName == "firefox")
                {
                    var containersForm = new FirefoxContainersForm();
                    containersForm.FirefoxHandle = handle;
                    containersForm.Show();
                    SetForegroundWindow(containersForm.Handle);
               }
                else
                {

                    PostKeyMessage(handle, KeyMessage.KeyDown, Keys.Control);
                    PostKeyMessage(handle, KeyMessage.KeyDownUp, Keys.T);
                    // lifting up control doesn't seem necessary
                }
            }
        }

        

        private void RegisterControlTab()
        {
            
            RegisterHotKey(this.Handle, HOTKEY_ID, KeyModifier.Control, Keys.T);
        }

        private void UnregisterControlTab()
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
        }

        public MainForm()
        {
            InitializeComponent();
            
            this.minimizeToTrayButton.Click += (_sender,_e) => MinimizeToTray();
            this.Load += MainForm_Load;
            this.FormClosing += Form1_FormClosing;

            if (isUsingGlobalHook)
                GlobalHook.SetHook();
            
            RegisterControlTab();
        }

        private void MinimizeToTray()
        {
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void UnminimizeFromTray()
        {
            this.ShowInTaskbar = true;
            this.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Icon firefoxIcon() => new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firefox.ico"));

            this.Icon = firefoxIcon();
            this.notifyIcon = new NotifyIcon
            {
                Visible = true,
                // Icon = SystemIcons.Application,
                Icon = firefoxIcon(),
                Text = "Firefox New Tab App"
            };

            this.notifyIcon.DoubleClick += (_sender, _e) => UnminimizeFromTray();
            // MinimizeToTray();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.notifyIcon.Visible = false;
            UnregisterControlTab();

            if (isUsingGlobalHook)
                GlobalHook.Unhook();

            //Unhook();
        }
    }
}
