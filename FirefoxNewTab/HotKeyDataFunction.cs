using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FirefoxNewTab
{
    /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */
    public static class HotKeyDataFunction
    {
        public static (int id, KeyModifier keyModifier, Keys key) HotKeyData(Message message)
        {
            int id = message.WParam.ToInt32();                                        // The id of the hotkey that was pressed.
            KeyModifier modifier = (KeyModifier)((int)message.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
            Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.

            return (id, modifier, key);
        }
    }
}
