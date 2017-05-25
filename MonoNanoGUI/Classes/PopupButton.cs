using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class PopupButton : Button
    {
        public Popup popup { get; private set; }
        public int chevronIcon { get; set; }

        public PopupButton (Widget parent
                            , string caption = "Popup Button"
                            , int buttonIcon = 0
                            , int chevronIcon = (int)Font.Entypo.ICON_CHEVRON_SMALL_RIGHT
                           )
            : base (parent, caption, buttonIcon)
        {
            this.chevronIcon = chevronIcon;
            this.flags = Flags.ToggleButton | Flags.PopupButton;
            Window parentWindow = GetParentWindow ();
            if (parentWindow)
            {
                this.popup = new Popup (parentWindow.parent, GetParentWindow ());
                this.popup.isVisible = false;
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

            Window parentWindow = GetParentWindow ();

            if (null == parentWindow)
            {
                return;
            }

        }
        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);
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
