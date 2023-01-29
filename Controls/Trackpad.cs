using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;

namespace GDTIMDotNet
{
    public class Trackpad : GestureConsumer<GuiConstrainedGestureInterpreter>
    {
        [Export] string _vncHandlerPathRelative = "../../VncHandler";
        [Export] bool _mouseAcceleration = true;
        
        GuiConstrainedGestureInterpreter _interpreter;
        VncHandler _vncHandler;
        MouseButton _longPressedButton = MouseButton.Left;
        bool IsMultiLongPressed => _state == MouseState.LongPress && _longPressedButton != MouseButton.Left;
	
        float _deltaTime;
        // constants here are just comfortable numbers after regulating input speeds by
        // control resolution and frame time
        float ScrollSpeed => 2000f * _deltaTime;
        float ZoomSpeed => 800f * _deltaTime;
        float MouseSpeed => 40f * _deltaTime;

        MouseState _state = MouseState.Default;
        bool StateIsDefault => _state == MouseState.Default;
        enum MouseState { Default, Zooming, Scrolling, LongPress }
        const KeyList ZoomModifierKey = KeyList.Control;
        
        Vector2 _cumulativeScroll;
        Vector2 _cumulativeMouseMovement;
        float _cumulativeZoom;
        
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
            if (!StateIsDefault) return;
            GDLogger.Log(this,"Twist");
        }

        protected override void OnSingleLongPress(object sender, Vector2 e)
        {
            LongPress(true, MouseButton.Left);
        }

        protected override void OnSingleDrag(object sender, SingleDragArgs e)
        {
            HandleDrag(e.Relative);
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
            if (e.Fingers > 3) return;
            
            MouseButton longPressButton = e.Fingers == 2 ? MouseButton.Right : MouseButton.Middle;
            LongPress(true, longPressButton);
        }

        protected override void OnMultiSwipe(object sender, MultiDragArgs e)
        {
            if (IsMultiLongPressed) return;
            if (e.Fingers != 2)
                return;

            Scroll(e.Relative);
        }

        protected override void OnMultiTap(object sender, MultiTapArgs e)
        {
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
            LongPress(false, _longPressedButton);
            ResetState();
        }

        protected override void OnPinch(object sender, PinchArgs e)
        {
            if (!StateIsDefault && _state != MouseState.Zooming) return;
            HandleZoom(e.Relative);
        }

        protected override void OnMultiDrag(object sender, MultiDragArgs e)
        {
            if (IsMultiLongPressed)
            {
                HandleDrag(e.Relative);
                return;
            }

            if (StateIsDefault || _state == MouseState.Scrolling)
                Scroll(e.Relative);
        }
        
        void ResetState()
        {
            if(_state == MouseState.Zooming)
                _vncHandler.SendKey(ZoomModifierKey, false);
            
            _cumulativeScroll = Vector2.Zero;
            _cumulativeMouseMovement = Vector2.Zero;
            _cumulativeZoom = 0;
            _state = MouseState.Default;
        }
        
        void RightClick()
        {
            _vncHandler.MouseButtonDown(MouseButton.Right);
            _vncHandler.MouseButtonUp(MouseButton.Right);
        }

        void MiddleClick()
        {
            _vncHandler.MouseButtonDown(MouseButton.Middle);
            _vncHandler.MouseButtonUp(MouseButton.Middle);
        }
        
        #region Stateful Interactions
        void Scroll(Vector2 relative)
        {
            Vector2 realSize = _interpreter.ControlRealSize;
            Vector2 increment = relative / realSize;
            int scrollX = GetIntegerInput(ref _cumulativeScroll.x, increment.x * ScrollSpeed);
            int scrollY = GetIntegerInput(ref _cumulativeScroll.y, increment.y * ScrollSpeed);
            var scrollAmt = new Vector2(scrollX, scrollY);

            if (scrollAmt == Vector2.Zero) return;
            
            _state = MouseState.Scrolling;
            _vncHandler.Scroll(scrollAmt);
        }

        void HandleZoom(float relative)
        {
            Vector2 controlSize = _interpreter.ControlRealSize;
            float maxDimension = Mathf.Max(controlSize.x, controlSize.y);
            relative /= maxDimension;
            int zoomAmount = GetIntegerInput(ref _cumulativeZoom, relative * ZoomSpeed);
            if (zoomAmount == 0) return;

            var zoomScroll = new Vector2(0, zoomAmount);
            
            if(_state != MouseState.Zooming)
                _vncHandler.SendKey(ZoomModifierKey, true);
            
            _state = MouseState.Zooming;
            _vncHandler.Scroll(zoomScroll);
        }
        
        void LongPress(bool pressed, MouseButton button)
        {
            _longPressedButton = button;
            if (pressed)
            {
                _state = MouseState.LongPress;
                _vncHandler.MouseButtonDown(button);
                Input.VibrateHandheld(50);
            }
            else if (_state == MouseState.LongPress)
            {
                _state = MouseState.Default;
                _vncHandler.MouseButtonUp(button);
            }
        }
        #endregion Stateful Interactions
        
        void HandleDrag(Vector2 relative)
        {
            Vector2 trackpadSize = _interpreter.ControlRealSize;
            float minTrackpadDimension = Mathf.Min(trackpadSize.x, trackpadSize.y);

            Vector2 serverResolution = _vncHandler.Resolution;
            float minServerResolution = Mathf.Min(serverResolution.x, serverResolution.y);

            Vector2 dragDelta = relative / minTrackpadDimension;
            float dragAccelerationT = dragDelta.LengthSquared();
            //float dragAccelerationT = dragDelta.Length();

            float speed = _mouseAcceleration ? MouseSpeed + MouseSpeed * dragAccelerationT : MouseSpeed;
            Vector2 moveAmount = dragDelta * speed * minServerResolution;

            MoveMouse(moveAmount);
        }

        void MoveMouse(Vector2 moveAmount)
        {
            Vector2 calculatedMouseMove;
            calculatedMouseMove.x = GetIntegerInput(ref _cumulativeMouseMovement.x, moveAmount.x);
            calculatedMouseMove.y = GetIntegerInput(ref _cumulativeMouseMovement.y, moveAmount.y);
            
            if (calculatedMouseMove == Vector2.Zero) return;
            
            _vncHandler.MoveMouse(calculatedMouseMove);
        }

        static int GetIntegerInput(ref float trackedInput, float increment)
        {
            trackedInput += increment;
            
            var inputAmount = (int)trackedInput;
            
            if (trackedInput > 1 || trackedInput < -1)
                trackedInput = 0;
            
            return inputAmount;
        }
    }
}