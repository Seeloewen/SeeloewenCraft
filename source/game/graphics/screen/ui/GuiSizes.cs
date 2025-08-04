namespace SeeloewenCraft.game.graphics
{
    internal static class GuiSizes
    {
        internal static int mx { get => Resolution.WIDTH / 2; }
        internal static int my { get => Resolution.HEIGHT / 2; }

        //Inventory
        internal const int slotSize = 70;
        internal const int edgeSize = 5;

        internal const int ySlotOffset = 150;
    }
}