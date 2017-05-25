using System;
using OpenTK;

namespace MonoNanoGUI
{
    public class GroupLayout : Layout
    {
        public int margin { get; set; }
        public int spacing { get; set; }
        public int groupSpacing { get; set; }
        public int groupIndent { get; set; }

        public GroupLayout (int margin = 15, int spacing = 6, int groupSpacing = 14, int groupIndent = 20)
        {
            this.margin = margin;
            this.spacing = spacing;
            this.groupSpacing = groupSpacing;
            this.groupIndent = groupIndent;
        }

        public override Vector2 GetPreferredSize (NanoVGDotNet.NVGcontext ctx, Widget widget)
        {
            Vector2 ret = Vector2.Zero;

            int width = this.margin * 2;
            int height = width;

            Window window = widget as Window;
            if (window && !string.IsNullOrEmpty (window.title))
            {
                height += widget.theme.windowHeaderHeight - margin / 2;
            }

            bool first = true;
            bool indent = false;
            int childCount = widget.childCount;
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = widget.GetChild (i);
                if (!child.isVisible)
                {
                    continue;
                }

                Label label = child as Label;
                if (!first)
                {
                    height += (null == label) ? this.spacing : this.groupSpacing;
                }
                else
                {
                    first = false;
                }

                Vector2 targetSize = child.GetTargetSize (ctx);
                bool indentCur = indent && (null == label);
                height += (int)targetSize.Y;
                width = (int)Math.Max (width, targetSize.X + 2 * this.margin + (indentCur ? this.groupIndent : 0f));

                // start indent next child when we encounter non-empty label.
                if (label)
                {
                    indent = !string.IsNullOrEmpty (label.caption);
                }
            }

            ret.X = width;
            ret.Y = height;
            return ret;
        }

        public override void PerformLayout (NanoVGDotNet.NVGcontext ctx, Widget widget)
        {
            int height = this.margin;
            int width = (int)((0 < widget.fixedSize.X) ? widget.fixedSize.X : widget.width);
            int availableWidth = width - 2 * this.margin;

            Window window = widget as Window;
            if (window && window.HasTitle ())
            {
                height += (widget.theme.windowHeaderHeight - this.margin / 2);
            }

            bool first = true;
            bool indent = false;
            int childCount = widget.childCount;
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = widget.GetChild (i);
                if (!child.isVisible)
                {
                    continue;
                }

                Label label = child as Label;
                if (!first)
                {
                    height += (null == label) ? this.spacing : this.groupSpacing;
                }
                else
                {
                    first = false;
                }

                int currIndent = (indent && (null == label)) ? this.groupIndent : 0;

                Vector2 ps = child.GetPreferredSize (ctx);
                ps.X = availableWidth - currIndent;
                Vector2 targetSize = child.GetTargetSize (ctx, ps);

                Vector2 pos;
                pos.X = this.margin + currIndent;
                pos.Y = height;

                child.localPosition = pos;
                child.size = targetSize;
                child.PerformLayout (ctx);

                height += (int)targetSize.Y;

                if (label)
                {
                    indent = label.HasCaption ();
                }
            }
        }
    }
}
