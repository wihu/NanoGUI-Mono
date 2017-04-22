using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Window : Widget
    {
        public string title { get; set; }
        //protected Widget m_ButtonPanel;

        public Window () : this (null)
        {
        }

        public Window (Widget parent, string title = "Window")
            : base (parent)
        {
            this.title = title;   
        }

        public override void Draw (NVGcontext ctx)
        {
            Theme style = this.theme;
            int ds = style.windowDropShadowSize;
            int cr = style.windowCornerRadius;
            int hh = style.windowHeaderHeight;

            Vector2 pos = this.localPosition;
            Vector2 size = this.size;

            NanoVG.nvgSave (ctx);
            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, size.Y, cr);
            NanoVG.nvgFillColor (ctx, m_MouseFocus ? style.windowFillFocusedColor
                                 : style.windowFillUnfocusedColor);

            NanoVG.nvgFill (ctx);

            NVGpaint shadowPaint = NanoVG.nvgBoxGradient (ctx
                                                          , pos.X, pos.Y, size.X, size.Y
                                                          , cr * 2, ds * 2
                                                          , style.dropShadowColor
                                                          , style.transparentColor);

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRect (ctx, pos.X - ds, pos.Y - ds, size.X + 2 * ds, size.Y + 2 * ds);
            NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, size.Y, cr);
            NanoVG.nvgPathWinding (ctx, (int)NVGsolidity.NVG_HOLE);
            NanoVG.nvgFillPaint (ctx, shadowPaint);
            NanoVG.nvgFill (ctx);

            // draw header.
            if (!string.IsNullOrEmpty (this.title))
            {
                NVGpaint headerPaint = NanoVG.nvgLinearGradient (
                    ctx
                    , pos.X, pos.Y, pos.X, pos.Y + hh
                    , style.windowHeaderGradientTopColor
                    , style.windowHeaderGradientBotColor
                );

                NanoVG.nvgBeginPath (ctx);
                NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, hh, cr);
                NanoVG.nvgFillPaint (ctx, headerPaint);
                NanoVG.nvgFill (ctx);

                NanoVG.nvgBeginPath (ctx);
                NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, hh, cr);
                NanoVG.nvgStrokeColor (ctx, style.windowHeaderSepTopColor);

                NanoVG.nvgSave (ctx);
                NanoVG.nvgIntersectScissor (ctx, pos.X, pos.Y, size.X, 0.5f);
                NanoVG.nvgStroke (ctx);
                NanoVG.nvgResetScissor (ctx);
                NanoVG.nvgRestore (ctx);

                NanoVG.nvgBeginPath (ctx);
                NanoVG.nvgMoveTo (ctx, pos.X + 0.5f, pos.Y + hh - 1.5f);
                NanoVG.nvgLineTo (ctx, pos.X + size.X - 0.5f, pos.Y + hh - 1.5f);
                NanoVG.nvgStrokeColor (ctx, style.windowHeaderSepBotColor);
                NanoVG.nvgStroke (ctx);

                NanoVG.nvgFontSize (ctx, style.standardFontSize + 2f);
                NanoVG.nvgFontFace (ctx, style.fontBold);
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));

                Vector2 headerTextPos;
                headerTextPos.X = pos.X + size.X * 0.5f;
                headerTextPos.Y = pos.Y + hh * 0.5f;

                NanoVG.nvgFontBlur (ctx, 2f);
                NanoVG.nvgFillColor (ctx, style.dropShadowColor);
                NanoVG.nvgText (ctx, headerTextPos.X, headerTextPos.Y, this.title);

                NanoVG.nvgFontBlur (ctx, 0f);
                NanoVG.nvgFillColor (ctx, this.focused ? style.windowTitleFocusedColor
                                     : style.windowTitleUnfocusedColor);
                NanoVG.nvgText (ctx, headerTextPos.X, headerTextPos.Y - 1f, this.title);
            }

            NanoVG.nvgRestore (ctx);
            base.Draw (ctx);
        }

        public Window WithTitle (string title)
        {
            this.title = title;
            return this;
        }
    }
}
