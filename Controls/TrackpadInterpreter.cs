using Godot;
using System;
using System.Diagnostics;
using GodotExtensions;

public class TrackpadInterpreter : NodeExt
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void OnMultiDrag(Vector2 position, Vector2 relative, int fingers)
	{
		
	}

	public void OnMultiLongPress(Vector2 position, int fingers)
	{
		
	}

	public void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers)
	{
		
	}

	public void OnMultiTap(Vector2 position, int fingers)
	{
		
	}

	public void OnPinch(Vector2 position, float relative, float distance, int fingers)
	{
		
	}

	public void OnSingleDrag(Vector2 position, Vector2 relative)
	{
		Log($"Dragging! {position.ToString()}, {relative.ToString()}");
	}

	public void OnSingleLongPress(Vector2 position)
	{
		
	}

	public void OnSingleSwipe(Vector2 position, Vector2 relative)
	{
		
	}

	public void OnSingleTap(Vector2 position)
	{
		
	}

	public void OnSingleTouch(Vector2 position, bool cancelled)
	{
		
	}

	public void OnTwist(Vector2 position, float relative, int fingers)
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
