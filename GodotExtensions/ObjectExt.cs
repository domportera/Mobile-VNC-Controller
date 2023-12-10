using CustomDotNetExtensions;
using Godot;
using System;

namespace GodotExtensions
{
	public abstract partial class ObjectExt : GodotObject, IIdentified
	{
		public ulong ID => GetInstanceId();

		[Export] DebugMode debugMode = DebugMode.Debug;

		public DebugMode DebugMode => debugMode;

		const string DEFAULT_OBJECT_NAME = "Default Object Name";

		public virtual void HighlightInEditor()
		{
			//override this with however this object could be made more clear in editor
		}
		
		protected virtual void Log(string toLog)
		{
			GDLogger.Log(this, toLog);
		}
		
		protected virtual void LogError(string toLog)
		{
			GDLogger.Error(this, toLog);
		}
		
		protected virtual void LogException(string toLog, Exception exception)
		{
			GDLogger.Exception(this, toLog, exception);
		}
	}
}
