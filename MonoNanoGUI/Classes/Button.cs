using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    /// <summary>
    /// [Normal/Toggle/Radio/Popup] Button widget.
    /// </summary>
    public class Button : Widget
    {
        /// Flags to specify the button behavior (can be combined with binary OR).
        [Flags]
        public enum Flags
        {
            NormalButton = (1 << 0),
            RadioButton  = (1 << 1),
            ToggleButton = (1 << 2),
            PopupButton  = (1 << 3)
        }

        /// The available icon anchor positions.
        public enum IconAnchorType
        {
            Left,
            LeftCentered,
            RightCentered,
            Right
        }

        protected bool m_Pushed;
        protected List<Button> m_ButtonGroup = new List<Button> ();

        public string caption { get; set; }
        public int icon { get; set; }
        public IconAnchorType iconAnchorType { get; set; }
        public Flags flags { get; set; }
        public NVGcolor backgroundColor { get; set; }
        public NVGcolor textColor { get; set; }

        public bool pushed
        {
            get
            {
                return m_Pushed;
            }
            set
            {
                bool changed = m_Pushed ^ value;
                m_Pushed = value;

                if (changed)
                {
                    OnChangeCallback (this, m_Pushed);
                }
            }
        }

        /// Set the push callback (for any type of button)
        public event Action<Button> OnClickCallback = delegate { };
        /// Set the change callback (for toggle buttons)
        public event Action<Button, bool> OnChangeCallback = delegate { };

        public Button () : this (null)
        {
        }

        public Button (Widget parent, string caption = "Button", int icon = 0)
            : base (parent)
        {
            this.caption = caption;
            this.flags = Flags.NormalButton;
            this.pushed = false;
        }

        public bool HasFlag (Flags flag)
        {
            bool ret = (0 != (this.flags & flag));
            return ret;
        }

        public override bool HandleMouseButtonEvent (Vector2 p, int button, bool down, int modifiers)
        {
            base.HandleMouseButtonEvent (p, button, down, modifiers);

            if (!this.enabled)
            {
                return false;
            }

            if ((int)MouseButton.Left == button)
            {
                if (down)
                {
                    if (HasFlag (Flags.RadioButton))
                    {
                        int groupCount = m_ButtonGroup.Count;
                        if (0 == groupCount)
                        {
                            int siblingCount = this.parent.childCount;
                            for (int i = 0; siblingCount > i; ++i)
                            {
                                Button b = this.parent.GetChild (i) as Button;
                                if (null == b || this == b)
                                {
                                    continue;
                                }
                                if (b.HasFlag (Flags.RadioButton) && b.pushed)
                                {
                                    b.pushed = false;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; groupCount > i; ++i)
                            {
                                Button b = m_ButtonGroup[i];
                                if (this == b)
                                {
                                    continue;
                                }
                                if (b.HasFlag (Flags.RadioButton) && b.pushed)
                                {
                                    b.pushed = false;
                                }
                            }
                        }
                    }
                    if (HasFlag (Flags.PopupButton))
                    {
                        int siblingCount = this.parent.childCount;
                        for (int i = 0; siblingCount > i; ++i)
                        {
                            Button b = this.parent.GetChild (i) as Button;
                            if (null == b || this == b)
                            {
                                continue;
                            }
                            if (b.HasFlag (Flags.PopupButton) && b.pushed)
                            {
                                b.pushed = false;
                            }
                        }
                    }

                    if (HasFlag (Flags.ToggleButton))
                    {
                        this.pushed = !this.pushed;
                    }
                    else
                    {
                        this.pushed = true;
                    }
                }
                else if (this.pushed)
                {
                    if (ContainsPoint (p))
                    {
                        OnClickCallback (this);
                    }
                    if (HasFlag (Flags.NormalButton))
                    {
                        this.pushed = false;
                    }
                }

                return true;
            }

            return false;
        }

        public override Vector2 GetPreferredSize (NVGcontext ctx)
        {
            return Vector2.Zero;
        }
        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);

            Theme style = this.theme;

            NVGcolor gradTopColor = style.buttonGradientTopUnfocusedColor;
            NVGcolor gradBotColor = style.buttonGradientBotUnfocusedColor;

            if (this.pushed)
            {
                gradTopColor = style.buttonGradientTopPushedColor;
                gradBotColor = style.buttonGradientBotPushedColor;
            }
            else
            {
                gradTopColor = style.buttonGradientTopFocusedColor;
                gradBotColor = style.buttonGradientBotFocusedColor;
            }

            Vector2 pos = this.localPosition;
            Vector2 size = this.size;

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X + 1f, pos.Y + 1f, size.X - 2f, size.Y - 2f
                                   , style.buttonCornerRadius - 1f);

            NanoVG.nvgFillColor (ctx, gradTopColor);
            NanoVG.nvgFill (ctx);

            if (0 < this.backgroundColor.a)
            {
                // fill background.
            }

            NVGpaint gradient = NanoVG.nvgLinearGradient (ctx, pos.X, pos.Y, pos.X, pos.Y + size.Y
                                                          , gradTopColor, gradBotColor);
            NanoVG.nvgFillPaint (ctx, gradient);
            NanoVG.nvgFill (ctx);

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgStrokeWidth (ctx, 1f);
            NanoVG.nvgRoundedRect (ctx, pos.X + 0.5f, pos.Y + (this.pushed ? 0.5f : 1.5f)
                                   , size.X - 1f, size.Y - 1 - (this.pushed ? 0f : 1f)
                                   , style.buttonCornerRadius);
            NanoVG.nvgStrokeColor (ctx, style.borderLightColor);
            NanoVG.nvgStroke (ctx);

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X + 0.5f, pos.Y + 0.5f
                                   , size.X - 1f, size.Y - 2f
                                   , style.buttonCornerRadius);
            NanoVG.nvgStrokeColor (ctx, style.borderDarkColor);
            NanoVG.nvgStroke (ctx);

            //int fontSize = m_FontSize;
            int fontSize = style.buttonFontSize;
            NanoVG.nvgFontSize (ctx, fontSize);
            NanoVG.nvgFontFace (ctx, style.fontBold);
            NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));

            float tw = NanoVG.nvgTextBounds (ctx, 0f, 0f, this.caption, null);
            Vector2 center = pos + size * 0.5f;
            Vector2 textPos = new Vector2 (center.X - tw * 0.5f, center.Y - 1f);

            NanoVG.nvgFillColor (ctx, style.textColor);
            NanoVG.nvgText (ctx, textPos.X, textPos.Y + 1f, this.caption);
        }

        #region Builder Methods
        public Button WithCaption (string caption)
        {
            this.caption = caption;
            return this;
        }
        public Button WithClickCallback (Action<Button> callback)
        {
            OnClickCallback -= callback;
            OnClickCallback += callback;
        	return this;
        }

        public Button WithFlags (Flags flags)
        {
            this.flags = flags;
            return this;
        }

        public Button WithChangeCallback (Action<Button, bool> callback)
        {
            OnChangeCallback -= callback;
            OnChangeCallback += callback;
            return this;
        }

        public Button WithBackgroundColor (NVGcolor color)
        {
            this.backgroundColor = color;
            return this;
        }

        public Button WithIcon (int icon)
        {
            this.icon = icon;
            return this;
        }
#endregion
    }
}
