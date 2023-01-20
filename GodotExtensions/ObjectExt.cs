using CustomDotNetExtensions;
using Godot;
using System;
using System.Xml.Linq;
using Object = Godot.Object;

namespace GodotExtensions
{
    public abstract class ObjectExt : Object, IIdentified
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
