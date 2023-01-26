using Godot;
using GodotExtensions;

namespace GDTIMDotNet
{
    public abstract class GestureConsumer<T> : Node where T : GestureInterpreter
    {
        [Export] string _gestureInterpreterRelativePath;

        public override void _Ready()
        {
            base._Ready();
            T gestureInterpreter = GetNode(_gestureInterpreterRelativePath) as T;

            if (gestureInterpreter is null)
            {
                GDLogger.Error(this, $"No {nameof(GestureInterpreter)} found at {GetPath()}/{_gestureInterpreterRelativePath}");
                return;
            }
            
            gestureInterpreter.MultiDrag += OnMultiDrag;
            gestureInterpreter.MultiSwipe += OnMultiSwipe;
            gestureInterpreter.Pinch += OnPinch;
            gestureInterpreter.SingleTouch += OnSingleTouch;
            gestureInterpreter.MultiTap += OnMultiTap;
            gestureInterpreter.SingleTap += OnSingleTap;
            gestureInterpreter.SingleDrag += OnSingleDrag;
            gestureInterpreter.SingleLongPress += OnSingleLongPress;
            gestureInterpreter.MultiLongPress += OnMultiLongPress;
            gestureInterpreter.Twist += OnTwist;
            gestureInterpreter.SingleSwipe += OnSingleSwipe;
            OnReady(gestureInterpreter);
        }

        protected abstract void OnSingleSwipe(object sender, SingleDragArgs e);

        /// <summary>
        /// Use this instead of typical _Ready() function in order to avoid overriding critical functionality
        /// and to ensure that the gesture system is ready
        /// </summary>
        protected abstract void OnReady(T interpreter);
        protected abstract void OnMultiLongPress(object sender, MultiTapArgs e);

        protected abstract void OnMultiSwipe(object sender, MultiDragArgs e);
        
        protected abstract void OnTwist(object sender, TwistArgs e);

        protected abstract void OnSingleLongPress(object sender, Vector2 e);

        protected abstract void OnSingleDrag(object sender, SingleDragArgs e);

        protected abstract void OnSingleTap(object sender, Vector2 e);

        protected abstract void OnMultiTap(object sender, MultiTapArgs e);

        protected abstract void OnSingleTouch(object sender, SingleTouchArgs e);

        protected abstract void OnPinch(object sender, PinchArgs e);

        protected abstract void OnMultiDrag(object sender, MultiDragArgs e);
    }
}