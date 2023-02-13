using CustomDotNetExtensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GodotExtensions;
using GodotExtensions.GDScript_Extension_Methods;

public class GDConsoleBridge : Node
{
	[Export] int _maxLogs = 1000;
	[Export] NodePath _buttonPath = null;
	[Export] NodePath _vSplitPath = null;
	[Export] NodePath _scrollContainerPath = null;
	[Export] NodePath _vBoxPath = null;
	[Export] bool _showInGameConsole = true;
	[Export] bool _openByDefault = false;
	[Export] bool _autoScroll = true;
	
	readonly Queue<Log> _logs = new Queue<Log>();
	VBoxContainer _logVBox;
	VSplitPullDown _vSplitPullDown;
	ScrollContainer _scrollContainer;
	IButtonUpDown _buttonUpDown;
	
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
		
		_vSplitPullDown = FindNode(_vSplitPath) as VSplitPullDown;
		
		//should probably just split into 2 classes but that's a task for another day
		if (!_showInGameConsole)
		{
			if (_vSplitPullDown != null)
			{
				_vSplitPullDown.Collapsed = true;
				_vSplitPullDown.DraggerVisibility = SplitContainer.DraggerVisibilityEnum.HiddenCollapsed;
			}

			return;
		}

		_scrollContainer = GetNode<ScrollContainer>(_scrollContainerPath);
		_scrollContainer.FollowFocus = _autoScroll;

		_logVBox = GetNode<VBoxContainer>(_vBoxPath);

		//_scrollContainer.Connect("scroll_started", this, DisallowAutoScroll);
		//_scrollContainer.Connect("scroll_ended", this, AllowAutoScrollIfAtBottom);

		_buttonUpDown = GetNode<IButtonUpDown>(_buttonPath);

		if (_buttonUpDown != null)
		{
			_buttonUpDown.PressUp += (_, __) =>  _vSplitPullDown.ToggleWithButton();  
		}
		
		if(_vSplitPullDown != null && _openByDefault)
			_vSplitPullDown.ToggleWithButton();

		GetViewport().Connect("gui_focus_changed", this, "OnFocusChanged");
		_scrollContainer.Connect("scroll_ended", this, "RefocusPrevious");
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

	Control _previouslyFocused;
	void OnFocusChanged(Control control)
	{
		if (control is Log) return;
		_previouslyFocused = control;
	}

	void RefocusPrevious()
	{
		if (_previouslyFocused == null) return;
		Control.FocusModeEnum focus = _previouslyFocused.FocusMode;
		_previouslyFocused.FocusMode = Control.FocusModeEnum.All;
		_previouslyFocused.CallDeferred("grab_focus");
		_previouslyFocused.FocusMode = focus;
	}

	async void AddLog(LogType type, string log)
	{
		Log logItem = GetNextLogItem();
		logItem.Type = type;
		logItem.Text = log;
		logItem.Text = log;
		logItem.AddColorOverride(ColorOverrideName, _logColors[type]);
		_logs.Enqueue(logItem);

		if (_autoScroll)
		{
			await Task.Delay(100); // need to wait for instantiation to complete?
			logItem.CallDeferred("grab_focus");
		}

		Log GetNextLogItem()
		{
			Log nextLogItem;
			if (_logs.Count == _maxLogs)
			{
				nextLogItem = _logs.Dequeue();
				nextLogItem.RemoveColorOverride(ColorOverrideName);

				int childCount = _logVBox.GetChildCount();
				_logVBox.MoveChild(nextLogItem, childCount);
			}
			else
			{
				nextLogItem = new Log();
				nextLogItem.AnchorRight = 1;
				nextLogItem.FocusMode = Control.FocusModeEnum.All;

				_logVBox.AddChild(nextLogItem);
			}

			return nextLogItem;
		}
	}

	async void AllowAutoScrollIfAtBottom()
	{
		await Task.Delay(100); // needs at least one frame of delay for scroll container to be updated by engine
		if (_scrollContainer.ScrollVertical >= (int)_scrollContainer.GetVScrollbar().MaxValue)
			_scrollContainer.ScrollVertical = int.MaxValue;
	}
	enum LogType {Log, Error, Exception}
	class Log : Label
	{
		public LogType Type;
	}
}
