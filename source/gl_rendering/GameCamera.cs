
namespace SeeloewenCraft.gl_rendering
{
    public class GameCamera
    {

        public float blockLength = 0.2f;
        public float blockXAnchor;
        public float blockYAnchor;

        //window constants
        public float ratio = 16 / 9.0f;

        public void SetCamCenterPhysicsCoord(int x, int y)
        {
            blockXAnchor = -(x / 1000.0f) * blockLength;
            blockYAnchor = (y / 1000.0f) * blockLength * ratio;


        }


    }
}
