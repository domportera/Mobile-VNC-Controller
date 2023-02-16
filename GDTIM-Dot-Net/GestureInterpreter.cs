using System;
using Godot;

namespace GDTIMDotNet
{
    public class GestureInterpreter : Node, IGestureInterpreter
    {
        [Export] bool _consumeMultiTouchOnSingleTouch = false;
        [Export] bool _preventPropagation = false;
        [Export] GestureInputMode _inputMode = GestureInputMode.UnhandledInput;
        public event EventHandler<TouchBegin> TouchBegin;
        public event EventHandler<TouchEnd> TouchEnd;
        public event EventHandler<SingleTap> SingleTap;
        public event EventHandler<SingleDrag> SingleDrag;
        public event EventHandler<SingleTap> SingleLongPress;
        public event EventHandler<SingleDrag> SingleSwipe;
        public event EventHandler<MultiDrag> MultiDrag;
        public event EventHandler<MultiDrag> MultiSwipe;
        public event EventHandler<MultiTap> MultiTap;
        public event EventHandler<MultiTap> MultiLongPress;
        public event EventHandler<Pinch> Pinch;
        public event EventHandler<Twist> Twist;

        enum GestureInputMode {Input, UnhandledInput}

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);

            if (_inputMode != GestureInputMode.Input)
                return;
            
            AcceptGestures(@event);
        }
        
        public override void _UnhandledInput(InputEvent @event)
        {
            base._UnhandledInput(@event);
            
            if (_inputMode != GestureInputMode.UnhandledInput)
                return;
            
            AcceptGestures(@event);
        }

        void AcceptGestures(InputEvent input)
        {
            switch (input)
            {
                case TouchBegin touchBegin:
                    touchBegin.AcceptGesturesNode(this, _consumeMultiTouchOnSingleTouch, _preventPropagation);
                    return;
                case MultiTouch multiTouchBegin:
                    multiTouchBegin.AcceptGestures(this);
                    break;
            }
        }

        public virtual void OnTouchBegin(TouchBegin args)
        {
            TouchBegin?.Invoke(this, args);
        }

        public void OnTouchEnd(TouchEnd args)
        {
            TouchEnd?.Invoke(this, args);
        }

        public virtual void OnSingleDrag(SingleDrag args)
        {
            SingleDrag?.Invoke(this, args);
        }
        
        public virtual void OnSingleLongPress(SingleTap args)
        {
            SingleLongPress?.Invoke(this, args);
        }

        public virtual void OnSingleSwipe(SingleDrag args)
        {
            SingleSwipe?.Invoke(this, args);
        }

        public virtual void OnSingleTap(SingleTap args)
        {
            SingleTap?.Invoke(this, args);
        }

        public virtual void OnTwist(Twist args)
        {
            Twist?.Invoke(this, args);
        }

        public virtual void OnMultiDrag(MultiDrag args)
        {
            MultiDrag?.Invoke(this, args);
        }

        public virtual void OnMultiLongPress(MultiTap args)
        {
            MultiLongPress?.Invoke(this, args);
        }

        public virtual void OnMultiSwipe(MultiDrag args)
        {
            MultiSwipe?.Invoke(this, args);
        }

        public virtual void OnMultiTap(MultiTap args)
        {
            MultiTap?.Invoke(this, args);
        }

        public virtual void OnPinch(Pinch args)
        {
            Pinch?.Invoke(this, args);
        }
        
        public void SubscribeToGestures(IGestureConsumer consumer)
        {
            MultiDrag += consumer.OnMultiDrag;
            MultiSwipe += consumer.OnMultiSwipe;
            Pinch += consumer.OnPinch;
            TouchBegin += consumer.OnSingleTouch;
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