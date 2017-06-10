using System;

namespace StarWarrier
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (StarWarrier game = new StarWarrier())
            {
                game.Run();
            }
        }
    }
#endif
}

