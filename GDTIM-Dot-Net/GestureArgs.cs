using System;
using System.Collections.Generic;
using GDTIMDotNet;
using GDTIMDotNet.GestureReceiving;
using Godot;
using GodotExtensions;

// todo: rename
public abstract class GDTIMTouchAction : InputEventAction
{
	public readonly Vector2 Position;
	public readonly int Index;

	public GDTIMTouchAction(Vector2 position, int index)
	{
		Position = position;
		Action = GetType().Name;
		Pressed = true;
		Index = index;
		
		if (!InputMap.HasAction(Action))
		{
			InputMap.AddAction(Action);
			GD.Print(this, $"ADDED: {Action}");
		}
	}

}

public class TouchBegin : GDTIMTouchAction
{
	bool _preventPropagation = false;

	internal event EventHandler<TouchBeginEventArgs> Accepted;
	internal class TouchBeginEventArgs : EventArgs
	{
		public readonly IGestureInterpreter Interpreter;
		public readonly int Index;
		public readonly bool SubscribeToMultiTouch;

		public TouchBeginEventArgs(IGestureInterpreter interpreter, int index, bool subscribeToMultiTouch)
		{
			Interpreter = interpreter;
			Index = index;
			SubscribeToMultiTouch = subscribeToMultiTouch;
		}
	}

	public TouchBegin(Vector2 position, int index) : base(position, index) { }
	
	public void AcceptGesturesControl<T>(T control, bool subscribeToMultiTouch, bool disregardMouseFilter, bool acceptFocus = true)
		where T : Control, IGestureInterpreter
	{
		if (!disregardMouseFilter)
		{
			switch (control.MouseFilter)
			{
				case Control.MouseFilterEnum.Ignore:
					return;
				case Control.MouseFilterEnum.Stop:
					AcceptNode(control, subscribeToMultiTouch);
					control.AcceptEvent();
					break;
				case Control.MouseFilterEnum.Pass:
					AcceptNode(control, subscribeToMultiTouch);
					break;
				default:
					throw new NotImplementedException();
			}
		}
		else
		{
			AcceptNode(control, subscribeToMultiTouch);
		}

		if (acceptFocus && control.FocusMode != Control.FocusModeEnum.None)
		{
			Control.FocusModeEnum focusMode = control.FocusMode;
			control.FocusMode = Control.FocusModeEnum.All;
			control.GrabFocus();
			control.FocusMode = focusMode;
		}
	}
	

	public void AcceptGesturesNode<T>(T node, bool subscribeToMultiTouch, bool preventPropagation)
		where T : Node, IGestureInterpreter
	{
		if (_preventPropagation) return;
		_preventPropagation = preventPropagation;
		
		AcceptNode(node, subscribeToMultiTouch);
	}

	void AcceptNode<T>(T node, bool subscribeToMultiTouch) where T : Node, IGestureInterpreter
	{
		var args = new TouchBeginEventArgs(node, Index, subscribeToMultiTouch);
		Accepted?.Invoke(this, args);
	}
}

public class TouchEnd : GDTIMTouchAction
{
	/// <summary>
	/// If the touch has been "cancelled", it means that it has been moved into a multi-finger gesture
	/// </summary>
	public readonly bool Cancelled;
	
	public TouchEnd(Vector2 position, int index, bool cancelled) : base(position, index)
	{
		Cancelled = cancelled;
		Pressed = false;
	}
}
public abstract class SingleGesture : InputEventAction
{
	public Vector2 Position { get; }

	public SingleGesture(Vector2 position)
	{
		Position = position;
	}
}

public class SingleTap : SingleGesture
{
	public SingleTap(Vector2 position) : base(position) { }
}


public class SingleDrag : SingleGesture
{
	public readonly Vector2 Relative;
	
	public SingleDrag(Vector2 position, Vector2 relative): base(position)
	{
		Relative = relative;
	}
}

public class MultiTouch : InputEventAction
{
	public readonly int Fingers;
	public readonly Vector2 Position;
	internal readonly HashSet<IGestureInterpreter> NodesTouched;

	public MultiTouch(HashSet<IGestureInterpreter> touchers, Vector2 position, int fingers)
	{
		NodesTouched = touchers;
		Fingers = fingers;
		Position = position;
		Action = GetType().Name;
		Pressed = true;
	}

	public void AcceptGestures(IGestureInterpreter interpreter)
	{
		NodesTouched.Add(interpreter);
	}
}

public class Pinch : MultiTouch
{
	public readonly float Relative, Distance;

	public Pinch(HashSet<IGestureInterpreter> touchers, Vector2 position,
		float relative, float distance, int fingers) : base(touchers, position, fingers)
	{
		Relative = relative;
		Distance = distance;
	}
}

public class Twist : MultiTouch
{
	public readonly float Relative;

	public Twist(HashSet<IGestureInterpreter> touchers, Vector2 position, float relative, int fingers) :
		base(touchers, position, fingers)
	{
		Relative = relative;
	}
}

public class MultiTap : MultiTouch
{
	public MultiTap(HashSet<IGestureInterpreter> touchers, Vector2 position, int fingers):
		base(touchers, position, fingers){}
}

public class MultiDrag : MultiTouch
{
	public readonly Vector2 Relative;
	public MultiDrag(HashSet<IGestureInterpreter> touchers, Vector2 position, Vector2 relative, int fingers) 
		: base (touchers, position, fingers)
	{
		Relative = relative;
	}
}