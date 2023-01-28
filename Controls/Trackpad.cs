using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;

namespace GDTIMDotNet
{
    public class Trackpad : GestureConsumer<GuiConstrainedGestureInterpreter>
    {
        GuiConstrainedGestureInterpreter _interpreter;
        VncHandler _vncHandler;
        [Export] float _mouseSpeed = 100f;
        [Export] float _scrollSpeed = 10f;
        [Export] string _vncHandlerPathRelative = "../../VncHandler";
        [Export] bool _shouldSendVncCommands = true;
        [Export] bool _mouseAcceleration = true;
        bool _longPressed;
	
        Vector2 _cumulativeScroll;
        float _deltaTime;
        float TimeAdjustment => _deltaTime;
        
        protected override void OnReady(GuiConstrainedGestureInterpreter interpreter)
        {
            _interpreter = interpreter;

            if (_interpreter is null)
            {
                GDLogger.Log(this,$"Interpreter received is not the expected type. Interpreter is {interpreter.GetType()}," +
                             $"while we were expecting {nameof(GuiConstrainedGestureInterpreter)}");
            }
            
            _vncHandler = GetNode(_vncHandlerPathRelative) as VncHandler;
        }
        
        public override void _Process(float delta)
        {
            base._Process(delta);
            _deltaTime = delta;
        }

        protected override void OnTwist(object sender, TwistArgs e)
        {
            GDLogger.Log(this,"Twist");
        }

        protected override void OnSingleLongPress(object sender, Vector2 e)
        {
            LongPress(true);
        }

        protected override void OnSingleDrag(object sender, SingleDragArgs e)
        {
            Vector2 trackpadSize = _interpreter.ControlRealSize;
            float minTrackpadDimension = Mathf.Min(trackpadSize.x, trackpadSize.y);
		
            Vector2 serverResolution = _vncHandler.Resolution;
            float minServerResolution = Mathf.Min(serverResolution.x, serverResolution.y);
		
            Vector2 dragDelta = e.Relative / minTrackpadDimension;
            float dragAccelerationT = dragDelta.LengthSquared();
            //float dragAccelerationT = dragDelta.Length();

            float speed = _mouseSpeed * TimeAdjustment;
            speed = _mouseAcceleration ? speed + _mouseSpeed * dragAccelerationT : speed;
            Vector2 moveAmount = dragDelta * speed * minServerResolution;
		
            MoveMouse(moveAmount);
        }

        protected override void OnSingleTap(object sender, Vector2 e)
        {
            _vncHandler.MouseButtonDown(MouseButton.Left);
            _vncHandler.MouseButtonUp(MouseButton.Left);
        }
        
        protected override void OnSingleSwipe(object sender, SingleDragArgs e)
        {
            GDLogger.Log(this,"Single Swipe");
        }

        protected override void OnMultiLongPress(object sender, MultiTapArgs e)
        {
            GDLogger.Log(this, "Multi long press");
        }

        protected override void OnMultiSwipe(object sender, MultiDragArgs e)
        {
            if (e.Fingers != 2)
                return;

            Scroll(e.Relative);
        }

        protected override void OnMultiTap(object sender, MultiTapArgs e)
        {
            GDLogger.Log(this,"Multi tap");
            if (e.Fingers == 2)
            {
                RightClick();
            }
            else if (e.Fingers == 3)
            {
                MiddleClick();
            }
        }

        protected override void OnSingleTouch(object sender, SingleTouchArgs e)
        {
            _cumulativeScroll = Vector2.Zero;
            LongPress(false);
        }

        protected override void OnPinch(object sender, PinchArgs e)
        {
            GDLogger.Log(this, "Pinch");
        }

        protected override void OnMultiDrag(object sender, MultiDragArgs e)
        {
            Scroll(e.Relative);
        }
        void RightClick()
        {
            if (!_shouldSendVncCommands) return;
            _vncHandler.MouseButtonDown(MouseButton.Right);
            _vncHandler.MouseButtonUp(MouseButton.Right);
        }

        void MiddleClick()
        {
            if (!_shouldSendVncCommands) return;
            _vncHandler.MouseButtonDown(MouseButton.Middle);
            _vncHandler.MouseButtonUp(MouseButton.Middle);
        }
        void Scroll(Vector2 relative)
        {
            if (!_shouldSendVncCommands) return;
            bool vertical = Mathf.Abs(relative.y) > Mathf.Abs(relative.x);
            Vector2 realSize = _interpreter.ControlRealSize;

            if(vertical)
                _cumulativeScroll.y += relative.y / realSize.y * _scrollSpeed;
            else
                _cumulativeScroll.x += relative.x / realSize.x * _scrollSpeed;

            var scrollAmount = new Vector2((int)_cumulativeScroll.x, (int)_cumulativeScroll.y);
            _vncHandler.Scroll(scrollAmount);
		
            if (_cumulativeScroll.x > 1f || _cumulativeScroll.x < -1f)
            {
                _cumulativeScroll.x = 0f;
            }
            if (_cumulativeScroll.y > 1f || _cumulativeScroll.y < -1f)
            {
                _cumulativeScroll.y = 0f;
            }
        }

        void MoveMouse(Vector2 moveAmount)
        {
            if (!_shouldSendVncCommands) return;
            _vncHandler.MoveMouse(moveAmount);
        }

	
        void LongPress(bool pressed)
        {
            if (_shouldSendVncCommands)
            {
                if (pressed)
                {
                    _vncHandler.MouseButtonDown(MouseButton.Left);
                    Input.VibrateHandheld(50);
                }
                else if (_longPressed)
                    _vncHandler.MouseButtonUp(MouseButton.Left);
            }

            _longPressed = pressed;
        }
	
    }
}