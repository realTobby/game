using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Assets
{
    public enum TileType
    {
        Grass,
        // Add other tile types as needed
    }

    public class GameAssets
    {
        private static GameAssets instance;
        public static GameAssets Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameAssets();
                }
                return instance;
            }
        }

        private SpriteSheetLoader spriteLoader;
        private Dictionary<TileType, Sprite> sprites;

        public Font normalFont = new Font("Assets/Fonts/jellyjam.otf");
        public Font pixelFont1 = new Font("Assets/Fonts/m6x11.ttf");
        public Font pixelFont2 = new Font("Assets/Fonts/Pixeled.ttf");

        public TextureLoader TextureLoader;

        private GameAssets()
        {
            // Initialize the SpriteSheetLoader with the path to your sprite sheet
            spriteLoader = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

            TextureLoader = new TextureLoader();

            // Initialize the dictionary
            sprites = new Dictionary<TileType, Sprite>();

            // Load sprites from the sprite sheet
            LoadSprites();
        }

        private void LoadSprites()
        {
            // Load each sprite using its tile type and coordinates on the sprite sheet
            // The coordinates will depend on your sprite sheet's layout
            sprites[TileType.Grass] = spriteLoader.GetSpriteFromSheet(0, 0);
            // Add other sprites as needed
        }

        public static Sprite GetTile(TileType type)
        {
            if (Instance.sprites.TryGetValue(type, out Sprite sprite))
            {
                return sprite;
            }
            else
            {
                throw new ArgumentException("TileType not found in sprite dictionary.");
            }
        }
    }
}

