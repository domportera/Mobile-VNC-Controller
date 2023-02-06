using System.Collections.Generic;
using CustomDotNetExtensions;
using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;
using static PCRemoteControl.Controls.SystemKeyDefinitions;

namespace PCRemoteControl.Controls
{
    public class SystemKeys : GridContainer
    {
        [Export] NodePath _vncHandlerRelativePath = "../../../VncHandler";
        [Export] bool _padButtons = true;
        VncHandler _vncHandler;
        
        public override void _Ready()
        {
            _vncHandler = GetNode<VncHandler>(_vncHandlerRelativePath);
            foreach (KeyCommand command in ExtraKeys)
            {
                var button = InstantiateKeyCommandButton(command);
                AddChild(button.Label);
            }
        }

        MultiLineTouchableButton InstantiateKeyCommandButton(KeyCommand command)
        {
            var button = new MultiLineTouchableButton(command.Name, _padButtons);
            command.AttachButton(button.ButtonUpDown, ExecuteKeys);
            
            return button;
        }

        void ExecuteKeys(KeyList[] keys, bool pressed)
        {
            foreach (KeyList key in keys)
            {
                _vncHandler.SendKey(key, pressed);
                GDLogger.Log(this, $"Button {key.AsString()} pressed {(pressed ? "down" : "up")}!\n");
            }
        }
    }
}