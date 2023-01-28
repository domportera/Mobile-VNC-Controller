using Godot;
using Godot.Collections;
using RemoteViewing.Vnc;

namespace PCRemoteControl.VNC
{
    internal static class KeymapConversion
    {
        static readonly Dictionary<KeyList, KeySym> Keymap = new Dictionary<KeyList, KeySym>()
        {
            //Modifiers
            { KeyList.Shift, KeySym.ShiftLeft },
            { KeyList.Alt, KeySym.AltLeft },
            { KeyList.Control, KeySym.ControlLeft },

            //Navigation
            { KeyList.Left, KeySym.Left },
            { KeyList.Right, KeySym.Right },
            { KeyList.Up, KeySym.Up },
            { KeyList.Down, KeySym.Down },

            //control
            { KeyList.Escape, KeySym.Escape },
            { KeyList.Capslock, KeySym.CapsLock },
            { KeyList.Scrolllock, KeySym.ScrollLock },
            { KeyList.Tab, KeySym.Tab },
            { KeyList.Delete, KeySym.Delete },
            { KeyList.Backspace, KeySym.Backspace },
            { KeyList.Insert, KeySym.Insert },
            { KeyList.Home, KeySym.Home },
            { KeyList.End, KeySym.End },
            { KeyList.Pageup, KeySym.PageUp },
            { KeyList.Pagedown, KeySym.PageDown },
            { KeyList.Meta, KeySym.MetaLeft },
            { KeyList.Numlock, KeySym.Num_Lock },

            //other characers
            { KeyList.Space, KeySym.Space },
            { KeyList.Minus, KeySym.Minus },
            { KeyList.Bracketleft, KeySym.BraceLeft },
            { KeyList.Bracketright, KeySym.Bracketright },
            { KeyList.Semicolon, KeySym.Semicolon },
            { KeyList.Apostrophe, KeySym.Apostrophe },
            { KeyList.Comma, KeySym.Comma },
            { KeyList.Period, KeySym.Period },
            { KeyList.Backslash, KeySym.Backslash },
            { KeyList.Slash, KeySym.Slash },
            { KeyList.Quoteleft, KeySym.Grave },

            //keys requiring SHIFT key
            { KeyList.Asciitilde, KeySym.AsciiTilde },
            { KeyList.Exclam, KeySym.Exclamation },
            { KeyList.At, KeySym.At },
            { KeyList.Numbersign, KeySym.NumberSign },
            { KeyList.Dollar, KeySym.Dollar },
            { KeyList.Percent, KeySym.Percent },
            { KeyList.Asciicircum, KeySym.AsciiCircum },
            { KeyList.Ampersand, KeySym.Ampersand },
            { KeyList.Asterisk, KeySym.Asterisk },
            { KeyList.Parenleft, KeySym.ParenthesisLeft },
            { KeyList.Parenright, KeySym.ParenthesisRight },
            { KeyList.Underscore, KeySym.Underscore },
            { KeyList.Plus, KeySym.Plus },
            { KeyList.Braceleft, KeySym.BraceLeft },
            { KeyList.Braceright, KeySym.BraceRight },
            { KeyList.Colon, KeySym.Colon },
            { KeyList.Quotedbl, KeySym.Quote },
            { KeyList.Question, KeySym.Question },
            { KeyList.Less, KeySym.Less },
            { KeyList.Greater, KeySym.Greater },
            { KeyList.Bar, KeySym.Bar },

            // letters
            { KeyList.A, KeySym.a },
            { KeyList.B, KeySym.b },
            { KeyList.C, KeySym.c },
            { KeyList.D, KeySym.d },
            { KeyList.E, KeySym.e },
            { KeyList.F, KeySym.f },
            { KeyList.G, KeySym.g },
            { KeyList.H, KeySym.h },
            { KeyList.I, KeySym.i },
            { KeyList.J, KeySym.j },
            { KeyList.K, KeySym.k },
            { KeyList.L, KeySym.l },
            { KeyList.M, KeySym.m },
            { KeyList.N, KeySym.n },
            { KeyList.O, KeySym.o },
            { KeyList.P, KeySym.p },
            { KeyList.Q, KeySym.q },
            { KeyList.R, KeySym.r },
            { KeyList.S, KeySym.s },
            { KeyList.T, KeySym.t },
            { KeyList.U, KeySym.u },
            { KeyList.V, KeySym.v },
            { KeyList.W, KeySym.w },
            { KeyList.X, KeySym.x },
            { KeyList.Y, KeySym.y },
            { KeyList.Z, KeySym.z },

            //Fn
            { KeyList.F1, KeySym.F1 },
            { KeyList.F2, KeySym.F2 },
            { KeyList.F3, KeySym.F3 },
            { KeyList.F4, KeySym.F4 },
            { KeyList.F5, KeySym.F5 },
            { KeyList.F6, KeySym.F6 },
            { KeyList.F7, KeySym.F7 },
            { KeyList.F8, KeySym.F8 },
            { KeyList.F9, KeySym.F9 },
            { KeyList.F10, KeySym.F10 },
            { KeyList.F11, KeySym.F11 },
            { KeyList.F12, KeySym.F12 },
            { KeyList.F13, KeySym.F13 },
            { KeyList.F14, KeySym.F14 },
            { KeyList.F15, KeySym.F15 },
            { KeyList.F16, KeySym.F16 },

            //Top row numbers
            { KeyList.Key0, KeySym.D0 },
            { KeyList.Key1, KeySym.D1 },
            { KeyList.Key2, KeySym.D2 },
            { KeyList.Key3, KeySym.D3 },
            { KeyList.Key4, KeySym.D4 },
            { KeyList.Key5, KeySym.D5 },
            { KeyList.Key6, KeySym.D6 },
            { KeyList.Key7, KeySym.D7 },
            { KeyList.Key8, KeySym.D8 },
            { KeyList.Key9, KeySym.D9 },
        };

        static readonly Dictionary<KeyList, string> UnsupportedKeys = new Dictionary<KeyList, string>()
        {
            { KeyList.Questiondown, "¿" },
            { KeyList.Exclamdown, "¡" },
        };

        internal static bool ToKeySym(this KeyList key, out KeySym keySym)
        {
            return Keymap.TryGetValue(key, out keySym);
        }

        /// <summary>
        /// Unsupported keys can be supported through copy/paste
        /// </summary>
        internal static bool ToClipboardContents(this KeyList key, out string keyString)
        {
            return UnsupportedKeys.TryGetValue(key, out keyString);
        }
    }
}