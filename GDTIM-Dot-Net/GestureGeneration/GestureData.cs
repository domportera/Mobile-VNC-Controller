using System.Collections.Generic;
using Godot;

namespace GDTIMDotNet.GestureGeneration
{

    interface IMultiFingerGesture
    {
        Vector2 Center { get; }
        Vector2 CenterDelta { get; }
    }

    interface ITwoFingerGesture
    {
        RawTwoFingerDragData RawData { get; }
        Touch Touch1 { get; }
        Touch Touch2 { get; }
    }
    

    public struct RawTwoFingerDragData
    {
        public RawTwoFingerDragData(Touch touch1, Touch touch2, float separationAmount,
            float twistRadians, Vector2 centroid, Vector2 centroidDelta,
            DragRelationshipType relationship)
        {
            Touch1 = touch1;
            Touch2 = touch2;
            SeparationAmount = separationAmount;
            Relationship = relationship;
            Centroid = centroid;
            CentroidDelta = centroidDelta;
            TwistRadians = twistRadians;
        }

        public Touch Touch1 { get; }
        public Touch Touch2 { get; }
        public float SeparationAmount { get; }
        public float TwistRadians { get; }
        public float TwistDegrees => Mathf.Rad2Deg(TwistRadians);
        public DragRelationshipType Relationship { get; }

        public Vector2 Centroid { get; }
        public Vector2 CentroidDelta { get; }

    }

    public struct RawMultiDragData
    {
        public IReadOnlyCollection<Touch> Touches { get; }
        public Vector2 Center => Touches.Centroid();
        public Vector2 CenterDelta => Touches.CentroidDelta();

        public RawMultiDragData(IReadOnlyCollection<Touch> touches)
        {
            Touches = touches;
        }

    }

    public struct PinchData : ITwoFingerGesture
    {
        public PinchData(ref RawTwoFingerDragData rawData)
        {
            RawData = rawData;
        }

        public Vector2 Center => RawData.Centroid;
        public Vector2 CenterDelta => RawData.CentroidDelta;

        public RawTwoFingerDragData RawData { get; }
        public float SeparationAmount => RawData.SeparationAmount;
        public Touch Touch1 => RawData.Touch1;
        public Touch Touch2 => RawData.Touch2;
    }

    public struct TwistData : ITwoFingerGesture
    {
        public TwistData(ref RawTwoFingerDragData rawData)
        {
            RawData = rawData;
        }

        public Vector2 Center => RawData.Centroid;
        public Vector2 CenterDelta => RawData.CentroidDelta;
        public RawTwoFingerDragData RawData { get; }
        public Touch Touch1 => RawData.Touch1;
        public Touch Touch2 => RawData.Touch2;
        public float TwistRadians => RawData.TwistRadians;
        public float TwistDegrees => RawData.TwistDegrees;
    }

    public struct MultiDragData
    {
        public IReadOnlyList<Touch> Touches { get; }
        public float DirectionRadians => Touches.AverageDirectionRadians();
        public float DirectionDegrees => Touches.AverageDirectionDegrees();
        public Vector2 Center => Touches.Centroid();
        public Vector2 CenterDelta => Touches.CentroidDelta();

        public MultiDragData(Touch[] touches)
        {
            Touches = touches;
        }
    }

    public struct MultiLongPressData
    {
        public MultiLongPressData(IReadOnlyList<Touch> touches)
        {
            Touches = touches;
        }

        public float DirectionRadians => Touches.AverageDirectionRadians();
        public float DirectionDegrees => Touches.AverageDirectionDegrees();
        public IReadOnlyList<Touch> Touches { get; }
        public Vector2 Center => Touches.Centroid();
        public Vector2 CenterDelta => Touches.CentroidDelta();
    }

    public struct MultiSwipeData
    {
        public MultiSwipeData(IReadOnlyList<Touch> touches)
        {
            Touches = touches;
        }

        public IReadOnlyList<Touch> Touches { get; }
        public double AverageSpeed => Touches.AverageSpeed();
        public double AverageSpeedInches => Touches.AverageSpeedInches();
        public double AverageSpeedCm => Touches.AverageSpeedCm();
        public double AverageSpeedMm => Touches.AverageSpeedMm();
        public double MaxSpeed => Touches.MaxSpeed();
        public double MaxSpeedInches => Touches.MaxSpeedInches();
        public double MaxSpeedCm => Touches.MaxSpeedCm();
        public double MaxSpeedMm => Touches.MaxSpeedMm();
        
    }
    
    public struct MultiTapData
    {
        public MultiTapData(IReadOnlyList<Touch> touches)
        {
            Touches = touches;
        }

        public IReadOnlyList<Touch> Touches { get; }
        public Vector2 Center => Touches.Centroid();
    }

    #region Unused - unnecessary for now
    public struct SingleTapData
    {
        public SingleTapData(Touch touch)
        {
            Touch = touch;
            Position = touch.Position;
        }

        public Touch Touch { get; }
        public Vector2 Position { get; }
        public double Duration => Touch.TimeAlive;
    }

    public struct LongPressData
    {
        public LongPressData(Touch touch)
        {
            Touch = touch;
            Position = touch.Position;
        }
        
        public Touch Touch { get; }
        public Vector2 Position { get; }
    }

    public struct SingleDragData
    {
        public SingleDragData(Touch touch)
        {
            Touch = touch;
            Position = touch.Position;
            PositionDelta = touch.PositionDelta;
        }

        public Touch Touch { get; }
        public Vector2 Position { get; }
        public Vector2 PositionDelta { get; }
        public double Speed => Touch.Speed;
        public double SpeedInches => Touch.SpeedInches;
        public double SpeedCm => Touch.SpeedCm;
        public double SpeedMm => Touch.SpeedMm;
    }
    #endregion
}