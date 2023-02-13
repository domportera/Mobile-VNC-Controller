using Godot;

namespace GodotExtensions.GDScript_Extension_Methods
{
    public static class CallDeferredExtensions
    {
        public static void GrabFocus(this Control control)
        {
            control.CallDeferred("grab_focus");
        }
    }
}