namespace SeeloewenCraft
{
    public static class EntityRegister
    {

        public static Entity GenerateEntity(string id, World world)
        {
            switch (id)
            {
                case "sc:slime": return new Slime(0, 0, 0, 0, world);
                case "sc:zombie": return new Zombie(0, 0, 0, 0, world);
                case "sc:skeleton": return new Skeleton(0, 0, 0, 0, world);
                default: return null;
            }
        }
    }
}
