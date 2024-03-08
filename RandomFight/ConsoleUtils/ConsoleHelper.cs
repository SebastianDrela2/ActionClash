using System.Runtime.InteropServices;

public static class ConsoleHelper
{
    private static IntPtr ConsoleOutputHandle => GetStdHandle(StandardOutputHandle);

    private const int FixedWidthTrueType = 54;
    private const int StandardOutputHandle = -11;   

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);
   
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FontInfo
    {
        internal int cbSize;
        internal int FontIndex;
        internal short FontWidth;
        public short FontSize;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        
        public string FontName;
    }

    public static void SetCurrentFont(string font, short fontSize)
    {               
        var set = new FontInfo
        {
            cbSize = Marshal.SizeOf<FontInfo>(),
            FontIndex = 0,
            FontFamily = FixedWidthTrueType,
            FontName = font,
            FontWeight = 400,
            FontSize = fontSize
        };

        if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
        {
            var ex = Marshal.GetLastWin32Error();
            Console.WriteLine($"Setting font error: {ex}");

            throw new System.ComponentModel.Win32Exception(ex);
        }                
    }
}