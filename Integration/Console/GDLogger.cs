using Godot;
using CustomDotNetExtensions;
using Exception = System.Exception;

namespace GodotExtensions
{
    /// <summary>
    /// A using the Logger class for more robust and useful logs in Godot
    /// Don't mind the millions of overrides - 
    /// I just wanted to avoid performance bottlenecks with generics and interface casting. Let the compiler do the work
    /// </summary>
    public static class GDLogger
    {
        public static void Log(object obj, string log) => Logger.Log(obj, log, false);
        public static void Error(object obj, string log) => Logger.Error(obj, log, false);
        public static void Exception(object obj, string log, Exception e) => Logger.Exception(obj, log, e, false);

        public static void Log(Object obj, string log) => Logger.Log(obj, GenerateHeader(obj), log, false);
        public static void Error(Object obj, string log) => Logger.Error(obj, GenerateHeader(obj), log, false);
        public static void Exception(Object obj, string log, Exception e) => Logger.Exception(obj, GenerateHeader(obj), log, e, false);

        public static void Log(Node node, string log) => Logger.Log(node, GenerateHeader(node), log, false);
        public static void Error(Node node, string log) => Logger.Error(node, GenerateHeader(node), log, false);
        public static void Exception(Node node, string log, Exception e) => Logger.Exception(node, GenerateHeader(node), log, e, false);

        public static void Log(NodeExt node, string log) => Logger.Log(node, log, false);
        public static void Error(NodeExt node, string log) => Logger.Error(node, log, false);
        public static void Exception(NodeExt node, string log, Exception e) => Logger.Exception(node, log, e, false);

        public static void Log(ObjectExt obj, string log) => Logger.Log(obj, log, false);
        public static void Error(ObjectExt obj, string log) => Logger.Error(obj, log, false);
        public static void Exception(ObjectExt obj, string log, Exception e) => Logger.Exception(obj, log, e, false);

        public static void Log(IIdentified obj, string log) => Logger.Log(obj, log, false);
        public static void Error(IIdentified obj, string log) => Logger.Error(obj, log, false);
        public static void Exception(IIdentified obj, string log, Exception e) => Logger.Exception(obj, log, e, false);
        
        static string GenerateHeader(Object obj)
        {
            return $"{obj.GetType()} {obj.GetInstanceId().ToString()}";
        }

        static string GenerateHeader(Node node)
        {
            return $"{node.GetType()} \"{node.Name}\" ({node.GetInstanceId().ToString()})";
        }
    }

    /// <summary>
    /// A shorter-named GDLogger to avoid having to use a Using statement on every script to shorten the name
    /// Again, way too many overrides in here. For something that can be thrown into any hot path, it's for the best.
    /// </summary>
    public static class GDL
    {
        public static void Log(object obj, string log) => GDLogger.Log(obj, log);
        public static void Error(object obj, string log) => GDLogger.Error(obj, log);
        public static void Exception(object obj, string log, Exception e) => GDLogger.Exception(obj, log, e);

        public static void Log(IIdentified obj, string log) => GDLogger.Log(obj, log);
        public static void Error(IIdentified obj, string log) => GDLogger.Error(obj, log);
        public static void Exception(IIdentified obj, string log, Exception e) => GDLogger.Exception(obj, log, e);

        public static void Log(Object obj, string log) => GDLogger.Log(obj, log);
        public static void Error(Object obj, string log) => GDLogger.Error(obj, log);
        public static void Exception(Object obj, string log, Exception e) => GDLogger.Exception(obj, log, e);
        
        public static void Log(Node node, string log) => GDLogger.Log(node, log);
        public static void Error(Node node, string log) => GDLogger.Error(node, log);
        public static void Exception(Node node, string log, Exception e) => GDLogger.Exception(node, log, e);

        public static void Log(NodeExt obj, string log) => GDLogger.Log(obj, log);
        public static void Error(NodeExt obj, string log) => GDLogger.Error(obj, log);
        public static void Exception(NodeExt obj, string log, Exception e) => GDLogger.Exception(obj, log, e);
        
        public static void Log(ObjectExt obj, string log) => GDLogger.Log(obj, log);
        public static void Error(ObjectExt obj, string log) => GDLogger.Error(obj, log);
        public static void Exception(ObjectExt obj, string log, Exception e) => GDLogger.Exception(obj, log, e);

    }
}