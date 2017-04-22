using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using NanoVGDotNet;
using MonoNanoGUI;

namespace MonoNanoGUIDemo
{
    public class DemoWindow : GameWindow
    {
        NVGcontext ctx;
        Window window;

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

            Button button = new Button ();
            button.localPosition = new Vector2 (5f, 5f);
            button.size = new Vector2 (200f, 40f);

            window = new Window ();
            window.localPosition = new Vector2 (50f, 50f);
            window.size = new Vector2 (250f, 400f);
            window.AddChild (button);

            PerfGraph.InitGraph ((int)GraphrenderStyle.GRAPH_RENDER_FPS, "FPS");
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

            PerfGraph.UpdateGraph ((float)e.Time);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            NanoVG.nvgBeginFrame(ctx, Width, Height, 1);
            window.Draw (ctx);

            PerfGraph.RenderGraph (ctx, 5, 5);

            NanoVG.nvgEndFrame(ctx);

            this.SwapBuffers();
        }

        protected override void OnMouseDown (OpenTK.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown (e);

            Vector2 p = new Vector2 (e.Mouse.X, e.Mouse.Y);
            if (window && window.ContainsPoint (p))
            {
                //Console.WriteLine (e.Mouse.X + ", " + e.Mouse.Y);
                window.HandleMouseButtonEvent (p, (int)e.Button, e.IsPressed, 0);
            }
        }

        protected override void OnMouseUp (OpenTK.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp (e);

            if (window)
            {
                //Console.WriteLine (e.Mouse.X + ", " + e.Mouse.Y);
                Vector2 p = new Vector2 (e.Position.X, e.Position.Y);
                window.HandleMouseButtonEvent (p, 0, e.IsPressed, 0);
            }
        }
    }
}
