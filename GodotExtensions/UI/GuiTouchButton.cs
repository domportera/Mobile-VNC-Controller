using System;
using GDTIMDotNet;
using Godot;

namespace GodotExtensions
{
    public class GuiTouchButton : Button, IButtonUpDown
    {
        // ReSharper disable once InconsistentNaming
        [Signal] //gdscript-style naming
        delegate void pressed_down();
        
        // ReSharper disable once InconsistentNaming
        [Signal] //gdscript-style naming
        delegate void pressed_up();
        
        public event EventHandler PressDown;
        public event EventHandler PressUp;
        public event EventHandler<bool> Toggled;

        //prevent tampering from the outside
        public new bool ToggleMode => base.ToggleMode;
        
        public override void _Ready()
        {
            base.ToggleMode = true;
            var interpreter = new ControlGestureInterpreter(this);
            interpreter.TouchBegin += InterpreterOnSingleTouch;
        }

        void InterpreterOnSingleTouch(object sender, TouchBegin e)
        { 
            GDLogger.Log(this, $"This!!!!");
            Pressed = e.Pressed;
            string signalUpDown;
            EventHandler pressEvent;
            
            if (e.Pressed)
            {
                signalUpDown = nameof(pressed_down);
                pressEvent = PressDown;
            }
            else
            {
                signalUpDown = nameof(pressed_up);
                pressEvent = PressUp;
            }
            
            EmitSignal(signalUpDown);
            pressEvent?.Invoke(this, EventArgs.Empty);

            bool actionModePressed = ActionMode == ActionModeEnum.Press;
            if(e.Pressed == actionModePressed)
                EmitSignal("pressed");
        }
    }
}