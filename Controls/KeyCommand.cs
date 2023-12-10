using System;
using Godot;
using GodotExtensions;

namespace PCRemoteControl.Controls
{
    [Serializable]
    internal class KeyCommand
    {
        public string Name;
        public Key[] Keys;
        public KeyInteractionMode Interaction;
        KeyExecution _onKey;

        public delegate void KeyExecution(Key[] keys, bool pressed);
        
        internal KeyCommand(Key key, KeyInteractionMode interaction, string name = "")
        {
            Keys = new []{ key };
            Interaction = interaction;
            Name = name == string.Empty ? key.AsString() : name;
        }

        internal KeyCommand(params Key[] keys)
        {
            Interaction = KeyInteractionMode.Press;
            Keys = keys;

            Name = keys[0].GetName();
            for(int i = 1; i < keys.Length; i++)
            {
                Name += $" {keys[i].GetName()}";
            }
        }

        internal KeyCommand(string name, params Key[] keys)
        {
            Interaction = KeyInteractionMode.Press;
            Keys = keys;
            Name = name;
        }

        public void AttachButton(IButtonUpDown buttonUpDown, KeyExecution onKey)
        {
            _onKey = onKey;
            if (Interaction == KeyInteractionMode.Press)
            {
                buttonUpDown.PressUp += (_,__) => onKey(Keys, false);
                buttonUpDown.PressDown += (_,__) => onKey(Keys, true);
            }
            else
            {
                buttonUpDown.Toggled += (_, pressed) => onKey(Keys, pressed);
            }
        }
    }
    
    internal enum KeyInteractionMode {Toggle, Press}
}