using System;

namespace Morningstar
{
#if WINDOWS || XBOX
    static class Run
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }
    }
#endif
}

