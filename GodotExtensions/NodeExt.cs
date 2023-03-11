using Godot;
using System;
using CustomDotNetExtensions;


namespace GodotExtensions
{
    public abstract class NodeExt : Node, IIdentifiedNamed
    {
        public ulong ID => GetInstanceId();

        [Export] DebugMode debugMode = DebugMode.Debug;
        public DebugMode DebugMode => debugMode;
        string IIdentifiedNamed.Name => Name;
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