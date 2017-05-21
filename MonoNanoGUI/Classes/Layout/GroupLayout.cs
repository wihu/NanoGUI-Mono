using System;
using OpenTK;

namespace MonoNanoGUI
{
    public class GroupLayout : Layout
    {
        public GroupLayout ()
        {
        }

        public override void PerformLayout (NanoVGDotNet.NVGcontext ctx, Widget widget)
        {
        }

        public override OpenTK.Vector2 GetPreferredSize (NanoVGDotNet.NVGcontext ctx, Widget widget)
        {
            Vector2 ret = Vector2.Zero;
            //int margin = this.margin;
            //int width = margin * 2;
            //int height = width;

            //Window window = widget as Window;
            //if (window && !string.IsNullOrEmpty (window.title))
            //{
            //    height += widget.theme.windowHeaderHeight - margin / 2;
            //}

            //bool first = true;
            //bool indent = false;
            //int childCount = widget.childCount;
            //for (int i = 0; childCount > i; ++i)
            //{
            //    Widget child = widget.GetChild (i);
            //    if (!child.isVisibleSelf)
            //    {
            //        continue;
            //    }

            //    Label label = child as Label;
            //    if (!first)
            //    {
            //        //height += 
            //    }
            //}

            return ret;
        }
    }
}
