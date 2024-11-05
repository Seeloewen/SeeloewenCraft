
using OpenTK.Graphics.OpenGL;
using SeeloewenCraft.entity;
using System.Diagnostics;

namespace SeeloewenCraft.gl_rendering
{
    internal class ItemEntityRenderer
    {

        ItemTextureMap textureMap;
        Shader shader;
        VertexBuffer vertexBuffer;

        GameCamera cam;

        float[] vertices;
        int index;
        bool drawing;

        internal ItemEntityRenderer(TextureManager textureManager)
        {
            textureMap = new ItemTextureMap(textureManager);
            shader = new Shader("shader/blockworld");
            vertexBuffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2).AddAttribute(1), 1024);

        }

        internal void ApplyCam(GameCamera cam)
        {
            this.cam = cam;
        }

        internal void DrawItemEntity(ItemEntity entity)
        {
            float x1 = cam.blockXAnchor + cam.blockLength * (entity.posX / 1000.0f);
            float y1 = cam.blockYAnchor - cam.blockLength * (entity.posY / 1000.0f) * 16/9f;
            float x2 = x1 + cam.blockLength * (entity.sizeX / 1000.0f);
            float y2 = y1 - cam.blockLength * (entity.sizeY / 1000.0f) * 16 / 9f;

            (float s1, float t1, float s2, float t2) = textureMap.GetTexture(entity.itemID);

            Draw(x1, y1, x2, y2, s1, t1, s2, t2);
        }

        void Draw(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2)
        {
            Debug.Assert(drawing);
            if(index + 4*6 >= 1024)
            {
                End();
                Begin();
            }
            Put(x1, y1, s1, t1, 1.0f);
            Put(x1, y2, s1, t2, 1.0f);
            Put(x2, y1, s2, t1, 1.0f);
            Put(x1, y2, s1, t2, 1.0f);
            Put(x2, y1, s2, t1, 1.0f);
            Put(x2, y2, s2, t2, 1.0f);
        }

        void Put(float x, float y, float s, float t, float g)
        {
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = s;
            vertices[index++] = t;
            vertices[index++] = g;
        }

        internal void Begin()
        {
            drawing = true;
            index = 0;
            vertices = new float[1024];
        }

        internal void End()
        {
            drawing = false;
            vertexBuffer.SetVertices(vertices);
            vertexBuffer.Bind();
            shader.Use();
            textureMap.Bind();

            GL.DrawArrays(PrimitiveType.Triangles, 0, index);
        }


    }
}
