using System;
using System.Text;
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

        static byte[] icon = new byte[8];

        /// <summary>
        /// Unicode code point to UTF8. (mysterious code)
        /// </summary>
        /// <returns>UTF8 string of the unicode.</returns>
        /// <param name="cp">code point.</param>
        public static string UnicodeToUTF8 (int cp)
        {
        	int n = 0;
        	if (cp < 0x80)
        		n = 1;
        	else if (cp < 0x800)
        		n = 2;
        	else if (cp < 0x10000)
        		n = 3;
        	else if (cp < 0x200000)
        		n = 4;
        	else if (cp < 0x4000000)
        		n = 5;
        	else if (cp <= 0x7fffffff)
        		n = 6;
        	icon[n] = (byte)'\0';
        	switch (n)
        	{
        		case 6:
        			goto case_6;
        		case 5:
        			goto case_5;
        		case 4:
        			goto case_4;
        		case 3:
        			goto case_3;
        		case 2:
        			goto case_2;
        		case 1:
        			goto case_1;
        	}
        	goto end;

            case_6:
            	icon[5] = (byte)(0x80 | (cp & 0x3f));
            	cp = cp >> 6;
            	cp |= 0x4000000;
            case_5:
            	icon[4] = (byte)(0x80 | (cp & 0x3f));
            	cp = cp >> 6;
            	cp |= 0x200000;
            case_4:
            	icon[3] = (byte)(0x80 | (cp & 0x3f));
            	cp = cp >> 6;
            	cp |= 0x10000;
            case_3:
            	icon[2] = (byte)(0x80 | (cp & 0x3f));
            	cp = cp >> 6;
            	cp |= 0x800;
            case_2:
            	icon[1] = (byte)(0x80 | (cp & 0x3f));
            	cp = cp >> 6;
            	cp |= 0xc0;
            case_1:
            	icon[0] = (byte)cp;

            end:

            	string r = new string (Encoding.UTF8.GetChars (icon, 0, n));
            	r = r.Trim (new char[] { '\0' });
            	int rl = r.Length;

	        return r;
        }
    }
}
