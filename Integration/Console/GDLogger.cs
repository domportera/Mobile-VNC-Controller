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
}