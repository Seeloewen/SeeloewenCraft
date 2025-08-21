
using System;
using System.Windows;

namespace SeeloewenCraft.game.core.entities
{
    public record SyncPosInfo(
        int id,
        int posX,
        int posY,
        int velX,
        int velY)
    {
        internal static SyncPosInfo Create(string s)
        {
            string[] args = s.Split('|');
            try
            {
                return new SyncPosInfo(int.Parse(args[0]),
                    int.Parse(args[1]),
                    int.Parse(args[2]),
                    int.Parse(args[3]),
                    int.Parse(args[4]));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        internal SyncPosInfo(Entity e) : this(e.id, e.posX, e.posY, e.velX, e.velY) { }

        public override string ToString()
        {
            return $"{id}|{posX}|{posY}|{velX}|{velY}";
        }

    }
}
