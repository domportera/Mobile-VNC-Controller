using System;

namespace GodotExtensions
{
    public interface IButtonUpDown
    {
        event EventHandler PressDown;
        event EventHandler PressUp;
        event EventHandler<bool> Toggled;
    }
}