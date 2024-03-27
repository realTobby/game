using game.Managers;
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

        public Font MainFont = new Font("Assets/Fonts/m6x11.ttf");

        public int GetDamagerNumberPoolSize => _components.Where(x => x.GetType() == typeof(UI_DamageNumber)).Count();

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
            
            component.IsActive = false;

            if(component.GetType() == typeof(UI_DamageNumber))
            {
                component.IsActive = false;
            }
            else
            {
                _components.Remove(component);
            }

        }

        public void CreateDamageNumber(int amount, Vector2f worldPos, View view, float duration)
        {
            // find non-active object
            UI_DamageNumber freeDamageNumber = _components.Where(x => x.IsActive == false && x.GetType() == typeof(UI_DamageNumber)).FirstOrDefault() as UI_DamageNumber;

            if(freeDamageNumber == null)
            {
                freeDamageNumber = new UI_DamageNumber(amount, worldPos, view, duration);
                _components.Add(freeDamageNumber);
            }
            else
            {
                freeDamageNumber.ResetFromPool(worldPos);
                freeDamageNumber.IsActive = true;
            }

        }

        public void Update()
        {
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Update();

            }
        }

        public void Draw(RenderTexture renderTexture)
        {
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Draw(renderTexture);

            }
        }

        public void Draw(RenderTexture renderTexture, View view)
        {
            RenderWindow window = Game.Instance.GetRenderWindow();

            // Store the original view
            View originalView = window.GetView();

            // Set the temporary view on the render window
            window.SetView(view);

            // Draw the UI components
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Draw(renderTexture);

            }

            // Restore the original view
            window.SetView(originalView);
        }

        public void DrawDirectlyToWindow()
        {
            RenderWindow window = Game.Instance.GetRenderWindow();
            foreach (UIComponent component in _components.ToList())
            {
                if (component != null) component.DrawDirectlyToWindow();

            }
        }


        
    }
}
