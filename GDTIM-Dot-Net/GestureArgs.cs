using System;
using System.Collections.Generic;
using GDTIMDotNet;
using Godot;

namespace Godot
{
	public abstract class GDTIMTouchAction : InputEventAction
	{
		internal List<IGestureInterpreter> NodesTouched = new List<IGestureInterpreter>();
		internal List<IGestureInterpreter> NodesConsumingMultiTouch = new List<IGestureInterpreter>();
		public readonly Vector2 Position;

		public GDTIMTouchAction(Vector2 position)
		{
			Position = position;
		}

		// todo : InputEventActions probably don't prevent propagation with OnGui etc.
		// therefore, in implementing IGestureInterpreters, they need to determine this themselves with
		// MouseFilter, etc
		public void AcceptGestures(IGestureInterpreter node, bool subscribeToMultiTouch, bool preventPropagation)
		{
			NodesTouched.Add(node);
			
			if(subscribeToMultiTouch)
				NodesConsumingMultiTouch.Add(node);
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
	public abstract class GDTIMGestureArgs : InputEventAction
	{
		public Vector2 Position { get; protected set; }

		public GDTIMGestureArgs(Vector2 position)
		{
			Position = position;
		}
	}
	
	public class SingleTap : GDTIMGestureArgs
	{
		public SingleTap(Vector2 position) : base(position) { }
	}
	

	public class SingleDrag : GDTIMGestureArgs
	{
		public readonly Vector2 Relative;
		
		public SingleDrag(Vector2 position, Vector2 relative): base(position)
		{
			Relative = relative;
		}
	}

	public abstract class MultiTouch : InputEventAction
	{
		public readonly int Fingers;
		public readonly Vector2 Position;
		internal HashSet<IGestureInterpreter> NodesTouched;

		public MultiTouch(HashSet<IGestureInterpreter> touchers, int fingers, Vector2 position)
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
			float relative, float distance, int fingers) : base(touchers, fingers, position)
		{
			Relative = relative;
			Distance = distance;
		}
	}

	public class Twist : MultiTouch
	{
		public readonly float Relative;

		public Twist(HashSet<IGestureInterpreter> touchers, Vector2 position, float relative, int fingers) :
			base(touchers, fingers, position)
		{
			Relative = relative;
		}
	}
	
	public class MultiTap : MultiTouch
	{
		public MultiTap(HashSet<IGestureInterpreter> touchers, int fingers, Vector2 position):
			base(touchers, fingers, position){}
	}
	
	public class MultiDrag : MultiTouch
	{
		public readonly Vector2 Relative;
		public MultiDrag(HashSet<IGestureInterpreter> touchers, Vector2 position, Vector2 relative, int fingers) 
			: base (touchers, fingers, position)
		{
			Relative = relative;
		}
	}
}