using SeeloewenCraft.game.networking;
using System.Collections.Generic;
using System.Text;

namespace SeeloewenCraft.game.core.entities
{
    public record SyncPosEvent(SyncPosInfo[] infos)
    {
        public static SyncPosEvent Create(List<Entity> entities)
        {
            SyncPosInfo[] infos = new SyncPosInfo[entities.Count];
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = new SyncPosInfo(entities[i]);
            }
            return new SyncPosEvent(infos);
        }

        public static SyncPosEvent Create(string s)
        {
            string[] args = s.Split(',');
            SyncPosInfo[] infos = new SyncPosInfo[args.Length];
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = SyncPosInfo.Create(args[i]);
            }
            return new SyncPosEvent(infos);
        }

        public void Send()
        {
            StringBuilder sb = new StringBuilder(infos[0].ToString());
            for (int i = 1; i < infos.Length; i++)
            {
                sb.Append(",");
                sb.Append(infos[i].ToString());
            }

            NetworkHandler.SendData(MultiplayerPacketType.SYNC_POS, sb.ToString());
        }

    }
}
