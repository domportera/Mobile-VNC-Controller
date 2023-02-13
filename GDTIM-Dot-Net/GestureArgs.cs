using System;
using System.Collections.Generic;
using GDTIMDotNet;
using Godot;
using GodotExtensions;

namespace Godot
{
	// todo: rename
	public abstract class GDTIMTouchAction : InputEventAction
	{
		public readonly Vector2 Position;
		public bool PreventPropagation { get; private set; }

		public GDTIMTouchAction(Vector2 position)
		{
			Position = position;
			Action = GetType().Name;
			Pressed = true;
		}

		// todo: interrogate need for PreventPropagation - does "consuming" these events in typical godot 
		// fashion achieve this?
		public void AcceptGestures(IGestureInterpreter node, bool subscribeToMultiTouch, bool preventPropagation)
		{
			if (PreventPropagation)
				return;
			
			GDLogger.Log(this, $"Accepted!");

			PreventPropagation = preventPropagation;
		}
	}
	
	public class TouchBegin : GDTIMTouchAction
	{
		public TouchBegin(Vector2 position) : base(position)
		{
		}
	}
	
	public class TouchEnd : GDTIMTouchAction
	{
		public readonly bool Cancelled;
		
		public TouchEnd(Vector2 position, bool cancelled) : base(position)
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

		public MultiTouch(Vector2 position, int fingers)
		{
			Fingers = fingers;
			Position = position;
			Action = GetType().Name;
			Pressed = true;
		}
	}
	
	public class Pinch : MultiTouch
	{
		public readonly float Relative, Distance;

		public Pinch(Vector2 position, float relative, float distance, int fingers) : base(position, fingers)
		{
			Relative = relative;
			Distance = distance;
		}
	}

	public class Twist : MultiTouch
	{
		public readonly float Relative;

		public Twist(Vector2 position, float relative, int fingers) : base(position, fingers)
		{
			Relative = relative;
		}
	}
	
	public class MultiTap : MultiTouch
	{
		public MultiTap(Vector2 position, int fingers) : base(position, fingers){}
	}
	
	public class MultiDrag : MultiTouch
	{
		public readonly Vector2 Relative;
		public MultiDrag(Vector2 position, Vector2 relative, int fingers) : base (position, fingers)
		{
			Relative = relative;
		}
	}
}