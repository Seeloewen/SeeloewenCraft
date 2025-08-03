
namespace SeeloewenCraft.game.graphics
{
    public static class GameCamera
    {

        public static float blockLength = 0.09f;
        public static float blockXAnchor;
        public static float blockYAnchor;

        //window constants

        public static void SetCamCenterPhysicsCoord(int x, int y)
        {
            blockXAnchor = -(x / 1000.0f) * blockLength;
            blockYAnchor = (y / 1000.0f) * blockLength * Resolution.RATIO;


        }


    }
}
