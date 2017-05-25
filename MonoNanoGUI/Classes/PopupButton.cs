using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class PopupButton : Button
    {
        private Popup m_Popup;
        private Window m_ParentWindow;

        public int chevronIcon { get; set; }

        public Popup popup 
        { 
            get
            {
                RefreshPopup ();

                return m_Popup;
            }
        }

        public PopupButton ()
            : this (null)
        {
        }

        public PopupButton (Widget parent
                            , string caption = "Popup Button"
                            , int buttonIcon = 0
                            , int chevronIcon = (int)Font.Entypo.ICON_CHEVRON_SMALL_RIGHT
                           )
            : base (parent, caption, buttonIcon)
        {
            this.chevronIcon = chevronIcon;
            this.flags = Flags.ToggleButton | Flags.PopupButton;

            RefreshPopup ();
        }

        public void RefreshPopup ()
        {
            // TODO: make popup works without parent window?
            Window parentWindow = GetParentWindow ();
            if (null == parentWindow)
            {
                return;
            }
            if ((null == m_Popup) || (parentWindow != m_ParentWindow))
            {                    
                m_ParentWindow = parentWindow;
                m_Popup = new Popup (parentWindow.parent, parentWindow);
                m_Popup.isVisible = false;
            }
        }

        public override Vector2 GetPreferredSize (NVGcontext ctx)
        {
            Vector2 ret = base.GetPreferredSize (ctx);
            ret.X += 15f;
            return ret;
        }

        public override void PerformLayout (NVGcontext ctx)
        {
            base.PerformLayout (ctx);

            if (null == m_Popup || null == m_ParentWindow)
            {
                return;
            }

            Vector2 anchor;
            anchor.X = m_ParentWindow.width + 15f;
            anchor.Y = this.GetScreenPosition ().Y - m_ParentWindow.localPosition.Y + this.size.Y * 0.5f;

            m_Popup.anchorPos = anchor;
        }

        public override void Draw (NVGcontext ctx)
        {
            if (!this.enabled && this.pushed)
            {
                this.pushed = false;
            }

            if (m_Popup)
            {
                m_Popup.isVisible = this.pushed;
            }

            base.Draw (ctx);

            if (0 != this.chevronIcon)
            {
                Theme style = this.theme;
                string iconStr = Fonts.GetIconUTF8 (this.chevronIcon);
                NVGcolor currTextColor = GetCurrTextColor ();
                int currFontSize = (0 <= this.fontSize) ? this.fontSize : style.buttonFontSize;
                int fontFace = Fonts.Get (style.fontIcons);

                NanoVG.nvgFontSize (ctx, currFontSize * 1.5f);
                NanoVG.nvgFontFace (ctx, fontFace);
                NanoVG.nvgFillColor (ctx, currTextColor);
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));

                float iw = NanoVG.nvgTextBounds (ctx, 0f, 0f, iconStr, null);
                Vector2 iconPos = this.localPosition;
                iconPos.X += this.size.X - iw - 8;
                iconPos.Y += this.size.Y * 0.5f - 1;
                NanoVG.nvgText (ctx, iconPos.X, iconPos.Y, iconStr);
            }
        }

        public override void Save (Serializer s)
        {
            base.Save (s);
        }

        public override void Load (Serializer s)
        {
            base.Load (s);
        }

        #region Builder Methods
        public PopupButton WithChevron (int icon)
        {
            this.chevronIcon = icon;
            return this;
        }
        #endregion
    }
}
