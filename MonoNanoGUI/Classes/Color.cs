using System;
using OpenTK;

namespace MonoNanoGUI
{
    public struct Color
    {
        private Vector4 v;

        public float r { get { return v.X; } }
        public float g { get { return v.Y; } }
        public float b { get { return v.Z; } }
        public float a { get { return v.W; } }

        public byte rb { get { return (byte)(v.X * 255); } }
        public byte gb { get { return (byte)(v.Y * 255); } }
        public byte bb { get { return (byte)(v.Z * 255); } }
        public byte ab { get { return (byte)(v.W * 255); } }

        public Color (Vector4 color)
        {
            this.v = color;
        }

        public Color (float r, float g, float b, float a)
            : this (new Vector4 (r, g, b, a)) { }

        public Color (Vector3 color, float alpha) 
            : this (color.X, color.Y, color.Z, alpha) { }

        public Color (float intensity, float alpha) 
            : this (Vector3.One * intensity, alpha) { }

        public Color (int intensity, int alpha) 
            : this (Vector3.One * intensity / 255f, alpha / 255f) { }
        
        public Color (Vector3 color) 
            : this (color, 1f) { }

        public Color (Vector3i color) 
            : this (color.Div (255f)) { }
        
        public Color (int r, int g, int b, int a) 
            : this ((new Vector4i (r, g, b, a).Div (255f))) { }

        public Color Contrast ()
        {
            float luminance = v.Dot (new Vector4 (0.299f, 0.587f, 0.144f, 0f));
            float intensity = luminance < 0.5f ? 1f : 0f;
            return new Color (intensity, 1f);
        }
    }
}
