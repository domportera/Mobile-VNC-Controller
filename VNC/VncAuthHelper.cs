using Godot;
using GodotExtensions;

namespace PCRemoteControl.VNC
{
    public partial class VncAuthHelper : NodeExt
    {
        [Export] NodePath _pathToIpField = "../IP Field";
        [Export] NodePath _pathToPasswordField = "../Password Field";
        [Export] NodePath _pathToPortField = "../Port Field";
        LineEdit _ipField;
        SpinBox _portField;
        LineEdit _passwordField;
        
        public override void _Ready()
        {
            base._Ready();
            _ipField = GetNode<LineEdit>(_pathToIpField);
            _passwordField = GetNode<LineEdit>(_pathToPasswordField);
            _portField = GetNode<SpinBox>(_pathToPortField);

            _passwordField.Secret = true;
            _passwordField.SecretCharacter = "*";
        }

        public AuthInfo GetAuthInfo()
        {
            return new AuthInfo(_ipField.Text.Trim(), (int)_portField.Value, _passwordField.Text);
        }

        public struct AuthInfo
        {
            public string Ip { get; }
            public int Port { get; }
            public string Password { get; }

            public AuthInfo(string ip, int port, string password)
            {
                Ip = ip;
                Port = port;
                Password = password;
            }
        }
    }
}