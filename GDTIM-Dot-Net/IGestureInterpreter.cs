using System;
using Godot;

namespace GDTIMDotNet
{
    public interface IGestureInterpreter
    {
        event EventHandler<TouchBegin> TouchBegin;
        event EventHandler<TouchEnd> TouchEnd;
        event EventHandler<SingleTap> SingleTap;
        event EventHandler<SingleDrag> SingleDrag;
        event EventHandler<SingleTap> SingleLongPress;
        event EventHandler<SingleDrag> SingleSwipe;
        event EventHandler<MultiDrag> MultiDrag;
        event EventHandler<MultiDrag> MultiSwipe;
        event EventHandler<MultiTap> MultiTap;
        event EventHandler<MultiTap> MultiLongPress;
        event EventHandler<Pinch> Pinch;
        event EventHandler<Twist> Twist;

        void OnTouchBegin(TouchBegin args);
        void OnTouchEnd(TouchEnd args);

        void OnSingleDrag(SingleDrag args);
        
        void OnSingleTap(SingleTap args);

        void OnSingleLongPress(SingleTap args);

        void OnSingleSwipe(SingleDrag args);

        void OnTwist(Twist args);

        void OnMultiDrag(MultiDrag args);

        void OnMultiLongPress(MultiTap args);

        void OnMultiSwipe(MultiDrag args);

        void OnMultiTap(MultiTap args);

        void OnPinch(Pinch args);

        void SubscribeToGestures(IGestureConsumer consumer);
    }
}