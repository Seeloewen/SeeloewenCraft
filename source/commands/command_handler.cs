
namespace SeeloewenCraft
{
     public static partial class CommandHandler
    {
        static World world;


        public static void HandleCommand(string command, World world)
        {
            CommandHandler.world = world;

            command = command.ToLower();
            command = command.Remove(0, 1);
            
            world.log.Write($"Handling command: {command}", "Info");

            string[] args = command.Split(' ');

            switch(args[0])
            {
                case "give":
                    HandleGiveCommand(args);
                    break;
                case "setblock":
                    HandleSetBlockCommand(args);
                    break;
                case "ping":
                    Write("pong");
                    break;
                case "help":
                    Write("not implemented yet lol");
                    break;
                default:
                    Write("Command not found. Type /help for a list of commands.");
                    break;
            }
        }


        private static void Write(string msg)
        {
            world.log.Write($"{msg}", "Info");
        }

    }
}
