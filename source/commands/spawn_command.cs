
namespace SeeloewenCraft
{
    partial class CommandHandler
    {
        private static void HandleSpawnCommand(string[] args)
        {
            if (args.Length != 4)
            {
                Write("error: incorrect command length");
                return;
            }

            Entity entity = EntityRegister.GenerateEntity(args[1], world);
            if (entity == null)
            {
                Write("error: entity id not found");
                return;
            }

            try
            {
                entity.posX = int.Parse(args[2]);
                entity.posY = int.Parse(args[3]);
            } catch
            {
                Write("error: couldnt parse coordinates");
                return;
            }

            world.AddEntity(entity);
            Write("successfully spawned entity");
        }
    }
}
