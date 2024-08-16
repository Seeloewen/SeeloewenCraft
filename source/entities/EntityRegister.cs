namespace SeeloewenCraft
{
    public static class EntityRegister
    {

        public static Entity GenerateEntity(string id)
        {
            switch (id)
            {
                case "sc:slime": return new Slime(0, 0, 0, 0);
                case "sc:zombie": return new Zombie(0, 0, 0, 0);
                case "sc:skeleton": return new Skeleton(0, 0, 0, 0);
                default: return null;
            }
        }
    }
}
