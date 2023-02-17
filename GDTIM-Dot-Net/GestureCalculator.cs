using System;
using Godot;

namespace GDTIMDotNet
{
    internal class GestureCalculator
    {
        public GestureCalculator(MultiTouch touchProvider)
        {
            touchProvider.TouchAdded += OnTouchAdded;
            touchProvider.TouchRemoved += OnTouchRemoved;
            touchProvider.Process += OnProcess;
        }

        void OnProcess(object sender, double time)
        {
            throw new System.NotImplementedException();
        }

        void OnTouchRemoved(object sender, Touch touch)
        {
            touch.Updated -= TouchUpdated;
        }

        void OnTouchAdded(object sender, Touch touch)
        {
            touch.Updated += TouchUpdated;
        }

        void TouchUpdated(object sender, EventArgs _)
        {
            Touch touch = (Touch)sender;
            bool isDragging = IsDragging(touch);
        }

        static bool IsDragging(Touch touch)
        {
            
        }
    }
}