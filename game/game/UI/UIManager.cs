using game.Managers;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public enum MenuType
    {
        MainMenu,
        Ingame
    }

    // todo: interface for menu
    // easily swap menu implementation to show different menus

    public class UIManager
    {
        

        public List<Drawable> UIElements = new List<Drawable>();

        public UIManager()
        {
            //GameManager.Instance.OnRedrawUI += Draw;
        }

        public void Draw()
        {
            foreach(var item in UIElements)
            {
                //GameManager.Instance.GetWindow().Draw(item);
            }
        }

    }
}
