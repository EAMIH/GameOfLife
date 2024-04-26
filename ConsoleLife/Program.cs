using System;
using System.Runtime.InteropServices;

namespace ConsoleLife
{

    public class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int MAXIMIZE = 3;
        static void Main(string[] args)
        {
            Console.ReadLine();

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(GetConsoleWindow(), MAXIMIZE);

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            GameEngine gameEngine = new GameEngine
            (
                rows: 168,
                cols: 630,
                density: 2
            );
                 
            while ( true )
            {
                Console.Title = gameEngine.currentGeneration.ToString();

                var field = gameEngine.GetCurrentGenetation();

                for ( int y = 0; y < field.GetLength(1); y++ ) 
                {
                    var str = new char[field.GetLength(0)];

                    for ( int x = 0; x < field.GetLength(0); x++ )
                    {
                        if (field[x, y])
                            str[x] = '#';
                        else
                            str[x] = ' ';
                    }
                    Console.WriteLine(str);
                }
                Console.SetCursorPosition(0, 0);
                gameEngine.NextGeneration();
            }

        }
    }
}
