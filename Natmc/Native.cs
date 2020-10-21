using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc
{
    public static class Native
    {
        public static void ErrorBoxAndExit(string title, string msg)
        {
            // This might as well display a Win32 MessageBox
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length + 1));

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press any key to exit\a");
            Console.ReadKey(true);

            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(1);
        }
    }
}
