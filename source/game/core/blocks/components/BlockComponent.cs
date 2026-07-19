using Newtonsoft.Json.Linq;

namespace SeeloewenCraft.game.core.blocks.components
{
    public enum BlockComponentType
    {
        Default,
        Inventory,
        Workstation,
        Unchiseler
    }


    public abstract class BlockComponent
    {
        public JObject ToJson()
        {
            //Default construct for the block component
            JObject obj = new JObject
            {
                { "type", GetType().ToString() },
                { "content", GetContentJson() }
            };

            return obj;
        }

        protected virtual JToken GetContentJson() => new JValue("");

        public virtual void FromJson(JObject json) { }

        public virtual void AddDebugMenu() { }

        public new abstract BlockComponentType GetType();
    }
}
