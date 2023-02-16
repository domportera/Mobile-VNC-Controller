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
        public float Time {get; private set;}
        public int Index {get; private set; }
        
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
            return $"({nameof(Event)}) {nameof(Time)}: {Time.ToString("f2")}, {nameof(Index)}: {Index.ToString()}\n";
        }
    }

    public class Touch : Event
    {
        public Vector2 Position {get; private set;}
        public bool Pressed {get; private set;}
        
        public override void SetValuesFromObject(Object obj)
        {
            base.SetValuesFromObject(obj);
            Position = obj.GetVec2("position");
            Pressed = obj.GetBool("pressed");
        }

        public override string ToString()
        {
            return $"({nameof(Touch)}) {nameof(Position)}: {Position.ToString("f2")}\n" +
                   $"{nameof(Pressed)}: {Pressed.ToString()}\n" + base.ToString();
        }
    }

    public class Drag : Event
    {
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

    public Dictionary<int, Touch> Presses {get;} // Dictionary # Touch
    public Dictionary<int, Touch> Releases {get;}// Dictionary # Touch
    public Dictionary<int, Drag> Drags {get;}    // Dictionary # Drag
   // Dictionary<int, string> history; //Dictionary # Array of events

    public int ActiveTouches { get; }
    public float StartTime { get; }
    public float ElapsedTime { get; }

    public RawGesture(object rawGestureObj, bool clearReleasesFromPresses = true)
    {
        var rawGesture = (Godot.Object)rawGestureObj;
        
        ActiveTouches = (int)rawGesture.Get("active_touches");
        StartTime = (float)rawGesture.Get("start_time");
        ElapsedTime = (float)rawGesture.Get("elapsed_time");
        
        var gdpresses = (Godot.Collections.Dictionary)rawGesture.Get("presses");
        var gdreleases = (Godot.Collections.Dictionary)rawGesture.Get("releases");
        var gddrags = (Godot.Collections.Dictionary)rawGesture.Get("drags");
        //var gdhistory = (Godot.Collections.Dictionary)rawGesture.Get("history");

        Presses = gdpresses.ToDictionary<int, Touch>();
        Releases = gdreleases.ToDictionary<int, Touch>();
        Drags = gddrags.ToDictionary<int, Drag>();
        
        if(clearReleasesFromPresses)
            RemoveReleasesFromPresses();
    }

    public override string ToString()
    {
        return $"Raw gesture:\n" +
               $"active_touches: {ActiveTouches.ToString()}\n" +
               $"start_time: {StartTime.ToString("f3")}\n" +
               $"elapsed_time: {ElapsedTime.ToString("f3")}\n" +
               $"presses: {Presses.Count.ToString()}\n" +
               $"releases: {Releases.Count.ToString()}\n" +
               $"drags: {Drags.Count.ToString()}";// +
             // $"history: {history.Count.ToString()}\n";
    }

    public void PrintTouchData()
    {
        string log = this.ToString() + "\n \n";

        foreach (var p in Presses)
        {
            log += $"Press {p.Key.ToString()}: {p.Value}\n";
        }

        log += "\n";
        
        foreach (var p in Releases)
        {
            log += $"Release {p.Key.ToString()}: {p.Value}\n";
        }
        
        log += "\n";
        
        foreach (var p in Drags)
        {
            log += $"Drags {p.Key.ToString()}: {p.Value}";
        }
        
        GDLogger.Log(this, log);
    }

    /// <summary>
    /// Currently, Touch Input Manager still contains the Presses after they have been released
    /// So I just remove it manually here for now
    /// </summary>
    void RemoveReleasesFromPresses()
    {
        foreach (var kvp in Releases)
        {
            Presses.Remove(kvp.Key);
        }
    }
}