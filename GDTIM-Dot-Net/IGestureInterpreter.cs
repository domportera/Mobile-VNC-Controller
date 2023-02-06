using System;
using Godot;

namespace GDTIMDotNet
{
    public interface IGestureInterpreter
    {
        event EventHandler<SingleTouchArgs> SingleTouch;
        event EventHandler<SingleTapArgs> SingleTap;
        event EventHandler<SingleDragArgs> SingleDrag;
        event EventHandler<SingleTapArgs> SingleLongPress;
        event EventHandler<SingleDragArgs> SingleSwipe;
        event EventHandler<MultiDragArgs> MultiDrag;
        event EventHandler<MultiDragArgs> MultiSwipe;
        event EventHandler<MultiTapArgs> MultiTap;
        event EventHandler<MultiTapArgs> MultiLongPress;
        event EventHandler<PinchArgs> Pinch;
        event EventHandler<TwistArgs> Twist;

        void OnSingleTouch(SingleTouchArgs args);

        void OnSingleDrag(SingleDragArgs args);
        
        void OnSingleTap(SingleTapArgs args);

        void OnSingleLongPress(SingleTapArgs args);

        void OnSingleSwipe(SingleDragArgs args);

        void OnTwist(TwistArgs args);

        void OnMultiDrag(MultiDragArgs args);

        void OnMultiLongPress(MultiTapArgs args);

        void OnMultiSwipe(MultiDragArgs args);

        void OnMultiTap(MultiTapArgs args);

        void OnPinch(PinchArgs args);

        void SubscribeToGestures(IGestureConsumer consumer);
    }
}