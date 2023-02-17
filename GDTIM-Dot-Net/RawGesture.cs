using System.Collections.Generic;
using Godot;
using GodotExtensions;
using GodotExtensions.GDScript_Extension_Methods;
using GodotExtensions.TypeConversions;
using Object = Godot.Object;
using System.Linq;

namespace GDTIMDotNet
{
    public class RawGesture
    {
        public Dictionary<int, Touch> Presses {get;} // Dictionary # Touch
        public Dictionary<int, Touch> Releases {get;}// Dictionary # Touch
        public Dictionary<int, Drag> Drags {get;}    // Dictionary # Drag
       // Dictionary<int, string> history; //Dictionary # Array of events

        public int ActiveTouches { get; }
        public float StartTime { get; }
        public float ElapsedTime { get; }

        // todo: can this be optimized by simply updating a single raw gesture object? i think that's how the gdscript works under the hood.
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

          //  Presses = gdpresses.ToDictionary<int, Touch>();
           // Releases = gdreleases.ToDictionary<int, Touch>();
           // Drags = gddrags.ToDictionary<int, Drag>();
            
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

        public int GetDragIndex(Vector2 position, Vector2 relative)
        {
            foreach (KeyValuePair<int, Drag> dragEntry in Drags)
            {
                var drag = dragEntry.Value;
                if (drag.Relative == relative && drag.Position == position)
                    return dragEntry.Key;
            }

            return InvalidIndex;
        }

        public const int InvalidIndex = -1;

        public int GetTouchIndex(Vector2 position, bool presses)
        {
            var touches = presses ? Presses : Releases;
            foreach (KeyValuePair<int, Touch> touchEntry in touches)
            {
                if (touchEntry.Value.PositionPixels == position)
                    return touchEntry.Key;
            }
            
            return InvalidIndex;
        }
    }
}