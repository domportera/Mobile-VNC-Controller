using Godot;
using GodotExtensions;

namespace GDTIMDotNet
{
    public interface IGestureConsumer
    {
        void OnSingleSwipe(object sender, SingleDragArgs e);

        void OnMultiLongPress(object sender, MultiTapArgs e);

        void OnMultiSwipe(object sender, MultiDragArgs e);
        
        void OnTwist(object sender, TwistArgs e);

        void OnSingleLongPress(object sender, SingleTapArgs e);

        void OnSingleDrag(object sender, SingleDragArgs e);

        void OnSingleTap(object sender, SingleTapArgs e);

        void OnMultiTap(object sender, MultiTapArgs e);

        void OnSingleTouch(object sender, SingleTouchArgs e);

        void OnPinch(object sender, PinchArgs e);

        void OnMultiDrag(object sender, MultiDragArgs e);
    }
}