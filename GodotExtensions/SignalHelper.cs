using System;
using Godot;

namespace GodotExtensions
{
    public static class SignalHelper
    {
        public delegate void SignalFunction();
        public delegate void SignalFunction<in T>(T parameter);
        public delegate void SignalFunction<in T1, in T2>(T1 param1, T2 param2);
        public delegate void SignalFunction<in T1, in T2, in T3>(T1 param1, T2 param2, T3 param3);
        public delegate void SignalFunction<in T1, in T2, in T3, in T4>(T1 param1, T2 param2, T3 param3, T4 param4);
        public delegate void SignalFunction<in T1, in T2, in T3, in T4, in T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
        
        public static Godot.Error Connect(this GodotObject obj, string signalName, GodotObject owner, SignalFunction method)
        {
            Error error = obj.Connect(signalName, new Callable(owner, method.Method.Name));
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }
        
        public static Godot.Error Connect(this GodotObject obj, string signalName, ObjectExt owner, SignalFunction method)
        {
            Error error = obj.Connect(signalName, new(owner, method.Method.Name));
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }
        
        public static Godot.Error Connect(this GodotObject obj, string signalName, NodeExt owner, SignalFunction method)
        {
            Error error = obj.Connect(signalName, new Callable(owner, method.Method.Name));
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }

        static string GenerateSignalErrorString(GodotObject obj, string signalName, Error error)
        {
            return $"Error connecting signal \"{signalName}\" to {obj.GetType()}: {error}";
        }
    }
}