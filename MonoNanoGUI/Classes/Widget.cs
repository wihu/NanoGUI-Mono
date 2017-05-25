using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Widget
    {
        protected Theme m_Theme = Theme.DefaultTheme;
        protected string m_Id;
        protected Vector2 m_LocalPos;
        protected Vector2 m_Size;
        protected Vector2 m_FixedSize;
        protected bool m_MouseFocus;
        protected string m_Tooltip;
        protected CursorType m_CursorType;
        protected List<Widget> m_Children = new List<Widget> ();

        public Widget parent { get; set; }
        public Layout layout { get; set; }
        public int instanceId { get; set; }
        public bool enabled { get; set; }
        public bool focused { get; set; }
        public string tooltipText { get; set; }
        public int fontSize { get; set; }
        public bool hasFontSize { get { return (0 < this.fontSize); } }
        public Vector2 size { get { return m_Size; } set { m_Size = value; } }
        public float width { get { return m_Size.X; } set { m_Size.X = value; } }
        public float height { get { return m_Size.Y; } set { m_Size.Y = value; } }
        public Vector2 fixedSize { get { return m_FixedSize; } set { m_FixedSize = value; } }
        public float fixedWidth { get { return m_FixedSize.X; } set { m_FixedSize.X = value; } }
        public float fixedHeight { get { return m_FixedSize.Y; } set { m_FixedSize.Y = value; } }
        public bool isVisible { get; set; }
        public Cursor cursor { get; set; }

        public bool isVisibleRecursive
        {
            get
            {
                bool visible = true;
                Widget widget = this;
                while (widget && visible)
                {
                    visible &= widget.isVisible;
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

        public Vector2 localPosition
        {
            get
            {
                return m_LocalPos;
            }
            set
            {
                m_LocalPos = value;
            }
        }

        public Theme theme
        {
            get
            {
                return m_Theme ?? (m_Theme = Theme.DefaultTheme);
            }
            set
            {
                m_Theme = value;
            }
        }

        public Widget ()
            : this (null)
        {
        }

        public Widget (Widget parent)
        {
            if (parent)
            {
                parent.AddChild (this);
            }

            this.enabled = true;
            this.isVisible = true;
        }

        public virtual int GetPreferredFontSize ()
        {
            // put override font size in higher priority?
            int ret = this.fontSize;
            if (0 <= ret)
            {
                return ret;
            }
            if (this.theme)
            {
                return this.theme.standardFontSize;
            }
            return ret;
        }

        public virtual void AddChild (int index, Widget widget)
        {
            m_Children.Insert (index, widget);
            widget.parent = this;
            widget.theme = this.theme;
        }

        public void AddChild (Widget widget)
        {
            AddChild (this.childCount, widget);
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
            return m_Children[index];
        }

        public int GetChildIndex (Widget widget)
        {
            int ret = m_Children.IndexOf (widget);
            return ret;
        }

        public Window GetParentWindow ()
        {
            Widget widget = this;
            while (widget)
            {
                Window window = widget as Window;
                if (window)
                {
                    return window;
                }
                widget = widget.parent;
            }
            return null;
        }

        public void RequestFocus ()
        {
        }

        public Vector2 GetScreenPosition ()
        {
            if (this.parent)
            {
                return this.parent.GetScreenPosition () + this.localPosition;
            }
            return this.localPosition;
        }

        public bool ContainsPoint (Vector2 p)
        {
            Vector2 local = (p - this.localPosition);
            if (0 > local.X || 0 > local.Y)
            {
                return false;
            }
            if (local.X > m_Size.X || local.Y > m_Size.Y)
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
            Vector2 local = p - this.localPosition;
            int childCount = m_Children.Count;
            for (int i = childCount - 1; 0 <= i; --i)
            {
                Widget child = m_Children[i];
                if (!child.isVisible || !child.ContainsPoint (local))
                {
                    continue;
                }
                if (child.HandleMouseButtonEvent (local, button, down, modifiers))
                {
                    return true;
                }
            }
            if ((int)MouseButton.Left == button && down && !this.focused)
            {
                RequestFocus ();
            }
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

        public virtual Vector2 GetPreferredSize (NVGcontext ctx)
        {
            if (this.layout)
            {
                return this.layout.GetPreferredSize (ctx, this);
            }
            return this.size;
        }

        public Vector2 GetTargetSize (NVGcontext ctx, Vector2 preferredSize)
        { 
            Vector2 fixedSize = this.fixedSize;
            Vector2 targetSize;
            targetSize.X = 0f < fixedSize.X? fixedSize.X : preferredSize.X;
            targetSize.Y = 0f < fixedSize.Y? fixedSize.Y : preferredSize.Y;

            return targetSize;            
        }

        public Vector2 GetTargetSize (NVGcontext ctx)
        {
            Vector2 ps = GetPreferredSize (ctx);
            return GetTargetSize (ctx, ps);
        }

        public void ApplyTargetSize (NVGcontext ctx)
        {
            this.size = GetTargetSize (ctx);
        }

        public virtual void PerformLayout (NVGcontext ctx)
        {
            if (this.layout)
            {
                this.layout.PerformLayout (ctx, this);
                return;
            }

            int childCount = this.childCount;
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = this.GetChild (i);
                child.ApplyTargetSize (ctx);
                child.PerformLayout (ctx);
            }
        }

        protected void DrawChildren (NVGcontext ctx)
        {
            int childCount = m_Children.Count;
            if (0 == childCount)
            {
                return;
            }

            Vector2 pos = this.localPosition;
            NanoVG.nvgTranslate (ctx, pos.X, pos.Y);
            for (int i = 0; childCount > i; ++i)
            {
                Widget child = m_Children[i];
                if (child.isVisible)
                {
                    child.Draw (ctx);
                }
            }
            NanoVG.nvgTranslate (ctx, -pos.X, -pos.Y);
        }

        public virtual void Draw (NVGcontext ctx)
        {
            // TODO: draw debug bounds.

            DrawChildren (ctx);
        }

        public virtual void Save (Serializer s)
        {
        }
        public virtual void Load (Serializer s)
        {
        }

        #region Builder Methods
        public Widget WithLocalPosition (Vector2 pos)
        {
            this.localPosition = pos;
            return this;
        }

        public Widget WithFontSize (int size)
        {
            this.fontSize = size;
            return this;
        }
        public Widget WithSize (Vector2 size)
        {
            this.size = size;
            return this;
        }
        public Widget WithFixedSize (Vector2 size)
        {
            this.fixedSize = size;
            return this;
        }
        public Widget WithLayout (Layout layout)
        {
            this.layout = layout;
            return this;
        }
#endregion

#region Helper Methods
        public T AddNewWidget<T> () where T : Widget, new()
        {
            T widget = new T ();
            this.AddChild (widget);
            return widget;
        }
#endregion

        public static implicit operator bool (Widget obj)
        {
            return (null != obj);
        }
    }
}
