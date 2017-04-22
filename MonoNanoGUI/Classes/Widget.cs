using System;
using System.Collections.Generic;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Widget
    {
        protected Widget m_Parent;
        protected Theme m_Theme = Theme.DefaultTheme;
        protected string m_Id;
        protected Vector2 m_Pos;
        protected Vector2 m_Size;
        protected Vector2 m_FixedSize;
        protected bool m_Visible;
        protected bool m_Enabled;
        protected bool m_Focused;
        protected bool m_MouseFocus;
        protected string m_Tooltip;
        protected CursorType m_CursorType;
        protected List<Widget> m_Children = new List<Widget> ();

        public int instanceId { get; set; }
        public bool enabled { get; set; }
        public bool focused { get; set; }
        public string tooltipText { get; set; }
        public int fontSize { get; set; }
        public bool hasFontSize { get { return (0 < this.fontSize); } }
        public Vector2 fixedSize { get { return m_FixedSize; } set { m_FixedSize = value; } }
        public float fixedWidth { get { return m_FixedSize.X; } set { m_FixedSize.X = value; } }
        public float fixedHeight { get { return m_FixedSize.Y; } set { m_FixedSize.Y = value; } }
        public bool isVisibleSelf { get; set; }
        public Cursor cursor { get; set; }

        public bool isVisibleInHierarchy
        {
            get
            {
                bool visible = true;
                Widget widget = this;
                while (widget && visible)
                {
                    visible &= widget.isVisibleSelf;
                    widget = widget.parent;
                }

                return visible;
            }
        }

        public int childCount
        {
            get
            {
                return m_Children.Count;
            }
        }

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

        public Theme theme
        {
            get
            {
                return m_Theme ?? (m_Theme = Theme.DefaultTheme);
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

        public Widget[] GetChildren ()
        {
            return m_Children.ToArray ();
        }

        public Widget GetChild (int index)
        {
            return m_Children [index];
        }

        public int GetChildIndex (Widget widget)
        {
            int ret = m_Children.IndexOf (widget);
            return ret;
        }

        public void RequestFocus ()
        {
        }

        public Vector2 GetScreenPosition ()
        {
            if (m_Parent)
            {
                return m_Parent.GetScreenPosition () + m_Pos;
            }
            return m_Pos;
        }

        public bool ContainsPoint (Vector2 p)
        {
            Vector2 pos = GetScreenPosition ();
            Vector2 rel = (p - pos);
            if (0 > rel.X || 0 > rel.Y)
            {
                return false;
            }
            if (rel.X > m_Size.X || rel.Y > m_Size.Y)
            {
                return false;
            }
            return true;
        }

        public Widget FindWidgetAtPoint (Vector2 p)
        {
            return null;
        }

        public virtual bool HandleMouseButtonEvent (Vector2 p, int button, bool down, int modifiers)
        {
            return false;
        }

        public virtual bool HandleMouseMotionEvent (Vector2 p, Vector2 rel, int button, int modifiers)
        {
            return false;
        }

        public virtual bool HandleMouseDragEvent (Vector2 p, Vector2 rel, int button, int modifiers)
        {
            return false;
        }
        public virtual bool HandleMouseEnterEvent (Vector2 p, bool enter)
        {
            return false;
        }

        public virtual bool HandleScrollEvent (Vector2 p, Vector2 rel)
        {
            return false;
        }

        public virtual bool HandleFocusEvent (bool focused)
        {
            return false;
        }

        public virtual bool HandleKeyEvent (int key, int scanCode, int action, int modifiers)
        {
            return false;
        }
        public virtual bool HandleKeyCharEvent (uint codePoint)
        {
            return false;
        }

        public virtual Vector2i GetPreferredSize (NVGcontext ctx)
        {
	        return Vector2i.Zero;
        }

        public virtual void PerformLayout (NVGcontext ctx)
        {
            
        }
        public virtual void Draw (NVGcontext ctx)
        {
        }

        public virtual void Save (Serializer s)
        {
        }
        public virtual void Load (Serializer s)
        {
        }

#region Builder Methods
        public Widget WithPosition (Vector2 pos)
        {
            this.localPosition = pos;
            return this;
        }

        public Widget WithFontSize (int size)
        {
            this.fontSize = size;
            return this;
        }
        public Widget WithFixedSize (Vector2 size)
        {
            this.fixedSize = size;
            return this;
        }
#endregion
        public static implicit operator bool (Widget obj)
        {
            return (null != obj);
        }
    }
}
