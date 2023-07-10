using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UIManager
    {
        private List<UIComponent> components;

        public UIManager()
        {
            components = new List<UIComponent>();
        }

        public void AddComponent(UIComponent component)
        {
            components.Add(component);
        }

        public void Update()
        {
            foreach (UIComponent component in components)
            {
                component.Update();
            }
        }

        public void Draw()
        {
            foreach (UIComponent component in components)
            {
                component.Draw();
            }
        }
    }
}
