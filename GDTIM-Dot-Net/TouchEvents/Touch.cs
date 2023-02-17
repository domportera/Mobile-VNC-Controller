#define ERROR_CHECK_GDTIM

using System;
using System.Collections.Generic;
using Godot;
using GodotExtensions;

namespace GDTIMDotNet
{
    public class Touch
    {
        public double LastUpdated {get; private set;}
        public TouchPositionData Current;
        public int Index {get; }

        const double DragHistoryDuration = 0.3;

        public event EventHandler Updated;

        Queue<TouchPositionData> _history = new Queue<TouchPositionData>();
        

        public Touch(double time, int index, Vector2 position)
        {
            Current = new TouchPositionData(time, position);
            _history.Enqueue(Current);
            LastUpdated = time;
            Index = index;
        }

        public override string ToString()
        {
            return $"({nameof(Touch)}): {Current.ToString()}";
        }

        // todo: speeds will not be updated as this Update function currently wont be called when touch is still
        public void Update(double time, Vector2 position, Vector2 relative)
        {
#if ERROR_CHECK_GDTIM
            if(position != Current.Position + relative)
                GDLogger.Error(this, $"Touch {Index.ToString()} missed an update!");
#endif

            bool hasHistory = _history.Count > 0;
            TouchPositionData oldest = hasHistory ? _history.Peek() : Current;
            var positionData = new TouchPositionData(time, position, relative, Current, oldest);
            
            LastUpdated = time;
            Current = positionData;
            _history.Enqueue(positionData);

            double timeElapsed = time - oldest.Time;
            if (hasHistory && timeElapsed > DragHistoryDuration)
                _history.Dequeue();
            
#if ERROR_CHECK_GDTIM
            if (Updated == null)
            {
                GDLogger.Error(this, $"Touch {Index.ToString()} does not have any listeners");
                return;
            }
            
            Updated.Invoke(this, EventArgs.Empty);
#else
            Updated.Invoke(this, EventArgs.Empty);
#endif
        }
    }
}