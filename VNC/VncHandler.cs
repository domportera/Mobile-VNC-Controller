using System;
using Godot;
using GodotExtensions;
using RemoteViewing.Vnc;

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
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitializeClient(_ip, _password, _port);
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

    public void KeyDown(KeySym key)
    {
        _client.SendKeyEvent(key, true);
    }
    
    public void KeyUp(KeySym key)
    {
        _client.SendKeyEvent(key, false);
    }

    public void Scroll(int amount)
    {
        MouseButton scrollButton = amount > 0 ? MouseButton.ScrollUp : MouseButton.ScrollDown;

        amount = Mathf.Abs(amount);

        for (int i = 0; i < amount; i++)
        {
            MouseButtonDown(scrollButton);
            MouseButtonUp(scrollButton);
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
        var frameBuffer = client.Framebuffer;
        _serverResolution = new Vector2(frameBuffer.Width, frameBuffer.Height);
        _mousePosition = _serverResolution / 2f;
        Log($"Client connected. Server version: {client.ServerVersion}");
        Log($"Client resolution: {_serverResolution.ToString()}");
    }

    void ClientOnRemoteClipboardChanged(object sender, RemoteClipboardChangedEventArgs e)
    {
        Log($"Received clipboard: {e.Contents}");
        OS.Clipboard = e.Contents;
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

    void UpdateMouse()
    {
        _client.SendPointerEvent((int)_mousePosition.x, (int)_mousePosition.y, _currentPressedButtons);
    }
}

public enum MouseButton
{
    Left = 1,
    Middle = 2,
    Right = 4,
    ScrollUp = 8,
    ScrollDown = 16
}