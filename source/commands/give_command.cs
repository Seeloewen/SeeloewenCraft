
namespace SeeloewenCraft
{
    partial class CommandHandler
    {

        private static void HandleGiveCommand(string[] args)
        {
            if (!(args.Length == 2 || args.Length == 3))
            {
                Write("error: incorrect number of arguments");
                return;
            }
            string id = args[1];
            int amount = 1;
            if (args.Length > 2)
            {
                try
                {
                    amount = int.Parse(args[2]);
                }
                catch
                {
                    Write("error: cant parse amount to int");
                    return;
                }
            }
            for (int i = 0; i < amount; i++)
            {
                Item item = ItemRegister.GenerateItem(id, world);
                if(i == 0 && item == null)
                {
                    Write("error: item id not found");
                    return;
                }
                world.player.inventory.AddItem(item);
            }
            Write($"succesfully gave player {amount} of {id}");
        }

    }
}
