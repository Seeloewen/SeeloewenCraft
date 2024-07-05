using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            imgElementEmpty = world.images.HealthEmpty;
            imgElementHalf = world.images.HealthHalf;
            imgElementFull = world.images.HealthFull;
        }
    }
}
