using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using MonoNanoGUI;

namespace MonoNanoGUIDemo
{
    class MainClass
    {
        static int[] CalculeAntiAliasingModes ()
        {
	        List<int> aa_modes = new List<int> ();
	        int aa = 0;
        	do
        	{
        		try
        		{
        			GraphicsMode mode = new GraphicsMode (32, 0, 0, aa);
        			if (!aa_modes.Contains (mode.Samples))
        				aa_modes.Add (aa);
        		}
        		catch (Exception)
        		{
        		}
        		finally
        		{
        			aa += 2;
        		}
        	} while (aa <= 32);

	        return aa_modes.ToArray();
        }

        protected static int GetMaxAntiAliasingAvailable (int aaDesired)
        {
	        int aa = 0;
            int[] aaModes = CalculeAntiAliasingModes ();
	        foreach (int i in aaModes)
		    if (i == aaDesired)
		    {
			    aa = i;
			    break;
		    }
		    else if (i < aaDesired && i > aa)
		    {
			    aa = i;
		    }

	        return aa;
        }

        [STAThread]
        public static void Main (string[] args)
        {
            ToolkitOptions options = new ToolkitOptions ();
            options.Backend = PlatformBackend.Default;
            Toolkit.Init (options);

            // need to enable depth / stencil buffers for composite path to work properly
            // e.g. Popup window with rounded rectangle + arrow.
            int aa = GetMaxAntiAliasingAvailable (2);
            GraphicsMode gm = new GraphicsMode (32, 16, 8, aa);

            Console.WriteLine ("Start Demo");
            using (GameWindow window = new DemoWindow (1000, 600, gm))
            {
                try
                {
                    //Screen screen = new Screen (window, Vector2.Zero, "Screen");
                    //Console.WriteLine ("Running Screen = " + screen.ToString ());

                    window.Run (30f, 0f);
                }
                catch (Exception e)
                {
                    Console.WriteLine (e.Message);
                }
            }
        }
    }
}
