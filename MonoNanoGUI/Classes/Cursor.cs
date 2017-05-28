using System;
namespace MonoNanoGUI
{
    public enum Cursor
    {
        Arrow = 0,
        IBeam,
        Crosshair,
        Hand,
        HResize,
        VResize,
        /// Not a cursor --- should always be last: enables a loop over the cursor types.
        CursorCount
    }
}
