
using System;
using System.Diagnostics;

namespace SeeloewenCraft.game.ui.ui_lib
{
    public class UIRoot
    {

        Func<Component> create;

        Component component;

        public UIRoot(Func<Component> create)
        {
            this.create = create;
        }


        #region input handling

        int mouseX;
        int mouseY;

        bool mouseLeftDown;
        bool mouseRightDown;

        void updateInput()
        {
            Debug.Assert(component != null);

            //detect if mouse was moved
            if (mouseX != InputHandler.mouseXPixel || mouseY != InputHandler.mouseYPixel)
            {
                mouseX = InputHandler.mouseXPixel;
                mouseY = InputHandler.mouseYPixel;
                component.cascadeInputEvent(new MouseMoveEvent(mouseX, mouseY));
            }

            //detect if mouse was clicked
            if (mouseLeftDown != InputHandler.pressedLeft || mouseRightDown != InputHandler.pressedRight)
            {

                if (mouseLeftDown != InputHandler.pressedLeft)
                {
                    component.cascadeInputEvent(new ClickEvent(ButtonType.LEFT, InputHandler.pressedLeft, InputHandler.pressedLeft, mouseRightDown));
                }
                if (mouseRightDown != InputHandler.pressedRight)
                {
                    component.cascadeInputEvent(new ClickEvent(ButtonType.RIGHT, InputHandler.pressedRight, InputHandler.pressedLeft, InputHandler.pressedRight));
                }
                mouseLeftDown = InputHandler.pressedLeft;
                mouseRightDown = InputHandler.pressedRight;
            }


        }

        #endregion


        public void Update()
        {
            updateInput();

        }


        public void Render()
        {
            TextureRenderer.Begin();
            PrimitiveRenderer.Begin();
            TextRenderer.Begin();

            component.Render();

            TextureRenderer.End();
            PrimitiveRenderer.End();
            TextRenderer.End();
        }


        public void Show()
        {
            component = create();
        }

        public void Hide()
        {
            component = null;
        }




    }
}
