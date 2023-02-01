using Godot;

namespace GodotExtensions
{
    public class MultiLineButton : MultiLineButtonBase<Label>
    {
        public MultiLineButton(string labelText, bool addVerticalPadding, bool autoWrap = true, bool maximizeAnchors = true) 
            : base(labelText, addVerticalPadding, autoWrap, maximizeAnchors) { }
    }
    
    public class MultiLineButtonBase<T> where T : Label, new()
    {
        public MultiLineButtonBase(string labelText, bool addVerticalPadding, bool autoWrap, bool maximizeAnchors)
        {
            Button = new Button();
            ConfigureButtonControl(Button);
            Label = new T();
            
            ConfigureLabel(Label, labelText, autoWrap, addVerticalPadding);
            ConfigureLabelControl(label: Label, child: Button);
            
            if (maximizeAnchors)
            {
                Button.MaximizeAnchors();
                Label.MaximizeAnchors();
            }
        }

        void ConfigureButtonControl(Control button)
        {
            button.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
            button.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
            button.ShowBehindParent = true;
        }

        void ConfigureLabelControl(Control label, Control child)
        {
            label.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
            label.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
            label.MouseFilter = Control.MouseFilterEnum.Ignore;
            Label.AddChild(child);
        }

        protected static void ConfigureLabel(Label label, string text, bool autoWrap, bool addVerticalPadding)
        {
            label.Text = addVerticalPadding ? $"\n{text}\n" : text;
            label.Autowrap = autoWrap;
            label.Align = Godot.Label.AlignEnum.Center;
            label.Valign = Godot.Label.VAlign.Center;
        }

        public Control Control => Label;
        public Button Button { get; }
        public Label Label { get; }
    }
}