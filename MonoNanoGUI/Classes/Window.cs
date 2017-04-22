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
            
        }

        public override void Draw (NanoVGDotNet.NVGcontext ctx)
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

            //NVGpaint shadowPaint = NanoVG.nvgBoxGradient (ctx
            //                                              , pos.X, pos.Y, size.X, size.Y
            //                                              , cr * 2, ds * 2
            //                                              , style.dropShadowColor
            //                                              , style.transparentColor);

            //NanoVG.nvgBeginPath (ctx);
            //NanoVG.nvgRect (ctx, pos.X - ds, pos.Y - ds, size.X + 2 * ds, size.Y + 2 * ds);
            //NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, size.Y, cr);
            //NanoVG.nvgPathWinding (ctx, (int)NVGsolidity.NVG_HOLE);
            //NanoVG.nvgFillPaint (ctx, shadowPaint);
            //NanoVG.nvgFill (ctx);

            // draw header.
            if (!string.IsNullOrEmpty (this.title))
            { 
                
            }

            NanoVG.nvgRestore (ctx);
            base.Draw (ctx);
        }
    }
}
