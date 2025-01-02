
using System.Globalization;
using System.Windows;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {

        public static void HandleHealCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[1], CultureInfo.InvariantCulture);

                Game.world.player.Heal(amount);
                NotificationHandler.ShowNotification($"Succesfully healed player {amount}hp to {Game.world.player.hp}hp", 3000);
            }
            catch
            {
                NotificationHandler.ShowNotification("Invalid command syntax: can't parse healing amount to double", 3000);
                return;
            }

        }
        public static void HandleDamageCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[1], CultureInfo.InvariantCulture);

                Game.world.player.Damage(amount);
                NotificationHandler.ShowNotification($"Succesfully damaged player {amount}hp to {Game.world.player.hp}hp", 3000);
            }
            catch
            {
                NotificationHandler.ShowNotification("Invalid command syntax: can't parse damage amount to double", 3000);
                return;
            }
        }

        public static void HandleHPCommand(string[] args)
        {
            NotificationHandler.ShowNotification($"Current hp: {Game.world.player.hp}", 3000);
        }
    }
}
