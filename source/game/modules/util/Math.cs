using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.game.modules.util
{
    public static class SMath
    {
        public static float NormalizePerlin(float f)
        {
            return (f + 1) / 2;
        }

        public static float ScalePerlin(float normalized, int min, int max)
        {
            return normalized * (max - min) + min;
        }
    }
}
