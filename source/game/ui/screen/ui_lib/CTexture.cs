using System;

namespace SeeloewenCraft.game.ui.ui_lib
{

    /// <summary>
    /// Basic Image component
    /// </summary>
    internal class CTexture : Component
    {

        private string id;
        private float brightness = 1f;
        private TextureMap map;

        private CRectangle rectBrightness;


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

            rectBrightness = new CRectangle(new Color(0f, 0f, 0f, 0f), bounds);
            AddChild(rectBrightness);
        }

        /// <summary>
        /// Changes the drawn texture
        /// </summary>
        /// <param name="id">Id of new texture</param>
        public void SetId(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Changes the brightness of the texture
        /// </summary>
        /// <param name="brightness">New brightness of the texture</param>
        public void SetBrightness(float brightness) //Only temporary solution, please replace with proper OpenGL stuff
        {
            this.brightness = Math.Abs(1 - brightness);

            Color oldColor = rectBrightness.GetColor();
            rectBrightness.SetColor(new Color(oldColor.r, oldColor.g, oldColor.b, this.brightness));
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
