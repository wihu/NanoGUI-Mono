using System;
using OpenTK;
using MonoNanoGUI;

namespace MonoNanoGUIDemo
{
    class MainClass
    {
        [STAThread]
        public static void Main (string[] args)
        {
            ToolkitOptions options = new ToolkitOptions ();
            options.Backend = PlatformBackend.PreferNative;
            Toolkit.Init (options);

            Console.WriteLine ("Start Demo");
            using (GameWindow window = new DemoWindow (1000, 600))
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
