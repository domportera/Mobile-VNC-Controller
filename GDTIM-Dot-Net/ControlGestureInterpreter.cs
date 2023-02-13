using Godot;
using GodotExtensions;
using System;

namespace GDTIMDotNet
{
	public class ControlGestureInterpreter : Control, IGestureInterpreter
	{
		Control _control;

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

		readonly bool _controlIsThis;

		protected ControlGestureInterpreter()
		{
			_control = this;
			_controlIsThis = true;
		}

		public ControlGestureInterpreter(Control control)
		{
			_control = control;
			_controlIsThis = control == this; //always false?
		}

		public override void _Ready()
		{
			if(!_controlIsThis)
				_control.AddChild(this);
		}

		public override void _GuiInput(InputEvent @event)
		{
			base._GuiInput(@event);
			if (!_controlIsThis) return;

			InterpretTouchActions(@event, true);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			base._UnhandledInput(@event);
			if (_controlIsThis) return;
			
			InterpretTouchActions(@event, false);
		}

		void InterpretTouchActions(InputEvent @event, bool controlIsSelf)
		{
			GDLogger.Log(this, $"Action {@event.GetType()}");
			if (!(@event is GDTIMTouchAction action)) return;

			GDLogger.Log(this, $"GOT TOUCH ACTION {action.GetType()}");
			bool myTouch = true;
			if (!controlIsSelf)
			{
				myTouch = _control.HasPoint(action.Position);
			}
			
			if(!myTouch) return;
				
			switch (@event)
			{
				case TouchBegin begin:
					begin.AcceptGestures(this, false, false);
					break;
			}
			
			if(_control.MouseFilter == MouseFilterEnum.Stop)
				AcceptEvent();

			GDTIMForwarder.AcceptTouch(this);
		}
		
		public virtual void OnTouchBegin(TouchBegin args)
		{
			TouchBegin?.Invoke(this, args);
		}

		public void OnTouchEnd(TouchEnd args)
		{	
			TouchEnd?.Invoke(this, args);
		}

		public virtual void OnSingleTap(SingleTap args)
		{
			SingleTap?.Invoke(this, args);
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

		public virtual void OnTwist(Twist args)
		{
			Twist?.Invoke(this, args);
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