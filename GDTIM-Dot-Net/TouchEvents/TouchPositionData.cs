using Godot;
using static GDTIMDotNet.UnitConstants;
namespace GDTIMDotNet
{
    public struct TouchPositionData //translation into c++ may require this to be a Godot.Reference
    {
        public TouchPositionData(double time, Vector2 position, float dpi)
        {
            Time = time;
            Position = position;
            Speed = 0;
            _dpi = dpi;
            PositionDelta = Vector2.Zero;
            DistanceTraveled = 0f;
        }

        public TouchPositionData(double time, double previousTime, Vector2 position, Vector2 relative, float dpi)
        {
            Time = time;
            Position = position;
            _dpi = dpi;
            DistanceTraveled = relative.DistanceTo(Vector2.Zero);
            PositionDelta = relative;
            Speed = DistanceTraveled / (time - previousTime);
        }

        readonly float _dpi;
        public double Time { get; }
        public Vector2 Position { get; }
        public Vector2 PositionDelta { get; }
        public float DistanceTraveled { get; }
        public double Speed { get; }
        
        public Vector2 PositionDeltaInches => PositionDelta * _dpi;
        public Vector2 PositionDeltaCm => PositionDeltaInches * InchesToCmF;
        public Vector2 PositionDeltaMm => PositionDeltaCm * CmToMm;
        
        public Vector2 PositionInches => Position * _dpi;
        public Vector2 PositionCm => PositionInches * InchesToCmF;
        public Vector2 PositionMm => PositionCm * CmToMm;
        
        public double SpeedInches => Speed * _dpi;
        public double SpeedCm => SpeedInches * InchesToCmD;
        public double SpeedMm => SpeedCm * CmToMm;

        public float DistanceTraveledInches => DistanceTraveled * _dpi;
        public float DistanceTraveledCm => DistanceTraveledInches * InchesToCmF;
        public float DistanceTraveledMm => DistanceTraveledCm * CmToMm;

        public override string ToString()
        {
            return nameof(TouchPositionData); //todo: useful data lol
        }
        
    }
}