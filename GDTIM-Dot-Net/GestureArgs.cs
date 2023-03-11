using System.Collections.Generic;
using GDTIMDotNet;
using GDTIMDotNet.GestureGeneration;
using GDTIMDotNet.GestureReceiving;
using Godot;


public class TouchAction : NiceTouchAction
{
	public Touch Touch { get; }
	public TouchAction(Touch touch) : base(touch.Position)
	{
		Touch = touch;
	}
}

public class TouchBegin : TouchAction
{
	public TouchBegin(Touch touch) : base(touch) {}
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

public class MultiLongPress : RawMultiTouch<MultiLongPressData>
{
	public MultiLongPress(ref MultiLongPressData data) : base(ref data) { }
}

public class MultiDrag : RawMultiTouch<MultiDragData>
{
	public float DirectionRadians => Data.DirectionRadians;
	public float DirectionDegrees => Data.DirectionDegrees;
	public MultiDrag(ref MultiDragData data) : base(ref data) { }
}