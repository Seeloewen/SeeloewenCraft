using System.Collections.Generic;

namespace SeeloewenCraft.game.ui.ui_lib
{

    /// <summary>
    /// Abstract base class for all ui components. A component can have child components.
    /// </summary>
    public abstract class Component
    {

        /// <summary>
        /// Rectangular bounding box of the component
        /// </summary>
        protected Rectangle bounds { get; set; }

        /// <summary>
        /// Creates a component with specified bounds
        /// </summary>
        /// <param name="bounds">Recangular bounding box of component</param>
        public Component(Rectangle bounds) : this()
        {
            this.bounds = bounds;
        }

        /// <summary>
        /// Creates a component without specified bounds. A bounding box must be set in overridden OnAdd() method
        /// </summary>
        public Component()
        {
            children = new List<Component>();
        }

        internal void Update()
        {
            OnUpdate();
            foreach (var c in children)
            {
                c.OnUpdate();
            }
        }

        /// <summary>
        /// Called each update tick after handling input events and before rendering. After executing this method will be called on all child components
        /// </summary>
        protected virtual void OnUpdate() { }



        #region children/parent
        List<Component> children;
        /// <summary>
        /// Reference to parent component
        /// </summary>
        protected Component parent { get; private set; }

        /// <summary>
        /// Adds a component and calls the OnAdd() method on it
        /// </summary>
        /// <param name="component">Component to be added</param>
        public void AddChild(Component component)
        {
            children.Add(component);
            component.parent = this;
            component.OnAdd(this);
        }

        /// <summary>
        /// This method is executed when this component is added to a parent component
        /// </summary>
        /// <param name="parent">New parent component</param>
        protected virtual void OnAdd(Component parent) { }

        #endregion


        #region rendering
        //renders this and every child component. expects that all renderers are begun, and will be ended
        internal void Render()
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

        /// <summary>
        /// This method is called to render the component. All Renderers must not be started at beginning and ended at the end (Begin() and End() method)
        /// </summary>
        protected virtual void OnRender() { }

        #endregion


        #region input handling

        bool hovered;

        internal void cascadeInputEvent(InputEvent inputEvent)
        {
            foreach (Component c in children)
            {
                c.cascadeInputEvent(inputEvent);
                if (inputEvent.consumed)
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
                if (hovered != bounds.IsInBounds(mouseMoveEvent.x, mouseMoveEvent.y))
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
                if (hovered) OnClickEvent(clickEvent);
            }
        }


        /// <summary>
        /// Is called when the mouse enters the bounding box of the component
        /// </summary>
        protected virtual void OnMouseEnter() { }

        /// <summary>
        /// Is called when the mouse leaves the bounding box of the component
        /// </summary>
        protected virtual void OnMouseLeave() { }

        /// <summary>
        /// Is called when the mouse position changes and the new mouse position is inside the bounding box of the component
        /// </summary>
        /// <param name="mouseMoveEvent">MouseMoveEvent associated with this method call</param>
        protected virtual void OnMouseMoveEvent(MouseMoveEvent mouseMoveEvent) { }

        /// <summary>
        /// Is called when the mouse buttons click status changes and the current mouse position is inside the bounding box of the component
        /// </summary>
        /// <param name="mouseMoveEvent">MouseMoveEvent associated with this method call</param>
        protected virtual void OnClickEvent(ClickEvent mouseClickEvent) { }


        #endregion


        #region interaction

        /// <summary>
        /// Gets the bounding box of this component
        /// </summary>
        /// <returns>Copy of the bounding box</returns>
        public Rectangle GetBounds()
        {
            return new Rectangle(bounds.x1S, bounds.y1S, bounds.x2S, bounds.y2S);
        }

        /// <summary>
        /// Sets the bounding box of this component
        /// </summary>
        internal void SetBounds(Rectangle bounds) //Once again, I'm sorry CDLemmi for tampering with your lib but I needed this feature :) 25.02.2025
        {
            this.bounds = bounds;
        }
        #endregion

    }
}
