using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    /// <summary>
    /// Simple horizontal/vertical box layout.
	/// This layout stacks up a bunch of widgets horizontally or vertically. 
    /// It adds margins around the entire container and a custom spacing between adjacent widgets.
    /// </summary>
    public class BoxLayout : Layout
    {
        public Orientation orientation { get; set; }
        public Alignment alignment { get; set; }
        public int margin { get; set; }
        public int spacing { get; set; }

        public BoxLayout (Orientation orientation, Alignment alignment = Alignment.Middle, int margin = 0, int spacing = 0)
        {
            this.orientation = orientation;
            this.alignment = alignment;
            this.margin = margin;
            this.spacing = spacing;
        }

        public override void PerformLayout (NVGcontext ctx, Widget widget)
        {
            Vector2 fixedSize = widget.fixedSize;
            Vector2 containerSize = fixedSize;
            if (0 >= containerSize.X)
            {
                containerSize.X = widget.width;
            }
            if (0 >= containerSize.Y)
            {
                containerSize.Y = widget.height;
            }
            int axis1 = (int)this.orientation;
            int axis2 = (int)(this.orientation + 1) % 2;
            int position = this.margin;
            int offsetY = 0;

            Window window = widget as Window;
            if (window && !string.IsNullOrEmpty(window.title))
            {
                if (Orientation.Vertical == this.orientation)
                {
                    position += widget.theme.windowHeaderHeight;// - this.margin / 2; // why margin / 2?
                }
                else
                {
                    offsetY = widget.theme.windowHeaderHeight;
                    containerSize.Y -= offsetY;
                }
            }

            //bool first = true;
            int childCount = widget.childCount;
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = widget.GetChild (i);
                if (!child.isVisible)
                {
                    continue;
                }
                //if (first)
                //{
                //    first = false;
                //}
                //else
                //{
                //    position += this.spacing;
                //}

                Vector2 ps = child.GetPreferredSize (ctx);
                Vector2 fs = child.fixedSize;
                Vector2 targetSize;
                targetSize.X = 0 < fs.X ? fs.X : ps.X;
                targetSize.Y = 0 < fs.Y ? fs.Y : ps.Y;

                Vector2 pos;
                pos.X = 0f;
                pos.Y = offsetY;

                pos[axis1] = position;

                switch (this.alignment)
                {
                    case Alignment.Min:
                    {
                        pos[axis2] += this.margin;
                    }
                    break;
                    case Alignment.Middle:
                    {
                        pos[axis2] += (containerSize[axis2] - targetSize[axis2]) / 2;
                    }
                    break;
                    case Alignment.Max:
                    {
                        pos[axis2] += containerSize[axis2] - targetSize[axis2] - this.margin * 2; // why margin * 2 (instead of margin * 1)?
                    }
                    break;
                    case Alignment.Fill:
                    {
                        pos[axis2] += this.margin;
                        targetSize[axis2] = (0 < fs[axis2]) ? fs[axis2] : (containerSize[axis2] - this.margin * 2);
                    }
                    break;
                    default:
                    break;
                }

                child.localPosition = pos;
                child.size = targetSize;
                child.PerformLayout (ctx);
                position += ((int)targetSize[axis1] + this.spacing);
            }
        }

        public override Vector2 GetPreferredSize (NanoVGDotNet.NVGcontext ctx, Widget widget)
        {
            Vector2 ret = Vector2.Zero;
            Vector2 size = Vector2.One * (2 * this.margin);

            float offsetY = 0f;
            Window window = widget as Window;
            if (window && !string.IsNullOrEmpty (window.title))
            {
                if (Orientation.Vertical == this.orientation)
                {
                    size.Y += widget.theme.windowHeaderHeight;// - this.margin / 2;
                }
                else
                {
                    offsetY = widget.theme.windowHeaderHeight;
                }
            }

            int axis1 = (int)this.orientation;
            int axis2 = (axis1 + 1) % 2;

            int childCount = widget.childCount;
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = widget.GetChild (i);
                if (!child.isVisible)
                {
                    continue;
                }
                Vector2 ps = child.GetPreferredSize (ctx);
                Vector2 fs = child.fixedSize;
                Vector2 targetSize;
                targetSize.X = (0 < fs.X) ? fs.X : ps.X;
                targetSize.Y = (0 < fs.Y) ? fs.Y : ps.Y;

                size[axis1] += targetSize[axis1] + this.spacing;
                size[axis2] = Math.Max (size[axis2], targetSize[axis2] + 2 * this.margin);
            }
            ret = size;
            ret.Y += offsetY;

            return ret;
        }
    }
}
