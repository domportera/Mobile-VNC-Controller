using System;
using System.Collections.Generic;
using Godot;

namespace Godot
{
	public class GDTIMTouchAction : InputEventAction
	{
		internal List<Godot.Node> NodesTouched = new List<Godot.Node>();

		public GDTIMTouchAction(Vector2 position)
		{
			Position = position;
		}

		public Vector2 Position { get; }
	}
	
	public abstract class GDTIMGestureArgs : InputEventAction
	{
		public Vector2 Position { get; protected set; }
		
		public new virtual bool Pressed { get; }
	}
	
	public class SingleTapArgs : GDTIMGestureArgs
	{
		public SingleTapArgs(Vector2 position)
		{
			Position = position;
		}
	}
	
	public class SingleTouchArgs : GDTIMGestureArgs
	{
		public readonly bool Pressed;
		public readonly bool Cancelled;
		
		public SingleTouchArgs(Vector2 position, bool pressed, bool cancelled)
		{
			Position = position;
			Pressed = pressed;
			Cancelled = cancelled;
		}
		
	}
	
	public class PinchArgs : EventArgs
	{
		public readonly Vector2 Position;
		public readonly float Relative, Distance;
		public readonly int Fingers;

		public PinchArgs(Vector2 position, float relative, float distance, int fingers)
		{
			Position = position;
			Relative = relative;
			Distance = distance;
			Fingers = fingers;
		}
	}

	public class TwistArgs : EventArgs
	{
		public readonly Vector2 Position;
		public readonly float Relative;
		public readonly int Fingers;

		public TwistArgs(Vector2 position, float relative, int fingers)
		{
			Position = position;
			Relative = relative;
			Fingers = fingers;
		}
	}
	
	public class MultiTapArgs : EventArgs
	{
		public readonly Vector2 Position;
		public readonly int Fingers;

		public MultiTapArgs(Vector2 position, int fingers)
		{
			Position = position;
			Fingers = fingers;
		}
	}
	public class MultiDragArgs : SingleDragArgs
	{
		public readonly int Fingers;
		public MultiDragArgs(Vector2 position, Vector2 relative, int fingers) : base (position, relative)
		{
			Fingers = fingers;
		}
	}

	public class SingleDragArgs : EventArgs
	{
		public readonly Vector2 Position, Relative;
		
		public SingleDragArgs(Vector2 position, Vector2 relative)
		{
			Position = position;
			Relative = relative;
		}
	}
}