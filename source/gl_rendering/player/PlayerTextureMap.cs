using System.Drawing;

namespace SeeloewenCraft.gl_rendering
{
    internal class PlayerTextureMap : TextureMap
    {

        internal PlayerTextureMap(TextureManager manager) : base(48, 128)
        {
            Bitmap head = new Bitmap(manager.GetPlayerTexture("head"));
            Bitmap body = new Bitmap(manager.GetPlayerTexture("body"));
            Bitmap armLeft = new Bitmap(manager.GetPlayerTexture("arm_left"));
            Bitmap armRight = new Bitmap(manager.GetPlayerTexture("arm_right"));
            Bitmap legLeft = new Bitmap(manager.GetPlayerTexture("leg_left"));
            Bitmap legRight = new Bitmap(manager.GetPlayerTexture("leg_right"));

            AddMapping("head", head, 0, 0);
            AddMapping("body", body, 32, 0);
            AddMapping("arm_front", armLeft, 0, 32);
            AddMapping("arm_back", armRight, 16, 32);
            AddMapping("leg_front", legLeft, 0, 80);
            AddMapping("leg_back", legRight, 16, 80);

            Finalize();
        }

    }
}
