using System;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Button : Widget
    {
        enum Flags
        {
            NormalButton = (1 << 0),
            RadioButton  = (1 << 1),
            ToggleButton = (1 << 2),
            PopupButton  = (1 << 3)
        }

        enum IconAnchorType
        {
            Left,
            LeftCentered,
            RightCentered,
            Right
        }

        protected string m_Caption;
        protected int m_Icon;

        public Button (Widget parent, string caption = "Button", int icon = 0)
            : base (parent)
        {
            
        }

        public override void Draw (NVGcontext ctx)
        {
        }
    }
}
