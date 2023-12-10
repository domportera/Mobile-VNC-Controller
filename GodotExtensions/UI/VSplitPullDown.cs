using Godot;

namespace GodotExtensions
{
    public partial class VSplitPullDown : VSplitContainer
    {
        [Export] float _defaultHeight = 0.3f;
        
        Control _panel;
        int DefaultHeight => (int)(OS.WindowSize.y * _defaultHeight);
        bool _canToggleMouseFilter = true;
        const int DragItemHeight = 12;
        const int TouchDragItemHeight = 24;

        public override void _Ready()
        {
            base._Ready();
            if (GetChildCount() > 2)
            {
                GD.PrintErr($"{nameof(VSplitPullDown)} should only contain two child elements - " +
                            $"your drop down contents and a transparent mouse-pass-through control.");
            }
            else if (GetChildCount() < 2)
            {
                GD.PrintErr($"{nameof(VSplitPullDown)} should contain two child elements - " +
                            $"your drop down contents and a transparent mouse-pass-through control.");
            }

            _panel = GetChild(0) as Control;
            var passthrough = GetChild(1) as Control;
            passthrough.MouseFilter = MouseFilterEnum.Ignore;
        }

        public override void _Input(InputEvent @event)
        {
            switch (@event)
            {
                case InputEventScreenTouch screenTouch:
                    ChangeMouseFilterBasedOnPosition(screenTouch.Position, TouchDragItemHeight);
                    break;
                case InputEventScreenDrag drag:
                    break;
                case InputEventMouseButton buttonPress:
                    _canToggleMouseFilter = !buttonPress.Pressed;
                    break;
                case InputEventMouseMotion motion:
                    if (_canToggleMouseFilter)
                    {
                        ChangeMouseFilterBasedOnPosition(motion.Position, DragItemHeight);
                    }
                    break;
            }
		
            base._Input(@event);
		
            void ChangeMouseFilterBasedOnPosition(Vector2 position, int dragItemHeight)
            {
                MouseFilter = position.y > _panel.Size.y + dragItemHeight
                    ? MouseFilterEnum.Ignore
                    : MouseFilterEnum.Stop;
            }
        }

        public void ToggleWithButton()
        {
            SplitOffset = SplitOffset == 0 ? DefaultHeight : 0;
        }
        
    }
}