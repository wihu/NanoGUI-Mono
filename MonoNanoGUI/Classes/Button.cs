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
        protected int m_Icon;
        protected List<Button> m_ButtonGroup = new List<Button> ();

        private string m_IconUTF8 = string.Empty;

        public string caption { get; set; }
        public IconAnchorType iconAnchorType { get; set; }
        public Flags flags { get; set; }
        public NVGcolor backgroundColor { get; set; }
        public NVGcolor textColor { get; set; }

        public int icon 
        { 
            get
            {
                return m_Icon;
            }
            set
            {
                if (value != m_Icon)
                {
                    m_IconUTF8 = Fonts.UnicodeToUTF8 (value);
                }
                m_Icon = value;
            }
        }

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
            this.fontSize = -1;
            this.icon = icon;
            this.iconAnchorType = IconAnchorType.LeftCentered;
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

        public override int GetPreferredFontSize ()
        {
            // put override font size in higher priority?
            int ret = 0 <= this.fontSize ? this.fontSize : this.theme.buttonFontSize;
            return ret;
        }

        public override Vector2 GetPreferredSize (NVGcontext ctx)
        {
            Vector2 ret;

            int prefFontSize = GetPreferredFontSize ();
            NanoVG.nvgFontSize (ctx, prefFontSize);
            NanoVG.nvgFontFace (ctx, this.theme.fontBold);
            float tw = NanoVG.nvgTextBounds (ctx, 0f, 0f, this.caption, null);
            float iw = 0f;
            float ih = prefFontSize;

            int btnIcon = this.icon;

            if (0 < btnIcon)
            {
                if (NanoVG.nvgIsFontIcon (btnIcon))
                {
                    ih *= 1.5f;
                    NanoVG.nvgFontFace (ctx, this.theme.fontIcons);
                    NanoVG.nvgFontSize (ctx, ih);
                    iw = NanoVG.nvgTextBounds (ctx, 0f, 0f, m_IconUTF8, null) + this.size.Y * 0.15f;
                }
                else
                {
                    int w, h;
                    w = h = 1;
                    ih *= 0.9f;
                    NanoVG.nvgImageSize (ctx, btnIcon, ref w, ref h);
                    iw = w * ih / h;
                }
            }
            ret.X = (int)(tw + iw) + 20;
            ret.Y = prefFontSize + 10;

            return ret;
        }

        protected NVGcolor GetCurrTextColor ()
        {
            if (!this.enabled)
            {
                return this.theme.disabledTextColor;
            }

            NVGcolor ret = (0f < this.textColor.a) ? this.textColor : this.theme.textColor;
            return ret;
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

            if (0f < this.backgroundColor.a)
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

            int currFontSize = 0 > this.fontSize ? style.buttonFontSize : this.fontSize;
            NanoVG.nvgFontSize (ctx, currFontSize);
            NanoVG.nvgFontFace (ctx, style.fontBold);

            float tw = NanoVG.nvgTextBounds (ctx, 0f, 0f, this.caption, null);
            Vector2 center = pos + size * 0.5f;
            Vector2 textPos = new Vector2 (center.X - tw * 0.5f, center.Y - 1f);
            NVGcolor currTextColor = GetCurrTextColor ();

            int btnIcon = this.icon;
            if (0 != btnIcon)
            {
                float iw, ih;
                iw = 0f;
                ih = currFontSize;
                if (NanoVG.nvgIsFontIcon (btnIcon))
                {
                    ih *= 1.5f;
                    NanoVG.nvgFontSize (ctx, ih);
                    NanoVG.nvgFontFace (ctx, style.fontIcons);
                    iw = NanoVG.nvgTextBounds (ctx, 0, 0, m_IconUTF8, null);
                }
                else
                {
                    int w, h;
                    w = h = 1;
                    ih *= 0.9f;
                    NanoVG.nvgImageSize (ctx, btnIcon, ref w, ref h);
                    if (0 < h)
                    {
                        iw = w * ih / h;
                    }
                }

                if (!string.IsNullOrEmpty (this.caption))
                {
                    iw += size.Y * 0.15f;
                }
                NanoVG.nvgFillColor (ctx, currTextColor);
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
                Vector2 iconPos = center;
                iconPos.Y -= 1f;

                switch (this.iconAnchorType)
                { 
                    case IconAnchorType.LeftCentered:
                        {
                            iconPos.X -= (tw + iw) * 0.5f;
                            textPos.X += iw * 0.5f;
                        }
                        break;
                    case IconAnchorType.RightCentered:
                        {
                            textPos.X -= iw * 0.5f;
                            iconPos.X += tw * 0.5f;
                        }
                        break;
                    case IconAnchorType.Left:
                        {
                            iconPos.X = pos.X + 8f;
                        }
                        break;
                    case IconAnchorType.Right:
                        {
                            iconPos.X = pos.X + size.X - iw - 8f;
                        }
                        break;
                    default:
                        break;
                }

                if (NanoVG.nvgIsFontIcon (btnIcon))
                {
                    // NOTE: icon rendering bug, any unicode > 0x10000 not being rendered correctly.
                    //       e.g. 0x1F680 (Font.Entypo.ICON_ROCKET).
                    NanoVG.nvgText (ctx, iconPos.X, iconPos.Y + 1f, m_IconUTF8);
                }
                else
                {
                    float imgAlpha = this.enabled ? 0.5f : 0.25f;
                    NVGpaint imgPaint = NanoVG.nvgImagePattern (
                        ctx
                        , iconPos.X, iconPos.Y - ih * 0.5f, iw, ih
                        , 0f, btnIcon, imgAlpha);
                    NanoVG.nvgFillPaint (ctx, imgPaint);
                    NanoVG.nvgFill (ctx);
                }

                // DEBUG: ICON BOUNDS
                //NanoVG.nvgStrokeWidth (ctx, 1.0f);
                //NanoVG.nvgBeginPath (ctx);
                //NanoVG.nvgRect (ctx, iconPos.X, iconPos.Y - ih * 0.5f, iw, ih);
                //NanoVG.nvgStrokeColor (ctx, textColor);
                //NanoVG.nvgStroke(ctx);
            }

            NanoVG.nvgFontSize (ctx, currFontSize);
            NanoVG.nvgFontFace (ctx, style.fontBold);
            NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
            NanoVG.nvgFillColor (ctx, style.textShadowColor);
            NanoVG.nvgText (ctx, textPos.X, textPos.Y, this.caption);
            NanoVG.nvgFillColor (ctx, currTextColor);
            NanoVG.nvgText (ctx, textPos.X, textPos.Y + 1f, this.caption);
        }

        #region Helper Methods
        public static Button MakeToolButton (int icon)
        {
            Button button = new Button (null, "", icon);
            button.flags = (Flags.RadioButton | Flags.ToggleButton);
            button.fixedSize = new Vector2 (25, 25);
            return button;
        }
        #endregion

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

        public Button WithIcon (int icon, IconAnchorType anchorType = IconAnchorType.Left)
        {
            this.icon = icon;
            this.iconAnchorType = anchorType;
            return this;
        }
        #endregion
    }
}
