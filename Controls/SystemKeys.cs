using System.Collections.Generic;
using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;
using static PCRemoteControl.Controls.SystemKeyDefinitions;

namespace PCRemoteControl.Controls
{
    public class SystemKeys : GridContainer
    {
        [Export] NodePath _vncHandlerRelativePath = "../../../VncHandler";
        VncHandler _vncHandler;
        
        public override void _Ready()
        {
            _vncHandler = GetNode<VncHandler>(_vncHandlerRelativePath);
            foreach (KeyCommand command in ExtraKeys)
            {
                var button = InstantiateKeyCommandButton(command);
                AddChild(button.Control);
            }
        }

        MultiLineButton InstantiateKeyCommandButton(KeyCommand command)
        {
            var button = new MultiLineButton(command.Name, true);

            if (command.Interaction == KeyInteractionMode.Press)
                button.Button.Connect("pressed", this, "ExecuteButton", new Godot.Collections.Array { command.Name });
            
            return button;
        }

        // ReSharper disable once UnusedMember.Local
        void ExecuteButton(string commandName)
        {
            GDLogger.Log(this, $"Button {commandName} pressed!");
            KeyCommand command = KeyDict[commandName];
            command.Execute(_vncHandler);
        }
    }
}