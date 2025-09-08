
using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.legacy
{

    //-- Bars --//

    public class HealthBar : AttributeBar
    {
        public HealthBar(int top, int left) : base(top, left)
        {
            name = "Health";
            id = "sc:health_bar";
            value = 10;
        }

        public override void SetupTextures()
        {
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
