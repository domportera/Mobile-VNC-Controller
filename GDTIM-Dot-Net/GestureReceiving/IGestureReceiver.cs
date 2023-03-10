using GDTIMDotNet.GestureGeneration;
using Godot;

namespace GDTIMDotNet.GestureReceiving
{
    /// <summary>
    /// Interface that must be implemented to receive events from InputManager.gd
    /// Once inherited, you will want to autoload your script and modify InputManager.gd to reference that??
    /// </summary>
    public interface IGestureReceiver // TODO: should use ref structs instead (c# 7)
    {
        void OnSingleTouch(object sender, TouchData touchData);

        void OnSingleDrag(object sender, Touch touch);

        void OnSingleLongPress(object sender, Touch touch);

        void OnSingleSwipe(object sender, Touch touch);

        void OnSingleTap(object sender, Touch touch);

        void OnTwist(object sender, TwistData twistData);

        void OnMultiDrag(object sender, MultiDragData multiDragData);

        void OnMultiLongPress(object sender, MultiLongPressData multiLongPressData);

        void OnMultiSwipe(object sender, MultiSwipeData multiSwipeData);

        void OnMultiTap(object sender, MultiTapData multiTapData);

        void OnPinch(object sender, PinchData pinchData);
        void OnRawMultiDrag(object sender, RawMultiDragData e);
        void OnRawPinchTwist(object sender, RawTwoFingerDragData e);
    }
}