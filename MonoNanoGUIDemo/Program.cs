using System;
using OpenTK;
using MonoNanoGUI;

namespace MonoNanoGUIDemo
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Console.WriteLine ("Start Demo");
            using (GameWindow window = new GameWindow (1000, 600))
            {
                try
                {
                    Screen screen = new Screen (window, Vector2.Zero, "Screen");
                    Console.WriteLine ("Running Screen = " + screen.ToString ());

                    window.Run ();
                }
                catch (Exception e)
                {
                    Console.WriteLine (e.Message);
                }
            }
        }
    }
}
