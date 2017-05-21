using System;
using NanoVGDotNet;
using OpenTK;

namespace MonoNanoGUI
{
    public abstract class Layout
    {
        public enum Alignment
        {
            Min = 0,
            Middle,
            Max,
            Fill,
        }

        public enum Orientation
        {
            Horizontal = 0,
            Vertical,
        }

        public abstract void PerformLayout (NVGcontext ctx, Widget widget);
        public abstract Vector2 GetPreferredSize (NVGcontext ctx, Widget widget);

        public static implicit operator bool (Layout obj)
        {
	        return (null != obj);
        }
    }
}
