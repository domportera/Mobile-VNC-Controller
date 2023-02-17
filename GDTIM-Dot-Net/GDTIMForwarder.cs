using Godot;
using System.Collections.Generic;
using System.Linq;
using GDTIMDotNet;
using GodotExtensions;

public class GDTIMForwarder : Node, IGestureReceiver
{
    // this currently does not support multiple concurrent gestures. GDTIM might not either?
    // this class should probably be used each time the base
    // system tracks an individual gesture
    // this may require fat finger width set? it might double up "single drag events"? 

    Dictionary<int, HashSet<IGestureInterpreter>> _singleInterpreters = new Dictionary<int, HashSet<IGestureInterpreter>>();
    readonly HashSet<IGestureInterpreter> _multiInterpreters = new HashSet<IGestureInterpreter>();

    // if true, single interpreters that choose to consume multiTouch
    // will claim them all as their own.
    // this can cause issues where only the first finger's touch
    // determine if receiving nodes get multitouch events
    readonly bool _exclusiveMultiTouch = false;

    bool _cancelSingleTouchOnMultiTouch = false;
    bool _cancelled = false;
    
    //state variable
    bool _checkedForMultiInterpreters = false;

    RawGesture _previousSingleTouchGesture;
    public virtual void OnSingleTouch(Vector2 position, bool pressed, bool cancelled, object rawGesture)
    {
        RawGesture gesture = new RawGesture(rawGesture);
       // gesture.PrintTouchData();
        
        if (cancelled)
        {
            if (_cancelSingleTouchOnMultiTouch)
            {
                int index = gesture.Releases.First().Key;
                // maybe we want to raise a separate OnSingleChangedToMultiTouch event instead?
                EndSingleTouch(position, true, index);
            }
            
            return;
        }
        
        if (pressed)
        {
            int newestIndex = _previousSingleTouchGesture is null 
                ? gesture.Presses.First().Key 
                : gesture.Presses.Keys.Except(_previousSingleTouchGesture.Presses.Keys).First();
            
            BeginSingleTouch(position, newestIndex);
        }
        else
        {
            int index = gesture.Releases.First().Key;
            EndSingleTouch(position, false, index);
            _multiInterpreters.Clear();
            _checkedForMultiInterpreters = false;
        }
    }
    void BeginSingleTouch(Vector2 position, int index)
    {
        if (!_singleInterpreters.ContainsKey(index))
        {
            _singleInterpreters.Add(index, new HashSet<IGestureInterpreter>());
        }
        
        var args = new TouchBegin(position, index);
        args.Accepted += OnSingleTouchAccepted;
        Input.ParseInputEvent(args); //unfortunately limited to not triggering _GuiInput /:
    }

    void OnSingleTouchAccepted(object sender, TouchBegin.TouchBeginEventArgs args)
    {
        if(args.SubscribeToMultiTouch)
            _multiInterpreters.Add(args.Interpreter);

        HashSet<IGestureInterpreter> thisFingersInterpreters = _singleInterpreters[args.Index];
        thisFingersInterpreters.Add(args.Interpreter);
        
        args.Interpreter.OnTouchBegin((TouchBegin)sender);
    }

    void EndSingleTouch(Vector2 position, bool cancelled, int index)
    {
        var args = new TouchEnd(position, index, cancelled);

        foreach (IGestureInterpreter interpreter in _singleInterpreters[index])
        {
            interpreter.OnTouchEnd(args);
        }

        // TODO: clear next input event? this will create bugs as SingleTap gestures will not be processed if we clear this immediately
        _singleInterpreters[index].Clear();
    }

    void BeginMultiTouch(MultiTouch args)
    {
        if (_checkedForMultiInterpreters) 
            return;
        if (_exclusiveMultiTouch && _multiInterpreters.Count > 0) 
            return;
        
        Input.ParseInputEvent(args);
        _checkedForMultiInterpreters = true;
    }


    #region Single Touch

    public virtual void OnSingleDrag(Vector2 position, Vector2 relative, object rawGesture)
    {
        var gesture = new RawGesture(rawGesture);
        int index = gesture.GetDragIndex(position, relative);

        if (index == RawGesture.InvalidIndex)
        {
            GDLogger.Log(this, $"Invalid drag index for gesture: {gesture}");
            return;
        }
        
        var args = new SingleDrag(position, relative);
        foreach(var g in _singleInterpreters[index])
            g.OnSingleDrag(args);
    }

    public virtual void OnSingleLongPress(Vector2 position, object rawGesture)
    {
        var gesture = new RawGesture(rawGesture);
        int index = gesture.GetTouchIndex(position, true);

        if (index == RawGesture.InvalidIndex)
        {
            GDLogger.Log(this, $"Invalid long press index for gesture: {gesture}");
            return;
        }
        
        var args = new SingleTap(position);
        foreach(var g in _singleInterpreters[index])
            g.OnSingleLongPress(args);
    }

    public virtual void OnSingleSwipe(Vector2 position, Vector2 relative, object rawGesture)
    {
        var gesture = new RawGesture(rawGesture);
        int index = gesture.GetDragIndex(position, relative);

        if (index == RawGesture.InvalidIndex)
        {
            GDLogger.Log(this, $"Invalid drag index for gesture: {gesture}");
            return;
        }
        
        var args = new SingleDrag(position, relative);
        foreach(var g in _singleInterpreters[index])
            g.OnSingleSwipe(args);
    }

    public virtual void OnSingleTap(Vector2 position, object rawGesture)
    {
        var gesture = new RawGesture(rawGesture);
        int index = gesture.GetTouchIndex(position, false);

        if (index == RawGesture.InvalidIndex)
        {
            GDLogger.Log(this, $"Invalid tap index for gesture: {gesture}");
            return;
        }
        
        var args = new SingleTap(position);
        foreach(var g in _singleInterpreters[index])
            g.OnSingleTap(args);
    }
    
    #endregion Single Touch


    #region Multi Touch
    public virtual void OnTwist(Vector2 position, float relative, int fingers, object rawGesture)
    {
        var args = new Twist(_multiInterpreters, position, relative, fingers);

        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnTwist(args);
    }

    public virtual void OnMultiDrag(Vector2 position, Vector2 relative, int fingers, object rawGesture)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiDrag(args);
    }

    public virtual void OnMultiLongPress(Vector2 position, int fingers, object rawGesture)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiLongPress(args);
    }

    public virtual void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers, object rawGesture)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiSwipe(args);
    }

    public virtual void OnMultiTap(Vector2 position, int fingers, object rawGesture)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiTap(args);
    }

    public virtual void OnPinch(Vector2 position, float relative, float distance, int fingers, object rawGesture)
    {
        var args = new Pinch(_multiInterpreters, position, relative, distance, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnPinch(args);
    }
    #endregion Multi Touch
}
