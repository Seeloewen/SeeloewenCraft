

using SeeloewenCraft.entity;

namespace SeeloewenCraft.gl_rendering
{
    internal class EntityRenderer
    {

        PlayerRenderer playerRenderer;
        ItemEntityRenderer itemEntityRenderer;


        internal EntityRenderer(TextureManager textureManager)
        {
            playerRenderer = new PlayerRenderer(textureManager);
            itemEntityRenderer = new ItemEntityRenderer(textureManager);
        }

        internal void Render() {

            itemEntityRenderer.Begin();

            foreach(Entity entity in Game.world.entityManager.entities) {
                if(entity is Player player)
                {
                    playerRenderer.Render(player.playerRenderInfo);
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
