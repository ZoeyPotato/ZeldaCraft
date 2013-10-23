using System;


namespace ZeldaCraft
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ZeldaCraft game = new ZeldaCraft())
            {
                game.Run();
            }
        }
    }
#endif
}
