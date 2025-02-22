


namespace SeeloewenCraft.game.ui.ui_lib
{
    internal class CTexture : Component
    {

        string id;
        TextureMap map;

        public CTexture(TextureMap map, string id, Rectangle bounds) : base(bounds)
        {
            this.map = map;
            this.id = id;
        }

        public void setID(string id)
        {
            this.id = id;
        }

        protected override void OnRender()
        {
            if (id != null)
            {
                TextureRenderer.SetTexture(map);
                TextureRenderer.Draw(id, bounds);
            }
        }


    }
}
