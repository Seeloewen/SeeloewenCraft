using System.Windows;
using SeeloewenCraft.entity;

namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleSpawnCommand(string[] args)
        {
            if (Settings.enableMobs)
            {
                if (args.Length != 4)
                {
                    NotificationHandler.ShowNotification("Invalid command syntax: incorrect number of arguments", 3000);
                    return;
                }

                Entity entity = EntityRegister.GenerateEntity(args[1]);
                if (entity == null)
                {
                    NotificationHandler.ShowNotification($"Invalid command syntax: entity id was not found ({args[1]})", 3000);
                    return;
                }

                try
                {
                    entity.posX = int.Parse(args[2]);
                    entity.posY = int.Parse(args[3]);
                }
                catch
                {
                    NotificationHandler.ShowNotification("Invalid command syntax: couldn't parse coordinates to int", 3000);
                    return;
                }

                Game.world.AddEntity(entity);
                NotificationHandler.ShowNotification($"Successfully spawned entity {entity.id} at x{entity.posX} y{entity.posY}", 3000);
            }
            else
            {
                NotificationHandler.ShowNotification($"Cannot spawn mobs because it's disabled in the settings.", 3000);
            }
        }
    }
}
