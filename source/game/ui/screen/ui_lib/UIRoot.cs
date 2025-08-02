
using System;
using System.Diagnostics;
using System.Drawing;

namespace SeeloewenCraft.game.ui.ui_lib
{

    /// <summary>
    /// Each ui menu has a instance of the class UIRoot as its root element. It should be managed in Screen.cs. This class detects all input events and sends them to the root component
    /// </summary>
    public class UIRoot
    {

        Func<Component> create;

        public Component component;

        /// <summary>
        /// Creates the ui root
        /// </summary>
        /// <param name="create">Method that returns the root component. This method is called each time the ui menu is beeing created</param>
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

        /// <summary>
        /// Checks for and sends input events and calls OnUpdate() on every child component
        /// </summary>
        public void Update()
        {
            updateInput();
            component.Update();
        }

        /// <summary>
        /// Calls OnRender() on root and all child components
        /// </summary>
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

        /// <summary>
        /// Creates and shows the component returned by onCreate
        /// </summary>
        public void Show()
        {
            component = create();
        }

        /// <summary>
        /// Hides and destroys the component returned by onCreate
        /// </summary>
        public void Hide()
        {
            component = null;
        }




    }
}
