using Godot;

namespace GDTIMDotNet
{
    public struct TouchPositionData //translation into c++ may require this to be a Godot.Reference
    {
        public TouchPositionData(double time, Vector2 position)
        {
            Time = time;
            Position = position;
            Speed = 0;
            SmoothedSpeed = 0;
            DPI = OS.GetScreenDpi();
            PositionDelta = Vector2.Zero;
        }

        // TODO: move these calculations into the Touch class and let this remain a relatively slim struct.
        // the touch is the one that should be making sense of its data
        public TouchPositionData(double time, Vector2 position, Vector2 relative)
        {
            Time = time;
            Position = position;
            DPI = OS.GetScreenDpi();
            Speed = position.DistanceTo(previous.Position) / (time - previous.Time);
            SmoothedSpeed = position.DistanceTo(final.Position) / (time - final.Time);
            PositionDelta = relative;
            SmoothPositionDelta = position - final.Position;
            
            //todo: can replace the "smooth" with another name ("historical? idfk) and do actual smoothing on these
            //values by interpolating values between this and previous/final
            // can choose to make this a class, but do I really want all those heap allocations?
        }

        Vector2 _previousPosition, _finalPositon;
        
        static float CalculateSpeed(TouchPositionData data, )

        readonly float DPI;
        public double Time { get; }
        public Vector2 Position { get; }
        public Vector2 PositionDelta { get; }
        public Vector2 SmoothPositionDelta { get; }
        public double Speed { get; }
        public double SmoothedSpeed { get; }
        
        public Vector2 PositionInches => Position * OS.GetScreenDpi();
        public Vector2 PositionCm => PositionInches * InchesToCmF;
        public Vector2 PositionMm => PositionCm * CmToMm;
        public double SpeedInches => Speed * OS.GetScreenDpi();
        public double SpeedCm => SpeedInches * InchesToCmD;
        public double SpeedMm => SmoothedSpeedCm * CmToMm;
        public double SmoothedSpeedInches => SmoothedSpeed * OS.GetScreenDpi();
        public double SmoothedSpeedCm => SmoothedSpeedInches * InchesToCmD;
        public double SmoothedSpeedMm => SmoothedSpeedCm * CmToMm;

        const float InchesToCmF = 2.54f;
        const double InchesToCmD = 2.54;
        const int CmToMm = 10;

        public override string ToString()
        {
            return nameof(TouchPositionData); //todo: useful data lol
        }
        
    }
}