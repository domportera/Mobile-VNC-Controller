using Godot;
using GodotExtensions;
using NiceTouch.GestureGeneration;
using NiceTouch.GestureReceiving;
using PCRemoteControl.VNC;

namespace NiceTouch
{
    public class Trackpad : Control, IGestureConsumer
    {
        [Export] NodePath _vncHandlerPath = string.Empty;
        [Export] bool _mouseAcceleration = true;

        Vector2 RealSize => this.RealPixelSize();
        
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
        
        public override void _Ready()
        {
            base._Ready();
            var interpreter = new ControlGestureInterpreter(this);
            interpreter.SubscribeToGestures(this);
            _vncHandler = GetNode(_vncHandlerPath) as VncHandler;
        }
        
        public override void _Process(float delta)
        {
            base._Process(delta);
            _deltaTime = delta;
        }

        public void OnSingleTouch(object sender, Touch e)
        {
            LongPress(false, _longPressedButton);
            ResetState();
        }

        public void OnSingleLongPress(object sender, Touch e)
        {
            LongPress(true, MouseButton.Left);
        }

        public void OnSingleDrag(object sender, Touch e)
        {
            HandleDrag(e.PositionDelta);
        }

        public void OnSingleTap(object sender, Touch e)
        {
            _vncHandler.MouseButtonDown(MouseButton.Left);
            _vncHandler.MouseButtonUp(MouseButton.Left);
        }
        
        public void OnSingleSwipe(object sender, Touch e)
        {
            GDLogger.Log(this,"Single Swipe");
        }

        public void OnMultiLongPress(object sender, MultiLongPressData e)
        {
            if (e.TouchCount > 3) return;
            
            MouseButton longPressButton = e.TouchCount == 2 ? MouseButton.Right : MouseButton.Middle;
            LongPress(true, longPressButton);
        }

        public void OnMultiSwipe(object sender, MultiSwipeData e)
        {
            if (IsMultiLongPressed) return;
            if (e.TouchCount != 2)
                return;

            Scroll(e.CenterDelta);
        }

        public void OnMultiTap(object sender, MultiTapData e)
        {
            if (e.TouchCount == 2)
            {
                RightClick();
            }
            else if (e.TouchCount == 3)
            {
                MiddleClick();
            }
        }

        public void OnTwist(object sender, TwistData e)
        {
            if (!StateIsDefault) return;
        }

        public void OnPinch(object sender, PinchData e)
        {
            if (!StateIsDefault && _state != MouseState.Zooming) return;
            HandleZoom(e.SeparationAmount);
        }

        public void OnMultiDrag(object sender, MultiDragData e)
        {
            if (IsMultiLongPressed)
            {
                HandleDrag(e.CenterDelta);
                return;
            }

            if (StateIsDefault || _state == MouseState.Scrolling)
                Scroll(e.CenterDelta);
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
            Vector2 realSize = RealSize;
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
            Vector2 controlSize = RealSize;
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
        
        //todo : should continue to move mouse up/left/etc if drag is held on the side after running out of room on the trackpad
        void HandleDrag(Vector2 relative)
        {
            Vector2 trackpadSize = RealSize;
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