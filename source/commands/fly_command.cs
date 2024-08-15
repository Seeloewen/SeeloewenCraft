
using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {

        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 1)
            {
                world.player.flying = !world.player.flying;
                MessageBox.Show($"Flying status set to {world.player.flying}");
            }
            else
            {
                try
                {
                    world.player.flying = bool.Parse(args[1]);
                    MessageBox.Show($"Flying status set to {world.player.flying}");
                }
                catch
                {
                    MessageBox.Show($"error: couldnt parse flying status to bool");
                }
            }
        }

    }
}
