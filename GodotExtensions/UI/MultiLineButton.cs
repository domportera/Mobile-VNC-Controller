using Godot;

namespace GodotExtensions
{
    public partial class MultiLineButton : MultiLineControlBase<Label, Button>
    {
        public MultiLineButton(string labelText, bool addVerticalPadding, bool autoWrap = true, bool maximizeAnchors = true) 
            : base(labelText, addVerticalPadding, autoWrap, maximizeAnchors){}
        
        public Button Button => BackgroundControl;
    }

    public partial class MultiLineTouchableButton : MultiLineControlBase<Label, GuiTouchButton>
    {
        public MultiLineTouchableButton(string labelText, bool addVerticalPadding, bool autoWrap = true, bool maximizeAnchors = true) 
            : base(labelText, addVerticalPadding, autoWrap, maximizeAnchors){}

        public IButtonUpDown ButtonUpDown => BackgroundControl;
    }
    
    /// <summary>
    /// A class which wraps a text-wrapped label in a surrounding Control behind it
    /// Originally developed for buttons with multiple lines of text, but can be used for multi-line
    /// controls of any kind.
    ///
    /// Label ignores mouse input
    ///
    /// It is recommended to instantiate these as a child of a control or container so it can auto-expand and provide
    /// multi-line functionality out of the box.
    /// </summary>
    /// <typeparam name="TLabel">Type of label, inheriting from <see cref="Label"/></typeparam>
    /// <typeparam name="TControl">Type of control, inheriting from <see cref="Control"/></typeparam>
    public partial class MultiLineControlBase<TLabel, TControl> where TLabel : Label, new() where TControl : Control, new()
    {
        protected MultiLineControlBase(string labelText, bool addVerticalPadding, bool autoWrap, bool maximizeAnchors)
        {
            BackgroundControl = new TControl();
            ConfigureButtonControl(BackgroundControl);
            Label = new TLabel();

            const int nameLength = 12;
            string nameText = labelText.Length > nameLength 
                ? labelText.Substring(0, nameLength - 3) + "..." 
                : labelText;
            Label.Name = LabelName(nameText);
            BackgroundControl.Name = ControlName(nameText);
            
            ConfigureLabel(Label, labelText, autoWrap, addVerticalPadding);
            ConfigureLabelControl(label: Label, child: BackgroundControl);
            
            if (maximizeAnchors)
            {
                BackgroundControl.MaximizeAnchors();
                Label.MaximizeAnchors();
            }
        }
        
        protected virtual string LabelName(string nameText)
        {
            return $"{GetType().Name} Label {nameText}";
        }
        
        protected virtual string ControlName(string nameText)
        {
            return $"{GetType().Name} Control {nameText}";
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

        static void ConfigureLabel(TLabel label, string text, bool autoWrap, bool addVerticalPadding)
        {
            label.Text = addVerticalPadding ? $"\n{text}\n" : text;
            label.Autowrap = autoWrap;
            label.Align = Godot.Label.AlignEnum.Center;
            label.Valign = Godot.Label.VAlign.Center;
        }
        
        public TControl BackgroundControl { get; }
        public TLabel Label { get; }
    }
}