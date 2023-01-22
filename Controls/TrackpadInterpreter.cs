using Godot;
using System;
using System.Diagnostics;
using GodotExtensions;

public class TrackpadInterpreter : GDTIMInterpreter
{
	private VncHandler _vncHandler;
	[Export] private float _mouseSpeed = 10f;
	[Export] private float _scrollSpeed = 10f;
	[Export] private string _trackpadControlPathRelative = "../Panel";
	[Export] private string _vncHandlerPathRelative = "../../VncHandler";
	private Control _trackpadGui;
	private Vector2 RealSize => _trackpadGui.RealPixelSize();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vncHandler = GetNode(_vncHandlerPathRelative) as VncHandler;
		_trackpadGui = GetNode(_trackpadControlPathRelative) as Control;
	}

	public override void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
	{
		//bug wrangling - I don't want a drag if it hasn't been dragged
		if (relative == Vector2.Zero) 
			return;
		
		Log($"Multi drag: {position.ToString()}, {relative.ToString()}, {fingers.ToString()}");
	}

	public override void OnMultiLongPress(Vector2 position, int fingers)
	{
		Log("Multi long press");
	}

	public override void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
	{
		if (fingers != 2)
			return;

		Scroll(relative);
	}

	public override void OnMultiTap(Vector2 position, int fingers)
	{
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
		Log("Pinch");
	}

	public override void OnSingleDrag(Vector2 position, Vector2 relative)
	{
		//bug wrangling - I don't want a drag if it hasn't been dragged
		if (relative == Vector2.Zero) 
			return;

		var realSize = RealSize;
		float minResolution = Mathf.Min(realSize.x, realSize.y);
		Vector2 moveAmount = relative / minResolution * _mouseSpeed;
		_vncHandler.MoveMouse(moveAmount);
		
		Log($"Dragging {moveAmount.ToString()}");
	}

	public override void OnSingleLongPress(Vector2 position)
	{
		Log("Single long press");
		// want to use this for click and drag
	}

	public override void OnSingleSwipe(Vector2 position, Vector2 relative)
	{
		Log("Single Swipe");
	}

	public override void OnSingleTap(Vector2 position)
	{
		Log("Single tap");
		_vncHandler.MouseButtonDown(MouseButton.Left);
		_vncHandler.MouseButtonUp(MouseButton.Left);
	}

	public override void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
	{
		Log($"Single touch {(pressed ? "down": "up")}");
		return;
		if (pressed)
		{
			_vncHandler.MouseButtonUp(MouseButton.Left);
		}
		else
		{
			_vncHandler.MouseButtonDown(MouseButton.Left);
		}
	}

	public override void OnTwist(Vector2 position, float relative, int fingers)
	{
		Log("Twist");
	}
	
	private void RightClick()
	{
		_vncHandler.MouseButtonDown(MouseButton.Right);
		_vncHandler.MouseButtonUp(MouseButton.Right);
	}

	private void MiddleClick()
	{
		_vncHandler.MouseButtonDown(MouseButton.Middle);
		_vncHandler.MouseButtonUp(MouseButton.Middle);
	}

	private void Scroll(Vector2 relative)
	{
		var vertical = Mathf.Abs(relative.y) > Mathf.Abs(relative.x);

		if (!vertical)
			return;

		var scrollAmount = (int)(relative.y / RealSize.y * _scrollSpeed);
		_vncHandler.Scroll(scrollAmount);
	}
}
