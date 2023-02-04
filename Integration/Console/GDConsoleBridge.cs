using CustomDotNetExtensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GodotExtensions;

public class GDConsoleBridge : Node
{
	[Export] int _maxLogs = 1000;
	[Export] NodePath _buttonPath;
	[Export] bool _showInGameConsole = true;
	
	readonly Queue<Log> _logs = new Queue<Log>();
	VBoxContainer _logVBox;
	VSplitDropDown _vSplitContainer;
	ScrollContainer _scrollContainer;
	Button _button;
	
	const string ColorOverrideName = "font_color";

	readonly Dictionary<LogType, Color> _logColors = new Dictionary<LogType, Color>()
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
		EditorDescription = "A bridge between the native c# console and the Godot console, homogenizing their output." +
		                    "Also contains a log console in-game.";

		//should probably just split into 2 classes but that's a task for another day
		if (!_showInGameConsole)
		{
			if (_vSplitContainer != null)
			{
				_vSplitContainer.Collapsed = true;
				_vSplitContainer.DraggerVisibility = SplitContainer.DraggerVisibilityEnum.HiddenCollapsed;
			}

			return;
		}
		
		_vSplitContainer = FindNode("VSplitContainer") as VSplitDropDown;

		_scrollContainer = FindNode("ScrollContainer") as ScrollContainer;
		_logVBox = _scrollContainer.GetNode<VBoxContainer>("VBoxContainer");
		

		_logVBox.Connect("resized", this, ScrollToBottom);
		_scrollContainer.Connect("scroll_started", this, DisallowAutoScroll);
		_scrollContainer.Connect("scroll_ended", this, AllowAutoScrollIfAtBottom);

		_button = GetNode<Button>(_buttonPath);

		if (_button != null)
		{
			_button.Connect("pressed", this, _vSplitContainer.ToggleWithButton);
		}
	}
	
	void HandleLog(object sender, LogEventArgs a)
	{
		GD.Print(a.Log);
		AddLog(LogType.Log, a.Log);
	}
	
	void HandleError(object sender, LogEventArgs a)
	{
		GD.PrintErr(a.Log);
		GD.PushError(a.Log);
		AddLog(LogType.Error, a.Log);
	}
	
	void HandleException(object sender, ExceptionEventArgs a)
	{
		GD.PrintErr(a.Log);
		GD.PushError(a.Log);
		AddLog(LogType.Exception, a.Log);
	}

	void AddLog(LogType type, string log)
	{
		Log logItem = GetNextLogItem();
		logItem.Type = type;
		logItem.Text = log;
		logItem.Label.Text = log;
		logItem.Label.AddColorOverride(ColorOverrideName, _logColors[type]);
		_logs.Enqueue(logItem);
		
		Log GetNextLogItem()
		{
			Log nextLogItem;
			if (_logs.Count == _maxLogs)
			{
				nextLogItem = _logs.Dequeue();
				nextLogItem.Label.RemoveColorOverride(ColorOverrideName);

				int childCount = _logVBox.GetChildCount();
				_logVBox.MoveChild(nextLogItem.Label, childCount);
			}
			else
			{
				nextLogItem = new Log();
				var label = new Label();
				label.AnchorRight = 1;
				nextLogItem.Label = label;
				_logVBox.AddChild(label);
			}

			return nextLogItem;
		}
	}

	bool _allowAutoScroll = true;
	async void AllowAutoScrollIfAtBottom()
	{
		await Task.Delay(100); // needs at least one frame of delay for scroll container to be updated by engine
		if (_scrollContainer.ScrollVertical >= (int)_scrollContainer.GetVScrollbar().MaxValue)
			_allowAutoScroll = true;
	}

	void DisallowAutoScroll()
	{
		_allowAutoScroll = false;
	}

	async void ScrollToBottom()
	{
		if (!_allowAutoScroll) return;
		await Task.Delay(100); // needs at least one frame of delay for scroll container to be updated by engine
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
