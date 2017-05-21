using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class Theme
    {
        public string name                              { get; private set; }

        // Fonts.
        public string fontNormal                        { get; private set; }
        public string fontBold                          { get; private set; }
        public string fontIcons                         { get; private set; }

        // Spacing related parameters.
        public int standardFontSize                     { get; private set; }
        public int buttonFontSize                       { get; private set; }
        public int textBoxFontSize                      { get; private set; }
        public int windowCornerRadius                   { get; private set; }
        public int windowHeaderHeight                   { get; private set; }
        public int windowDropShadowSize                 { get; private set; }
        public int buttonCornerRadius                   { get; private set; }
        public float tabBorderWidth                     { get; private set; }
        public int tabInnerMargin                       { get; private set; }
        public int tabMinButtonWidth                    { get; private set; }
        public int tabMaxButtonWidth                    { get; private set; }
        public int tabControlWidth                      { get; private set; }
        public int tabButtonHorizontalPadding           { get; private set; }
        public int tabButtonVerticalPadding             { get; private set; }

        // Generic colors.
        public NVGcolor dropShadowColor                 { get; private set; }
        public NVGcolor transparentColor                { get; private set; }
        public NVGcolor borderDarkColor                 { get; private set; }
        public NVGcolor borderLightColor                { get; private set; }
        public NVGcolor borderMediumColor               { get; private set; }
        public NVGcolor textColor                       { get; private set; }
        public NVGcolor disabledTextColor               { get; private set; }
        public NVGcolor textShadowColor                 { get; private set; }
        public NVGcolor iconColor                       { get; private set; }

        // Button colors.
        public NVGcolor buttonGradientTopFocusedColor   { get; private set; }
        public NVGcolor buttonGradientBotFocusedColor   { get; private set; }
        public NVGcolor buttonGradientTopUnfocusedColor { get; private set; }
        public NVGcolor buttonGradientBotUnfocusedColor { get; private set; }
        public NVGcolor buttonGradientTopPushedColor    { get; private set; }
        public NVGcolor buttonGradientBotPushedColor    { get; private set; }

        // Window colors.
        public NVGcolor windowFillUnfocusedColor        { get; private set; }
        public NVGcolor windowFillFocusedColor          { get; private set; }
        public NVGcolor windowTitleUnfocusedColor       { get; private set; }
        public NVGcolor windowTitleFocusedColor         { get; private set; }

        public NVGcolor windowHeaderGradientTopColor    { get; private set; }
        public NVGcolor windowHeaderGradientBotColor    { get; private set; }
        public NVGcolor windowHeaderSepTopColor         { get; private set; }
        public NVGcolor windowHeaderSepBotColor         { get; private set; }

        public NVGcolor windowPopupColor                { get; private set; }
        public NVGcolor windowPopupTransparentColor     { get; private set; }


        private Theme (string name)
        {
            this.name = name;
        }

        public static implicit operator bool (Theme obj)
        {
	        return (null != obj);
        }

        public static readonly Theme DefaultTheme = new Theme ("Default Dark")
        {
            standardFontSize                = 16,
            buttonFontSize                  = 20,
            textBoxFontSize                 = 20,
            windowCornerRadius              = 2,
            windowHeaderHeight              = 30,
            windowDropShadowSize            = 10,
            buttonCornerRadius              = 2,
            tabBorderWidth                  = 0.75f,
            tabInnerMargin                  = 5,
            tabMinButtonWidth               = 20,
            tabMaxButtonWidth               = 160,
            tabControlWidth                 = 20,
            tabButtonHorizontalPadding      = 10,
            tabButtonVerticalPadding        = 2,

            dropShadowColor                 = new Color4f (0, 128).ToNVGColor (),
            transparentColor                = new Color4f (0, 0).ToNVGColor (),
            borderDarkColor                 = new Color4f (29, 255).ToNVGColor (),
            borderLightColor                = new Color4f (92, 255).ToNVGColor (),
            borderMediumColor               = new Color4f (35, 255).ToNVGColor (),
            textColor                       = new Color4f (255, 160).ToNVGColor (),
            disabledTextColor               = new Color4f (255, 80).ToNVGColor (),
            textShadowColor                 = new Color4f (0, 160).ToNVGColor (),
            iconColor                       = new Color4f (255, 160).ToNVGColor (),

            buttonGradientTopFocusedColor   = new Color4f (64, 255).ToNVGColor (),
            buttonGradientBotFocusedColor   = new Color4f (48, 255).ToNVGColor (),
            buttonGradientTopUnfocusedColor = new Color4f (74, 255).ToNVGColor (),
            buttonGradientBotUnfocusedColor = new Color4f (58, 255).ToNVGColor (),
            buttonGradientTopPushedColor    = new Color4f (41, 255).ToNVGColor (),
            buttonGradientBotPushedColor    = new Color4f (29, 255).ToNVGColor (),

            windowFillUnfocusedColor        = new Color4f (43, 230).ToNVGColor (),
            windowFillFocusedColor          = new Color4f (45, 230).ToNVGColor (),
            windowTitleUnfocusedColor       = new Color4f (220, 160).ToNVGColor (),
            windowTitleFocusedColor         = new Color4f (255, 190).ToNVGColor (),

            windowHeaderGradientTopColor    = new Color4f (74, 255).ToNVGColor (),
            windowHeaderGradientBotColor    = new Color4f (58, 255).ToNVGColor (),
            windowHeaderSepTopColor         = new Color4f (92, 255).ToNVGColor (),
            windowHeaderSepBotColor         = new Color4f (29, 255).ToNVGColor (),

            windowPopupColor                = new Color4f (50, 255).ToNVGColor (),
            windowPopupTransparentColor     = new Color4f (50, 0).ToNVGColor (),

            fontNormal                      = "sans",
            fontBold                        = "sans-bold",
            fontIcons                       = "icons",
        };

        public static Theme FromFile (string filename)
        {
            Theme ret = new Theme (filename);
            // TODO:
            return ret;
        }
    }
}
