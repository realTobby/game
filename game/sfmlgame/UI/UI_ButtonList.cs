using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace sfmlgame.UI
{
    public class UI_ButtonList : UIComponent
    {
        public UI_Button baseButton;
        public bool IsOpen = false;
        private UI_List childList;  // Using UI_List for managing children
        private float horizontalOffset = 10;  // Horizontal offset for child list

        public bool HideBase = false;
        public bool IsCombo = false;

        public UI_ButtonList(Vector2f position, string name, bool isCombo) : base(position)
        {
            baseButton = new UI_Button(position, name, 40, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            baseButton.ClickAction = ToggleVisibility;
            baseButton.Hide = true;


            Width = baseButton.Width;
            Height = baseButton.Height;

            IsCombo = isCombo;
            childList = new UI_List(new Vector2f(Position.X + (IsCombo ? baseButton.Width + horizontalOffset : -horizontalOffset), Position.Y));
        }

        private void ToggleVisibility()
        {
            if (!HideBase)
            {
                IsOpen = !IsOpen;
            }
        }

        public void AddChild(UIComponent comp, Vector2f offset)
        {
            childList.AddChild(comp, offset);
            UpdateHeight();
        }

        public void AddChild(UIComponent comp)
        {
            childList.AddChild(comp, new Vector2f(0,0));
            UpdateHeight();
        }

        private void UpdateHeight()
        {
            Height = baseButton.Height + (IsOpen ? childList.Height : 0);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if(!Hide) baseButton.Draw(renderTexture);

            if (IsOpen)
            {
                childList.Draw(renderTexture);
            }
        }

        public override void Update(float deltaTime)
        {
            baseButton.Update(deltaTime);
            if (IsOpen)
            {
                childList.Update(deltaTime);
            }
        }
    }
}
