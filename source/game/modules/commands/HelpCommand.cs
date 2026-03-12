namespace SeeloewenCraft.game.core.commands
{
    partial class ChatHandler
    {
        private static void HandleHelpCommand(string[] args)
        {
            HandleSystemMessage("Available commands:");
            foreach(var cmd in commandMap)
            {
                HandleSystemMessage($"- /{cmd.Key}");
            }
        }
    }
}
