using System.Collections.Generic;
using GDTIMDotNet;
using GDTIMDotNet.GestureGeneration;
using GDTIMDotNet.GestureReceiving;
using Godot;

public class TouchBegin : NiceTouchAction
{
	public Touch Touch { get; }
	public TouchBegin(Touch touch) : base(touch.Position)
	{
		Touch = touch;
	}
}

public class MultiTouchBegin : NiceTouchAction
{
	public int Fingers => Touches.Count;
	public IReadOnlyCollection<Touch> Touches { get; }

	public MultiTouchBegin(IReadOnlyCollection<Touch> touches) : base(touches.Centroid())
	{
		Touches = touches;
		Action = GetType().Name;
		Pressed = true;
	}
}

public class RawMultiTouch
{
	public IReadOnlyCollection<Touch> Touches { get; }
	public RawMultiTouch(IReadOnlyCollection<Touch> touches)
	{
		Touches = touches;
	}
}

public class Pinch : RawMultiTouch
{
	public readonly float Relative, Distance;

	public Pinch(HashSet<IGestureInterpreter> touchers, Vector2 position,
		float relative, float distance, int fingers) : base(touchers, position, fingers)
	{
		Relative = relative;
		Distance = distance;
	}
}

public class Twist : RawMultiTouch
{
	public readonly float Relative;

	public Twist(HashSet<IGestureInterpreter> touchers, Vector2 position, float relative, int fingers) :
		base(touchers, position, fingers)
	{
		Relative = relative;
	}
}

public class MultiTap : RawMultiTouch
{
	public MultiTap(HashSet<IGestureInterpreter> touchers, Vector2 position, int fingers):
		base(touchers, position, fingers){}
}

public class MultiDrag : RawMultiTouch
{
	public readonly Vector2 Relative;
	public MultiDrag(HashSet<IGestureInterpreter> touchers, Vector2 position, Vector2 relative, int fingers) 
		: base (touchers, position, fingers)
	{
		Relative = relative;
	}
}