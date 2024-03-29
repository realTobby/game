using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame
{
    public class Assets
    {
        private static Assets _instance;

        public static Assets Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Assets();
                }
                return _instance;
            }
        }

        public Font normalFont = new Font("Assets/Fonts/jellyjam.otf");
        public Font pixelFont1 = new Font("Assets/Fonts/m6x11.ttf");
        public Font pixelFont2 = new Font("Assets/Fonts/Pixeled.ttf");

    }

    
}
