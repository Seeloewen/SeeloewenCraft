using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.ui.ui_lib
{
    public abstract class Component
    {


        protected Rectangle bounds { get; set; }

        public Component(Rectangle bounds) : this()
        {
            this.bounds = bounds;
        }

        public Component()
        {
            children = new List<Component>();
        }

        public void onUpdate()
        {

        }


        #region children/parent
        List<Component> children;
        Component parent;

        public void AddChild(Component component)
        {
            children.Add(component);
            component.parent = this;
            component.OnAdd(this);
        }

        protected virtual void OnAdd(Component parent)
        {
        }

        #endregion


        #region rendering
        //renders this and every child component. expects that all renderers are begun, and will be ended
        public void Render()
        {
            OnRender();
            foreach (Component child in children)
            {
                PrimitiveRenderer.End();
                PrimitiveRenderer.Begin();
                TextRenderer.End();
                TextRenderer.Begin();
                TextureRenderer.End();
                TextureRenderer.Begin();

                child.Render();
            }
        }

        protected virtual void OnRender() { }

        #endregion


        #region input handling

        bool hovered;

        internal void cascadeInputEvent(InputEvent inputEvent)
        {
            foreach(Component c in children)
            {
                c.cascadeInputEvent(inputEvent);
                if(inputEvent.consumed)
                {
                    return;
                }
            }

            handleInputEvent(inputEvent);
        }

        private void handleInputEvent(InputEvent inputEvent)
        {
            if (inputEvent is MouseMoveEvent mouseMoveEvent)
            {
                if (hovered != bounds.isInBounds(mouseMoveEvent.x, mouseMoveEvent.y))
                {
                    hovered = !hovered;
                    if (hovered)
                    {
                        OnMouseEnter();
                    }
                    else
                    {
                        OnMouseLeave();
                    }
                }

                OnMouseMoveEvent(mouseMoveEvent);
            }
            else if (inputEvent is ClickEvent clickEvent)
            {
                if(hovered) OnClickEvent(clickEvent);
            }


        }


        //event hooks
        protected virtual void OnMouseEnter()
        {

        }

        protected virtual void OnMouseLeave()
        {

        }

        protected virtual void OnMouseMoveEvent(MouseMoveEvent mouseMoveEvent)
        {

        }

        protected virtual void OnClickEvent(ClickEvent mouseClickEvent)
        {

        }


        #endregion


        #region interaction



        public Rectangle getBounds()
        {
            return new Rectangle(bounds.x1, bounds.y1, bounds.x2, bounds.y2);
        }

        #endregion

    }
}
