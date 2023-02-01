using System.Collections.Generic;
using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;
using static PCRemoteControl.Controls.SystemKeyDefinitions;

namespace PCRemoteControl.Controls
{
    public partial class SystemKeys : GridContainer
    {
        [Export] string _vncHandlerRelativePath = "../../VncHandler";
        VncHandler _vncHandler;
        
        public override void _Ready()
        {
            _vncHandler = GetNode<VncHandler>(_vncHandlerRelativePath);
            foreach (KeyCommand command in ExtraKeys)
            {
                Button button = new Button();
                button.Text = command.Name;
                button.SizeFlagsHorizontal = (int)SizeFlags.ExpandFill;
                button.SizeFlagsVertical = (int)SizeFlags.ExpandFill;

                if (command.Interaction == KeyInteractionMode.Press)
                    button.Connect("pressed", this, "ExecuteButton", new Godot.Collections.Array{command.Name});
                
                AddChild(button);
            }
        }

        void ExecuteButton(string commandName)
        {
            GDLogger.Log(this, $"Button {commandName} pressed!");
            KeyCommand command = KeyDict[commandName];
            command.Execute(_vncHandler);
        }
    }
}