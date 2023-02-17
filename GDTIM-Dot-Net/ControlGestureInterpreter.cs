using Godot;
using GodotExtensions;
using System;

namespace GDTIMDotNet
{
	public class ControlGestureInterpreter : Control, IGestureInterpreter
	{
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
		readonly Control _control;

		protected ControlGestureInterpreter()
		{
			_control = this;
			_controlIsThis = true;
		}

		public ControlGestureInterpreter(Control control, MouseFilterEnum mouseFilter = MouseFilterEnum.Stop)
		{
			_control = control;
			if(GetParent() != control)
				_control.AddChild(this);

			// this interpreter will be taking over the mouse interactions
			control.MouseFilter = MouseFilterEnum.Ignore;
			MouseFilter = mouseFilter;

			// take their tooltip too
			HintTooltip = control.HintTooltip;
			
			_controlIsThis = control == this; //always false?
			Name = $"{_control.Name} (Touch)";
		}

		public override void _Input(InputEvent input)
		{
			InterpretTouchActions(input);
		}

		public override void _GuiInput(InputEvent input)
		{
			if (!_controlIsThis) return;
			InterpretTouchActions(input);
		}

		public override void _UnhandledInput(InputEvent input)
		{
			InterpretTouchActions(input);
		}

		void InterpretTouchActions(InputEvent input)
		{
			if (!(input is GDTIMTouchAction action)) return;
			
			if (action is TouchBegin begin)
			{
				if(_control.GetGlobalRect().HasPoint(begin.Position))
					begin.AcceptGesturesControl(this, true, false);
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