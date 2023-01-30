using System.Diagnostics;
using Godot;
using GodotExtensions;
using System;
using GDTIMDotNet;

namespace GDTIMDotNet
{
	public class GuiConstrainedGestureInterpreter : GestureInterpreter
	{
		[Export] string _controlPathRelative = "Panel";

		Control _control;
		public Vector2 ControlRealSize => _control.RealPixelSize();
		Rect2 ControlRect => _control.GetRect();
		int _touchCount;
		bool _shouldProcessEvents;

		public override void _Ready()
		{
			base._Ready();
			_control = GetNode(_controlPathRelative) as Control;
		}

		public override void _Process(float delta)
		{
			base._Process(delta);
			if (_touchCount == 0)
				_shouldProcessEvents = false;
		}

		public override void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
		{
			if (pressed)
			{
				_shouldProcessEvents = IsInsideControl(position);
				if (_shouldProcessEvents)
				{
					_touchCount++;
					base.OnSingleTouch(position, true, cancelled);
				}

				return;
			}

			_touchCount--;
			if (!_shouldProcessEvents)
				return;

			base.OnSingleTouch(position, false, cancelled);
		}

		public override void OnSingleTap(Vector2 position)
		{
			if (!_shouldProcessEvents) return;
			base.OnSingleTap(position);
		}

		//todo : should continue to move mouse up/left/etc if drag is held on the side after running out of room on the trackpad
		public override void OnSingleDrag(Vector2 position, Vector2 relative)
		{
			if (!_shouldProcessEvents) return;
			base.OnSingleDrag(position, relative);
		}

		public override void OnSingleLongPress(Vector2 position)
		{
			if (!_shouldProcessEvents) return;
			base.OnSingleLongPress(position);
		}

		public override void OnSingleSwipe(Vector2 position, Vector2 relative)
		{
			if (!_shouldProcessEvents) return;
			base.OnSingleSwipe(position, relative);
		}

		public override void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnMultiDrag(position, relative, fingers);
		}

		public override void OnMultiLongPress(Vector2 position, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnMultiLongPress(position, fingers);
		}

		public override void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnMultiSwipe(position, relative, fingers);
		}

		public override void OnMultiTap(Vector2 position, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnMultiTap(position, fingers);
		}


		public override void OnPinch(Vector2 position, float relative, float distance, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnPinch(position, relative, distance, fingers);
		}

		public override void OnTwist(Vector2 position, float relative, int fingers)
		{
			if (!_shouldProcessEvents) return;
			base.OnTwist(position, relative, fingers);
		}

		bool IsInsideControl(Vector2 position) => ControlRect.HasPoint(position);
	}
}