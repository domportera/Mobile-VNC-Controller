using System.Collections.Generic;
using Godot;
using GodotExtensions;

namespace PCRemoteControl.Controls
{
    public partial class SystemKeys : GridContainer
    {
        public override void _Ready()
        {
            foreach (KeyCommand command in _extraKeys)
            {
                Button button = new Button();
                button.Text = command.Name;

                if (command.Interaction == KeyInteractionMode.Press)
                    button.Connect("pressed", this, "ExecuteButton", new Godot.Collections.Array{command.Name});
                
                AddChild(button);
            }
        }

        void ExecuteButton(string commandName)
        {
            GDLogger.Log(this, $"Button {commandName} pressed!");
            KeyCommand command = _keyDict[commandName];
        }
    }
}