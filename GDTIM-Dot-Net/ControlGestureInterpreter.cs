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
			GDTIMForwarder.Register(this);
			
			if(_control != this)
				_control.AddChild(this);
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
			if (_touchCount == 0)
				_shouldProcessEvents = false;
		}
		
		public virtual void OnSingleTouch(SingleTouchArgs args)
		{
			if (args.Pressed)
			{
				_shouldProcessEvents = IsInsideControl(args.Position);
				if (_shouldProcessEvents)
				{
					_touchCount++;
					SingleTouch?.Invoke(this, args);
				}

				return;
			}

			_touchCount--;
			if (!_shouldProcessEvents)
				return;

			SingleTouch?.Invoke(this, args);
		}

		public virtual void OnSingleTap(SingleTapArgs args)
		{
			if (!_shouldProcessEvents) return;
			SingleTap?.Invoke(this, args);
		}

		public virtual void OnSingleDrag(SingleDragArgs args)
		{
			if (!_shouldProcessEvents) return;
			SingleDrag?.Invoke(this, args);
		}

		public virtual void OnSingleLongPress(SingleTapArgs args)
		{
			if (!_shouldProcessEvents) return;
			SingleLongPress?.Invoke(this, args);
		}
		
		public virtual void OnSingleSwipe(SingleDragArgs args)
		{
			if (!_shouldProcessEvents) return;
			SingleSwipe?.Invoke(this, args);
		}

		public virtual void OnMultiDrag(MultiDragArgs args)
		{
			if (!_shouldProcessEvents) return;
			MultiDrag?.Invoke(this, args);
		}

		public virtual void OnMultiLongPress(MultiTapArgs args)
		{
			if (!_shouldProcessEvents) return;
			MultiLongPress?.Invoke(this, args);
		}

		public virtual void OnMultiSwipe(MultiDragArgs args)
		{
			if (!_shouldProcessEvents) return;
			MultiSwipe?.Invoke(this, args);
		}
		
		public virtual void OnMultiTap(MultiTapArgs args)
		{
			if (!_shouldProcessEvents) return;
			MultiTap?.Invoke(this, args);
		}


		public virtual void OnPinch(PinchArgs args)
		{
			if (!_shouldProcessEvents) return;
			Pinch?.Invoke(this, args);
		}

		public virtual void OnTwist(TwistArgs args)
		{
			if (!_shouldProcessEvents) return;
			Twist?.Invoke(this, args);
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

		//todo: needs to be updated for GUI sorting. can this be done with a raycast? should listeners worry about this instead?
		bool IsInsideControl(Vector2 position)
		{
			Rect2 relevantRect = new Rect2(_control.RectGlobalPosition, _control.RealRectSize());
			return _control.HasPoint(position);
		}
	}
}