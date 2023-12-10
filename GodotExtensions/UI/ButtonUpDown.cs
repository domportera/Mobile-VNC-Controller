using System;
using Godot;

namespace GodotExtensions
{
    /// <summary>
    /// A button pre-configured for touch input
    /// Uses toggling logic on touch or mouse up/down
    ///
    /// Can use native signals Toggle(bool buttonPressed), but also raises additional signals "PressDown" and "PressUp"
    /// for convenience.
    ///
    /// Currently, touch input does not work due to a bug in Godot: https://github.com/godotengine/godot/issues/29525
    /// this is being held here anyway until I can test it in Godot 4 once Android export is implemented.
    /// </summary>
    public partial class ButtonUpDown : Button, IButtonUpDown
    {
        // ReSharper disable once InconsistentNaming
        [Signal] //gdscript-style naming
        delegate void pressed_down();
        
        // ReSharper disable once InconsistentNaming
        [Signal] //gdscript-style naming
        delegate void pressed_up();

        // ReSharper disable once InconsistentNaming
        [Signal] //gdscript-style naming
        delegate void toggle_immediate();
        // godot event toggles erroneously, so it should be avoided as it will fire on mouse up
        // use this instead 

        public event EventHandler PressDown;
        public event EventHandler PressUp;
        public event EventHandler<bool> Toggled;

        int _touchIndex = NoTouchIndex;
        const int NoTouchIndex = -1;
        const int MouseTouchIndex = int.MinValue;
        bool TouchInProgress => _touchIndex != NoTouchIndex;

        bool _pressed;
        
        //prevent tampering from the outside
        public new bool ToggleMode => base.ToggleMode;

        public ButtonUpDown()
        {
            MouseFilter = MouseFilterEnum.Pass;
        }

        struct PressInfo
        {
            public bool Pressed;
            public Vector2 Position;
            public int Index;

            public PressInfo(bool pressed, Vector2 position, int index)
            {
                Pressed = pressed;
                Position = position;
                Index = index;
            }

            public override string ToString()
            {
                string index = Index == MouseTouchIndex ? "Mouse" : $"Touch {Index.ToString()}";
                return $"{index}: Press {(Pressed ? "DOWN" : "UP")} at {Position.ToString()}";
            }
        }

        public override void _Ready()
        {
            base.ToggleMode = true;
        }

        public override void _Input(InputEvent input)
        {
            bool shouldIgnoreInput = MouseFilter == MouseFilterEnum.Ignore
                                                        || !TouchInProgress
                                                        || input.IsPressed();
            if (shouldIgnoreInput) return;

            PressInfo? pressInfoNullable = GetPressInfo(input);
            if (!pressInfoNullable.HasValue) return;

            PressInfo pressInfo = pressInfoNullable.Value;
            
            bool shouldHandleInGuiInput = this.PointIsInside(pressInfo.Position);
            if (shouldHandleInGuiInput) return;

            bool consumed = HandleTouchUp(pressInfo);
            if(consumed)
                AcceptEvent();
        }

        PressInfo? GetPressInfo(InputEvent input)
        {
            switch (input)
            {
                case InputEventScreenTouch touch:
                    return new PressInfo(touch.Pressed, touch.Position, touch.Index);
                case InputEventMouseButton mouse:
                    return new PressInfo(mouse.Pressed, mouse.Position, MouseTouchIndex);
                default:
                    return null;
            }
        }

        public override void _GuiInput(InputEvent input)
        {
            if (MouseFilter == MouseFilterEnum.Ignore) return;
            PressInfo? pressInfoNullable = GetPressInfo(input);
            if (pressInfoNullable is null) return;

            PressInfo pressInfo = pressInfoNullable.Value;
            GDLogger.Log(this, pressInfo.ToString());
            
            bool consumed;
            if (pressInfo.Pressed)
            {
                consumed = HandleTouchDown(pressInfo);
            }
            else
            {
                consumed = HandleTouchUp(pressInfo);
            }
            
            if(consumed)
                AcceptEvent();
        }

        public override void _Toggled(bool pressed)
        {
            SetPressedNoSignal(_pressed);
        }

        void ToggleImmediate(bool buttonPressed)
        {
            _pressed = buttonPressed;
            Toggled?.Invoke(this, buttonPressed);
            if (buttonPressed)
            {
                EmitSignal(nameof(pressed_down));
                PressDown?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                EmitSignal(nameof(pressed_up));
                PressUp?.Invoke(this, EventArgs.Empty);
            }
            
            SetPressedNoSignal(_pressed);
        }

        bool HandleTouchDown(PressInfo input)
        {
            if (!TouchInProgress)
            {
                ToggleImmediate(true);
            }

            //even if this is a second touch on this button, we take this touch's index for our button up detection.
            _touchIndex = input.Index;

            if (MouseFilter == MouseFilterEnum.Stop)
            {
                return true;
            }

            return false;
        }

        bool HandleTouchUp(PressInfo input)
        {
            bool touchingFingerReleased = input.Index == _touchIndex;
            if (!touchingFingerReleased) return false;

            ToggleImmediate(false);
            _touchIndex = NoTouchIndex;

            if (MouseFilter == MouseFilterEnum.Stop)
            {
                return true;
            }

            return false;
        }
    }
}