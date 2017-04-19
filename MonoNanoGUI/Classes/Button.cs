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
        protected bool m_Pushed;

        public Button () : this (null)
        {
        }

        public Button (Widget parent, string caption = "Button", int icon = 0)
            : base (parent)
        {
            
        }

        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);

            //Color4f color = new Color4f (92, 255);

            //NanoVG.nvgBeginPath (ctx);
            //NanoVG.nvgRect (ctx, this.localPosition.X, this.localPosition.Y, this.size.X, this.size.Y);
            //NanoVG.nvgFillColor (ctx, NanoVG.nvgRGBAf (color.r, color.g, color.b, color.a));
            //NanoVG.nvgFill (ctx);

            NVGcolor gradTopColor = this.theme.buttonGradientTopUnfocusedColor;
            NVGcolor gradBotColor = this.theme.buttonGradientBotUnfocusedColor;

            if (m_Pushed)
            {

            }
            else
            {
            }

            int fontSize = m_FontSize;
            NanoVG.nvgFontSize (ctx, fontSize);
            NanoVG.nvgFontFace (ctx, "sans");

        }
    }
}
