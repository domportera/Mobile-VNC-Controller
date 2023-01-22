using Godot;
using System;
using System.Diagnostics;
using GodotExtensions;

public class TrackpadInterpreter : NodeExt
{
	private VncHandler _vncHandler;
	[Export] private float _scrollSpeed = 1f;
	private Vector2 _resolution;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vncHandler = GetNode("../../VncHandler") as VncHandler;
		_resolution = OS.WindowSize;
	}

	public void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
	{
		//bug wrangling - I don't want a drag if it hasn't been dragged
		if (relative == Vector2.Zero) 
			return;
		
		Log($"Multi drag: {position.ToString()}, {relative.ToString()}, {fingers.ToString()}");
	}

	public void OnMultiLongPress(Vector2 position, int fingers)
	{
		Log("Multi long press");
	}

	public void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
	{
		if (fingers != 2)
			return;

		Scroll(relative);
	}

	public void OnMultiTap(Vector2 position, int fingers)
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

	public void OnPinch(Vector2 position, float relative, float distance, int fingers)
	{
		Log("Pinch");
	}

	public void OnSingleDrag(Vector2 position, Vector2 relative)
	{
		//bug wrangling - I don't want a drag if it hasn't been dragged
		if (relative == Vector2.Zero) 
			return;
		
		Log($"Dragging! {position.ToString()}, {relative.ToString()}");
	}

	public void OnSingleLongPress(Vector2 position)
	{
		Log("Single long press");
		// want to use this for click and drag
	}

	public void OnSingleSwipe(Vector2 position, Vector2 relative)
	{
		Log("Single Swipe");
	}

	public void OnSingleTap(Vector2 position)
	{
		Log("Single tap");
		_vncHandler.MouseButtonDown(MouseButton.Left);
		_vncHandler.MouseButtonUp(MouseButton.Left);
	}

	public void OnSingleTouch(Vector2 position, bool pressed, bool cancelled)
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

	public void OnTwist(Vector2 position, float relative, int fingers)
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

		int scrollAmount = (int)(relative.y / _resolution.y * _scrollSpeed * 1080);
		_vncHandler.Scroll(scrollAmount);
	}
}
