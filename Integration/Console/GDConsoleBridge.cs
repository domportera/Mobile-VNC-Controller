using CustomDotNetExtensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using GodotExtensions;

public class GDConsoleBridge : Node
{
	[Export] private int _maxLogs = 1000;
	[Export] private float _height = 0.3f;
	private readonly Queue<Log> _logs = new Queue<Log>();
	private CanvasLayer _canvasLayer;
	private VBoxContainer _logVBox;
	private ScrollContainer _scrollContainer;
	private Panel _panel;
	
	private const string ColorOverrideName = "font_color";

	private readonly Dictionary<LogType, Color> _logColors = new Dictionary<LogType, Color>()
	{
		{LogType.Log, new Color(1f, 1f, 1f)},
		{LogType.Error, new Color(1f, 0.2f, 0.2f)},
		{LogType.Exception, new Color(1f, 0f, 0f)}
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Logger.LogEvent += HandleLog;
		Logger.ErrorEvent += HandleError;
		Logger.ExceptionEvent += HandleException;

		_canvasLayer = GetNode("CanvasLayer") as CanvasLayer;
		_panel = _canvasLayer.GetNode("Panel") as Panel;
		_scrollContainer = _panel.GetNode("ScrollContainer") as ScrollContainer;
		_logVBox = _scrollContainer.GetNode("VBoxContainer") as VBoxContainer;
		
		_panel.AnchorBottom = _height;
		EditorDescription = "A bridge between the native c# console and the Godot console, homogenizing their output";
	}

	private void HandleLog(object sender, LogEventArgs a)
	{
		GD.Print(a.Log);
		AddLog(LogType.Log, a.Log);
	}
	private void HandleError(object sender, LogEventArgs a)
	{
		GD.PrintErr(a.Log);
		AddLog(LogType.Error, a.Log);
	}
	private void HandleException(object sender, ExceptionEventArgs a)
	{
		GD.PrintErr(a.Log);
		AddLog(LogType.Exception, a.Log);
	}
	
	private void AddLog(LogType type, string log)
	{
		Log logItem;
		if (_logs.Count == _maxLogs)
		{
			logItem = _logs.Dequeue();
			logItem.Label.RemoveColorOverride(ColorOverrideName);
			
			int childCount = _logVBox.GetChildCount();
			_logVBox.MoveChild(logItem.Label, childCount);
		}
		else
		{
			logItem = new Log();
			var label = new Label();
			label.AnchorRight = 1;
			logItem.Label = label;
			_logVBox.AddChild(label);
		}

		logItem.Type = type;
		logItem.Text = log;
		logItem.Label.Text = log;
		logItem.Label.AddColorOverride(ColorOverrideName, _logColors[type]);
		_logs.Enqueue(logItem);
		_scrollContainer.ScrollVertical = (int)_scrollContainer.GetVScrollbar().MaxValue;
	}

	enum LogType {Log, Error, Exception}
	class Log
	{
		public LogType Type;
		public string Text;
		public Label Label;
	}
}
