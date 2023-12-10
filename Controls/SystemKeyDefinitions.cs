using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PCRemoteControl.Controls
{
    public static partial class SystemKeyDefinitions
    {
        static readonly Dictionary<Key, string> AlternateKeyNames = new Dictionary<Key, string>
        {
            {Key.Ctrl, "Ctrl"}
        };

        static readonly Dictionary<Key, string> KeyNames = Enum.GetValues(typeof(Key))
            .Cast<Key>()
            .ToDictionary(t => t, t => t.ToString());

        internal static string GetName(this Key key)
        {
            bool hasKey = AlternateKeyNames.TryGetValue(key, out string newName);
            string name = hasKey ? newName : key.AsString();
            return name;
        }
        
        // todo: custom button class that accepts GuiConstrainedGestureInterpreter for multitouch
        internal static readonly List<KeyCommand> ExtraKeys = new List<KeyCommand>()
        {
            new KeyCommand(Key.Alt, KeyInteractionMode.Toggle),
            new KeyCommand(Key.Ctrl, KeyInteractionMode.Toggle),
            new KeyCommand(Key.Shift, KeyInteractionMode.Toggle),
            new KeyCommand(Key.Escape, KeyInteractionMode.Press),
            new KeyCommand(Key.Meta, KeyInteractionMode.Press),
            
            new KeyCommand(Key.F1, KeyInteractionMode.Press),
            new KeyCommand(Key.F2, KeyInteractionMode.Press),
            new KeyCommand(Key.F3, KeyInteractionMode.Press),
            new KeyCommand(Key.F4, KeyInteractionMode.Press),
            new KeyCommand(Key.F5, KeyInteractionMode.Press),
            new KeyCommand(Key.F6, KeyInteractionMode.Press),
            new KeyCommand(Key.F7, KeyInteractionMode.Press),
            new KeyCommand(Key.F8, KeyInteractionMode.Press),
            new KeyCommand(Key.F9, KeyInteractionMode.Press),
            new KeyCommand(Key.F10, KeyInteractionMode.Press),
            new KeyCommand(Key.F11, KeyInteractionMode.Press),
            new KeyCommand(Key.F12, KeyInteractionMode.Press),
            
            new KeyCommand(Key.Up, KeyInteractionMode.Press),
            new KeyCommand(Key.Down, KeyInteractionMode.Press),
            new KeyCommand(Key.Left, KeyInteractionMode.Press),
            new KeyCommand(Key.Right, KeyInteractionMode.Press),
            
            new KeyCommand(Key.Print, KeyInteractionMode.Press),
            
            new KeyCommand(Key.Pageup, KeyInteractionMode.Press),
            new KeyCommand(Key.Pagedown, KeyInteractionMode.Press),
            new KeyCommand(Key.Home, KeyInteractionMode.Press),
            
            new KeyCommand(Key.Insert, KeyInteractionMode.Press),
            new KeyCommand(Key.Backspace, KeyInteractionMode.Press),
            new KeyCommand(Key.Delete, KeyInteractionMode.Press),
            new KeyCommand(Key.Tab, KeyInteractionMode.Press),
            new KeyCommand(Key.Enter, KeyInteractionMode.Press),
            
            //shortcuts
            new KeyCommand(Key.Alt, Key.Tab),
            new KeyCommand(Key.Meta, Key.Tab),
            new KeyCommand(Key.Meta, Key.D),
            new KeyCommand(Key.Alt, Key.Space),
            new KeyCommand(Key.Alt, Key.Enter),
            new KeyCommand(Key.Ctrl, Key.Alt, Key.Delete),
            new KeyCommand("Task Manager", Key.Ctrl, Key.Shift, Key.Escape),
            new KeyCommand("Copy", Key.Ctrl, Key.C),
            new KeyCommand("Paste", Key.Ctrl, Key.V),
        };

        internal static readonly Dictionary<string, KeyCommand> KeyDict = 
            ExtraKeys.ToDictionary(x => x.Name, x => x);

        public static string AsString(this Key key) => KeyNames[key];
    }
}