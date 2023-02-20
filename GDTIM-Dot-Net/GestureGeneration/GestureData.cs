using System;
using System.Collections.Generic;
using System.Linq;
using GDTIMDotNet;
using Godot;

namespace GDTIMDotNet.GestureGeneration
{
    public struct RawPinchTwistData
    {
        public RawPinchTwistData(Touch touch1, Touch touch2, float separationAmount)
        {
            Touch1 = touch1;
            Touch2 = touch2;
            SeparationAmount = separationAmount;
        }

        public Touch Touch1 { get; }
        public Touch Touch2 { get; }
        public float SeparationAmount { get; }
    }

    public struct RawMultiDragData
    {
        public IReadOnlyCollection<Touch> Touches { get; }

        public Vector2 Center
        {
            get
            {
                Vector2 totalPosition = Vector2.Zero;
                foreach (Touch touch in Touches)
                    totalPosition += touch.Position;

                return totalPosition / Touches.Count;
            }
        }

        public RawMultiDragData(IReadOnlyCollection<Touch> touches)
        {
            Touches = touches;
        }
        
    }

    public struct MultiDragData
    {
        public IReadOnlyList<Touch> Touches { get; }
        public float DirectionRadians { get; }
        public float DirectionDegrees => Mathf.Rad2Deg(DirectionRadians);

        public MultiDragData(Touch[] touches)
        {
            Touches = touches;
            DirectionRadians = touches.Average(x => x.DirectionRadians);
        }
    }
}