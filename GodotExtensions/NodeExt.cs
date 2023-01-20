using Godot;
using System;
using CustomDotNetExtensions;
using Object = Godot.Object;


namespace GodotExtensions
{
    public abstract class NodeExt : Node, IIdentifiedNamed
    {
        public ulong ID => GetInstanceId();

        [Export] DebugMode debugMode = DebugMode.Debug;
        public DebugMode DebugMode => debugMode;
        string IIdentifiedNamed.Name => Name;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public virtual void HighlightInEditor()
        {
            //override this with however this object could be made more clear in editor
        }

        protected virtual void Log(string toLog)
        {
            GDL.Log(this, toLog);
        }
        
        protected virtual void LogError(string toLog)
        {
            GDL.Error(this, toLog);
        }
        
        protected virtual void LogException(string toLog, Exception exception)
        {
            GDL.Exception(this, toLog, exception);
        }
    }
}