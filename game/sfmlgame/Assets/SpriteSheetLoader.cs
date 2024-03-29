using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Assets
{
    public class SpriteSheetLoader
    {
        private readonly Texture spriteSheet;

        public SpriteSheetLoader(string path)
        {
            spriteSheet = new Texture(path);
        }

        public Sprite GetSpriteFromSheet(int col, int row)
        {
            // Ensure that the (col, row) parameters are converted to pixel coordinates based on the 16x16 sprite size
            IntRect spriteRect = new IntRect(col * 16, row * 16, 16, 16);

            // Create a sprite with the specified texture rect
            Sprite sprite = new Sprite(spriteSheet, spriteRect);
            return sprite;
        }
    }
}
