using System.Runtime.InteropServices;

namespace FirefoxNewTab
{
    //Declare the wrapper managed MouseHookStruct class.
    [StructLayout(LayoutKind.Sequential)]
    public class KbLLHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }
}
