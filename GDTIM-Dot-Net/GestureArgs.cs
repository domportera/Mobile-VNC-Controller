using System;
using System.Collections.Generic;
using GDTIMDotNet;
using Godot;

namespace Godot
{
	// todo: rename
	public abstract class GDTIMTouchAction : InputEventAction
	{
		internal List<IGestureInterpreter> NodesTouched = new List<IGestureInterpreter>();
		internal List<IGestureInterpreter> NodesConsumingMultiTouch = new List<IGestureInterpreter>();
		public readonly Vector2 Position;
		public bool PreventPropagation { get; private set; }

		public GDTIMTouchAction(Vector2 position)
		{
			Position = position;
		}

		// todo: interrogate need for PreventPropagation - does "consuming" these events in typical godot 
		// fashion achieve this?
		public void AcceptGestures(IGestureInterpreter node, bool subscribeToMultiTouch, bool preventPropagation)
		{
			if (PreventPropagation)
				return;
			
			NodesTouched.Add(node);
			
			if(subscribeToMultiTouch)
				NodesConsumingMultiTouch.Add(node);

			PreventPropagation = preventPropagation;
		}
	}
	
	public class TouchBegin : GDTIMTouchAction
	{
		public TouchBegin(Vector2 position) : base(position){}
	}
	
	public class TouchEnd : GDTIMTouchAction
	{
		public readonly bool Cancelled;
		
		public TouchEnd(Vector2 position, bool cancelled) : base(position)
		{
			Cancelled = cancelled;
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
		internal HashSet<IGestureInterpreter> NodesTouched;

		public MultiTouch(HashSet<IGestureInterpreter> touchers, Vector2 position, int fingers)
		{
			NodesTouched = touchers;
			Fingers = fingers;
			Position = position;
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
}