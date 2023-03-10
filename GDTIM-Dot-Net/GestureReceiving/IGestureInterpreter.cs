using System;
using Godot;

namespace GDTIMDotNet.GestureReceiving
{
    public interface IGestureInterpreter
    {
        event EventHandler<TouchBegin> TouchBegin;
        event EventHandler<Touch> TouchEnd;
        event EventHandler<Touch> SingleTap;
        event EventHandler<Touch> SingleDrag;
        event EventHandler<Touch> SingleLongPress;
        event EventHandler<Touch> SingleSwipe;
        event EventHandler<MultiDrag> MultiDrag;
        event EventHandler<MultiDrag> MultiSwipe;
        event EventHandler<MultiTap> MultiTap;
        event EventHandler<MultiTap> MultiLongPress;
        event EventHandler<Pinch> Pinch;
        event EventHandler<Twist> Twist;

        void OnTouchBegin(TouchBegin args);
        void OnTouchEnd(Touch args);

        void OnSingleDrag(Touch args);
        
        void OnSingleTap(Touch args);

        void OnSingleLongPress(Touch args);

        void OnSingleSwipe(Touch args);

        void OnTwist(Twist args);

        void OnMultiDrag(MultiDrag args);

        void OnMultiLongPress(MultiTap args);

        void OnMultiSwipe(MultiDrag args);

        void OnMultiTap(MultiTap args);

        void OnPinch(Pinch args);

        void SubscribeToGestures(IGestureConsumer consumer);
    }
}