using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FirefoxNewTab
{
    using static User32;

    public static class GetProcessFunction
    {
        public static Process GetProcess(IntPtr handle)
        {
            uint pID;

            GetWindowThreadProcessId(handle, out pID);

            var process = Process.GetProcessById((int)pID);
            return process;
        }
    }
}
