using Newtonsoft.Json.Linq;

namespace SeeloewenCraft.game.graphics
{
    public interface IGuiData
    {
        public string guiId { get; set; }

        public string tags { get; set; } //Tags are saved as attributes of a JSON object

        public void Show()
        {
            Screen.guiHandler.guiData.Add(this);
            Screen.showGui = true;
        }

        public void Hide()
        {
            Screen.guiHandler.guiData.Remove(this);
            Screen.showGui = false;
        }

        public void AddTag(string tag, string value)
        {
            if (string.IsNullOrEmpty(tags)) tags = "{}"; //If tags is not initialized, init it

            JObject obj = JObject.Parse(tags);
            obj[tag] = value;

            tags = obj.ToString();
        }

        public void AddTag(string tag, int value)
        {
            AddTag(tag, value.ToString());
        }

        public void AddTag(string tag, bool value)
        {
            AddTag(tag, value.ToString());
        }

        public T GetTag<T>(string tag)
        {
            if (string.IsNullOrEmpty(tags)) tags = "{}"; //If tags is not initialized, init it

            JObject obj = JObject.Parse(tags);
            return obj[tag].ToObject<T>();
        }
    }
}
