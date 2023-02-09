using Godot;
using GodotExtensions;

namespace GDTIMDotNet
{
    public interface IGestureConsumer
    {
        void OnSingleSwipe(object sender, SingleDrag e);

        void OnMultiLongPress(object sender, MultiTap e);

        void OnMultiSwipe(object sender, MultiDrag e);
        
        void OnTwist(object sender, Twist e);

        void OnSingleLongPress(object sender, SingleTap e);

        void OnSingleDrag(object sender, SingleDrag e);

        void OnSingleTap(object sender, SingleTap e);

        void OnMultiTap(object sender, MultiTap e);

        void OnSingleTouch(object sender, TouchBegin e);

        void OnPinch(object sender, Pinch e);

        void OnMultiDrag(object sender, MultiDrag e);
    }
}