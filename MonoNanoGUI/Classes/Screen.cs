using System;
using OpenTK;
using OpenTK.Platform;

namespace MonoNanoGUI
{
    public class Screen : Widget
    {
        public Screen (INativeWindow window, Vector2 size, string caption, bool resizable = true, bool fullscreen = false)
            : base (null)
        {
            InitializeWindow (window);
        }

        private void InitializeWindow (INativeWindow window)
        {
            Console.WriteLine ("Initializing window.");    
        }

    }
}
