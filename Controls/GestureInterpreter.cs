using System;
using GDTIMDotNet;
using Godot;
using GodotExtensions;

public abstract class GestureInterpreter : Node
{
    public event EventHandler<SingleTouchArgs> SingleTouch;
    public event EventHandler<Vector2> SingleTap;
    public event EventHandler<SingleDragArgs> SingleDrag;
    public event EventHandler<Vector2> SingleLongPress;
    public event EventHandler<SingleDragArgs> SingleSwipe;
    public event EventHandler<MultiDragArgs> MultiDrag;
    public event EventHandler<MultiDragArgs> MultiSwipe;
    public event EventHandler<MultiTapArgs> MultiTap;
    public event EventHandler<MultiTapArgs> MultiLongPress;
    public event EventHandler<PinchArgs> Pinch;
    public event EventHandler<TwistArgs> Twist;
    
    public virtual void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
    {
        SingleTouch?.Invoke(this, new SingleTouchArgs(position, false, cancelled));
    }

    public virtual void OnSingleDrag(Vector2 position, Vector2 relative)
    {
        SingleDrag?.Invoke(this, new SingleDragArgs(position, relative));
    }

    public virtual void OnSingleLongPress(Vector2 position)
    {
        SingleLongPress?.Invoke(this, position);
    }

    public virtual void OnSingleSwipe(Vector2 position, Vector2 relative)
    {
        SingleSwipe?.Invoke(this, new SingleDragArgs(position, relative));
    }

    public virtual void OnSingleTap(Vector2 position)
    {
        SingleTap?.Invoke(this, position);
    }

    public virtual void OnTwist(Vector2 position, float relative, int fingers)
    {
        Twist?.Invoke(this, new TwistArgs(position, relative, fingers));
    }

    public virtual void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
    {
        MultiDrag?.Invoke(this, new MultiDragArgs(position, relative, fingers));
    }
    public virtual void OnMultiLongPress(Vector2 position, int fingers)
    {
        MultiLongPress?.Invoke(this, new MultiTapArgs(position, fingers));
    }

    public virtual void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
    {
        MultiSwipe?.Invoke(this, new MultiDragArgs(position, relative, fingers));
    }

    public virtual void OnMultiTap(Vector2 position, int fingers)
    {
        MultiTap?.Invoke(this, new MultiTapArgs(position, fingers));
    }
    public virtual void OnPinch(Vector2 position, float relative, float distance, int fingers)
    {
        Pinch?.Invoke(this, new PinchArgs(position, relative, distance, fingers));
    }
}