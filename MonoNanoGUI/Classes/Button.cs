using System;
using System.Collections.Generic;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Button : Widget
    {
        [Flags]
        public enum Flags
        {
            NormalButton = (1 << 0),
            RadioButton  = (1 << 1),
            ToggleButton = (1 << 2),
            PopupButton  = (1 << 3)
        }

        public enum IconAnchorType
        {
            Left,
            LeftCentered,
            RightCentered,
            Right
        }

        protected string m_Caption = string.Empty;
        protected int m_Icon;
        protected IconAnchorType m_AnchorType;
        protected bool m_Pushed;
        protected int m_Flags;
        protected NVGcolor m_BackgroundColor;
        protected NVGcolor m_TextColor;

        protected event Action m_Callback;
        protected event Action<bool> m_ChangeCallback;
        protected List<Button> m_ButtonGroup = new List<Button> ();

        public Button () : this (null)
        {
        }

        public Button (Widget parent, string caption = "Button", int icon = 0)
            : base (parent)
        {
            m_Caption = caption;
        }

        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);

            //Color4f color = new Color4f (92, 255);

            //NanoVG.nvgBeginPath (ctx);
            //NanoVG.nvgRect (ctx, this.localPosition.X, this.localPosition.Y, this.size.X, this.size.Y);
            //NanoVG.nvgFillColor (ctx, NanoVG.nvgRGBAf (color.r, color.g, color.b, color.a));
            //NanoVG.nvgFill (ctx);

            Theme style = this.theme;

            NVGcolor gradTopColor = style.buttonGradientTopUnfocusedColor;
            NVGcolor gradBotColor = style.buttonGradientBotUnfocusedColor;

            if (m_Pushed)
            {
                gradTopColor = style.buttonGradientTopPushedColor;
                gradBotColor = style.buttonGradientBotPushedColor;
            }
            else
            {
                gradTopColor = style.buttonGradientTopFocusedColor;
                gradBotColor = style.buttonGradientBotFocusedColor;
            }

            Vector2 pos = this.localPosition;
            Vector2 size = this.size;

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X + 1f, pos.Y + 1f, size.X - 2f, size.Y - 2f
                                   , style.buttonCornerRadius - 1f);

            NanoVG.nvgFillColor (ctx, gradTopColor);
            NanoVG.nvgFill (ctx);

            if (0 < m_BackgroundColor.a)
            { 
                // fill background.
            }

            NVGpaint gradient = NanoVG.nvgLinearGradient (ctx, pos.X, pos.Y, pos.X, pos.Y + size.Y
                                                          , gradTopColor, gradBotColor);
            NanoVG.nvgFillPaint (ctx, gradient);
            NanoVG.nvgFill (ctx);


            //int fontSize = m_FontSize;
            int fontSize = style.buttonFontSize;
            NanoVG.nvgFontSize (ctx, fontSize);
            NanoVG.nvgFontFace (ctx, style.fontBold);
            NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));

            float tw = NanoVG.nvgTextBounds (ctx, 0f, 0f, m_Caption, null);
            Vector2 center = pos + size * 0.5f;
            Vector2 textPos = new Vector2 (center.X - tw * 0.5f, center.Y - 1f);

            NanoVG.nvgFillColor (ctx, style.textColor);
            NanoVG.nvgText (ctx, textPos.X, textPos.Y + 1f, m_Caption); 
        }
    }
}
