
namespace SeeloewenCraft
{
    partial class CommandHandler
    {

        private static void HandleSetBlockCommand(string[] args)
        {
            if (args.Length != 5)
            {
                Write("error: incorrect number of arguments");
                return;
            }
            string id = args[1];
            try
            {
                int posX = int.Parse(args[2]);
                int posY = int.Parse(args[3]);
                int chunkID = int.Parse(args[4]);

                Block block = BlockRegister.GenerateBlock(id, world);

                if(block == null)
                {
                    Write("error: block id not found");
                    return;
                }

                world.SetBlock(block , posX + 8 * chunkID, posY);
                Write($"successfully placed block {id} at x:{posX} and y:{posY} in chunk {chunkID}");
            }
            catch
            {
                Write("error: cant parse cooordinates to int");
                return;
            }

            

        }

    }
}
