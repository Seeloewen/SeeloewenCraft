
namespace SeeloewenCraft.gl_rendering
{
    internal class BlockTextureMap
    {

        Texture texture;

        internal BlockTextureMap()
        {
            texture = new Texture();
        }

        internal (float s1, float t1, float s2, float t2) getTexture(string blockID)
        {
            return (0.0f ,0.0f, 1.0f, 1.0f);
        }

        internal void Bind()
        {
            texture.Bind();
        }


    }
}
