#define ERROR_CHECK_GDTIM

using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions;
using static GDTIMDotNet.UnitConstants;

namespace GDTIMDotNet
{
    public class Touch // Nice Touch ™
    {
        const double DragHistoryDuration = 0.3;
        public Touch(double time, int index, Vector2 position)
        {
            Dpi = OS.GetScreenDpi();
            Current = new TouchPositionData(time, position, Dpi);
            _history.Enqueue(Current);
            StartPosition = position;
            Index = index;
            StartTime = time;
        }
        
        readonly Queue<TouchPositionData> _history = new Queue<TouchPositionData>();
        public readonly float Dpi;

        public event EventHandler Updated;
        public TouchPositionData Current;
        
        public int Index {get; }
        public double LastUpdateTime => Current.Time;
        public double TimeAlive => LastUpdateTime - StartTime;
        public double StartTime { get; }
        
        public Vector2 StartPosition { get; }
        public Vector2 StartPositionInches => StartPosition * Dpi;
        public Vector2 StartPositionCm => StartPositionInches * InchesToCmF;
        public Vector2 StartPositionMm => StartPositionCm * CmToMm;
        
        public Vector2 Position => Current.Position;
        
        public Vector2 PositionCm => Current.PositionCm;
        public Vector2 PositionInches => Current.PositionInches;
        public Vector2 PositionMm => Current.PositionMm;
        public Vector2 PositionDelta => Current.PositionDelta;
        
        public double Speed => Current.Speed;
        public double SpeedCm => Current.SpeedCm;
        public double SpeedMm => Current.SpeedMm;
        public double SpeedInches => Current.SpeedInches;
        
        
        public float TotalDistanceTraveled { get; private set; } = 0f;
        public float TotalDistanceTraveledInches => TotalDistanceTraveled * Dpi;
        public float TotalDistanceTraveledCm => TotalDistanceTraveledInches * InchesToCmF;
        public float TotalDistanceTraveledMm => TotalDistanceTraveledCm * CmToMm;

        public Vector2 PositionSmoothedSimple => (Current.Position + _history.Peek().Position) / 2f;
        public Vector2 DeltaSmoothedSimple => (Current.PositionDelta + _history.Peek().PositionDelta) / 2f;
        public double SpeedSmoothedSimple => (Current.Speed + _history.Peek().Speed) / 2;

        
        const float TapWiggleThresholdMm = 5f;
        public bool CanBeATap => TotalDistanceTraveledMm < TapWiggleThresholdMm;
        
        
        public override string ToString()
        {
            return $"({nameof(Touch)}): {Current.ToString()}";
        }

        // todo: speeds will not be updated as this Update function currently wont be called when touch is still
        public void Update(double time, Vector2 position, Vector2 relative)
        {
#if ERROR_CHECK_GDTIM
            if(position != Current.Position + relative)
                GDLogger.Error(this, $"Touch {Index.ToString()} missed an update!");
#endif

            bool hasHistory = _history.Count > 0;
            TouchPositionData oldest = hasHistory ? _history.Peek() : Current;
            var positionData = new TouchPositionData(time, LastUpdateTime, position, relative, Dpi);
            
            TotalDistanceTraveled += positionData.DistanceTraveled;
            Current = positionData;
            
            _history.Enqueue(positionData);

            double timeElapsed = time - oldest.Time;
            if (/*hasHistory && */timeElapsed > DragHistoryDuration) // add check if changing data structures for a more advanced "smoothing" system
                _history.Dequeue();
            
#if ERROR_CHECK_GDTIM
            if (Updated == null)
            {
                GDLogger.Error(this, $"Touch {Index.ToString()} does not have any listeners");
                return;
            }
            
            Updated.Invoke(this, EventArgs.Empty);
#else
            Updated.Invoke(this, EventArgs.Empty);
#endif
        }
    }
}