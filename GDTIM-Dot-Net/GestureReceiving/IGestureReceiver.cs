using Godot;

namespace GDTIMDotNet.GestureReceiving
{
    /// <summary>
    /// Interface that must be implemented to receive events from InputManager.gd
    /// Once inherited, you will want to autoload your script and modify InputManager.gd to reference that??
    /// </summary>
    public interface IGestureReceiver
    {
        void OnSingleTouch(Vector2 position, bool pressed, bool cancelled, int index);

        void OnSingleDrag(Vector2 position, Vector2 relative, int index);

        void OnSingleLongPress(Vector2 position, int index);

        void OnSingleSwipe(Vector2 position, Vector2 relative, int index);

        void OnSingleTap(Vector2 position, int index);

        void OnTwist(Vector2 position, float relative, int fingers);

        void OnMultiDrag(Vector2 position, Vector2 relative, int fingers);

        void OnMultiLongPress(Vector2 position, int fingers);

        void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers);

        void OnMultiTap(Vector2 position, int fingers, object rawGesture);

        void OnPinch(Vector2 position, float relative, float distance, int fingers);
    }
}