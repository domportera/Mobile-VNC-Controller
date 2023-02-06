using System;
using GDTIMDotNet;
using Godot;
using GodotExtensions;

namespace GDTIMDotNet
{
    public class GestureInterpreter : Node, IGestureInterpreter
    {
        public event EventHandler<SingleTouchArgs> SingleTouch;
        public event EventHandler<SingleTapArgs> SingleTap;
        public event EventHandler<SingleDragArgs> SingleDrag;
        public event EventHandler<SingleTapArgs> SingleLongPress;
        public event EventHandler<SingleDragArgs> SingleSwipe;
        public event EventHandler<MultiDragArgs> MultiDrag;
        public event EventHandler<MultiDragArgs> MultiSwipe;
        public event EventHandler<MultiTapArgs> MultiTap;
        public event EventHandler<MultiTapArgs> MultiLongPress;
        public event EventHandler<PinchArgs> Pinch;
        public event EventHandler<TwistArgs> Twist;

        public virtual void OnSingleTouch(SingleTouchArgs args)
        {
            SingleTouch?.Invoke(this, args);
        }

        public virtual void OnSingleDrag(SingleDragArgs args)
        {
            SingleDrag?.Invoke(this, args);
        }

        public virtual void OnSingleLongPress(SingleTapArgs args)
        {
            SingleLongPress?.Invoke(this, args);
        }

        public virtual void OnSingleSwipe(SingleDragArgs args)
        {
            SingleSwipe?.Invoke(this, args);
        }

        public virtual void OnSingleTap(SingleTapArgs args)
        {
            SingleTap?.Invoke(this, args);
        }

        public virtual void OnTwist(TwistArgs args)
        {
            Twist?.Invoke(this, args);
        }

        public virtual void OnMultiDrag(MultiDragArgs args)
        {
            MultiDrag?.Invoke(this, args);
        }

        public virtual void OnMultiLongPress(MultiTapArgs args)
        {
            MultiLongPress?.Invoke(this, args);
        }

        public virtual void OnMultiSwipe(MultiDragArgs args)
        {
            MultiSwipe?.Invoke(this, args);
        }

        public virtual void OnMultiTap(MultiTapArgs args)
        {
            MultiTap?.Invoke(this, args);
        }

        public virtual void OnPinch(PinchArgs args)
        {
            Pinch?.Invoke(this, args);
        }
        
        public void SubscribeToGestures(IGestureConsumer consumer)
        {
            MultiDrag += consumer.OnMultiDrag;
            MultiSwipe += consumer.OnMultiSwipe;
            Pinch += consumer.OnPinch;
            SingleTouch += consumer.OnSingleTouch;
            MultiTap += consumer.OnMultiTap;
            SingleTap += consumer.OnSingleTap;
            SingleDrag += consumer.OnSingleDrag;
            SingleLongPress += consumer.OnSingleLongPress;
            MultiLongPress += consumer.OnMultiLongPress;
            Twist += consumer.OnTwist;
            SingleSwipe += consumer.OnSingleSwipe;
        }
    }
}