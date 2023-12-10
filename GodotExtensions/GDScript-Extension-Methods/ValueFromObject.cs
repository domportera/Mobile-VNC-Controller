using Godot;

namespace GodotExtensions.GDScript_Extension_Methods
{
    public static class ValueFromObject
    {
        public static float GetFloat(this GodotObject obj, string name) => (float)obj.Get(name);
        public static int GetInt(this GodotObject obj, string name) => (int)obj.Get(name);
        public static bool GetBool(this GodotObject obj, string name) => (bool)obj.Get(name);
        public static string GetString(this GodotObject obj, string name) => (string)obj.Get(name);
        public static Vector2 GetVec2(this GodotObject obj, string name) => (Vector2)obj.Get(name);
        public static Vector3 GetVec3(this GodotObject obj, string name) => (Vector3)obj.Get(name);
        public static Quaternion GetQuat(this GodotObject obj, string name) => (Quaternion)obj.Get(name);

    }
}