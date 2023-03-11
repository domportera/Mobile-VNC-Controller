using Godot;
using GodotExtensions.GDScript_Extension_Methods;

namespace NiceTouch
{
    public class Drag : TouchEvent
    {
        public Drag(){}
        public Drag(double time, int index, Vector2 position, Vector2 relative, Vector2 speed) : base(time, index)
        {
            Position = position;
            Relative = relative;
            Speed = speed;
        }

        public Vector2 Position {get; private set; }
        public Vector2 Relative {get; private set; }
        public Vector2 Speed    {get; private set; }

        public override void SetValuesFromObject(Object obj)
        {
            base.SetValuesFromObject(obj);
            Position = obj.GetVec2("position");
            Relative = obj.GetVec2("relative");
            Speed = obj.GetVec2("speed");
        }
        
        public override string ToString()
        {
            return $"({nameof(Touch)}) {nameof(Position)}: {Position.ToString("f2")}\n" +
                   $"{nameof(Relative)}: {Relative.ToString("f2")}\n" +
                   $"{nameof(Speed)}: {Speed.ToString("f2")}\n" + base.ToString();
        }
    }
}