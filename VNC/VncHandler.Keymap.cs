using System;
using System.Collections.Generic;
using Godot;
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

        static bool _stringsInitialized = false;
        static Dictionary<char, KeySym> _characterDict = new Dictionary<char, KeySym>()
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
                bool success = _characterDict.TryGetValue(c, out KeySym key);
                if(success)
                    keys.Add(key);
            }

            return keys;
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