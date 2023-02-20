#define ERROR_CHECK_GDTIM
using System;
using System.Collections.Generic;
using GDTIMDotNet.GestureGeneration;
using Godot;

namespace GDTIMDotNet
{
    /// <summary>
    /// A class that manages an infinite number of touches
    /// </summary>
    internal class MultiTouch : Control
    {
        public event EventHandler<Touch> TouchAdded;
        public event EventHandler<Touch> TouchRemoved;
        public event EventHandler AfterInput;
        
        Dictionary<int, Touch> _touches = new Dictionary<int, Touch>();
        static Vector2 Resolution => OS.WindowSize;
        static float Dpi => OS.GetScreenDpi();

        readonly bool _allowMouse = true;
        readonly bool _acceptTouch = true;
        readonly bool _acceptMouse = true;

        static double Time => Godot.Time.GetTicksUsec() / (double)1000000;
        static double TimeElapsedSince(double time) => Time - time;

        static int _incrementingTouchIndex = 0;
        readonly Dictionary<int, int> _touchIndices = new Dictionary<int, int>();
        readonly HashSet<int> _mouseButtonsPressed = new HashSet<int>();


        public override void _Ready()
        {
            _ = new GestureGenerator(this, null);
        }

        public override void _Input(InputEvent input)
        {
            switch (input)
            {
                case InputEventMouseButton mouse:
                    HandleMouseButtonEvent(mouse);
                    break;
                case InputEventMouseMotion mouse:
                    HandleMouseMotionEvent(mouse);
                    break;
                case InputEventScreenTouch touch:
                    HandleTouchEvent(touch);
                    break;
                case InputEventScreenDrag drag:
                    HandleTouchDragEvent(drag);
                    break;
            }
            
            AfterInput.Invoke(this, EventArgs.Empty);
        }

        public override void _Process(float delta)
        {
            double time = Time;
            foreach (KeyValuePair<int, Touch> touchEntry in _touches)
            {
                Touch touch = touchEntry.Value;
                if (time - touch.LastUpdateTime > delta)
                {
                    touch.Update(time, touch.Position, Vector2.Zero);
                }
            }
        }

        void HandleTouchDragEvent(InputEventScreenDrag drag)
        {
            if (_acceptTouch)
                AcceptEvent();

            DragTouch(drag.Index, Time, drag.Position, drag.Relative);
        }

        void HandleTouchEvent(InputEventScreenTouch touch)
        {
            if (_acceptTouch)
                AcceptEvent();

            if (touch.Pressed)
            {
                AddTouch(touch.Index, Time, touch.Position);
                return;
            }
            
            RemoveTouch(touch.Index, Time, touch.Position);
        }

        void HandleMouseMotionEvent(InputEventMouseMotion mouse)
        {
            if (!_allowMouse) return;
            if(_acceptMouse) AcceptEvent();
            double time = Time;
            foreach (int mouseButton in _mouseButtonsPressed)
            {
                int buttonIndex = MouseToTouchIndex(mouseButton);
                DragTouch(buttonIndex, time, mouse.GlobalPosition, mouse.Relative);
            }
        }

        void HandleMouseButtonEvent(InputEventMouseButton mouse)
        {
            if (!_allowMouse) return;
            if(_acceptMouse) AcceptEvent();
            int buttonIndex = MouseToTouchIndex(mouse.ButtonIndex);
            if (mouse.Pressed)
            {
                AddTouch(buttonIndex, Time, mouse.GlobalPosition);
                _mouseButtonsPressed.Add(mouse.ButtonIndex);
            }
            else
            {
                RemoveTouch(buttonIndex, Time, mouse.GlobalPosition);
                _mouseButtonsPressed.Remove(mouse.ButtonIndex);
            }
        }

        static int MouseToTouchIndex(int mouseButton)
        {
            return int.MinValue + mouseButton;
        }

        static bool IsMouseBurton(int indez) => indez < 0;

        void AddTouch(int index, double time, Vector2 position)
        {
            #if ERROR_CHECK_GDTIM
            if (_touchIndices.ContainsKey(index))
            {
                throw new Exception($"Touch {index} was added when we already had that index");
            }
            #endif

            int touchIndex = _incrementingTouchIndex++;
            _touchIndices[index] = touchIndex;
            _touches[index] = new Touch(time, touchIndex, position);
        }

        void RemoveTouch(int index, double time, Vector2 position)
        {
            int touchIndex = _touchIndices[index];
            _touchIndices.Remove(index);
            
            Touch removedTouch = _touches[touchIndex];
            _touches.Remove(touchIndex);

            if (IsMouseBurton(index))
                return;
            
            // decrement other touch indexes
            // may need to do a finger width proximity calculation
            // for touch persistence but im gonna prwtend i domt have to
            var toShift = new List<int>();
            foreach (var kvp in _touchIndices)
            {
                if (kvp.Key > index)
                    toShift.Add(kvp.Key);
            }

            toShift.Sort();
            foreach (int i in toShift)
            {
                int adjusted = i - 1;
                var indexToMove = _touchIndices[i];
                _touchIndices.Remove(i);
                _touchIndices[adjusted] = indexToMove;
            }
        }

        void DragTouch(int index, double time, Vector2 position, Vector2 relative)
        {
            int touchIndex = _touchIndices[index];
            _touches[touchIndex].Update(time, position, relative);
        }

    }
}