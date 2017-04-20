using System;
using System.Collections.Generic;
using NanoVGDotNet;

namespace MonoNanoGUI
{
    public static class Fonts
    {
        private static Dictionary<string, int> s_FontMap = new Dictionary<string, int> ();
        private static readonly string RESOURCES_PATH = "Resources/Fonts/";

        public static void Load (NVGcontext ctx, string fontName, string fileName)
        {
            string filePath = RESOURCES_PATH + fileName;
            int fontHandle = NanoVG.nvgCreateFont (ctx, fontName, filePath);
            s_FontMap[fontName] = fontHandle;
        }

        public static int Get (string name)
        {
            int ret = -1;
            if (!s_FontMap.TryGetValue (name, out ret))
            {
                Console.WriteLine ("No font loaded with name = " + name);
                return ret;
            }
            return ret;
        }
    }
}
