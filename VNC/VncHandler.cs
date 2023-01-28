using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using GodotExtensions;
using RemoteViewing.Vnc;

namespace PCRemoteControl.VNC
{
    public class VncHandler : NodeExt
    {
        [Export] string _ip;
        [Export] int _port;
        [Export] string _password;
        public event EventHandler OnConnected;
        public event EventHandler OnDisconnected;
        Vector2 _mousePosition = Vector2.Zero;
        Vector2 _serverResolution;
        int _currentPressedButtons;
        VncClient _client;
        public Vector2 Resolution => _serverResolution;
        CancellationTokenSource _clipboardCts;
        const int ClipboardCheckIntervalMs = 500;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            InitializeClient(_ip, _password, _port);
        }

        public override void _Notification(int what)
        {
            if (what == MainLoop.NotificationWmQuitRequest)
                _clipboardCts?.Cancel();
        }

        private void InitializeClient(string ip, string password, int port)
        {
            var options = new VncClientConnectOptions()
            {
                Password = password.ToCharArray(),
                OnDemandMode = false
            };

            _client = new VncClient();

            _client.Connected += OnConnected;
            _client.Connected += ClientOnConnected;
            _client.ConnectionFailed += ClientOnConnectionFailed;
            _client.Closed += OnDisconnected;
            _client.Bell += ClientOnBell;
            _client.RemoteClipboardChanged += ClientOnRemoteClipboardChanged;
            _client.MaxUpdateRate = 0.000001f;

            TryConnect(ip, port, options);
        }

        void TryConnect(string ip, int port, VncClientConnectOptions options)
        {
            try
            {
                _client.Connect(ip, port, options);
            }
            catch (Exception e)
            {
                LogException($"Error connecting to server at {ip}:{port.ToString()}", e);
            }
        }

        public void SendKey(KeyList key, bool pressed)
        {
            bool gotKey = key.ToKeySym(out KeySym keySym);
            if (!gotKey) return;
            _client.SendKeyEvent(keySym, pressed);
        }

        public void Paste(string text)
        {
            _client.SendLocalClipboardChange(text);
            SendKey(KeyList.Control, true);
            SendKey(KeyList.V, true);
            SendKey(KeyList.Control, false);
            SendKey(KeyList.V, false);

            //revert clipboard to what user expects
            _client.SendLocalClipboardChange(OS.Clipboard);
        }

        public void Scroll(Vector2 amount)
        {
            MouseButton verticalScrollButton = amount.y > 0 ? MouseButton.ScrollUp : MouseButton.ScrollDown;
            MouseButton horizontalScrollButton = amount.x > 0 ? MouseButton.ScrollRight : MouseButton.ScrollLeft;

            amount = new Vector2(Mathf.Abs(amount.x), Mathf.Abs(amount.y));

            for (int i = 0; i < amount.y; i++)
            {
                MouseButtonDown(verticalScrollButton);
                MouseButtonUp(verticalScrollButton);
            }

            for (int i = 0; i < amount.x; i++)
            {
                MouseButtonDown(horizontalScrollButton);
                MouseButtonUp(horizontalScrollButton);
            }
        }

        public void MouseButtonUp(MouseButton button)
        {
            _currentPressedButtons &= ~(int)button;
            UpdateMouse();
        }

        public void MouseButtonDown(MouseButton button)
        {
            _currentPressedButtons |= (int)button;
            UpdateMouse();
        }

        public void MoveMouse(Vector2 delta)
        {
            _mousePosition += delta;
            _mousePosition = new Vector2(
                Mathf.Clamp(_mousePosition.x, 0, _serverResolution.x),
                Mathf.Clamp(_mousePosition.y, 0, _serverResolution.y)
            );

            UpdateMouse();
        }

        void ClientOnBell(object sender, EventArgs e)
        {
            LogError($"Bell???");
        }

        void ClientOnConnectionFailed(object sender, EventArgs e)
        {
            LogError($"Client disconnected");
        }

        void ClientOnConnected(object sender, EventArgs e)
        {
            var client = sender as VncClient;
            VncFramebuffer frameBuffer = client.Framebuffer;
            _serverResolution = new Vector2(frameBuffer.Width, frameBuffer.Height);
            _mousePosition = _serverResolution / 2f;

            Log($"Client connected. Server version: {client.ServerVersion}");
            Log($"Client resolution: {_serverResolution.ToString()}");

            _clipboardCts = new CancellationTokenSource();
            MonitorClipboard(_clipboardCts.Token);
        }


        void ClientOnRemoteClipboardChanged(object sender, RemoteClipboardChangedEventArgs e)
        {
            Log($"Received clipboard: {e.Contents}");
            OS.Clipboard = e.Contents;
        }

        void UpdateMouse()
        {
            _client.SendPointerEvent((int)_mousePosition.x, (int)_mousePosition.y, _currentPressedButtons);
        }


        string _myClipboard = string.Empty;

        async void MonitorClipboard(CancellationToken token)
        {
            _myClipboard = OS.Clipboard;
            _client.SendLocalClipboardChange(_myClipboard);
            while (!token.IsCancellationRequested)
            {
                SyncClipboard();
                await Task.Delay(ClipboardCheckIntervalMs);
            }

            void SyncClipboard()
            {
                bool shouldSyncClipboard = OS.HasClipboard() && _myClipboard.Equals(OS.Clipboard);
                if (!shouldSyncClipboard) return;

                _myClipboard = OS.Clipboard;
                _client.SendLocalClipboardChange(_myClipboard);
            }
        }
    }
}