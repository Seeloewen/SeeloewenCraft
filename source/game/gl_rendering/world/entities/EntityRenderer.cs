

using SeeloewenCraft.entity;

namespace SeeloewenCraft.gl_rendering
{
    internal class EntityRenderer
    {

        PlayerRenderer playerRenderer;
        ItemRenderer itemEntityRenderer;


        internal EntityRenderer(TextureManager textureManager, ItemRenderer itemRenderer)
        {
            playerRenderer = new PlayerRenderer(textureManager);
            itemEntityRenderer = itemRenderer;
        }

        internal void Render() {

            itemEntityRenderer.Begin();

            foreach(Entity entity in Game.world.entityManager.entities) {
                if(entity is Player player)
                {
                    //playerRenderer.Render(player.playerRenderInfo);
                }
                else if(entity is ItemEntity itemEntity)
                {
                    itemEntityRenderer.DrawItemEntity(itemEntity);
                }
            }

            itemEntityRenderer.End();

        }

    }
}
