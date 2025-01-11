

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

    }
}
