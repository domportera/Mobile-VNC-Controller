using Godot;

namespace GodotExtensions
{
    public static class ControlExtensions 
    {
        public static Vector2 RealRectSize(this Control control)
        {
            return control.RectSize * control.RectScale;
        }
        public static Vector2 RealPixelSize(this Control control)
        {
            return control.RealPixelSize(OS.WindowSize);
        }

        public static Vector2 RealPixelSize(this Control control, Vector2 resolution)
        {
            var anchorSize = new Vector2(control.AnchorRight - control.AnchorLeft,
                control.AnchorBottom - control.AnchorTop);
            var marginSize = new Vector2(control.MarginLeft + control.MarginRight,
                control.MarginTop + control.MarginBottom);
        
            return resolution * anchorSize + marginSize;
        }
        
        public static void MaximizeAnchors(this Control control)
        {
            control.AnchorLeft = 0;
            control.AnchorRight = 1;
            control.AnchorTop = 0;
            control.AnchorBottom = 1;
        }
    }
}
