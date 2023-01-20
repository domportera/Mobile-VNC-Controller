using CustomDotNetExtensions;
using Godot;
using System;

public class GDConsoleBridge : Node
{
	[Export] bool _printStackOnError = true;
	[Export] bool _printStackOnException = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Logger.LogEvent += HandleLog;
		Logger.ErrorEvent += HandleError;
		Logger.ExceptionEvent += HandleException;

		EditorDescription = "A bridge between the native c# console and the Godot console, homogenizing their output";
	}

	private void HandleLog(object sender, LogEventArgs a)
	{
		GD.Print(a.Log);
    }
    private void HandleError(object sender, LogEventArgs a)
    {
        GD.PrintErr(a.Log);

        if (_printStackOnError)
        {
            GD.PrintStack();
        }
    }
	private void HandleException(object sender, ExceptionEventArgs a)
	{
		GD.PrintErr(a.Log);

		if(_printStackOnException)
		{
			GD.PrintStack();
		}
	}
}
