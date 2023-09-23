using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VoiceCommentsExtension.Services
{
    public class WindowsService
    {
        public static Point GetStartWindowPosition(Window window)
        {
            var helper = new WindowInteropHelper(window);
            var screen = Screen.FromHandle(helper.Handle);

            return new Point(
                Math.Max(0, Math.Min(
                    Cursor.Position.X, 
                    screen.WorkingArea.Width - window.Width)),
                Math.Max(0, Math.Min(
                    Cursor.Position.Y, 
                    screen.WorkingArea.Height - window.Height)));
        }
    }
}