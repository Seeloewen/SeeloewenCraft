using SeeloewenCraft.game.core.entities;
using SeeloewenCraft.game.core.settings;
using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleSpawnCommand(string[] args)
        {
            if (Settings.enableMobs)
            {
                if (args.Length != 3)
                {
                    HandleSystemMessage("Invalid command syntax: incorrect number of arguments");
                    return;
                }

                Entity entity = EntityRegister.GenerateEntity(args[0]);
                if (entity == null)
                {
                    HandleSystemMessage($"Invalid command syntax: entity id was not found ({args[0]})");
                    return;
                }

                try
                {
                    entity.posX = int.Parse(args[1]);
                    entity.posY = int.Parse(args[2]);
                }
                catch
                {
                    HandleSystemMessage("Invalid command syntax: couldn't parse coordinates to int");
                    return;
                }

                World.Get().AddEntity(entity);
                HandleSystemMessage($"Successfully spawned entity {entity.id} at x{entity.posX} y{entity.posY}");
            }
            else
            {
                HandleSystemMessage($"Cannot spawn mobs because it's disabled in the settings.");
            }
        }
    }
}
