using System;
using GDTIMDotNet.GestureReceiving;
using Godot;

public abstract class NiceTouchAction : InputEventAction
{
    public Vector2 Position { get; }
    bool _preventPropagation = false;
	
    public NiceTouchAction(Vector2 position)
    {
        Action = GetType().Name;
        Pressed = true;
        Position = position;
		
        if (!InputMap.HasAction(Action))
        {
            InputMap.AddAction(Action);
            GD.Print(this, $"ADDED: {Action}");
        }
    }
	
    public void AcceptGesturesControl<T>(T control, bool disregardMouseFilter, bool acceptFocus = true)
        where T : Control, IGestureInterpreter
    {
        if (!disregardMouseFilter)
        {
            switch (control.MouseFilter)
            {
                case Control.MouseFilterEnum.Ignore:
                    return;
                case Control.MouseFilterEnum.Stop:
                    AcceptNode(control);
                    control.AcceptEvent();
                    break;
                case Control.MouseFilterEnum.Pass:
                    AcceptNode(control);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        else
        {
            AcceptNode(control);
        }

        if (acceptFocus && control.FocusMode != Control.FocusModeEnum.None)
        {
            Control.FocusModeEnum focusMode = control.FocusMode;
            control.FocusMode = Control.FocusModeEnum.All;
            control.GrabFocus();
            control.FocusMode = focusMode;
        }
    }
	

    public void AcceptGesturesNode<T>(T node, bool preventPropagation)
        where T : Node, IGestureInterpreter
    {
        if (_preventPropagation) return;
        _preventPropagation = preventPropagation;
		
        AcceptNode(node);
    }

    void AcceptNode<T>(T node) where T : Node, IGestureInterpreter
    {
        var args = new TouchBeginEventArgs(node);
        Accepted?.Invoke(this, args);
    }
	

    internal event EventHandler<TouchBeginEventArgs> Accepted;
    internal class TouchBeginEventArgs : EventArgs
    {
        public readonly IGestureInterpreter Interpreter;

        public TouchBeginEventArgs(IGestureInterpreter interpreter)
        {
            Interpreter = interpreter;
        }
    }
}