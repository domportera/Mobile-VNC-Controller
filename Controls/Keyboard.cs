using System;
using System.Threading.Tasks;
using Godot;
using GodotExtensions;
using PCRemoteControl.VNC;

namespace PCRemoteControl.Controls
{
    public class Keyboard : LineEdit
    {
        // raw text entry like this can use the clipboard from the TextEdit contents to auto-paste
        // using VncHandler.Paste(string)
        [Export] NodePath _activationButtonPath = string.Empty;
        [Export] NodePath _vncHandlerPath = string.Empty;
        [Export] float _textEditHeight = 0.07f;
        [Export] bool _hideOnSubmit = true;
        VncHandler _vncHandler;
        Button _activationButton;
        Viewport _viewport;
        
        public override void _Ready()
        {
            base._Ready();
            _activationButton = GetNode<Button>(_activationButtonPath);
            _activationButton.Connect("pressed", this, OnKeyboardButton);
            _viewport = GetViewport();
            
            _vncHandler = GetNode<VncHandler>(_vncHandlerPath);
            this.Connect("text_entered",this, SubmitText);
        }

        // todo: proper keyboard handling 
        public override void _Input(InputEvent @event)
        {
            return;
            // this logic is skipped while Godot does not know how to properly handle virtual keyboards.
            // in the meantime, typing will utilize the clipboard. ¯\_(ツ)_/¯
            // JK the current VNC library's clipboard doesn't work either ¯\_(ツ)_/¯
            // todo: re-enable for hardware keyboards if it's possible to detect errant virtual key presses vs actual hardware key presses
            base._Input(@event);
            if (!(@event is InputEventKey key)) return;
            
            bool pressed = key.Pressed;
            KeyList physicalKey = (KeyList)key.PhysicalScancode;
            KeyList softKey = (KeyList)key.Scancode;
            if (physicalKey != softKey)
            {
                GDLogger.Log(this, $"Keys differ. Soft: {softKey} | Physical: {physicalKey}");
            }

            GDLogger.Log(this, $"Keys: Soft: {softKey} | Physical: {physicalKey}");
            _vncHandler.SendKey(physicalKey, pressed);
        }

        // godot GetVirtualKeyboardHeight results seem unreliable and/or non-immediate
        // so don't mind the awkwardness of this code -
        // I promise it was necessary. It may not be in future versions of Godot.
        bool _keyboardOpen = false;
        void OnKeyboardButton()
        {
            GDLogger.Log(this, $"Keyboard button pressed");
            GrabFocus();
            if (_keyboardOpen) return;
            _keyboardOpen = true;
            
            OS.ShowVirtualKeyboard();
            Task.Run((Action)OpenTextEditWithKeyboard);
        }

        async void OpenTextEditWithKeyboard()
        {
            _keyboardOpen = true;
            int initialHeight = OS.GetVirtualKeyboardHeight();
            int height = initialHeight;

            await Task.Yield();
            
            while (height == initialHeight)
            {
                height = OS.GetVirtualKeyboardHeight();
                SetTextEditPosition(height);
                await Task.Yield();
            }
            
            WaitForKeyboardClosed();
        }

        async void WaitForKeyboardClosed()
        {
            while (OS.GetVirtualKeyboardHeight() > 0)
                await Task.Yield();
            
            SubmitText();
            _keyboardOpen = false;
            SetTextEditPosition((int)_viewport.Size.y);
        }

        void SetTextEditPosition(int yPosition)
        {
            float windowHeight = _viewport.Size.y;
            float bottom = 1f - yPosition / windowHeight;
            AnchorBottom = bottom;
            AnchorTop = bottom - _textEditHeight;
            AnchorLeft = 0;
            AnchorRight = 1;
            MarginLeft = 0;
            MarginRight = 0;
            MarginTop = 0;
            MarginBottom = 0;
        }

        void SubmitText()
        {
            if (string.IsNullOrEmpty(Text)) return;

            _vncHandler.SendText(Text);
            Clear();
            
            if(_hideOnSubmit)
                OS.HideVirtualKeyboard();
        }
    }
}