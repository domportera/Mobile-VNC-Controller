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

public class RawMultiTouch<T> where T : IMultiFingerGesture
{
	protected T Data;
	public IReadOnlyList<Touch> Touches => Data.Touches;
	public int TouchCount => Data.TouchCount;
	public Vector2 Center => Data.Center;
	public Vector2 CenterRelative => Data.CenterDelta;

	protected RawMultiTouch(ref T data) // todo: `ref` to `in` in C# 7
	{
		this.Data = data;
	}
}

public class Pinch : RawMultiTouch<PinchData>
{
	public readonly float SeparationAmount;

	public Pinch(ref PinchData data) : base(ref data)
	{
		SeparationAmount = data.SeparationAmount;
	}
}

public class Twist : RawMultiTouch<TwistData>
{
	public float TwistDegrees => Data.TwistDegrees;
	public float TwistRadians => Data.TwistRadians;
	
	public Twist(ref TwistData data) : base(ref data) {}
}

public class MultiTap : RawMultiTouch<MultiTapData>
{
	public MultiTap(ref MultiTapData data) : base(ref data) { }
}

public class MultiDrag : RawMultiTouch<MultiDragData>
{
	public float DirectionRadians => Data.DirectionRadians;
	public float DirectionDegrees => Data.DirectionDegrees;
	public MultiDrag(ref MultiDragData data) : base(ref data) { }
}