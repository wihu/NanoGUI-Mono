using System;
using OpenTK;

namespace MonoNanoGUI
{
    public class Theme
    {
        public int fontNormal           { get; private set; }
        public int fontBold             { get; private set; }
        public int fontIcons            { get; private set; }

        public int standardFontSize     { get; private set; }
        public int buttonFontSize       { get; private set; }
        public int textBoxFontSize      { get; private set; }

        public Color dropShadowColor    { get; private set; }
        public Color transparentColor   { get; private set; }
        public Color borderDarkColor    { get; private set; }
        public Color borderLightColor   { get; private set; }
        public Color borderMediumColor  { get; private set; }
        public Color textColor          { get; private set; }
        public Color disabledTextColor  { get; private set; }
        public Color textShadowColor    { get; private set; }
        public Color iconColor          { get; private set; }

        public Theme ()
        {
        }
    }
}
