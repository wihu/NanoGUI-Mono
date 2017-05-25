using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Popup : Window
    {
        public Window parentWindow { get; private set; }
        public Vector2 anchorPos { get; set; }
        public int anchorHeight { get; set; }

        public Popup (Widget parent, Window parentWindow)
            : base (parent, "")
        {
            // set default size.
            this.parentWindow = parentWindow;
            this.size = new Vector2 (320f, 250f);
            this.anchorPos = Vector2.Zero;
            this.anchorHeight = 30;
        }

        protected virtual void RefreshRelativePlacement ()
        {
            if (null == this.parentWindow)
            {
                return;
            }

            Popup parentPopup = this.parentWindow as Popup;
            if (parentPopup)
            {
                parentPopup.RefreshRelativePlacement ();
            }
            this.isVisible &= this.parentWindow.isVisibleRecursive;
            Vector2 pos = this.parentWindow.localPosition + this.anchorPos;
            pos.Y -= this.anchorHeight;
            this.localPosition = pos;
        }

        public override void PerformLayout (NVGcontext ctx)
        {
            if (this.layout || 1 != this.childCount)
            {
                base.PerformLayout (ctx);
            }
            else
            {
                Widget child = GetChild (0);
                child.localPosition = Vector2.Zero;
                child.size = this.size;
                child.PerformLayout (ctx);
            }
        }

        public override void Draw (NVGcontext ctx)
        {
            RefreshRelativePlacement ();
            if (!this.isVisible)
            {
                return;
            }

            Vector2 pos = this.localPosition;
            Vector2 size = this.size;
            Theme style = this.theme;

            int ds = style.windowDropShadowSize;
            int cr = style.windowCornerRadius;
            int ds2 = 2 * ds;
            int cr2 = 2 * cr;
            int ah = this.anchorHeight;

            // draw drop shadow.
            NVGpaint shadowPaint = NanoVG.nvgBoxGradient (ctx, pos.X, pos.Y, size.X, size.Y, cr2, ds2
                                                          , style.dropShadowColor
                                                          , style.transparentColor);
            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRect (ctx, pos.X - ds, pos.Y - ds, size.X + ds2, size.Y + ds2);
            NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, size.Y, cr);
            NanoVG.nvgPathWinding (ctx, (int)NVGsolidity.NVG_HOLE);
            NanoVG.nvgFillPaint (ctx, shadowPaint);
            NanoVG.nvgFill (ctx);

            // draw window.
            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X, pos.Y, size.X, size.Y, cr);
            // draw anchor triangle.
            NanoVG.nvgMoveTo (ctx, pos.X - 15, pos.Y + ah);
            NanoVG.nvgLineTo (ctx, pos.X + 1, pos.Y + ah - 15);
            NanoVG.nvgLineTo (ctx, pos.X + 1, pos.Y + ah + 15);

            NanoVG.nvgFillColor (ctx, style.windowPopupColor);
            NanoVG.nvgFill (ctx);

            // draw contents.
            DrawChildren (ctx);
        }

        public override void Save (Serializer s)
        {
            base.Save (s);
        }

        public override void Load (Serializer s)
        {
            base.Load (s);
        }
    }
}
