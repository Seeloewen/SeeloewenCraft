using OpenTK.Graphics.OpenGL;
using SeeloewenCraft.gl_rendering.math;
using System;
using System.Diagnostics;

namespace SeeloewenCraft.gl_rendering
{
    internal class PlayerRenderer
    {
        Shader shader;
        PlayerTextureMap textureMap;

        const float PI = 3.14159265358979323f;

        VertexBuffer buffer;

        float[] vertices;
        int index;

        float originX = -0.8f;
        float originY = 0.6f;
        float unitSize = 0.02f;
        const float ratio = 16.0f / 9.0f;

        internal PlayerRenderer(TextureManager manager)
        {
            shader = new Shader();
            textureMap = new PlayerTextureMap(manager);
            buffer = new VertexBuffer(new VBLayout().AddAttribute(2).AddAttribute(2), 4 * 6 * 7);
        }

        internal void ApplyCam(GameCamera camera, PlayerRenderInfo renderInfo)
        {
            originX = camera.blockXAnchor + (renderInfo.posX / 1000.0f)*camera.blockLength;
            originY = camera.blockYAnchor - (renderInfo.posY / 1000.0f) * camera.blockLength*ratio;
            unitSize = camera.blockLength * 0.475f * 0.125f; 
        }

        internal void Render(GameCamera cam, PlayerRenderInfo info)
        {
            ApplyCam(cam, info);

            vertices = new float[4 * 7 * 6];
            index = 0;

            DrawLegBack(info);
            DrawArmBack(info);
            DrawHead(info);
            DrawLegFront(info);
            DrawBody(info);
            DrawArmFront(info);

            buffer.SetVertices(vertices);
            buffer.Bind();
            textureMap.Bind();
            shader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 4*7*6);
        }

        void DrawLegBack(PlayerRenderInfo info)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("leg_back");

            var topRight = new Vector2f(unitSize * 2, 0);
            var topLeft = new Vector2f(unitSize * -2, 0);
            var botRight = new Vector2f(unitSize * 2, unitSize * -12);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -12);
            topRight = topRight.Rotate(info.legBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            topLeft = topLeft.Rotate(info.legBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            botRight = botRight.Rotate(info.legBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            botLeft = botLeft.Rotate(info.legBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);

            DrawRegion(topLeft, topRight, botLeft, botRight, s1, t1, s2, t2);
        }
        void DrawLegFront(PlayerRenderInfo info)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("leg_front");

            var topRight = new Vector2f(unitSize * 2, 0);
            var topLeft = new Vector2f(unitSize * -2, 0);
            var botRight = new Vector2f(unitSize * 2, unitSize * -12);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -12);
            topRight = topRight.Rotate(info.legFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            topLeft = topLeft.Rotate(info.legFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            botRight = botRight.Rotate(info.legFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);
            botLeft = botLeft.Rotate(info.legFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 20 * unitSize * ratio);

            DrawRegion(topLeft, topRight, botLeft, botRight, s1, t1, s2, t2);
        }

        void DrawArmBack(PlayerRenderInfo info)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("arm_back");

            var topRight = new Vector2f(unitSize * 2, unitSize * 2);
            var topLeft = new Vector2f(unitSize * -2, unitSize * 2);
            var botRight = new Vector2f(unitSize * 2, unitSize * -10);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -10);
            topRight = topRight.Rotate(info.armBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);
            topLeft = topLeft.Rotate(info.armBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);
            botRight = botRight.Rotate(info.armBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);
            botLeft = botLeft.Rotate(info.armBack).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);

            DrawRegion(topLeft, topRight, botLeft, botRight, s1, t1, s2, t2);
        }

        void DrawArmFront(PlayerRenderInfo info)
        {            
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("arm_front");

            var topRight = new Vector2f(unitSize * 2, unitSize * 2);
            var topLeft = new Vector2f(unitSize * -2, unitSize * 2);
            var botRight = new Vector2f(unitSize * 2, unitSize * -10);
            var botLeft = new Vector2f(unitSize * -2, unitSize * -10);
            topRight = topRight.Rotate(info.armFront).Scale(1, ratio).Add(originX + 4*unitSize, originY-10*unitSize * ratio);
            topLeft = topLeft.Rotate(info.armFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);
            botRight = botRight.Rotate(info.armFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);
            botLeft = botLeft.Rotate(info.armFront).Scale(1, ratio).Add(originX + 4 * unitSize, originY - 10 * unitSize * ratio);

            DrawRegion(topLeft, topRight, botLeft, botRight, s1, t1, s2, t2);
        }

        void DrawBody(PlayerRenderInfo info)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("body");
            float x1 = originX + 2*unitSize;
            float y1 = originY - 8*unitSize*ratio;
            float x2 = x1 + 4 * unitSize;
            float y2 = y1 - 12 * unitSize * ratio;
            DrawRegion(x1, y1, x2, y2, s1, t1, s2, t2);
        }

        void DrawHead(PlayerRenderInfo info)
        {
            (float s1, float t1, float s2, float t2) = textureMap.GetTexture("head");
            float x1 = originX;
            float y1 = originY;
            float x2 = originX + 8 * unitSize;
            float y2 = originY - 8 * unitSize * ratio;
            switch (info.direction)
            {
                case Direction.LEFT: DrawRegion(x1, y1, x2, y2, s1, t1, s2, t2); break;
                case Direction.RIGHT: DrawRegion(x2, y1, x1, y2, s1, t1, s2, t2); break;
            }
        }

        void DrawRegion(Vector2f topLeft, Vector2f topRight, Vector2f botLeft, Vector2f botRight, float s1, float t1, float s2, float t2)
        {
            Put(topLeft, s1, t1);
            Put(botLeft, s1, t2);
            Put(topRight, s2, t1);
            Put(botLeft, s1, t2);
            Put(topRight, s2, t1);
            Put(botRight, s2, t2);
        }

        void DrawRegion(float x1, float y1, float x2, float y2, float s1, float t1, float s2, float t2)
        {
            Put(x1, y1, s1, t1);
            Put(x1, y2, s1, t2);
            Put(x2, y1, s2, t1);
            Put(x1, y2, s1, t2);
            Put(x2, y1, s2, t1);
            Put(x2, y2, s2, t2);
        }

        void Put(Vector2f vector, float s, float t)
        {
            Put(vector.x, vector.y, s, t);
        }

        void Put(float x, float y, float s, float t)
        {
            vertices[index++] = x;
            vertices[index++] = y;
            vertices[index++] = s;
            vertices[index++] = t;
        }

    }
}
