using SeeloewenCraft.game.core.legacy;
using SeeloewenCraft.game.notifications;
using System.Globalization;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {

        public static void HandleHealCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[1], CultureInfo.InvariantCulture);

                Game.world.player.Heal(amount); //TODO: Replace icon
                NotificationHandler.Notify("sc:bedrock_item", $"Succesfully healed player {amount}hp to {Game.world.player.hp}hp");
            }
            catch
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: can't parse healing amount to double");
                return;
            }

        }
        public static void HandleDamageCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[1], CultureInfo.InvariantCulture);

                Game.world.player.Damage(amount);
                NotificationHandler.Notify("sc:bedrock_item", $"Succesfully damaged player {amount}hp to {Game.world.player.hp}hp");
            }
            catch
            {
                NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: can't parse damage amount to double");
                return;
            }
        }

        public static void HandleHPCommand(string[] args)
        {
            NotificationHandler.Notify("sc:bedrock_item", $"Current hp: {Game.world.player.hp}"); //TODO: Replace icon
        }
    }
}
