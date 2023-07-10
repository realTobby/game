using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class TextureLoader
    {
        private static TextureLoader _instance;
        public static TextureLoader Instance => _instance;

        public Dictionary<string, Texture> TextureCache = new Dictionary<string, Texture>(); 

        public TextureLoader()
        {
            if (_instance == null) _instance = this;
        }

        public Texture GetTexture(string textureName, string category)
        {
            string key = $"{category}/{textureName}";
            if(!TextureCache.ContainsKey(key))
            {
                TextureCache.Add(key, new Texture($"Assets/{category}/{textureName}.png"));
            }
            return TextureCache[key];
        }

    }
}
