using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using NanoVGDotNet;
using MonoNanoGUI;

namespace MonoNanoGUIDemo
{
    public class DemoWindow : GameWindow
    {
        NVGcontext ctx;
        Button button;

        // NOTE: Only works with Compiler x64, NativeWindow runs but freezes on x86.
        public DemoWindow (int width, int height)
            //: base (width, height, GraphicsMode.Default, "MonoNanoGUI Demo Window", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
            : base (width, height, GraphicsMode.Default, "MonoNanoGUI Demo Window")
        {
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            GL.ClearColor (System.Drawing.Color.White);

            GlNanoVG.nvgCreateGL (ref ctx, (int)NVGcreateFlags.NVG_ANTIALIAS |
                (int)NVGcreateFlags.NVG_STENCIL_STROKES |
                (int)NVGcreateFlags.NVG_DEBUG);

            Fonts.Load (ctx, "sans", "Roboto-Regular.ttf");
            Fonts.Load (ctx, "sans-bold", "Roboto-Bold.ttf");
            Fonts.Load (ctx, "icons", "entypo.ttf");

            button = new Button ();
            button.localPosition = new Vector2 (50f, 50f);
            button.size = new Vector2 (200f, 40f);

            Console.WriteLine ("Load");
        }

        protected override void OnResize (EventArgs e)
        {
            base.OnResize (e);

	        GL.Viewport (0, 0, Width, Height);

	        GL.MatrixMode (MatrixMode.Projection);
	        GL.LoadIdentity ();
	        GL.Ortho (-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
            Console.WriteLine ("Resize");
        }

        protected override void OnRenderFrame (FrameEventArgs e)
        {
            base.OnRenderFrame (e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            //GL.Flush ();
            NanoVG.nvgBeginFrame(ctx, Width, Height, 1);
            button.Draw (ctx);

            //NanoVG.nvgBeginPath (ctx);
            //NanoVG.nvgRect (ctx, 100,100, 120,30);
            //NanoVG.nvgFillColor (ctx, NanoVG.nvgRGBA(255,192,0,255));
            //NanoVG.nvgFill (ctx);

            NanoVG.nvgEndFrame(ctx);

            this.SwapBuffers();
        }
    }
}
