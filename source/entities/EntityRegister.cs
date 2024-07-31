using System;

namespace SeeloewenCraft
{
    public static class EntityRegister
    {

        public static Entity GenerateEntity(string id, World world)
        {
            switch(id)
            {
                case "slime": return new Slime(0, 0, 0, 0, world);
                case "zombie": return new Zombie(0,0,0,0, world);
                default: return null;
            }

        }

    }
}
