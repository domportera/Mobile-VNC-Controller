using System;
using GDTIMDotNet.GestureGeneration;
using Godot;

namespace GDTIMDotNet.GestureReceiving
{
    public interface IGestureInterpreter
    {
        event EventHandler<Touch> TouchBegin;
        event EventHandler<Touch> TouchEnd;
        event EventHandler<Touch> SingleTap;
        event EventHandler<Touch> SingleDrag;
        event EventHandler<Touch> SingleLongPress;
        event EventHandler<Touch> SingleSwipe;
        event EventHandler<MultiDragData> MultiDrag;
        event EventHandler<MultiDragData> MultiSwipe;
        event EventHandler<MultiTapData> MultiTap;
        event EventHandler<MultiTapData> MultiLongPress;
        event EventHandler<PinchData> Pinch;
        event EventHandler<TwistData> Twist;

        void OnTouchBegin(Touch args);
        void OnTouchEnd(Touch args);

        void OnSingleDrag(Touch args);
        
        void OnSingleTap(Touch args);

        void OnSingleLongPress(Touch args);

        void OnSingleSwipe(Touch args);

        void OnTwist(TwistData args);

        void OnMultiDrag(MultiDragData args);

        void OnMultiLongPress(MultiLongPressData args);

        void OnMultiSwipe(MultiSwipeData args);

        void OnMultiTap(MultiTapData args);

        void OnPinch(PinchData args);

        void SubscribeToGestures(IGestureConsumer consumer);
    }
}