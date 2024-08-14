
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

                world.player.Heal(amount);
                MessageBox.Show($"Succesfully healed player {amount}hp to {world.player.hp}hp", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch {
                MessageBox.Show("Invalid command syntax: can't parse healing amount to double", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
        public static void HandleDamageCommand(string[] args)
        {
            try
            {
                double amount = double.Parse(args[1], CultureInfo.InvariantCulture);

                world.player.Damage(amount);
                MessageBox.Show($"Succesfully damaged player {amount}hp to {world.player.hp}hp", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Invalid command syntax: can't parse damage amount to double", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static void HandleHPCommand(string[] args)
        {
            MessageBox.Show($"current hp: {world.player.hp}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
