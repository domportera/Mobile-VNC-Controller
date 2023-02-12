using System.Collections.Generic;
using Godot;
using GodotExtensions;
using GodotExtensions.GDScript_Extension_Methods;
using GodotExtensions.TypeConversions;
using Object = Godot.Object;
using System.Linq;

public class RawGesture
{
    public class Event: IGodotConversionBase
    {
        public float time {get; private set;}
        public int index {get; private set; }
        
        public virtual void SetValuesFromObject(Object obj)
        {
            time = obj.GetFloat("time");
            index = obj.GetInt("index");
        }
        
        protected static Vector2 GetVec2(Object obj, string name)
        {
            return Vector2.Zero; //(Vector2)obj.Get(name);
        }

        public override string ToString()
        {
            return $"({nameof(Event)}) Time: {time.ToString("f2")}, Index: {index.ToString()}\n";
        }
    }

    public class Touch : Event
    {
        public Vector2 position {get; private set;}
        public bool pressed {get; private set;}
        
        public override void SetValuesFromObject(Object obj)
        {
            base.SetValuesFromObject(obj);
            position = obj.GetVec2("position");
            pressed = obj.GetBool("pressed");
        }

        public override string ToString()
        {
            return $"({nameof(Touch)}) nameof{position.ToString("f2")}: {position.ToString("f2")}\n" +
                   $"Pressed: {pressed.ToString()}\n" + base.ToString();
        }
    }

    public class Drag : Event
    {
        Vector2 position  = Vector2.Zero;
        Vector2 relative  = Vector2.Zero;
        Vector2 speed     = Vector2.Zero;

        public override void SetValuesFromObject(Object obj)
        {
            base.SetValuesFromObject(obj);
            position = obj.GetVec2("position");
            relative = obj.GetVec2("relative");
            speed = obj.GetVec2("speed");
        }
        
        public override string ToString()
        {
            return $"({nameof(Touch)}) {nameof(position)}: {position.ToString("f2")}\n" +
                   $"{nameof(relative)}: {relative.ToString("f2")}\n" +
                   $"{nameof(speed)}: {speed.ToString("f2")}\n" + base.ToString();
        }
    }

    Dictionary<int, Touch> presses; // Dictionary # Touch
    Dictionary<int, Touch> releases;// Dictionary # Touch
    Dictionary<int, Drag> drags;    // Dictionary # Drag
   // Dictionary<int, string> history; //Dictionary # Array of events

    public int ActiveTouches { get; }
    public float StartTime { get; }
    public float ElapsedTime { get; }

    public RawGesture(object rawGestureObj)
    {
        Godot.Object rawGesture = (Godot.Object)rawGestureObj;
        
        ActiveTouches = (int)rawGesture.Get("active_touches");
        StartTime = (float)rawGesture.Get("start_time");
        ElapsedTime = (float)rawGesture.Get("elapsed_time");
        
        var gdpresses = (Godot.Collections.Dictionary)rawGesture.Get("presses");
        var gdreleases = (Godot.Collections.Dictionary)rawGesture.Get("releases");
        var gddrags = (Godot.Collections.Dictionary)rawGesture.Get("drags");
        //var gdhistory = (Godot.Collections.Dictionary)rawGesture.Get("history");

        presses = gdpresses.ToDictionary<int, Touch>();
        releases = gdreleases.ToDictionary<int, Touch>();
        drags = gddrags.ToDictionary<int, Drag>();
    }

    public override string ToString()
    {
        return $"Raw gesture:\n" +
               $"active_touches: {ActiveTouches.ToString()}\n" +
               $"start_time: {StartTime.ToString("f3")}\n" +
               $"elapsed_time: {ElapsedTime.ToString("f3")}\n" +
               $"presses: {presses.Count.ToString()}\n" +
               $"releases: {releases.Count.ToString()}\n" +
               $"drags: {drags.Count.ToString()}";// +
             // $"history: {history.Count.ToString()}\n";
    }
}