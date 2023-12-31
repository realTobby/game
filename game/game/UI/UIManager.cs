﻿using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;

namespace game.UI
{
    public class UIManager
    {
        private List<UIComponent> _components;

        public UIManager()
        {
            _components = new List<UIComponent>();
        }

        public void AddComponent(UIComponent component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(UIComponent component)
        {
            _components.Remove(component);
        }

        public void Update()
        {
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Update();

            }
        }

        public void Draw()
        {
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Draw();

            }
        }

        public void Draw(View view)
        {
            RenderWindow window = Game.Instance.GetRenderWindow();

            // Store the original view
            View originalView = window.GetView();

            // Set the temporary view on the render window
            window.SetView(view);

            // Draw the UI components
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Draw();

            }

            // Restore the original view
            window.SetView(originalView);
        }
    }
}
