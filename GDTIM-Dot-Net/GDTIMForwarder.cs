using Godot;
using System.Collections.Generic;
using System.Linq;
using GDTIMDotNet;
using GDTIMDotNet.GestureGeneration;
using GDTIMDotNet.GestureReceiving;
using GodotExtensions;

public class GDTIMForwarder : Node, IGestureReceiver
{
    // todo:  re-work how nodes claim touches and how the Forwarder splits up gestures based on the claims. If a multitouch in but a node claimed one of those
    // touches, do we split the multitouch/drag into a single touch + (multiTouch or singletouch)?

    // todo: raw version of each gesture that is unclaimed, fruit ninja style with typical godot bindings
    
    readonly Dictionary<Touch, HashSet<IGestureInterpreter>> _claimedTouches = new Dictionary<Touch, HashSet<IGestureInterpreter>>();
    readonly Dictionary<IGestureInterpreter, HashSet<Touch>> _touchesClaimedByInterpreters = new Dictionary<IGestureInterpreter, HashSet<Touch>>();
    readonly HashSet<Touch> _unclaimedTouches = new HashSet<Touch>();
    
    readonly Stack<HashSet<IGestureInterpreter>> _recycledInterpreterCollections = new Stack<HashSet<IGestureInterpreter>>();
    readonly Stack<HashSet<Touch>> _recycledTouchCollections = new Stack<HashSet<Touch>>();
    
    public void OnSingleTouch(object sender, TouchData touchData)
    {
        Touch touch = touchData.Touch;
        _unclaimedTouches.Add(touch);
        if (touchData.Pressed)
        {
            BeginSingleTouch(touch);
        }
        else
        {
            // todo: delay endsingletouch if it's in use by a multi-gesture?
            EndSingleTouch(touch);
        }
    }
    void BeginSingleTouch(Touch touch)
    {
        var args = new TouchBegin(touch);
        args.Accepted += OnSingleTouchAccepted;
        
        bool recycledAvailable = _recycledInterpreterCollections.Count > 0;

        HashSet<IGestureInterpreter> collection =
            recycledAvailable ? _recycledInterpreterCollections.Pop() : new HashSet<IGestureInterpreter>();
        
        _claimedTouches.Add(touch, collection);
        
        Input.ParseInputEvent(args); //unfortunately limited to not triggering _GuiInput /:
    }

    void OnSingleTouchAccepted(object sender, NiceTouchAction.TouchBeginEventArgs args)
    {
        var thisEvent = (TouchBegin)sender;
        Touch touch = thisEvent.Touch;
        _unclaimedTouches.Remove(touch);

        HashSet<IGestureInterpreter> claimers = _claimedTouches[touch];

        claimers.Add(args.Interpreter);
        args.Interpreter.OnTouchBegin((TouchBegin)sender);
    }

    void EndSingleTouch(Touch touch)
    {
        HashSet<IGestureInterpreter> claimers = _claimedTouches[touch];
        foreach (IGestureInterpreter interpreter in claimers)
        {
            interpreter.OnTouchEnd(touch);
        }

        claimers.Clear();
        _claimedTouches.Remove(touch);
        _recycledInterpreterCollections.Push(claimers);
        _unclaimedTouches.Add(touch);
    }


    #region Single Touch

    public void OnSingleDrag(object sender, Touch touch)
    {
        HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
        foreach(var g in touchers)
            g.OnSingleDrag(touch);
    }

    public void OnSingleLongPress(object sender, Touch touch)
    {
        HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
        foreach(var g in touchers)
            g.OnSingleTap(touch);
    }

    public void OnSingleSwipe(object sender, Touch touch)
    {
        HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
        foreach(var g in touchers)
            g.OnSingleSwipe(touch);
    }

    public void OnSingleTap(object sender, Touch touch)
    {   
        HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
        foreach(var g in touchers)
            g.OnSingleTap(touch);
    }
    
    #endregion Single Touch

    #region Multi Touch
    
    void BeginMultiTouch(MultiTouchBegin args)
    {
        _checkedForMultiInterpreters = true;
        
        Input.ParseInputEvent(args);
    }

    void DistributeMultiTouch<T>(ref T gesture) where T : IMultiFingerGesture// todo: `in` keyword in c# 7
    {
        
    }
    void DistributeTwoFingerTouch<T>(ref T gesture) where T : ITwoFingerGesture // todo: `in` keyword in c# 7
    {
        
    }
    
    public void OnTwist(object sender, TwistData twistData)
    {
        var args = new Twist(_multiInterpreters, position, relative, fingers);
        DistributeTwoFingerTouch(ref twistData);
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnTwist(args);
    }

    public void OnMultiDrag(object sender, MultiDragData multiDragData)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiDrag(args);
    }

    public void OnMultiLongPress(object sender, MultiLongPressData multiLongPressData)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiLongPress(args);
    }

    public void OnMultiSwipe(object sender, MultiSwipeData multiSwipeData)
    {
        var args = new MultiDrag(_multiInterpreters, position, relative, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiSwipe(args);
    }

    public void OnMultiTap(object sender, MultiTapData multiTapData)
    {
        var args = new MultiTap(_multiInterpreters, position, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnMultiTap(args);
    }

    public void OnPinch(object sender, PinchData pinchData)
    {
        var args = new Pinch(_multiInterpreters, position, relative, distance, fingers);
        
        BeginMultiTouch(args);
        
        foreach (IGestureInterpreter g in _multiInterpreters)
            g.OnPinch(args);
    }
    #endregion Multi Touch

    public void OnRawMultiDrag(object sender, RawMultiDragData e)
    {
        throw new System.NotImplementedException();
    }

    public void OnRawPinchTwist(object sender, RawTwoFingerDragData e)
    {
        throw new System.NotImplementedException();
    }
}
