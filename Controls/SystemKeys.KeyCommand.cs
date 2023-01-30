using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PCRemoteControl.Controls
{
    public partial class SystemKeys
    {
        // todo: custom button class that accepts GuiConstrainedGestureInterpreter for multitouch
        static List<KeyCommand> _extraKeys = new List<KeyCommand>()
        {
            new KeyCommand(KeyList.Alt, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Control, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Shift, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Escape, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F1, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F2, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F3, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F4, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F5, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F6, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F7, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F8, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F9, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F10, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F11, KeyInteractionMode.Press),
            new KeyCommand(KeyList.F12, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Up, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Down, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Left, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Right, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Tab, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Enter, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Meta, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Print, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Pageup, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Pagedown, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Home, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Insert, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Backspace, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Delete, KeyInteractionMode.Press),
            
            //shortcuts
            new KeyCommand(KeyList.Alt, KeyList.Tab),
            new KeyCommand(KeyList.Meta, KeyList.Tab),
            new KeyCommand(KeyList.Meta, KeyList.D),
            new KeyCommand(KeyList.Alt, KeyList.Space),
            new KeyCommand(KeyList.Alt, KeyList.Enter),
            new KeyCommand(KeyList.Control, KeyList.Shift, KeyList.Escape),
            new KeyCommand(KeyList.Control, KeyList.Alt, KeyList.Delete),
        };

        static Dictionary<string, KeyCommand> _keyDict = _extraKeys.ToDictionary(x => x.Name, x => x);
        
        enum KeyInteractionMode {Toggle, Press, Hold}
        
        [Serializable]
        class KeyCommand
        {
            public string Name;
            public KeyList[] Keys;
            public KeyInteractionMode Interaction;

            public KeyCommand(KeyList key, KeyInteractionMode interaction)
            {
                Keys = new []{ key };
                Interaction = interaction;
                Name = key.ToString();
            }

            public KeyCommand(params KeyList[] keys)
            {
                Interaction = KeyInteractionMode.Press;
                Keys = keys;

                Name = keys[0].ToString();
                for(int i = 1; i < keys.Length; i++)
                {
                    Name += $" + {keys[i].ToString()}";
                }
            }
        }
    }
}