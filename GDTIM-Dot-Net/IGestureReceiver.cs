using Godot;

/// <summary>
/// Interface that must be implemented to receive events from InputManager.gd
/// Once inherited, you will want to autoload your script and modify InputManager.gd to reference that??
/// </summary>
public interface IGestureReceiver
{
    void OnSingleTouch(Vector2 position, bool pressed, bool cancelled, int index, object rawGesture);
   
    void OnSingleDrag(Vector2 position, Vector2 relative, int index, object rawGesture);

    void OnSingleLongPress(Vector2 position, int index, object rawGesture);

    void OnSingleSwipe(Vector2 position, Vector2 relative, int index, object rawGesture);

    void OnSingleTap(Vector2 position, int index, object rawGesture);

    void OnTwist(Vector2 position, float relative, int fingers, object rawGesture);

    void OnMultiDrag(Vector2 position, Vector2 relative, int fingers, object rawGesture);

    void OnMultiLongPress(Vector2 position, int fingers, object rawGesture);

    void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers, object rawGesture);

    void OnMultiTap(Vector2 position, int fingers, object rawGesture);

    void OnPinch(Vector2 position, float relative, float distance, int fingers, object rawGesture);
}