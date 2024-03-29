using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Assets
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
            if (!TextureCache.ContainsKey(key))
            {
                TextureCache.Add(key, new Texture($"Assets/{category}/{textureName}.png"));
            }
            return TextureCache[key];
        }

        public Texture[] GetAnimations(string assetPath, string entityName, int frameCount)
        {
            Texture[] textures = new Texture[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                string key = $"{entityName}_{i}";
                if (!TextureCache.ContainsKey(key))
                {
                    TextureCache.Add(key, new Texture($"{assetPath}/{entityName}_{i}.png"));
                }
                textures[i] = TextureCache[key];
            }
            return textures;
        }

    }
}
