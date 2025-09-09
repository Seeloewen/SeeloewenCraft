using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.notifications;

namespace SeeloewenCraft.game.core.commands
{
    partial class CommandHandler
    {
        private static void HandleSpawnCommand(string[] args)
        {
            if (Settings.enableMobs)
            {
                if (args.Length != 4)
                {
                    NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: incorrect number of arguments");
                    return;
                }

                Entity entity = EntityRegister.GenerateEntity(args[1]);
                if (entity == null)
                {
                    NotificationHandler.Notify("sc:bedrock_item", $"Invalid command syntax: entity id was not found ({args[1]})");
                    return;
                }

                try
                {
                    entity.posX = int.Parse(args[2]);
                    entity.posY = int.Parse(args[3]);
                }
                catch
                {
                    NotificationHandler.Notify("sc:bedrock_item", "Invalid command syntax: couldn't parse coordinates to int");
                    return;
                }

                Game.world.AddEntity(entity);
                NotificationHandler.Notify("sc:diamond_sword_item", $"Successfully spawned entity {entity.id} at x{entity.posX} y{entity.posY}");
            }
            else
            {
                NotificationHandler.Notify("sc:bedrock_item", $"Cannot spawn mobs because it's disabled in the settings.");
            }
        }
    }
}
