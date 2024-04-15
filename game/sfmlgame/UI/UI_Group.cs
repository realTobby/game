using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace sfmlgame.UI
{
    public class UI_Group : UIComponent
    {
        public UI_Button baseButton;
        public bool IsOpen = false;
        public List<UIComponent> children = new List<UIComponent>();
        private float verticalSpacing = 10; // Space between child components

        public bool HideBase = false;

        public UI_Group(Vector2f position, string name) : base(position)
        {
            baseButton = new UI_Button(new Vector2f(position.X, position.Y), name, 40, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            baseButton.ClickAction = () =>
            {
                if(!HideBase)
                {
                    IsOpen = !IsOpen;
                }
                
            };


            Width = baseButton.Width;
            Height = baseButton.Height;
        }

        public void AddChild(UIComponent comp)
        {
            // Determine the new position for the child component
            Vector2f newPosition;

            if(HideBase)
            {
                // If this is the first child, it should be placed at the same y-level as the baseButton
                if (children.Count == 0)
                {
                    newPosition = new Vector2f(
                        Position.X + 10, // Offset x by the width of the baseButton and a little margin
                        Position.Y // Same y-level as baseButton
                    );
                }
                else
                {
                    // For subsequent children, position them below the previous child
                    newPosition = new Vector2f(
                        Position.X + 10, // Maintain the same x offset
                        children[^1].Position.Y + (children[^1]).Height + verticalSpacing // Position below the previous child
                    );
                }
            }
            else
            {
                // If this is the first child, it should be placed at the same y-level as the baseButton
                if (children.Count == 0)
                {
                    newPosition = new Vector2f(
                        Position.X + baseButton.Width + 10, // Offset x by the width of the baseButton and a little margin
                        Position.Y // Same y-level as baseButton
                    );
                }
                else
                {
                    // For subsequent children, position them below the previous child
                    newPosition = new Vector2f(
                        Position.X + baseButton.Width + 10, // Maintain the same x offset
                        children[^1].Position.Y + ((UI_Button)children[^1]).Height + verticalSpacing // Position below the previous child
                    );
                }
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

            Width = baseButton.Width;
            Height = children.Count * baseButton.Height + 10;

            // Add the component to the list of children
            children.Add(comp);
        }


        public override void Draw(RenderTexture renderTexture)
        {
            if(!HideBase)
            {
                baseButton.Draw(renderTexture);
            }
            
            if (!IsOpen) return;
            foreach (var child in children)
            {
                child.Draw(renderTexture);
            }
        }

        public override void Update(float deltaTime)
        {
            if (!HideBase)
            {
                baseButton.Update(deltaTime);
            }
            if (!IsOpen) return;
            foreach (var child in children)
            {
                child.Update(deltaTime);
            }
        }
    }
}
