using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GDTIMDotNet;
using GDTIMDotNet.GestureGeneration;
using GDTIMDotNet.GestureReceiving;
using GodotExtensions;

namespace GDTIMDotNet
{
    public partial class NiceTouchForwarder : Node, IGestureReceiver
    {
        // todo: raw version of each gesture that is unclaimed, fruit ninja style with typical godot bindings

        readonly Dictionary<Touch, HashSet<IGestureInterpreter>> _claimedTouches =
            new Dictionary<Touch, HashSet<IGestureInterpreter>>();

        readonly Dictionary<IGestureInterpreter, HashSet<Touch>> _touchesClaimedByInterpreters =
            new Dictionary<IGestureInterpreter, HashSet<Touch>>();

        readonly HashSet<Touch> _unclaimedTouches = new HashSet<Touch>();

        readonly Stack<HashSet<IGestureInterpreter>> _recycledInterpreterCollections =
            new Stack<HashSet<IGestureInterpreter>>();

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
        
        public void OnSingleDrag(object sender, Touch touch)
        {
            HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
            foreach (var g in touchers)
                g.OnSingleDrag(touch);
        }

        public void OnSingleLongPress(object sender, Touch touch)
        {
            HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
            foreach (var g in touchers)
                g.OnSingleTap(touch);
        }

        public void OnSingleSwipe(object sender, Touch touch)
        {
            HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
            foreach (var g in touchers)
                g.OnSingleSwipe(touch);
        }

        public void OnSingleTap(object sender, Touch touch)
        {
            HashSet<IGestureInterpreter> touchers = _claimedTouches[touch];
            foreach (var g in touchers)
                g.OnSingleTap(touch);
        }
        
        public void OnTwist(object sender, TwistData twistData)
        {
            FilterTouches(twistData, 
                out List<IGestureInterpreter> fullReceivers, 
                out List<InterpreterWithTouches> partialReceivers);
            
            foreach (InterpreterWithTouches ut in partialReceivers)
            {
                // handle partial gesture
            }

            if (fullReceivers.Count == 0)
                return;
            
            var args = new Twist(ref twistData);
            BeginMultiTouch(args);
            foreach (IGestureInterpreter g in fullReceivers)
            {
                g.OnTwist(args);
            }
        }


        public void OnPinch(object sender, PinchData pinchData)
        {
            var args = new Pinch(_multiInterpreters, position, relative, distance, fingers);

            BeginMultiTouch(args);

            foreach (IGestureInterpreter g in _multiInterpreters)
                g.OnPinch(args);
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
        
        public void OnRawMultiDrag(object sender, RawMultiDragData e)
        {
            throw new System.NotImplementedException();
        }

        public void OnRawPinchTwist(object sender, RawTwoFingerDragData e)
        {
            throw new System.NotImplementedException();
        }
    }
}