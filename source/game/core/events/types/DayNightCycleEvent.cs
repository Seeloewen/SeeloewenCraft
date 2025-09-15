using SeeloewenCraft.game.core.world;

namespace SeeloewenCraft.game.core.events
{
    internal class DayNightCycleEvent : GameEvent
    {
        public DayNightCycleEvent() : base(1200000/20, maxStacks: 10) { }

        protected override void Run()
        {
            //Set night state based on tick
            DayTime time = timesStacked switch
            {
                0 => DayTime.SUNSET1,
                1 => DayTime.SUNSET2,
                2 => DayTime.SUNSET3,
                3 => DayTime.SUNSET4,
                4 => DayTime.NIGHT,
                5 => DayTime.SUNRISE1,
                6 => DayTime.SUNRISE2,
                7 => DayTime.SUNRISE3,
                8 => DayTime.SUNRISE4,
                _ => DayTime.DAY
            };

            World.Get().SetDayTime(time);    
        }

        protected override bool IsReady()
        {
            return ticks >= 480000 && timesStacked == 0 //Sunset 1
                || ticks >= 510000 && timesStacked == 1 //Sunset 2
                || ticks >= 540000 && timesStacked == 2 //Sunset 3
                || ticks >= 570000 && timesStacked == 3 //Sunset 4
                || ticks >= 600000 && timesStacked == 4 //Night
                || ticks >= 1080000 && timesStacked == 5 //Sunrise 1
                || ticks >= 1110000 && timesStacked == 6 //Sunrise 2
                || ticks >= 1140000 && timesStacked == 7 //Sunrise 3
                || ticks >= 1170000 && timesStacked == 8 //Sunrise 4
                || ticks >= 1200000 && timesStacked == 9; //Day
        }
    }
}
