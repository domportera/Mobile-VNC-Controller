using System.Diagnostics;
using Godot;
using GodotExtensions;

/// <summary>
/// This should be refactored such that it has no reference to VncHandler, and should inherit an interface
/// that the VNC class is aware of and subscribes to its mouse events
/// then a UI-layer could potentially respond to the trackpad as well.
/// would go a long way to make this a generic solution for UI-constrained gestures.
/// </summary>
public class TrackpadInterpreter : GDTIMInterpreter
{
	VncHandler _vncHandler;
	[Export] float _mouseSpeed = 100f;
	[Export] float _scrollSpeed = 10f;
	[Export] string _trackpadControlPathRelative = "Panel";
	[Export] string _vncHandlerPathRelative = "../../VncHandler";
	[Export] bool _shouldSendVncCommands = true;
	
	Control _trackpadGui;
	Vector2 TrackpadRealSize => _trackpadGui.RealPixelSize();
	Rect2 TrackpadRect => _trackpadGui.GetRect();
	
	bool _shouldProcessEvents;
	Vector2 _cumulativeScroll;
	float _deltaTime;
	float TimeAdjustment => _deltaTime;
	int _touchCount;
	
	public override void _Ready()
	{
		_vncHandler = GetNode(_vncHandlerPathRelative) as VncHandler;
		_trackpadGui = GetNode(_trackpadControlPathRelative) as Control;
	}

	public override void _Process(float delta)
	{
		_deltaTime = delta;

		if (_touchCount == 0)
			_shouldProcessEvents = false;
	}

	public override void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
	{
		if (pressed)
		{
			_shouldProcessEvents = IsInsideTrackpad(position);
			if(_shouldProcessEvents)
				_touchCount++;
			return;
		}

		_touchCount--;
		if (!_shouldProcessEvents)
			return;

		_cumulativeScroll = Vector2.Zero;
		LongPress(false);
	}

	public override void OnSingleTap(Vector2 position)
	{
		if (!_shouldProcessEvents) return;
		_vncHandler.MouseButtonDown(MouseButton.Left);
		_vncHandler.MouseButtonUp(MouseButton.Left);
	}
	
	//todo : should continue to move mouse up/left/etc if drag is held on the side after running out of room on the trackpad
	public override void OnSingleDrag(Vector2 position, Vector2 relative)
	{
		if (!_shouldProcessEvents) return;

		Vector2 trackpadSize = TrackpadRealSize;
		float minTrackpadDimension = Mathf.Min(trackpadSize.x, trackpadSize.y);
		Vector2 serverResolution = _vncHandler.Resolution;
		float minServerResolution = Mathf.Min(serverResolution.x, serverResolution.y);
		Vector2 moveAmount = relative / minTrackpadDimension * _mouseSpeed * minServerResolution;
		
		MoveMouse(moveAmount);
		
		//Log($"Dragging {moveAmount.ToString()}");
	}

	bool _longPressed;
	public override void OnSingleLongPress(Vector2 position)
	{
		if (!_shouldProcessEvents) return;
		
		LongPress(true);
	}

	public override void OnSingleSwipe(Vector2 position, Vector2 relative)
	{
		if (!_shouldProcessEvents) return;
		Log("Single Swipe");
	}
	
	public override void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
	{
		if (!_shouldProcessEvents) return;
		Scroll(relative);
	}

	public override void OnMultiLongPress(Vector2 position, int fingers)
	{
		if (!_shouldProcessEvents) return;
		Log("Multi long press");
	}

	public override void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
	{
		if (!_shouldProcessEvents) return;
		if (fingers != 2)
			return;

		Scroll(relative);
	}

	public override void OnMultiTap(Vector2 position, int fingers)
	{
		if (!_shouldProcessEvents) return;
		if (fingers == 2)
		{
			RightClick();
		}
		else if (fingers == 3)
		{
			MiddleClick();
		}

		Log("Multi tap");
	}

	public override void OnPinch(Vector2 position, float relative, float distance, int fingers)
	{
		if (!_shouldProcessEvents) return;
		Log("Pinch");
	}

	public override void OnTwist(Vector2 position, float relative, int fingers)
	{
		if (!_shouldProcessEvents) return;
		Log("Twist");
	}

	void RightClick()
	{
		if (!_shouldSendVncCommands) return;
		_vncHandler.MouseButtonDown(MouseButton.Right);
		_vncHandler.MouseButtonUp(MouseButton.Right);
	}

	void MiddleClick()
	{
		if (!_shouldSendVncCommands) return;
		_vncHandler.MouseButtonDown(MouseButton.Middle);
		_vncHandler.MouseButtonUp(MouseButton.Middle);
	}
	void Scroll(Vector2 relative)
	{
		if (!_shouldSendVncCommands) return;
		bool vertical = Mathf.Abs(relative.y) > Mathf.Abs(relative.x);

		if(vertical)
			_cumulativeScroll.y += relative.y / TrackpadRealSize.y * _scrollSpeed;
		else
			_cumulativeScroll.x += relative.x / TrackpadRealSize.x * _scrollSpeed;

		var scrollAmount = new Vector2((int)_cumulativeScroll.x, (int)_cumulativeScroll.y);
		_vncHandler.Scroll(scrollAmount);
		
		if (_cumulativeScroll.x > 1f || _cumulativeScroll.x < -1f)
		{
			_cumulativeScroll.x = 0f;
		}
		if (_cumulativeScroll.y > 1f || _cumulativeScroll.y < -1f)
		{
			_cumulativeScroll.y = 0f;
		}
	}

	void MoveMouse(Vector2 moveAmount)
	{
		if (!_shouldSendVncCommands) return;
		moveAmount *= TimeAdjustment;
		_vncHandler.MoveMouse(moveAmount);
	}
	
	void LongPress(bool pressed)
	{
		if (_shouldSendVncCommands)
		{
			if (pressed)
			{
				_vncHandler.MouseButtonDown(MouseButton.Left);
				Input.VibrateHandheld(50);
			}
			else if (_longPressed)
				_vncHandler.MouseButtonUp(MouseButton.Left);
		}

		_longPressed = pressed;
	}
	
	bool IsInsideTrackpad(Vector2 position) => TrackpadRect.HasPoint(position);
}
