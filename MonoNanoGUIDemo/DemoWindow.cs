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
        Widget screen;

        // NOTE: Only works with Compiler x64, NativeWindow runs but freezes on x86.
        public DemoWindow (int width, int height, GraphicsMode gm)
            //: base (width, height, GraphicsMode.Default, "MonoNanoGUI Demo Window", GameWindowFlags.Default, DisplayDevice.Default, 3, 3, GraphicsContextFlags.ForwardCompatible)
            : base (width, height, gm, "MonoNanoGUI Demo Window")
        {
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            GL.ClearColor (Color.Cornsilk);

            GlNanoVG.nvgCreateGL (ref ctx, (int)NVGcreateFlags.NVG_ANTIALIAS |
                (int)NVGcreateFlags.NVG_STENCIL_STROKES |
                (int)NVGcreateFlags.NVG_DEBUG);

            Fonts.Load (ctx, "sans", "Roboto-Regular.ttf");
            Fonts.Load (ctx, "sans-bold", "Roboto-Bold.ttf");
            Fonts.Load (ctx, "icons", "entypo.ttf");

            screen = new Widget ()
                .WithLocalPosition (Vector2.Zero)
                .WithSize (new Vector2 (this.Width, this.Height));

            {
                Window window = screen.AddNewWidget<Window> ();
                window.WithTitle ("Button demo")
                      .WithLocalPosition (new Vector2 (15f, 50f))
                      .WithSize (new Vector2 (250f, 400f))
                      .WithLayout (new GroupLayout ());

                // -- Push buttons
                window.AddNewWidget<Label> ()
                      .WithCaption ("Push buttons")
                      .WithFont ("sans-bold");

                window.AddNewWidget<Button> ()
                      .WithCaption ("Plain button")
                      .WithClickCallback ((btn) => Console.WriteLine ("Click!"));

                window.AddNewWidget<Button> ()
                      .WithCaption ("Styled")
                      .WithIcon ((int)MonoNanoGUI.Font.Entypo.ICON_LOGIN, Button.IconAnchorType.LeftCentered);

                // -- Toggle buttons
                window.AddNewWidget<Label> ()
                      .WithCaption ("Toggle buttons")
                      .WithFont ("sans-bold");

                window.AddNewWidget<Button> ()
                      .WithFlags (Button.Flags.ToggleButton)
                      .WithCaption ("Toggle me");

                // -- Radio buttons
                window.AddNewWidget<Label> ()
                      .WithCaption ("Radio buttons")
                      .WithFont ("sans-bold");

                window.AddNewWidget<Button> ()
                      .WithCaption ("Radio button 1")
                      .WithFlags (Button.Flags.RadioButton);

                window.AddNewWidget<Button> ()
                      .WithCaption ("Radio button 2")
                      .WithFlags (Button.Flags.RadioButton);
                
                // -- Tool buttons palette
                window.AddNewWidget<Label> ()
                      .WithCaption ("A tool palette")
                      .WithFont ("sans-bold");

                Widget tools = window.AddNewWidget<Widget> ()
                                     .WithLayout (new BoxLayout (Layout.Orientation.Horizontal, Layout.Alignment.Middle, 0, 6));
                tools.AddChild (Button.MakeToolButton ((int)MonoNanoGUI.Font.Entypo.ICON_CLOUD));
                tools.AddChild (Button.MakeToolButton ((int)MonoNanoGUI.Font.Entypo.ICON_FF));
                tools.AddChild (Button.MakeToolButton ((int)MonoNanoGUI.Font.Entypo.ICON_COMPASS));
                tools.AddChild (Button.MakeToolButton ((int)MonoNanoGUI.Font.Entypo.ICON_INSTALL));

                // -- Popup buttons
                window.AddNewWidget<Label> ()
                      .WithCaption ("Popup buttons")
                      .WithFont ("sans-bold");

                Popup popup = new Popup (window.parent, window);
                Vector2 anchor;
                anchor.X = window.width + 15f;
                anchor.Y = 0f;
                popup.anchorPos = anchor;
                //window.AddChild (popup);

            }

            screen.PerformLayout (ctx);

            PerfGraph.InitGraph ((int)GraphrenderStyle.GRAPH_RENDER_FPS, "FPS");
            Console.WriteLine ("Load");
            //Console.WriteLine ("Test Unicode to UTF8 = " + Fonts.UnicodeToUTF8 (0).Length);
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

            if (screen)
            {
                screen.Draw (ctx);
            }
            PerfGraph.RenderGraph (ctx, 5, 5);

            NanoVG.nvgEndFrame(ctx);

            this.SwapBuffers();
        }

        protected override void OnMouseDown (OpenTK.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown (e);

            Vector2 p = new Vector2 (e.Mouse.X, e.Mouse.Y);
            if (screen && screen.ContainsPoint (p))
            {
                //Console.WriteLine (e.Mouse.X + ", " + e.Mouse.Y);
                screen.HandleMouseButtonEvent (p, (int)e.Button, e.IsPressed, 0);
            }
        }

        protected override void OnMouseUp (OpenTK.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp (e);

            if (screen)
            {
                //Console.WriteLine (e.Mouse.X + ", " + e.Mouse.Y);
                Vector2 p = new Vector2 (e.Mouse.X, e.Mouse.Y);
                screen.HandleMouseButtonEvent (p, 0, e.IsPressed, 0);
            }
        }
    }
}
