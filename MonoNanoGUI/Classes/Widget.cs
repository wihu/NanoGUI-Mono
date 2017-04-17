using System;
using System.Collections.Generic;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Widget
    {
        protected Widget m_Parent;
        protected Theme m_Theme;
        protected string m_Id;
        protected Vector2 m_Pos;
        protected Vector2 m_Size;
        protected Vector2 m_FixedSize;
        protected bool m_Visible;
        protected bool m_Enabled;
        protected bool m_Focused;
        protected bool m_MouseFocus;
        protected string m_Tooltip;
        protected int m_FontSize;
        protected CursorType m_CursorType;
        protected List<Widget> m_Children = new List<Widget> ();

        public Widget parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                m_Parent = value;
            }
        }

        public Vector2 localPosition
        {
            get
            {
                return m_Pos;
            }
            set
            {
                m_Pos = value;
            }
        }

        public Vector2 size
        {
            get
            {
                return m_Size;
            }
            set
            {
                m_Size = value;
            }
        }

        public Widget (Widget parent)
        {
            if (parent)
            {
                parent.AddChild (this);
            }
        }

        public virtual void AddChild (int index, Widget widget)
        {

        }

        public void AddChild (Widget widget)
        {
        }
        public void RemoveChild (int index)
        { 
        }
        public void RemoveChild (Widget widget)
        {
        }

        public virtual void Draw (NVGcontext ctx)
        {
        }

        public static implicit operator bool (Widget obj)
        {
            return (null != obj);
        }
    }
}
