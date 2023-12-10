using System.Collections.Generic;
using Godot;
using RemoteViewing.Vnc;

namespace PCRemoteControl.VNC
{
    internal static class KeymapConversion
    {
        static readonly Dictionary<Key, KeySym> Keymap = new Dictionary<Key, KeySym>()
        {
            //Modifiers
            { Key.Shift, KeySym.ShiftLeft },
            { Key.Alt, KeySym.AltLeft },
            { Key.Ctrl, KeySym.ControlLeft },

            //Navigation
            { Key.Left, KeySym.Left },
            { Key.Right, KeySym.Right },
            { Key.Up, KeySym.Up },
            { Key.Down, KeySym.Down },

            //control
            { Key.Escape, KeySym.Escape },
            { Key.Capslock, KeySym.CapsLock },
            { Key.Scrolllock, KeySym.ScrollLock },
            { Key.Tab, KeySym.Tab },
            { Key.Delete, KeySym.Delete },
            { Key.Backspace, KeySym.Backspace },
            { Key.Insert, KeySym.Insert },
            { Key.Home, KeySym.Home },
            { Key.End, KeySym.End },
            { Key.Pageup, KeySym.PageUp },
            { Key.Pagedown, KeySym.PageDown },
            { Key.Meta, KeySym.MetaLeft },
            { Key.Numlock, KeySym.Num_Lock },
            { Key.Print, KeySym.Print },
            { Key.Pause, KeySym.Pause },

            //other characers
            { Key.Space, KeySym.Space },
            { Key.Minus, KeySym.Minus },
            { Key.Bracketleft, KeySym.BraceLeft },
            { Key.Bracketright, KeySym.Bracketright },
            { Key.Semicolon, KeySym.Semicolon },
            { Key.Apostrophe, KeySym.Apostrophe },
            { Key.Comma, KeySym.Comma },
            { Key.Period, KeySym.Period },
            { Key.Backslash, KeySym.Backslash },
            { Key.Slash, KeySym.Slash },
            { Key.Quoteleft, KeySym.Grave },

            //keys requiring SHIFT key
            { Key.Asciitilde, KeySym.AsciiTilde },
            { Key.Exclam, KeySym.Exclamation },
            { Key.At, KeySym.At },
            { Key.Numbersign, KeySym.NumberSign },
            { Key.Dollar, KeySym.Dollar },
            { Key.Percent, KeySym.Percent },
            { Key.Asciicircum, KeySym.AsciiCircum },
            { Key.Ampersand, KeySym.Ampersand },
            { Key.Asterisk, KeySym.Asterisk },
            { Key.Parenleft, KeySym.ParenthesisLeft },
            { Key.Parenright, KeySym.ParenthesisRight },
            { Key.Underscore, KeySym.Underscore },
            { Key.Plus, KeySym.Plus },
            { Key.Braceleft, KeySym.BraceLeft },
            { Key.Braceright, KeySym.BraceRight },
            { Key.Colon, KeySym.Colon },
            { Key.Quotedbl, KeySym.Quote },
            { Key.Question, KeySym.Question },
            { Key.Less, KeySym.Less },
            { Key.Greater, KeySym.Greater },
            { Key.Bar, KeySym.Bar },

            // letters
            { Key.A, KeySym.a },
            { Key.B, KeySym.b },
            { Key.C, KeySym.c },
            { Key.D, KeySym.d },
            { Key.E, KeySym.e },
            { Key.F, KeySym.f },
            { Key.G, KeySym.g },
            { Key.H, KeySym.h },
            { Key.I, KeySym.i },
            { Key.J, KeySym.j },
            { Key.K, KeySym.k },
            { Key.L, KeySym.l },
            { Key.M, KeySym.m },
            { Key.N, KeySym.n },
            { Key.O, KeySym.o },
            { Key.P, KeySym.p },
            { Key.Q, KeySym.q },
            { Key.R, KeySym.r },
            { Key.S, KeySym.s },
            { Key.T, KeySym.t },
            { Key.U, KeySym.u },
            { Key.V, KeySym.v },
            { Key.W, KeySym.w },
            { Key.X, KeySym.x },
            { Key.Y, KeySym.y },
            { Key.Z, KeySym.z },

            //Fn
            { Key.F1, KeySym.F1 },
            { Key.F2, KeySym.F2 },
            { Key.F3, KeySym.F3 },
            { Key.F4, KeySym.F4 },
            { Key.F5, KeySym.F5 },
            { Key.F6, KeySym.F6 },
            { Key.F7, KeySym.F7 },
            { Key.F8, KeySym.F8 },
            { Key.F9, KeySym.F9 },
            { Key.F10, KeySym.F10 },
            { Key.F11, KeySym.F11 },
            { Key.F12, KeySym.F12 },
            { Key.F13, KeySym.F13 },
            { Key.F14, KeySym.F14 },
            { Key.F15, KeySym.F15 },
            { Key.F16, KeySym.F16 },

            //Top row numbers
            { Key.Key0, KeySym.D0 },
            { Key.Key1, KeySym.D1 },
            { Key.Key2, KeySym.D2 },
            { Key.Key3, KeySym.D3 },
            { Key.Key4, KeySym.D4 },
            { Key.Key5, KeySym.D5 },
            { Key.Key6, KeySym.D6 },
            { Key.Key7, KeySym.D7 },
            { Key.Key8, KeySym.D8 },
            { Key.Key9, KeySym.D9 },
        };

