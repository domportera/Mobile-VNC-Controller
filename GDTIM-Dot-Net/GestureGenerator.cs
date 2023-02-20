#define ERROR_CHECK_GDTIM

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace GDTIMDotNet
{
    internal partial class GestureGenerator
    {
        const double TapTime = 0.4f;
        const double LongPressTime = 0.6;
        const float Pi = (float)Math.PI;
        
        // the amount of imperfection of drag directions considered to be dragging in the same direction, thus forming multi-drags
        const float DragDirectionThreshold = Pi / 8f;

        // the amount of imperfection allowed in what is considered a pinch
        const float PinchAngleThreshold = Pi / 6f;
        
        // imperfection range for detecting touches with opposite directions
        static readonly Vector2 PinchAngleThresholdRange = new Vector2(Pi - PinchAngleThreshold/2f, Pi + PinchAngleThreshold/2f);
        
        // touches going in this direction should be growing closer/separating by at least their total movement deltas * (1 - PinchWiggleRoom). Or else it's a twist
        const float PinchWiggleRoom = 0.2f; 

        readonly Dictionary<Touch, GestureCalculator> _calculatorsByTouch = new Dictionary<Touch, GestureCalculator>();
        readonly HashSet<GestureCalculator> _gestureCalculators = new HashSet<GestureCalculator>();
        readonly List<GestureCalculator> _recycledGestureCalculators = new List<GestureCalculator>();
        readonly HashSet<Touch> _longPresses = new HashSet<Touch>();
        
        const int LiftTimeMs = 150;
        const float DragThresholdMm = 2f;
        const float SwipeSpeedThreshold = 1f;
        const float TouchAcceptDistanceCm = 7;
        const MultiGestureInterpretationType MultiGestureInterpretationMode = MultiGestureInterpretationType.RaiseAll;
        enum MultiGestureInterpretationType {IgnoreOddMenOut, IgnoreOddMenOutButSendThemAnyway, RaiseAll}

        public event EventHandler<Touch> LongPress;
        public event EventHandler<Touch> SingleDrag;
        public event EventHandler<IReadOnlyCollection<Touch>> MultiDrag;
        public event EventHandler<IReadOnlyCollection<Touch>> RawMultiDrag;

        public GestureGenerator(MultiTouch touchProvider, IGestureReceiver receiver)
        {
            touchProvider.TouchAdded += OnTouchAdded;
            touchProvider.TouchRemoved += OnTouchRemoved;
            touchProvider.AfterInput += OnInput;
        }

        public void OnInput(object sender, EventArgs e)
        {
            // todo: parse multidrags for directions that agree with eachother, pinch, pinch, twist, etc here instead
            foreach (GestureCalculator g in _gestureCalculators)
            {
                int dragCount = g.DraggingTouches.Count;
                if (dragCount== 0) continue;
                if (dragCount == 1)
                {
                    SingleDrag.Invoke(this, g.DraggingTouches.First());
                    continue;
                }

                InterpretMultiDrag(g.DraggingTouches);
            }
        }

        void InterpretMultiDrag(IReadOnlyCollection<Touch> draggingTouches)
        {
            
            RawMultiDrag.Invoke(this, draggingTouches);

            for (int i = 0; i < draggingTouches.Count; i++)
            {
                for (int j = i + 1; j < draggingTouches.Count; j++)
                {
                    
                }
            }
        }

        void OnTouchRemoved(object sender, Touch touch)
        {
            bool alreadyRemovedFromGesture = _longPresses.Remove(touch);
            var gesture = _calculatorsByTouch[touch];
            _calculatorsByTouch.Remove(touch);

            if (alreadyRemovedFromGesture)
                return;

            gesture.RemoveTouch(touch, true);
        }

        void OnTouchAdded(object sender, Touch touch)
        {
            GestureCalculator gesture = GetGesture(touch);

            if (gesture is null)
            {
                GestureCalculator nextCalculator;
                int calculatorsAvailable = _recycledGestureCalculators.Count;
                if (calculatorsAvailable > 0)
                {
                    nextCalculator = _recycledGestureCalculators.Last();
                    _recycledGestureCalculators.RemoveAt(calculatorsAvailable - 1);
                }
                else
                {
                    nextCalculator = CreateNewGestureCalculator(touch);
                }
                
                _gestureCalculators.Add(nextCalculator);
            }
            else
            {
                gesture.AddTouch(touch);
            }
            
            _calculatorsByTouch.Add(touch, gesture);
        }

        GestureCalculator CreateNewGestureCalculator(Touch touch)
        {
            var newGesture = new GestureCalculator(touch);
            newGesture.LongPress += OnLongPress;
            newGesture.GestureEnded += OnGestureEnded;
            newGesture.SingleTap += OnSingleTap;
            newGesture.SingleSwipe += OnSingleSwipe;
            newGesture.SingleDrag += OnSingleDrag;
            newGesture.MultiTap += OnMultiTap;
            newGesture.MultiSwipe += OnMultiSwipe;
            newGesture.MultiLongPress += OnMultiLongPress;
            newGesture.MultiDrag += OnMultiDrag;
            return newGesture;
        }

        void OnMultiLongPress(object sender, IReadOnlyList<Touch> e)
        {
            throw new NotImplementedException();
        }

        void OnMultiDrag(object sender, IReadOnlyCollection<Touch> e)
        {
            throw new NotImplementedException();
        }

        void OnMultiSwipe(object sender, IReadOnlyList<Touch> e)
        {
            throw new NotImplementedException();
        }

        void OnMultiTap(object sender, IReadOnlyList<Touch> e)
        {
            throw new NotImplementedException();
        }

        void OnSingleDrag(object sender, Touch e)
        {
            throw new NotImplementedException();
        }

        void OnSingleSwipe(object sender, Touch e)
        {
            throw new NotImplementedException();
        }

        void OnSingleTap(object sender, Touch e)
        {
            throw new NotImplementedException();
        }

        void OnGestureEnded(object sender, EventArgs e)
        {
            GestureCalculator calculator = (GestureCalculator)sender;
            _gestureCalculators.Remove(calculator);
            _recycledGestureCalculators.Add(calculator);
        }

        void OnLongPress(object sender, Touch e)
        {
            _longPresses.Add(e);
            LongPress.Invoke(this, e);
        }

        GestureCalculator GetGesture(Touch touch)
        {
            Vector2 positionCm = touch.PositionCm;

            GestureCalculator nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (GestureCalculator gesture in _gestureCalculators)
            {
                bool accepted = gesture.AssessTouchPosition(positionCm, out float distanceCm);
                if (!accepted) continue;
                if (distanceCm >= nearestDistance) continue;
                
                nearest = gesture;
                nearestDistance = distanceCm;
            }

            return nearest;
        }
    }
}