using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace sfmlgame.UI
{
    public class UI_List : UIComponent
    {
        private List<UIComponent> Content;
        private float verticalSpacing = 10; // Space between items

        public UI_List(Vector2f position) : base(position)
        {
            Content = new List<UIComponent>();
        }

        public void AddChild(UIComponent child)
        {
            AddChild(child, new Vector2f(0,0));
        }

        public void AddChild(UIComponent comp, Vector2f offset)
        {
            // Determine the new position for the child component
            Vector2f newPosition;
            if (Content.Count == 0)
            {
                newPosition = new Vector2f(GetCenterX+offset.X, GetCenterY+offset.Y);
            }
            else
            {
                UIComponent lastItem = Content[^1];
                newPosition = new Vector2f(
                    lastItem.Position.X + offset.X,
                    lastItem.Position.Y + lastItem.Height + verticalSpacing + offset.Y
                );
            }

            // Set the computed position to the component
            comp.Position = newPosition;

            // If the component is a UI_Button, update its internal sprite and text positioning
            if (comp is UI_Button button)
            {
                button.SetPosition(newPosition);
            }

            if (comp is UI_Text text)
            {
                text.SetPosition(newPosition);
            }

            // Add the component to the list of children
            Content.Add(comp);

            // Optionally update the height of the UI_List to encompass all items
            UpdateHeight();
        }

        private void UpdateHeight()
        {
            if (Content.Any())
            {
                UIComponent lastItem = Content[^1];
                Height = (int)lastItem.Position.Y + lastItem.Height - (int)Position.Y;
            }
        }

        public override void Draw(RenderTexture renderTexture)
        {
            foreach (var listItem in Content)
            {
                if (listItem.Hide) continue;
                listItem.Draw(renderTexture);
            }
        }

        public override void Update(float deltaTime)
        {
            foreach (var listItem in Content)
            {
                if(listItem.Hide) continue;
                listItem.Update(deltaTime);
            }
        }
    }
}
