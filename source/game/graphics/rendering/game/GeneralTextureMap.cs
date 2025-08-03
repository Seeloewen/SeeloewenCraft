namespace SeeloewenCraft.game.graphics
{
    internal static class GeneralTextureMap
    {
        private static TextureMap textureMap;

        public static void Init()
        {
            textureMap = new TextureMap("general");
        }

        public static TextureMap Get()
        {
            return textureMap;
        }
    }
}
