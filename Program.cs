using Avalonia;
using Avalonia.Threading;
using SeeloewenCraft;
using SeeloewenCraft.game;
using System;
using System.Linq.Expressions;

namespace SeeloewenCraft
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            StartOptions.Parse(args);

            try
            {
                BuildAvaloniaApp()
                                .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception e)
            {
                //Error Handling here
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .WithDeveloperTools()
#endif
                .WithInterFont()
                .LogToTrace();
    }
}
