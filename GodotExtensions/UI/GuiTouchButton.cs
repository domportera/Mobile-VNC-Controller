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
            MouseFilter = MouseFilterEnum.Ignore;
            var interpreter = new ControlGestureInterpreter(this);
            interpreter.TouchBegin += OnTouchBegin;
            interpreter.TouchEnd += OnTouchEnd;
        }

        void OnTouchEnd(object sender, TouchEnd e)
        {
            GDLogger.Log(this, $"BUTTON END");
            Pressed = false;
            
            EmitSignal(nameof(pressed_up));
            PressUp?.Invoke(this, EventArgs.Empty);

            if(ActionMode == ActionModeEnum.Release)
                EmitSignal("pressed");
        }

        void OnTouchBegin(object sender, TouchBegin e)
        { 
            GDLogger.Log(this, $"BUTTON START");
            Pressed = true;
            
            EmitSignal(nameof(pressed_down));
            PressDown?.Invoke(this, EventArgs.Empty);

            if(ActionMode == ActionModeEnum.Press)
                EmitSignal("pressed");
        }
    }
}