
namespace SeeloewenCraft
{

    //-- Bars --//

    public class HealthBar : AttributeBar
    {
        public HealthBar( int top, int left) : base( top, left)
        {
            name = "Health";
            id = "sc:health_bar";
            value = 10;
        }

        public override void SetupTextures()
        {
            imgElementEmpty = Images.Heart_Empty.GetTexture();
            imgElementHalf = Images.Heart_Half.GetTexture();
            imgElementFull = Images.Heart_Full.GetTexture();
        }

        public override void SetValue(double value)
        {
            if (Game.world.gamemode == Gamemode.Survival)
            {
                base.SetValue(value);
            }
        }
    }
}
