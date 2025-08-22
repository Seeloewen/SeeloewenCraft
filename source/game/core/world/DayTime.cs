using SeeloewenCraft.game.graphics;

namespace SeeloewenCraft.game.core.world
{
    public enum DayTime
    {
        DAY,
        NIGHT,
        SUNRISE1,
        SUNRISE2,
        SUNRISE3,
        SUNRISE4,
        SUNSET1,
        SUNSET2,
        SUNSET3,
        SUNSET4,
    }

    public static class SkyColors
    {
        public static Color DAY_COLOR = new Color(0.737f, 0.957f, 0.969f);
        public static Color NIGHT_COLOR = new Color(0.039f, 0.047f, 0.051f);
        public static Color SUNRISE4_SUNSET1_COLOR = new Color(0.588f, 0.765f, 0.776f);
        public static Color SUNRISE3_SUNSET2_COLOR = new Color(0.443f, 0.573f, 0.580f);
        public static Color SUNRISE2_SUNSET3_COLOR = new Color(0.294f, 0.384f, 0.388f);
        public static Color SUNRISE1_SUNSET4_COLOR = new Color(0.149f, 0.192f, 0.192f);
    }
}
