using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;

/*

TAHIR: read README.md for the link to the StackOverflow link to set a Global hook using C#

*/

namespace FirefoxNewTab
{
    using static User32;
    using static RegisterHotKeyFunction;
    using static PostKeyMessageFunction;
    using static GetProcessFunction;

    public partial class MainForm : Form
    {
        private static int _hookHandle = 0;

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

        private IntPtr KbHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = (KbLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KbLLHookStruct));

                // Quick and dirty check. You may need to check if this is correct. See GetKeyState for more info.
                bool ctrlDown = GetKeyState(VK_LCONTROL) != 0 || GetKeyState(VK_RCONTROL) != 0;

                var key = (Keys)hookStruct.vkCode;

                if (ctrlDown && key == Keys.T) // Ctrl+V
                {
                    Clipboard.SetText("Hi"); // Replace this with yours something here...
                    return new IntPtr(1);
                }
            }

            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);         
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

            //SetHook();
            
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

        private void SetHook()
        {
            // Set system-wide hook.
            _hookHandle = SetWindowsHookEx(
                WH_KEYBOARD_LL,
                KbHookProc,
                (IntPtr)0,
                0);
        }

        private void Unhook()
        {
            UnhookWindowsHookEx(_hookHandle);
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
            //Unhook();
        }
    }
}
