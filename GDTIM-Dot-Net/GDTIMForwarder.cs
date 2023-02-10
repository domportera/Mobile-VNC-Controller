using Godot;
using System.Collections.Generic;
using GDTIMDotNet;

public class GDTIMForwarder : Node, IGestureReceiver
{
    // this currently does not support multiple concurrent gestures. this class
    // should probably be made un-static and be used each time the base
    // system tracks an individual gesture
    // this may require fat finger width set? it might double up "single drag events"? 
    
    List<IGestureInterpreter> _singleInterpreters = new List<IGestureInterpreter>();
    List<IGestureInterpreter> _singleInterpretersConsumingMultiTouch = new List<IGestureInterpreter>();
    HashSet<IGestureInterpreter> _multiInterpreters;

    // if true, single interpreters that choose to consume multiTouch
    // will claim them all as their own.
    // this can cause issues where only the first finger's touch
    // determine if receiving nodes get multitouch events
    readonly bool _exclusiveMultiTouch = false;

    bool _cancelSingleTouchOnMultiTouch = false;
    bool _cancelled = false;
    
    //state variable
    bool _checkedForMultiInterpreters = false;
    
    public virtual void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
    {
        if (cancelled)
        {
            if (_cancelSingleTouchOnMultiTouch)
            {
                // maybe we want to raise a separate OnSingleChangedToMultiTouch event instead?
                EndSingleTouch(position, cancelled: true);
            }
            
            return;
        }
        
        if (pressed)
        {
            BeginSingleTouch(position);
        }
        else
        {
            EndSingleTouch(position, cancelled: false);
            _singleInterpretersConsumingMultiTouch.Clear();
            _checkedForMultiInterpreters = false;
        }
    }
    void BeginSingleTouch(Vector2 position)
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

    void EndSingleTouch(Vector2 position, bool cancelled)
    {
        var args = new TouchEnd(position, cancelled);

        foreach (IGestureInterpreter interpreter in _singleInterpreters)
        {
            interpreter.OnTouchEnd(args);
        }

        _singleInterpreters.Clear();
    }

    void PropagateMultiTouch(MultiTouch args)
    {
        if (_checkedForMultiInterpreters) 
            return;
        if (_exclusiveMultiTouch && _singleInterpretersConsumingMultiTouch.Count > 0) 
            return;
        
        Input.ParseInputEvent(args);
        _checkedForMultiInterpreters = true;
    }


    #region Single Touch

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

        PropagateMultiTouch(args);
        
        foreach(IGestureInterpreter g in _singleInterpretersConsumingMultiTouch)
            g.OnTwist(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnTwist(args);
    }

    public virtual void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        PropagateMultiTouch(args);
        
        foreach(IGestureInterpreter g in _singleInterpretersConsumingMultiTouch)
            g.OnMultiDrag(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiDrag(args);
    }

    public virtual void OnMultiLongPress(Vector2 position, int fingers)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        PropagateMultiTouch(args);
        
        foreach(var g in _singleInterpretersConsumingMultiTouch)
            g.OnMultiLongPress(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiLongPress(args);
    }

    public virtual void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        PropagateMultiTouch(args);
        
        foreach(IGestureInterpreter g in _singleInterpretersConsumingMultiTouch)
            g.OnMultiSwipe(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiSwipe(args);
    }

    public virtual void OnMultiTap(Vector2 position, int fingers)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        PropagateMultiTouch(args);
        
        foreach(IGestureInterpreter g in _singleInterpretersConsumingMultiTouch)
            g.OnMultiTap(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiTap(args);
    }

    public virtual void OnPinch(Vector2 position, float relative, float distance, int fingers)
    {
        var args = new Pinch(_multiInterpreters, position, relative, distance, fingers);
        
        PropagateMultiTouch(args);
        
        foreach (IGestureInterpreter g in _singleInterpretersConsumingMultiTouch)
            g.OnPinch(args);
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnPinch(args);
    }
    
}
