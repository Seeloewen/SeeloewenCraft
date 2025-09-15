
using SeeloewenCraft.game.core.entities;

namespace SeeloewenCraft.game.graphics;

public class HitboxRenderer
{


    public static void Render()
    {
        PrimitiveRenderer.Begin();
        var entities = Game.world.entityManager.entities;
        foreach (var e in entities)
        {
            if (e is MovingEntity entity)
            {
                (int x1, int y1) = Resolution.PosToPixel(e.posX, e.posY);
                (int x2, int y2) = Resolution.PosToPixel(e.posX + e.sizeX, e.posY + e.sizeY);
                PrimitiveRenderer.DrawRectangle(new Rectangle(x1, y1, x2, y2), entity.hitboxColor);
            }
        }

        PrimitiveRenderer.End();
    }
    
    
    
    
    
    
    
}