        static readonly Dictionary<Key, string> UnsupportedKeys = new Dictionary<Key, string>()
        {
        };

        internal static bool ToKeySym(this Key key, out KeySym keySym)
        {
            return Keymap.TryGetValue(key, out keySym);
        }

        static readonly Dictionary<char, KeySym> CharacterDict = new Dictionary<char, KeySym>()
        {
            //other characers
            { ' ', KeySym.Space },
            { '-', KeySym.Minus },
            { '[', KeySym.BraceLeft },
            { ']', KeySym.Bracketright },
            { ';', KeySym.Semicolon },
            { '\'', KeySym.Apostrophe },
            { ',', KeySym.Comma },
            { '.', KeySym.Period },
            { '/', KeySym.Backslash },
            { '\\', KeySym.Slash },
            { '`', KeySym.Grave },

            //keys requiring SHIFT key
            { '~', KeySym.AsciiTilde },
            { '!', KeySym.Exclamation },
            { '@', KeySym.At },
            { '#', KeySym.NumberSign },
            { '$', KeySym.Dollar },
            { '%', KeySym.Percent },
            { '^', KeySym.AsciiCircum },
            { '&', KeySym.Ampersand },
            { '*', KeySym.Asterisk },
            { '(', KeySym.ParenthesisLeft },
            { ')', KeySym.ParenthesisRight },
            { '_', KeySym.Underscore },
            { '+', KeySym.Plus },
            { '{', KeySym.BraceLeft },
            { '}', KeySym.BraceRight },
            { ':', KeySym.Colon },
            { '\"', KeySym.Quote },
            { '?', KeySym.Question },
            { '<', KeySym.Less },
            { '>', KeySym.Greater },
            { '|', KeySym.Bar },
            
            //letters
            { 'a', KeySym.a },
            { 'b', KeySym.b },
            { 'c', KeySym.c },
            { 'd', KeySym.d },
            { 'e', KeySym.e },
            { 'f', KeySym.f },
            { 'g', KeySym.g },
            { 'h', KeySym.h },
            { 'i', KeySym.i },
            { 'j', KeySym.j },
            { 'k', KeySym.k },
            { 'l', KeySym.l },
            { 'm', KeySym.m },
            { 'n', KeySym.n },
            { 'o', KeySym.o },
            { 'p', KeySym.p },
            { 'q', KeySym.q },
            { 'r', KeySym.r },
            { 's', KeySym.s },
            { 't', KeySym.t },
            { 'u', KeySym.u },
            { 'v', KeySym.v },
            { 'w', KeySym.w },
            { 'x', KeySym.x },
            { 'y', KeySym.y },
            { 'z', KeySym.z },
            
            { 'A', KeySym.A },
            { 'B', KeySym.B },
            { 'C', KeySym.C },
            { 'D', KeySym.D },
            { 'E', KeySym.E },
            { 'F', KeySym.F },
            { 'G', KeySym.G },
            { 'H', KeySym.H },
            { 'I', KeySym.I },
            { 'J', KeySym.J },
            { 'K', KeySym.K },
            { 'L', KeySym.L },
            { 'M', KeySym.M },
            { 'N', KeySym.N },
            { 'O', KeySym.O },
            { 'P', KeySym.P },
            { 'Q', KeySym.Q },
            { 'R', KeySym.R },
            { 'S', KeySym.S },
            { 'T', KeySym.T },
            { 'U', KeySym.U },
            { 'V', KeySym.V },
            { 'W', KeySym.W },
            { 'X', KeySym.X },
            { 'Y', KeySym.Y },
            { 'Z', KeySym.Z },
        };

        internal static List<KeySym> ToKeySyms(this string str)
        {
            var keys = new List<KeySym>();

            foreach (char c in str)
            {
                bool success = CharacterDict.TryGetValue(c, out KeySym key);
                if(success)
                    keys.Add(key);
            }

            return keys;
        }

        /// <summary>
        /// Unsupported keys can be supported through copy/paste
        /// </summary>
        internal static bool ToClipboardContents(this Key key, out string keyString)
        {
            return UnsupportedKeys.TryGetValue(key, out keyString);
        }
    }
}