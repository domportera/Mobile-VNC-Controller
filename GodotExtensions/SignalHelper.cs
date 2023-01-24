using CustomDotNetExtensions;
using Godot;

namespace GodotExtensions
{
    public static class SignalHelper
    {
        public delegate void DelegateParameterless();
        public static Godot.Error Connect(this Object obj, string signalName, Object owner, DelegateParameterless method)
        {
            Error error = obj.Connect(signalName, owner, method.Method.Name);
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }
        
        public static Godot.Error Connect(this Object obj, string signalName, ObjectExt owner, DelegateParameterless method)
        {
            Error error = obj.Connect(signalName, owner, method.Method.Name);
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }
        
        public static Godot.Error Connect(this Object obj, string signalName, NodeExt owner, DelegateParameterless method)
        {
            Error error = obj.Connect(signalName, owner, method.Method.Name);
            if(error != Error.Ok)
                GDLogger.Error(owner, GenerateSignalErrorString(obj, signalName, error));
            return error;
        }

        static string GenerateSignalErrorString(Object obj, string signalName, Error error)
        {
            return $"Error connecting signal \"{signalName}\" to {obj.GetType()}: {error}";
        }
    }
}