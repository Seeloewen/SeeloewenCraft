


namespace SeeloewenCraft.game.ui.ui_lib
{

    /// <summary>
    /// Basic Image component
    /// </summary>
    internal class CTexture : Component
    {

        string id;
        TextureMap map;

        /// <summary>
        /// Creates a basic image component
        /// </summary>
        /// <param name="map">TextureMap where the texture is drawn from</param>
        /// <param name="id">Id of the texture </param>
        /// <param name="bounds">Bounding box of the image</param>
        public CTexture(TextureMap map, string id, Rectangle bounds) : base(bounds)
        {
            this.map = map;
            this.id = id;
        }

        /// <summary>
        /// Changes the drawn texture
        /// </summary>
        /// <param name="id">Id of new texture</param>
        public void setID(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Renders the texture if an id is set
        /// </summary>
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
