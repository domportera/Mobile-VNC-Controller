using Godot;
using GodotExtensions;

namespace GDTIMDotNet.GestureReceiving
{
    public interface IGestureConsumer
    {
        void OnSingleTouch(object sender, TouchBegin e);

        void OnSingleDrag(object sender, SingleDrag e);

        void OnSingleLongPress(object sender, SingleTap e);

        void OnSingleSwipe(object sender, SingleDrag e);

        void OnSingleTap(object sender, SingleTap e);

        void OnTwist(object sender, Twist e);

        void OnMultiDrag(object sender, MultiDrag e);

        void OnMultiLongPress(object sender, MultiTap e);

        void OnMultiSwipe(object sender, MultiDrag e);

        void OnMultiTap(object sender, MultiTap e);

        void OnPinch(object sender, Pinch e);
    }
}