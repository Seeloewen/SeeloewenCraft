
using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {

        public static void HandleFlyCommand(string[] args)
        {
            if (args.Length == 1)
            {
                Game.world.player.flying = !Game.world.player.flying;
                MessageBox.Show($"Flying status set to {Game.world.player.flying}");
            }
            else
            {
                try
                {
                    Game.world.player.flying = bool.Parse(args[1]);
                    MessageBox.Show($"Flying status set to {Game.world.player.flying}");
                }
                catch
                {
                    MessageBox.Show($"error: couldnt parse flying status to bool");
                }
            }
        }

    }
}
