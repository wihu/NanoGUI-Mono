using System;
using OpenTK;
using OpenTK.Input;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public class CheckBox : Widget
    {

        public event Action<CheckBox> OnChangeCallback = delegate { };

        public string caption { get; set; }
        public bool check { get; set; }
        public bool pushed { get; set; }

        public CheckBox ()
            : this (null)
        {
        }

        public CheckBox (Widget parent, string caption = "Checkbox")
            : base (parent)
        {
            this.caption = caption;
            this.pushed = false;
            this.check = false;
        }

        public override Vector2 GetPreferredSize (NVGcontext ctx)
        {
            if (!Vector2.Zero.Equals (this.fixedSize))
            {
                return this.fixedSize;
            }

            int currFontSize = GetPreferredFontSize ();
            int currFontFace = Fonts.Get (this.theme.fontNormal);
            NanoVG.nvgFontSize (ctx, currFontSize);
            NanoVG.nvgFontFace (ctx, currFontFace);

            float tw = NanoVG.nvgTextBounds (ctx, 0f, 0f, this.caption, null);
            Vector2 ret;
            ret.X = tw + 1.7f * currFontSize;
            ret.Y = 1.3f * currFontSize;

            return ret;
        }

        public override void Draw (NVGcontext ctx)
        {
            base.Draw (ctx);

            Theme style = this.theme;
            Vector2 pos = this.localPosition;
            Vector2 size = this.size;

            if (!string.IsNullOrEmpty (this.caption))
            {
                int currFontSize = GetPreferredFontSize ();
                if (0 < currFontSize)
                {
                    int currFontFace = Fonts.Get (this.theme.fontNormal);
                    NVGcolor currTextColor = this.enabled ? style.textColor : style.disabledTextColor;

                    NanoVG.nvgFontSize (ctx, currFontSize);
                    NanoVG.nvgFontFace (ctx, currFontFace);
                    NanoVG.nvgFillColor (ctx, currTextColor);
                    NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));

                    NanoVG.nvgText (ctx, pos.X + size.Y * 1.2f + 5, pos.Y + this.size.Y * 0.5f, this.caption);
                }
            }

            Color4f col = Color4f.Black;
            Color4f icol = this.pushed ? col.WithAlpha (100) : col.WithAlpha (32);
            Color4f ocol = col.WithAlpha (180);

            NVGpaint bg = NanoVG.nvgBoxGradient (ctx, pos.X + 1.5f, pos.Y + 1.5f
                                                 , size.X - 2f, size.Y - 2f, 3f, 3f
                                                 , icol.ToNVGColor ()
                                                 , ocol.ToNVGColor ());

            NanoVG.nvgBeginPath (ctx);
            NanoVG.nvgRoundedRect (ctx, pos.X + 1f, pos.Y + 1f, size.Y - 2f, size.Y - 2f, 3f);
            NanoVG.nvgFillPaint (ctx, bg);
            NanoVG.nvgFill (ctx);

            if (this.check)
            {
                int fontIcons = Fonts.Get (style.fontIcons);
                NVGcolor iconColor = this.enabled ? style.iconColor : style.disabledTextColor;
                NanoVG.nvgFontSize (ctx, 1.8f * size.Y);
                NanoVG.nvgFontFace (ctx, fontIcons);
                NanoVG.nvgFillColor (ctx, iconColor);
                NanoVG.nvgTextAlign (ctx, (int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));
                byte[] icon = Fonts.GetIconUTF8 ((int)Font.Entypo.ICON_CHECK);
                NanoVG.nvgText (ctx, pos.X + size.Y * 0.5f + 1f, pos.Y + size.Y * 0.5f, icon);
            }
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
                    this.pushed = true;
                }
                else if (this.pushed)
                {
                    if (ContainsPoint (p))
                    {
                        this.check = !this.check;
                        OnChangeCallback (this);
                    }
                    this.pushed = false;
                }
                return true;
            }

            return false;
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
        public CheckBox WithCaption (string caption)
        {
            this.caption = caption;
            return this;
        }
        #endregion

    }
}
