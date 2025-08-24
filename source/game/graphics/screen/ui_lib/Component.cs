using System;
using System.Collections.Generic;

namespace SeeloewenCraft.game.graphics.ui_lib
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
        /// Height of the component in pixels
        /// </summary>
        public int height { get => Math.Abs(bounds.y2P - bounds.y1P); }

        /// <summary>
        /// Width of the component in pixels
        /// </summary>
        public int width { get => Math.Abs(bounds.x2P - bounds.x1P); }

        public bool visible = true;

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
                c.Update();
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
        internal Component parent { get; private set; }

        /// <summary>
        /// Adds a component and calls the OnAdd() method on it
        /// </summary>
        /// <param name="component">Component to be added</param>
        public void AddChild(Component component)
        {
            children.Add(component);
            component.parent = this;
            component.OnAdd(this);
            OnChildAdded(component);
        }

        /// <summary>
        /// Remove a component from the children
        /// </summary>
        /// <param name="component">Component to be removed</param>
        public void RemoveChild(Component component)
        {
            children.Remove(component);
        }

        /// <summary>
        /// Remove all children components
        /// </summary>
        public void ClearChildren()
        {
            children.Clear();
        }

        protected void ForEachChildren(Action<Component> action)
        {
            children.ForEach(action);
        }


        /// <summary>
        /// This method is executed when a component is added as a child
        /// </summary>
        /// <param name="parent">New child component</param>
        protected virtual void OnChildAdded(Component component) { }

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

            if (!visible)
            {
                return;
            }

            OnRender();

            PrimitiveRenderer.Flush();
            TextRenderer.Flush();
            TextureRenderer.Flush();

            foreach (Component child in children)
            {
                child.Render();
            }

            OnRenderEnd();
        }

        /// <summary>
        /// This method is called to render the component. All Renderers must not be started at beginning and ended at the end (Begin() and End() method)
        /// </summary>
        protected virtual void OnRender() { }

        /// <summary>
        /// This method is called after all child components are rendered.
        /// </summary>
        protected virtual void OnRenderEnd() { }

        #endregion


        #region input handling

        public bool hovered { get; private set; }

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
            else if (inputEvent is ScrollEvent scrollEvent)
            {
                if (hovered) OnScrollEvent(scrollEvent);
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

        /// <summary>
        /// Is called when the current mouse scroll amount is not equal to 0
        /// </summary>
        /// <param name="mouseMoveEvent">ScrollEvent associated with this method call</param>
        protected virtual void OnScrollEvent(ScrollEvent scrollEvent) { }

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
        /// <param name="bounds">New bounding box of component</param>

        internal void SetBounds(Rectangle bounds) //Once again, I'm sorry CDLemmi for tampering with your lib but I needed this feature :) 25.02.2025
        {
            this.bounds = bounds;
        }

        /// <summary>
        /// Moves the upper left corner of the bounding box of this component to the specified coordinates
        /// Also moves the children of this component by the necessary amount
        /// </summary>
        /// <param name="x">X coordinate of new upper left corner</param>
        /// <param name="y">Y coordinate of new upper left corner</param>
        internal virtual void MoveTo(int x, int y)
        {
            int oldheight = height;

            int stepX = x - bounds.x1P;
            int stepY = y - bounds.y1P;

            Rectangle newBounds = new Rectangle(x, y, x + width, y + height);
            SetBounds(newBounds);

            foreach (Component child in children)
            {
                child.MoveBy(stepX, stepY);
            }
        }

        /// <summary>
        /// Moves the upper left corner of the bounding box of this component by the specified amount
        /// Also moves the children of this component by the specified amount
        /// </summary>
        /// <param name="x">Amount of steps on positive x-axis</param>
        /// <param name="y">Amount of steps on positive y-axis</param>
        internal virtual void MoveBy(int x, int y)
        {
            Rectangle newBounds = new Rectangle(bounds.x1P + x, bounds.y1P + y, bounds.x2P + x, bounds.y2P + y);
            SetBounds(newBounds);

            foreach (Component child in children)
            {
                child.MoveBy(x, y);
            }
        }
        #endregion

    }
}
