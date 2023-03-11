using Godot;
using GodotExtensions.GDScript_Extension_Methods;
using GodotExtensions.TypeConversions;

namespace NiceTouch
{
    public abstract class TouchEvent: IGodotConversionBase
    {
        public double Time {get; private set;}
        public int Index {get; private set; }
        
        public TouchEvent() {}

        protected TouchEvent(double time, int index)
        {
            Time = time;
            Index = index;
        }
        
        public virtual void SetValuesFromObject(Object obj)
        {
            Time = obj.GetFloat("time");
            Index = obj.GetInt("index");
        }
        
        protected static Vector2 GetVec2(Object obj, string name)
        {
            return Vector2.Zero; //(Vector2)obj.Get(name);
        }

        public override string ToString()
        {
            return $"({nameof(TouchEvent)}) {nameof(Time)}: {Time.ToString("f2")}, {nameof(Index)}: {Index.ToString()}\n";
        }
    }
}