using System;
using Godot;
using GodotExtensions;
using Microsoft.Extensions.Logging;
using PCRemoteControl.VNC;

namespace GDTIMDotNet
{
    public class Keyboard : NodeExt
    {
        // raw text entry like this can use the clipboard from the TextEdit contents to auto-paste
        // using VncHandler.Paste(string)
        [Export] string _activationButtonPath = "../KeyboardButton";
        [Export] string _autoCorrectTextEditPath = "../KeyboardTextEdit";
        [Export] string _vncHandlerPath;
        VncHandler _vncHandler;
        Button _activationButton;
        TextEdit _autoCorrectTextEdit;
        bool _hasFocus;
        
        public override void _Ready()
        {
            base._Ready();
            _activationButton = GetNode<Button>(_activationButtonPath);
            _activationButton.Connect("pressed", this, OnKeyboardButton);
            
            _autoCorrectTextEdit = GetNode<TextEdit>(_autoCorrectTextEditPath);
        }

        public override void _UnhandledKeyInput(InputEventKey @event)
        {
            base._UnhandledKeyInput(@event);
            bool pressed = @event.Pressed;
            KeyList physicalKey = (KeyList)@event.PhysicalScancode;
            KeyList softKey = (KeyList)@event.Scancode;
            if (physicalKey != softKey)
            {
                Log($"Keys differ. Soft: {softKey} | Physical: {physicalKey}");
            }
            
            _vncHandler.SendKey(physicalKey, pressed);
        }

        void OnKeyboardButton()
        {
            _hasFocus = !_hasFocus;
            if (_hasFocus)
            {
                OS.ShowVirtualKeyboard();
                //_autoCorrectTextEdit.GrabFocus();
            }
            else
            {
                OS.HideVirtualKeyboard();
                //_autoCorrectTextEdit.ReleaseFocus();
            }
        }
    }
}