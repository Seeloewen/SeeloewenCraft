using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeloewenCraft.entity
{
    public record PressedChangeEvent(
        int id,
        bool pressedUp,
        bool pressedRight,
        bool pressedLeft,
        bool pressedSneak,
        bool pressedSprint)
    {

        //checks if at least one value changed, if not returns null
        internal static PressedChangeEvent Create(int id,
            bool entityPressedUp, bool newPressedUp,
            bool entityPressedRight, bool newPressedRight,
            bool entityPressedLeft, bool newPressedLeft,
            bool entityPressedSneak, bool newPressedSneak,
            bool entityPressedSprint, bool newPressedSprint)
        {
            if(entityPressedUp != newPressedUp
                || entityPressedRight != newPressedRight
                || entityPressedLeft != newPressedLeft
                || entityPressedSneak != newPressedSneak
                || entityPressedSprint != newPressedSprint)
            {
                return new PressedChangeEvent(id, newPressedUp, newPressedRight, newPressedLeft, newPressedSneak, newdPressedSprint);
            }
            else
            {
                return null;
            }
        }

        internal static PressedChangeEvent Create(string[] args)
        {
            return new PressedChangeEvent(
                int.Parse(args[1]),
                bool.Parse(args[2]),
                bool.Parse(args[3]),
                bool.Parse(args[4]),
                bool.Parse(args[5]),
                bool.Parse(args[6])
            );
        }

        public override string ToString()
        {
            return $"PressedChangedEvent;{id};{pressedUp};{pressedRight};{pressedLeft};{pressedSneak};{pressedSprint}";
        }

    }
}
