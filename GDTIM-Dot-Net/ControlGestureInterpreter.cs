using Godot;
using GodotExtensions;
using System;

namespace GDTIMDotNet
{
	public class ControlGestureInterpreter : Control, IGestureInterpreter
	{
		Control _control;
		public Vector2 ControlRealSize => _control.RealPixelSize();
		int _touchCount;
		bool _shouldProcessEvents;

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

		public ControlGestureInterpreter()
		{
			_control = this;
		}

		public ControlGestureInterpreter(Control control)
		{
			_control = control;
		}

		public override void _Ready()
		{
			if(_control != this)
				_control.AddChild(this);
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
			if (_touchCount == 0)
				_shouldProcessEvents = false;
		}
		
		public virtual void OnTouchBegin(TouchBegin args)
		{
			if (args.Pressed)
			{
				_shouldProcessEvents = IsInsideControl(args.Position);
				if (_shouldProcessEvents)
				{
					_touchCount++;
					TouchBegin?.Invoke(this, args);
				}

				return;
			}

		

			TouchBegin?.Invoke(this, args);
		}

		public void OnTouchEnd(TouchEnd args)
		{	
			_touchCount--;
			if (!_shouldProcessEvents)
				return;
			
			TouchEnd?.Invoke(this, args);
		}

		public virtual void OnSingleTap(SingleTap args)
		{
			if (!_shouldProcessEvents) return;
			SingleTap?.Invoke(this, args);
		}

		public virtual void OnSingleDrag(SingleDrag args)
		{
			if (!_shouldProcessEvents) return;
			SingleDrag?.Invoke(this, args);
		}

		public virtual void OnSingleLongPress(SingleTap args)
		{
			if (!_shouldProcessEvents) return;
			SingleLongPress?.Invoke(this, args);
		}
		
		public virtual void OnSingleSwipe(SingleDrag args)
		{
			if (!_shouldProcessEvents) return;
			SingleSwipe?.Invoke(this, args);
		}

		public virtual void OnMultiDrag(MultiDrag args)
		{
			if (!_shouldProcessEvents) return;
			MultiDrag?.Invoke(this, args);
		}

		public virtual void OnMultiLongPress(MultiTap args)
		{
			if (!_shouldProcessEvents) return;
			MultiLongPress?.Invoke(this, args);
		}

		public virtual void OnMultiSwipe(MultiDrag args)
		{
			if (!_shouldProcessEvents) return;
			MultiSwipe?.Invoke(this, args);
		}
		
		public virtual void OnMultiTap(MultiTap args)
		{
			if (!_shouldProcessEvents) return;
			MultiTap?.Invoke(this, args);
		}


		public virtual void OnPinch(Pinch args)
		{
			if (!_shouldProcessEvents) return;
			Pinch?.Invoke(this, args);
		}

		public virtual void OnTwist(Twist args)
		{
			if (!_shouldProcessEvents) return;
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

		public void EndTouch(TouchEnd args)
		{
			throw new NotImplementedException();
		}

		//todo: needs to be updated for GUI sorting. can this be done with a raycast? should listeners worry about this instead?
		bool IsInsideControl(Vector2 position)
		{
			Rect2 relevantRect = new Rect2(_control.RectGlobalPosition, _control.RealRectSize());
			return _control.HasPoint(position);
		}
	}
}