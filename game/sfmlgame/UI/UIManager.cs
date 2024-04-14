
using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;

namespace sfmlgame.UI
{
    public class UIManager
    {
        private List<UIComponent> _components;

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

        public void CreateDamageNumber(int amount, Vector2f worldPos, float duration)
        {
            try
            {
                // find non-active object
                UI_DamageNumber freeDamageNumber = _components.Where(x => x.IsActive == false && x.GetType() == typeof(UI_DamageNumber)).FirstOrDefault() as UI_DamageNumber;

                if (freeDamageNumber == null)
                {
                    freeDamageNumber = new UI_DamageNumber(amount, worldPos, duration);
                    _components.Add(freeDamageNumber);
                }
                else
                {
                    freeDamageNumber.ResetFromPool(worldPos, amount);
                    freeDamageNumber.IsActive = true;
                }
            }catch(Exception ex)
            {
                UniversalLog.LogInfo("couldnt care less.");
            }
            

        }

        public void Update(float deltaTime)
        {
            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Update(deltaTime);

            }
        }

        public void Draw(RenderTexture renderTexture)
        {
            

            foreach (UIComponent component in _components.ToList())
            {
                if(component != null) component.Draw(renderTexture);

            }

        }

    }
}
