using System;

namespace Space394
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Space394Game game = new Space394Game())
            {
                game.Run();
            }
        }
    }
#endif
}

