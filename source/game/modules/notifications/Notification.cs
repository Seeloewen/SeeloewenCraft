namespace SeeloewenCraft.game.notifications
{
    public class Notification
    {
        public string iconTexMap;
        public string iconId;
        public string content;
        public double lifetime;

        public Notification(string iconId, string content, string iconTexMap)
        {
            this.iconId = iconId;
            this.content = content;
            this.iconTexMap = iconTexMap;
            lifetime = 5;
        }

        public void Update(double dt)
        {
            if (lifetime <= 0) return;

            lifetime -= dt;
        }
    }
}
