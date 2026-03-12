using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.notifications;
using System.Globalization;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {

        public static void HandleHealCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[0], CultureInfo.InvariantCulture);

                Player.Get().Heal(amount);
                HandleSystemMessage($"Succesfully healed player {amount}hp to {Player.Get().hp}hp");
            }
            catch
            {
                HandleSystemMessage("Invalid command syntax: can't parse healing amount to double");
                return;
            }

        }
        public static void HandleDamageCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[0], CultureInfo.InvariantCulture);

                Player.Get().Damage(amount);
                HandleSystemMessage($"Succesfully damaged player {amount}hp to {Player.Get().hp}hp");
            }
            catch
            {
                HandleSystemMessage("Invalid command syntax: can't parse damage amount to double");
                return;
            }
        }

        public static void HandleHPCommand(string[] args)
        {
            HandleSystemMessage($"Current hp: {Player.Get().hp}");
        }
    }
}
