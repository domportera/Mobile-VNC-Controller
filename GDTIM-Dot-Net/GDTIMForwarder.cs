using Godot;
using System.Collections.Generic;
using GDTIMDotNet;

public class GDTIMForwarder : Node
{
    // todo: needs some kind of claiming system that roughly follows godot's input idioms
    
    static readonly List<IGestureInterpreter> GestureInterpreters = new List<IGestureInterpreter>();
    
    public static void Register(IGestureInterpreter interpreter)
    {
        GestureInterpreters.Add(interpreter);
    }

    public static bool Unregister(IGestureInterpreter interpreter)
    {
        return GestureInterpreters.Remove(interpreter);
    }
    
    public virtual void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
    {
        var args = new SingleTouchArgs(position, false, cancelled);
        foreach(var g in GestureInterpreters)
            g.OnSingleTouch(args);
    }

    public virtual void OnSingleDrag(Vector2 position, Vector2 relative)
    {
        var args = new SingleDragArgs(position, relative);
        foreach(var g in GestureInterpreters)
            g.OnSingleDrag(args);
    }

    public virtual void OnSingleLongPress(Vector2 position)
    {
        var args = new SingleTapArgs(position);
        foreach(var g in GestureInterpreters)
            g.OnSingleLongPress(args);
    }

    public virtual void OnSingleSwipe(Vector2 position, Vector2 relative)
    {
        var args = new SingleDragArgs(position, relative);
        foreach(var g in GestureInterpreters)
            g.OnSingleSwipe(args);
    }

    public virtual void OnSingleTap(Vector2 position)
    {
        var args = new SingleTapArgs(position);
        foreach(var g in GestureInterpreters)
            g.OnSingleTap(args);
    }

    public virtual void OnTwist(Vector2 position, float relative, int fingers)
    {
        var args = new TwistArgs(position, relative, fingers);
        foreach(var g in GestureInterpreters)
            g.OnTwist(args);
    }

    public virtual void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDragArgs(position, relative, fingers);
        foreach(var g in GestureInterpreters)
            g.OnMultiDrag(args);
    }

    public virtual void OnMultiLongPress(Vector2 position, int fingers)
    {
        var args = new MultiTapArgs(position, fingers);
        foreach(var g in GestureInterpreters)
            g.OnMultiLongPress(args);
    }

    public virtual void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDragArgs(position, relative, fingers);
        foreach(var g in GestureInterpreters)
            g.OnMultiSwipe(args);
    }

    public virtual void OnMultiTap(Vector2 position, int fingers)
    {
        var args = new MultiTapArgs(position, fingers);
        foreach(var g in GestureInterpreters)
            g.OnMultiTap(args);
    }

    public virtual void OnPinch(Vector2 position, float relative, float distance, int fingers)
    {
        var args = new PinchArgs(position, relative, distance, fingers);
        foreach (var g in GestureInterpreters)
            g.OnPinch(args);
    }
    
    
}
