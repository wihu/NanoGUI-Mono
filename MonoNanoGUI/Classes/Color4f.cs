using System;
using OpenTK;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public struct Color4f
    {
        public static readonly Color4f White = new Color4f (1f, 1f);
        public static readonly Color4f Black = new Color4f (0f, 1f);
        public static readonly Color4f Red   = new Color4f (1f, 0f, 0f, 1f);
        public static readonly Color4f Green = new Color4f (0f, 1f, 0f, 1f);
        public static readonly Color4f Blue  = new Color4f (0f, 0f, 1f, 1f);

        private Vector4 v;

        public float r { get { return v.X; } }
        public float g { get { return v.Y; } }
        public float b { get { return v.Z; } }
        public float a { get { return v.W; } }

        public byte rb { get { return (byte)(v.X * 255); } }
        public byte gb { get { return (byte)(v.Y * 255); } }
        public byte bb { get { return (byte)(v.Z * 255); } }
        public byte ab { get { return (byte)(v.W * 255); } }

        public Color4f (Vector4 color)
        {
            this.v = color;
        }

        public Color4f (float r, float g, float b, float a)
            : this (new Vector4 (r, g, b, a)) { }

        public Color4f (Vector3 color, float alpha) 
            : this (color.X, color.Y, color.Z, alpha) { }

        public Color4f (float intensity, float alpha) 
            : this (Vector3.One * intensity, alpha) { }

        public Color4f (int intensity, int alpha) 
            : this (Vector3.One * intensity / 255f, alpha / 255f) { }
        
        public Color4f (Vector3 color) 
            : this (color, 1f) { }

        public Color4f (Vector3i color) 
            : this (color.Div (255f)) { }
        
        public Color4f (int r, int g, int b, int a) 
            : this ((new Vector4i (r, g, b, a).Div (255f))) { }

        public Color4f Contrast ()
        {
            float luminance = v.Dot (new Vector4 (0.299f, 0.587f, 0.144f, 0f));
            float intensity = luminance < 0.5f ? 1f : 0f;
            return new Color4f (intensity, 1f);
        }

        public NVGcolor ToNVGColor ()
        {
            NVGcolor ret;
            ret.r = this.r;
            ret.g = this.g;
            ret.b = this.b;
            ret.a = this.a;
            return ret;
        }
    }
}
