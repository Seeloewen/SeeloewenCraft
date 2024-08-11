
namespace SeeloewenCraft
{

    //-- Bars --//

    public class HealthBar : AttributeBar
    {
        public HealthBar(World world, int top, int left) : base(world, top, left)
        {
            name = "Health";
            id = "sc:health_bar";
            value = 10;
        }

        public override void SetupTextures()
        {
            imgElementEmpty = Images.Heart_Empty;
            imgElementHalf = Images.Heart_Half;
            imgElementFull = Images.Heart_Full;
        }

        public override void SetValue(double value)
        {
            if (world.gamemode == Gamemode.Survival)
            {
                base.SetValue(value);
            }
        }
    }
}
