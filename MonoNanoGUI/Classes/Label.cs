using System;
using NanoVGDotNet;
using OpenTK;

namespace MonoNanoGUI
{
    public class Label : Widget
    {
        private float[] m_Bounds = new float[4];
        private string m_Font;
        private int m_FontId = -1;

        public string caption { get; set; }
        public NVGcolor color { get; set; }
        public string font
        {
            get
            {
                return m_Font;
            }
            set
            {
                m_Font = value;
                m_FontId = Fonts.Get (m_Font);
            }
        }

        public Label () : this (null)
        {
        }

        public Label (Widget parent, string caption = "Label", string font = "sans", int fontSize = -1)
            : base (parent)
        {
            this.caption = caption;
            this.font = font;
            this.fontSize = fontSize;
            if (null != this.theme)
            {
                this.color = this.theme.textColor;
            }
        }

        public override Vector2 GetPreferredSize (NVGcontext ctx)
        {
            if (string.IsNullOrEmpty (caption))
            {
                return Vector2.Zero;
            }

            Vector2 ret = Vector2.Zero;
            int prefFontSize = GetPreferredFontSize ();
            NanoVG.nvgFontFace (ctx, font);
            NanoVG.nvgFontSize (ctx, prefFontSize);

            int fixedWidth = (int)this.fixedSize.X;
            if (0 < fixedWidth)
            {
                Vector2 pos = this.localPosition;
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
                NanoVG.nvgTextBoxBounds (ctx, pos.X, pos.Y, fixedWidth, caption, m_Bounds);

                ret.X = fixedWidth;
                ret.Y = (m_Bounds[3] - m_Bounds[1]);
            }
            else
            {
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
                ret.X = NanoVG.nvgTextBounds (ctx, 0, 0, caption, m_Bounds);
                ret.Y = prefFontSize;
            }
            return ret;
        }

        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);

            if (0 > m_FontId)
            {
                return;
            }
            int prefFontSize = GetPreferredFontSize ();
            NanoVG.nvgFontFace (ctx, m_FontId);
            NanoVG.nvgFontSize (ctx, prefFontSize);
            NanoVG.nvgFillColor (ctx, this.color);

            int fixedWidth = (int)this.fixedSize.X;
            Vector2 pos = this.localPosition;
            if (0 < fixedWidth)
            {
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
                NanoVG.nvgTextBox (ctx, pos.X, pos.Y, fixedWidth, caption);
            }
            else
            {
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
                NanoVG.nvgText (ctx, pos.X, pos.Y + this.size.Y * 0.5f, caption);
            }

            // DEBUG: BOUNDS
            //NanoVG.nvgStrokeWidth (ctx, 1.0f);
            //NanoVG.nvgBeginPath (ctx);
            //NanoVG.nvgRect (ctx, pos.X, pos.Y, this.size.X, this.size.Y);
            //NanoVG.nvgStrokeColor (ctx, this.color);
            //NanoVG.nvgStroke(ctx);
        }

        public override void Save (Serializer s)
        {
            base.Save (s);
        }

        public override void Load (Serializer s)
        {
            base.Load (s);
        }

        #region Builder Methods
        public Label WithCaption (string caption)
        {
            this.caption = caption;
            return this;
        }
        public Label WithFont (string font)
        {
            this.font = font;
            return this;
        }
#endregion
    }
}
