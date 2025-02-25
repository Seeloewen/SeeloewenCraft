namespace SeeloewenCraft.game.ui
{
    internal class ItemRenderer
    {

        static TextureMap textureMap;

        static internal void Init()
        {
            textureMap = new TextureMap("items");
        }


        static internal void SetTexture()
        {
            TextureRenderer.SetTexture(textureMap);
        }

        static internal void Draw(string itemID, float x1, float y1, float x2, float y2)
        {
            TextureRenderer.Draw(itemID, x1, y1, x2, y2, 1f);
        }

        static internal void Draw(string itemID, int x1, int y1, int x2, int y2)
        {
            (float s1, float t1) = Resolution.PixelToScreen(x1, y1);
            (float s2, float t2) = Resolution.PixelToScreen(x2, y2);
            TextureRenderer.Draw(itemID, s1, t1, s2, t2, 1f);
        }

        static internal TextureMap GetTextureMap()
        {
            return textureMap;
        }

    }
}
