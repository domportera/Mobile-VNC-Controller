using Godot;
using GodotExtensions;

public abstract class GDTIMInterpreter : NodeExt
{
    public abstract void OnMultiDrag(Vector2 position, Vector2 relative, int fingers);
    public abstract void OnMultiLongPress(Vector2 position, int fingers);
    public abstract void OnMultiSwipe(Vector2 position, Vector2 relative, int fingers);
    public abstract void OnMultiTap(Vector2 position, int fingers);
    public abstract void OnPinch(Vector2 position, float relative, float distance, int fingers);
    public abstract void OnSingleDrag(Vector2 position, Vector2 relative);
    public abstract void OnSingleLongPress(Vector2 position);
    public abstract void OnSingleSwipe(Vector2 position, Vector2 relative);
    public abstract void OnSingleTap(Vector2 position);
    public abstract void OnSingleTouch(Vector2 position, bool pressed, bool cancelled);
    public abstract void OnTwist(Vector2 position, float relative, int fingers);
}