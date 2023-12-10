using Godot;

namespace GodotExtensions.GDScript_Extension_Methods
{
    public static class ValueFromObject
    {
        public static float GetFloat(this Godot.Object obj, string name) => (float)obj.Get(name);
        public static int GetInt(this Godot.Object obj, string name) => (int)obj.Get(name);
        public static bool GetBool(this Godot.Object obj, string name) => (bool)obj.Get(name);
        public static string GetString(this Godot.Object obj, string name) => (string)obj.Get(name);
        public static Vector2 GetVec2(this Godot.Object obj, string name) => (Vector2)obj.Get(name);
        public static Vector3 GetVec3(this Godot.Object obj, string name) => (Vector3)obj.Get(name);
        public static Quaternion GetQuat(this Godot.Object obj, string name) => (Quaternion)obj.Get(name);

    }
}