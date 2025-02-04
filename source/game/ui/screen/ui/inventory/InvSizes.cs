using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.ui
{
    internal static class InvSizes
    {

        internal static int mx { get => Resolution.WIDTH / 2; }
        internal static int my { get => Resolution.HEIGHT / 2; }

        internal const int slotSize = 70;
        internal const int edgeSize = 5;

        internal const int yOffset = 150;
    }
}
