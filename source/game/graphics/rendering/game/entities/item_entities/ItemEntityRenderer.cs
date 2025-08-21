using SeeloewenCraft.game.core.entities;

namespace SeeloewenCraft.game.graphics
{
    internal static class ItemEntityRenderer
    {


        internal static void Render()
        {
            ItemRenderer.SetTexture();
            TextureRenderer.Begin();
            var entities = Game.world.entityManager.entities;
            foreach (var entity in entities)
            {
                if (entity is ItemEntity itemEntity)
                {
                    Draw(itemEntity);
                }
            }
            TextureRenderer.End();
        }

        internal static void Draw(ItemEntity entity)
        {
            float x1 = GameCamera.blockXAnchor + GameCamera.blockLength * (entity.posX / 1000.0f);
            float y1 = GameCamera.blockYAnchor - GameCamera.blockLength * (entity.posY / 1000.0f) * Resolution.RATIO;
            float x2 = x1 + GameCamera.blockLength * (entity.sizeX / 1000.0f);
            float y2 = y1 - GameCamera.blockLength * (entity.sizeY / 1000.0f) * Resolution.RATIO;
            string itemID = entity.itemID;

            TextureRenderer.Draw(itemID, x1, y1, x2, y2, 1f);
        }

    }
}
