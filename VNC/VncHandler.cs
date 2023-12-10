using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using GodotExtensions;
using RemoteViewing.Vnc;

namespace PCRemoteControl.VNC
{
	/// <summary>
	/// Should be turned into an interface so this VNC library can be replaced
	/// </summary>
	public partial class VncHandler : NodeExt
	{
		[Export] NodePath _pathToVncAuthHelper = "../VncAuthHelper";
		[Export] NodePath _pathToConnectButton = "../ConnectButton";
		
		public event EventHandler OnConnected;
		public event EventHandler OnDisconnected;
		Vector2 _mousePosition = Vector2.Zero;
		Vector2 _serverResolution;
		int _currentPressedButtons;
		VncClient _client;
		public Vector2 Resolution => _serverResolution;
		CancellationTokenSource _clipboardCts;
		const int ClipboardCheckIntervalMs = 500;
		GuiTouchButton _connectButton;
		bool _connecting = false;

		VncAuthHelper _authHelper;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_authHelper = GetNode<VncAuthHelper>(_pathToVncAuthHelper);
			_connectButton = GetNode<GuiTouchButton>(_pathToConnectButton);
			_connectButton.Connect("pressed_up", new Callable(this, Connect));
			InitializeClient();
		}

		public override void _Notification(int what)
		{
			if (what == MainLoop.NotificationWmQuitRequest)
				_clipboardCts?.Cancel();
		}

		private void InitializeClient()
		{
			_client = new VncClient();
			_client.Connected += OnConnected;
			_client.Connected += ClientOnConnected;
			_client.ConnectionFailed += ClientOnConnectionFailed;
			_client.Closed += OnDisconnected;
			_client.Bell += ClientOnBell;
			_client.RemoteClipboardChanged += ClientOnRemoteClipboardChanged;
			_client.MaxUpdateRate = 0.000001f;

		}

		void Connect()
		{
			if (_client.IsConnected || _connecting) return;
			
			VncAuthHelper.AuthInfo authInfo = _authHelper.GetAuthInfo();
			var options = new VncClientConnectOptions()
			{
				Password = authInfo.Password.ToCharArray(),
				OnDemandMode = false
			};
			
			TryConnect(authInfo.Ip, new Callable(authInfo.Port, options));
		}

		void TryConnect(string ip, new Callable(int port, VncClientConnectOptions options))
		{
			if(_client.IsConnected)
				_client.Close();
			
			try
			{
				_connecting = true;
				_client.Connect(ip, new Callable(port, options));
			}
			catch (Exception e)
			{
				LogException($"Error connecting to server at {ip}:{port.ToString()}", e);
			}
			finally
			{
				_connecting = false;
			}
		}

		/// <summary>
		/// Simulates keyboard input from text
		/// Really nasty workaround for deficiencies in this VNC library and Godot
		/// </summary>
		/// <param name="text"></param>
		public void SendText(string text)
		{
			if (!_client.IsConnected) return;
			List<KeySym> keys = text.ToKeySyms();

			foreach (KeySym key in keys)
			{
				_client.SendKeyEvent(key, true);
				_client.SendKeyEvent(key, false);
			}
		}

		public void SendKey(KeyList key, bool pressed)
		{
			if (!_client.IsConnected) return;
			bool gotKey = key.ToKeySym(out KeySym keySym);
			if (!gotKey) return;
			_client.SendKeyEvent(keySym, pressed);
		}

		/// <summary>
		/// Sending local clipboard does not work. great!
		/// This app is in desperate need of an updated .NET version
		/// </summary>
		/// <param name="text"></param>
		public void Paste(string text)
		{
			if (!_client.IsConnected) return;
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
			if (!_client.IsConnected) return;
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
