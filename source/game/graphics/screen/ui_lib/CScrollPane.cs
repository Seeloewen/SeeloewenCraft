using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics.ui_lib;

public class CScrollPane : CRectangle
{

    const int SCROLL_SENSITIVITY = 15;

    private int i;

    internal int maxI;

    private List<Component> scrollableChildren;


    public CScrollPane(Color color, Rectangle bounds, int maxI) : base(color, bounds)
    {
        this.maxI = maxI;
        scrollableChildren = new List<Component>();
    }

    public void AddScrollable(Component component)
    {
        AddChild(component);
        scrollableChildren.Add(component);
    }

    protected override void OnScrollEvent(ScrollEvent scrollEvent)
    {
        int dy = -SCROLL_SENSITIVITY * scrollEvent.offset;
        dy = Math.Max(-i, dy);
        dy = Math.Min(maxI - i, dy);
        i += dy;
        scrollableChildren.ForEach(c => c.MoveBy(0, -dy));
    }

    protected override void OnRender()
    {
        GL.StencilFunc(StencilFunction.Always, 1, 0xff);
        GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        GL.StencilMask(0xff);
        base.OnRender();
        PrimitiveRenderer.End();
        PrimitiveRenderer.Begin();
        GL.StencilFunc(StencilFunction.Equal, 1, 0xff);
        GL.StencilMask(0x00);
    }

    protected override void OnRenderEnd()
    {
        GL.StencilFunc(StencilFunction.Always, 1, 0xff);
        GL.Clear(ClearBufferMask.StencilBufferBit);
    }
}