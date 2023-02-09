using Godot;
using System.Collections.Generic;
using GDTIMDotNet;

public class GDTIMForwarder : Node, IGestureReceiver
{
    // this currently does not support multiple concurrent gestures. this class
    // should probably be made un-static and be used each time the base
    // system tracks an individual gesture
    // this may require fat finger width set? it might double up "single drag events"? 
    
    List<IGestureInterpreter> _singleInterpreters;
    List<IGestureInterpreter> _singleInterpretersConsumingMultiTouch;
    HashSet<IGestureInterpreter> _multiInterpreters;

    // if true, single interpreters that choose to consume multiTouch
    // will claim them all as their own.
    // this can cause issues where only the first finger's touch
    // determine if receiving nodes get multitouch events
    bool _exclusiveMultiTouch = false;

    // if this is true, we should cancel the current single touch events (clear _singleInterpreters)
    // when a multi-touch is started. is that what the cancelled bool does in OnSingleTouch??
    bool _cancelSingleTouchOnMultiTouch = false;

    #region Single Touch
    public virtual void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
    {
        if (pressed)
        {
            var args = new TouchBegin(position);
            Input.ParseInputEvent(args);
            _singleInterpreters = args.NodesTouched;
            _singleInterpretersConsumingMultiTouch = args.NodesConsumingMultiTouch;
            
            foreach (IGestureInterpreter interpreter in _singleInterpreters)
            {
                interpreter.OnTouchBegin(args);
            }
        }
        else
        {
            var args = new TouchEnd(position, cancelled);
            
            foreach (IGestureInterpreter interpreter in _singleInterpreters)
            {
                interpreter.OnTouchEnd(args);
            }

            _singleInterpreters = null;
        }
    }

    public virtual void OnSingleDrag(Vector2 position, Vector2 relative)
    {
        var args = new SingleDrag(position, relative);
        foreach(var g in _singleInterpreters)
            g.OnSingleDrag(args);
    }

    public virtual void OnSingleLongPress(Vector2 position)
    {
        var args = new SingleTap(position);
        foreach(var g in _singleInterpreters)
            g.OnSingleLongPress(args);
    }

    public virtual void OnSingleSwipe(Vector2 position, Vector2 relative)
    {
        var args = new SingleDrag(position, relative);
        foreach(var g in _singleInterpreters)
            g.OnSingleSwipe(args);
    }

    public virtual void OnSingleTap(Vector2 position)
    {
        var args = new SingleTap(position);
        foreach(var g in _singleInterpreters)
            g.OnSingleTap(args);
    }
    
    #endregion Single Touch

    // todo: if _multiInterpreters count is 0 (cleared on touch up ??), then we raise
    // an InputEventAction to gather the touchers.
    // then we call the functions of _multiInterpreters + _singleInterpretersConsumingMultiTouch
    // if _multiInterpreters count is not 0 or we already checked, then just call on _singleInterpretersConsuming
    // if none of the above, don't bother calling anything until next TouchBegin maybe?
    public virtual void OnTwist(Vector2 position, float relative, int fingers)
    {
        var args = new Twist(_multiInterpreters, position, relative, fingers);
        
        foreach(var g in _singleInterpretersConsumingMultiTouch)
            g.OnTwist(args);
        
        foreach(var g in _multiInterpreters)
            g.OnTwist(args);
    }

    public virtual void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDrag(position, relative, fingers);
        foreach(var g in _singleInterpreters)
            g.OnMultiDrag(args);
    }

    public virtual void OnMultiLongPress(Vector2 position, int fingers)
    {
        var args = new MultiTap(position, fingers);
        foreach(var g in _singleInterpreters)
            g.OnMultiLongPress(args);
    }

    public virtual void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDrag(position, relative, fingers);
        foreach(var g in _singleInterpreters)
            g.OnMultiSwipe(args);
    }

    public virtual void OnMultiTap(Vector2 position, int fingers)
    {
        var args = new MultiTap(position, fingers);
        foreach(var g in _singleInterpreters)
            g.OnMultiTap(args);
    }

    public virtual void OnPinch(Vector2 position, float relative, float distance, int fingers)
    {
        var args = new Pinch(position, relative, distance, fingers);
        foreach (var g in _singleInterpretersConsumingMultiTouch)
            g.OnPinch(args);
    }
}
