#define ERROR_CHECK_GDTIM

using System;
using System.Collections.Generic;
using System.Linq;
using GDTIMDotNet.GestureReceiving;
using Godot;

namespace GDTIMDotNet.GestureGeneration
{
    internal partial class GestureGenerator
    {
        const int LiftTimeMs = 150;
        const float DragThresholdMm = 2f;
        const float SwipeSpeedThreshold = 1f;
        const float TouchAcceptDistanceCm = 7;
        const double TapTime = 0.4f;
        const double LongPressTime = 0.6;
        const float Pi = (float)Math.PI;
        
        // the amount of imperfection of drag directions considered to be dragging in the same direction, thus forming multi-drags
        const float DragDirectionThreshold = Pi / 8f;

        // the amount of imperfection allowed in what is considered a pinch or twist
        const float OppositeAngleThreshold = Pi / 10f;
        
        // touches going in this direction should be growing closer/separating by at least their total movement deltas * PinchDirectionPrecision. Or else it's a twist
        const float PinchDirectionPrecision = 0.8f; 

        readonly Dictionary<Touch, GestureCalculator> _calculatorsByTouch = new Dictionary<Touch, GestureCalculator>();
        readonly HashSet<GestureCalculator> _gestureCalculators = new HashSet<GestureCalculator>();
        readonly List<GestureCalculator> _recycledGestureCalculators = new List<GestureCalculator>();
        readonly HashSet<Touch> _longPresses = new HashSet<Touch>();
        
        const MultiGestureInterpretationType MultiGestureInterpretationMode = MultiGestureInterpretationType.RaiseAll;
        enum MultiGestureInterpretationType {IgnoreOddMenOut, IgnoreOddMenOutButSendThemAnyway, RaiseAll}

        public event EventHandler<SingleTapData> Tap;
        public event EventHandler<SingleDragData> Drag;
        public event EventHandler<LongPressData> LongPress;
        public event EventHandler<SingleSwipeData> Swipe;
        public event EventHandler<MultiDragData> MultiDrag;
        public event EventHandler<RawMultiDragData> RawMultiDrag;
        public event EventHandler<RawPinchTwistData> RawPinchTwist;
        public event EventHandler<MultiLongPressData> MultiLongPress;
        public event EventHandler<MultiSwipeData> MultiSwipe;
        public event EventHandler<MultiTapData> MultiTap;
        public event EventHandler<PinchData> Pinch;
        public event EventHandler<TwistData> Twist;


        public GestureGenerator(MultiTouch touchProvider, IGestureReceiver receiver)
        {
            if (TapTime > LongPressTime)
            {
                throw new ArgumentOutOfRangeException(nameof(TapTime), TapTime,
                    $"Value must be less than that of {nameof(LongPressTime)}");
            }
            
            touchProvider.TouchAdded += OnTouchAdded;
            touchProvider.TouchRemoved += OnTouchRemoved;
            touchProvider.AfterInput += OnInput;
        }

        void OnInput(object sender, EventArgs e)
        {
            foreach (GestureCalculator g in _gestureCalculators)
            {
                int dragCount = g.DraggingTouches.Count;
                if (dragCount== 0) continue;
                if (dragCount == 1)
                {
                    Touch touch = g.DraggingTouches.First();
                    Drag.Invoke(this, touch);
                    continue;
                }

                // copy list since DraggingTouches is a property with a backing non-readonly list
                InterpretMultiDrag(g.DraggingTouches.ToList());
            }
        }

        enum DragRelationshipType {None, Identical, Pinch, Twist}
        void InterpretMultiDrag(IReadOnlyList<Touch> draggingTouches)
        {
            RawMultiDrag.Invoke(this, new RawMultiDragData(draggingTouches));
            
            List<Touch> touchesToConsiderForGestures = draggingTouches.ToList();
            List<Touch> multiDragTouches = new List<Touch>();
            for (int i = 0; i < touchesToConsiderForGestures.Count; i++)
            {
                Touch primaryTouch = touchesToConsiderForGestures[i];
                
                for (int j = i + 1; j < touchesToConsiderForGestures.Count; j++)
                {
                    Touch secondaryTouch = touchesToConsiderForGestures[j];
                    DragRelationshipType relationship = InterpretDragRelationship(primaryTouch, secondaryTouch,
                                                                                    out float separationAmount);

                    bool moveToNextPrimaryTouch = false;
                    switch (relationship)
                    {
                        case DragRelationshipType.None:
                            break;
                        case DragRelationshipType.Identical:
                            touchesToConsiderForGestures.Remove(secondaryTouch);
                            j--; // decrement iterator to account for removed touch
                            break;
                        case DragRelationshipType.Pinch:
                            touchesToConsiderForGestures.Remove(secondaryTouch);
                            moveToNextPrimaryTouch = true;
                            Pinch.Invoke(this, todo);
                            break;
                        case DragRelationshipType.Twist:
                            touchesToConsiderForGestures.Remove(secondaryTouch);
                            moveToNextPrimaryTouch = true;
                            Twist.Invoke(this, todo);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (moveToNextPrimaryTouch) break;
                }


                if (multiDragTouches.Count > 0)
                {
                    Touch[] copiedDragList = multiDragTouches.ToArray();
                    MultiDrag.Invoke(this, new MultiDragData(copiedDragList));
                    multiDragTouches.Clear();
                }
            }

            // todo: raise SingleDrag events for these unused touches
            foreach (Touch touch in touchesToConsiderForGestures)
            {
                Drag.Invoke(this, touch);
            }
        }

        DragRelationshipType InterpretDragRelationship(Touch primaryTouch, Touch secondaryTouch, out float separationAmount, out float twistAmount)
        {
            // todo: calculate amount of twist so receivers can get that shit
            separationAmount = 0f;
            float difference = Mathf.Abs(primaryTouch.DirectionRadians - secondaryTouch.DirectionRadians);

            bool identical = EqualDirection(difference, tolerance: DragDirectionThreshold);
            bool opposite = OppositeDirection(difference, tolerance: OppositeAngleThreshold);

            if (identical)
            {
                return DragRelationshipType.Identical;
            }
            
            if (opposite)
            {
                bool isPinch = IsPinch(primaryTouch, secondaryTouch, out separationAmount);
                return isPinch ? DragRelationshipType.Pinch : DragRelationshipType.Twist;
            }

            return DragRelationshipType.None;

        }


        bool EqualDirection(float angleDifference, float tolerance)
        {
            return angleDifference < tolerance;
        }

        bool OppositeDirection(float angleDifference, float tolerance)
        {
            float toleranceHalved = tolerance / 2f;
            return angleDifference < Pi + toleranceHalved 
                   && angleDifference > Pi - toleranceHalved;
        }

        bool IsPinch(Touch touch1, Touch touch2, out float separationAmount)
        {
            Vector2 touch1PreviousPosition = touch1.Position - touch1.PositionDelta;
            Vector2 touch2PreviousPosition = touch2.Position - touch2.PositionDelta;
            separationAmount = touch1.Position.DistanceTo(touch2.Position) - touch1PreviousPosition.DistanceTo(touch2PreviousPosition);

            // if they're moving apart or closer to eachother at nearly the same amount as the total of their speeds,
            // then we can assume it is a pinching action.
            bool isPinch = Mathf.Abs(separationAmount) > (touch1.Speed + touch2.Speed) * PinchDirectionPrecision;
            return isPinch;
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