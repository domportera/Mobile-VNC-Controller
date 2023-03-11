using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PCRemoteControl.Controls
{
    public static partial class SystemKeyDefinitions
    {
        static readonly Dictionary<KeyList, string> AlternateKeyNames = new Dictionary<KeyList, string>
        {
            {KeyList.SuperL, "Super"},
            {KeyList.SuperR, "Super(R)"},
            {KeyList.Control, "Ctrl"}
        };

        static readonly Dictionary<KeyList, string> KeyListNames = Enum.GetValues(typeof(KeyList))
            .Cast<KeyList>()
            .ToDictionary(t => t, t => t.ToString());

        internal static string GetName(this KeyList key)
        {
            bool hasKey = AlternateKeyNames.TryGetValue(key, out string newName);
            string name = hasKey ? newName : key.AsString();
            return name;
        }
        
        // todo: custom button class that accepts GuiConstrainedGestureInterpreter for multitouch
        internal static readonly List<KeyCommand> ExtraKeys = new List<KeyCommand>()
        {
            new KeyCommand(KeyList.Alt, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Control, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Shift, KeyInteractionMode.Toggle),
            new KeyCommand(KeyList.Escape, KeyInteractionMode.Press),
            new KeyCommand(KeyList.SuperL, KeyInteractionMode.Press),
            
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
            
            new KeyCommand(KeyList.Print, KeyInteractionMode.Press),
            
            new KeyCommand(KeyList.Pageup, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Pagedown, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Home, KeyInteractionMode.Press),
            
            new KeyCommand(KeyList.Insert, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Backspace, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Delete, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Tab, KeyInteractionMode.Press),
            new KeyCommand(KeyList.Enter, KeyInteractionMode.Press),
            
            //shortcuts
            new KeyCommand(KeyList.Alt, KeyList.Tab),
            new KeyCommand(KeyList.SuperL, KeyList.Tab),
            new KeyCommand(KeyList.SuperL, KeyList.D),
            new KeyCommand(KeyList.Alt, KeyList.Space),
            new KeyCommand(KeyList.Alt, KeyList.Enter),
            new KeyCommand(KeyList.Control, KeyList.Alt, KeyList.Delete),
            new KeyCommand("Task Manager", KeyList.Control, KeyList.Shift, KeyList.Escape),
            new KeyCommand("Copy", KeyList.Control, KeyList.C),
            new KeyCommand("Paste", KeyList.Control, KeyList.V),
        };

        internal static readonly Dictionary<string, KeyCommand> KeyDict = 
            ExtraKeys.ToDictionary(x => x.Name, x => x);

        public static string AsString(this KeyList key) => KeyListNames[key];
    }
}