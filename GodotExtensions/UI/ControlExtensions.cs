using Godot;

namespace GodotExtensions
{
    public static class ControlExtensions 
    {
        public static Vector2 RealRectSize(this Control control)
        {
            return control.Size * control.Scale;
        }
        
        public static Vector2 RealPixelSize(this Control control)
        {
            return control.RealPixelSize(OS.WindowSize);
        }

        //todo: this calculation is likely incorrect (due to the negative nature of right-and-bottom margins 
        //and should be tested for if RectSize is a better alternative
        public static Vector2 RealPixelSize(this Control control, Vector2 resolution)
        {
            var anchorSize = new Vector2(control.AnchorRight - control.AnchorLeft,
                control.AnchorBottom - control.AnchorTop);
            var marginSize = new Vector2(control.OffsetLeft + control.OffsetRight,
                control.OffsetTop + control.OffsetBottom);
        
            return resolution * anchorSize + marginSize;
        }
        
        public static void MaximizeAnchors(this Control control)
        {
            control.AnchorLeft = 0;
            control.AnchorRight = 1;
            control.AnchorTop = 0;
            control.AnchorBottom = 1;
        }

        public static bool PointIsInside(this Control control, Vector2 position)
        {
            Rect2 relevantRect = new Rect2(control.GlobalPosition, control.RealRectSize());
            return relevantRect.HasPoint(position);
        }
    }
}
