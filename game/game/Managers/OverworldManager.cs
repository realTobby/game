using game.Helpers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace game.Managers
{
    public class OverworldManager
    {
        public List<OverworldTile> OverworldTiles = new List<OverworldTile>();

        // load every fucking sprite in memory
        SpriteSheetLoader texLoad = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

        public Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

        Random rnd = new Random();

        // Define tile types
        public enum TileType { Grass, Water, Rocks, Trees }

        // Noise parameters
        private const float NoiseScale = 0.1f;
        private const int Seed = 1234;

        private PerlinNoise perlinNoise;

        public OverworldManager(int startSize)
        {
            perlinNoise = new PerlinNoise(rnd.Next(-999999, 999999));

            spriteMap = new Dictionary<string, Sprite>
            {
                { "GrassTile", texLoad.GetSpriteFromSheet(4, 12)
                },
                {
                    "RandomTile", texLoad.GetSpriteFromSheet(rnd.Next(0,69), rnd.Next(0,47))
                },
                { "Water", texLoad.GetSpriteFromSheet(19, 15)
                },
                { "Rocks", texLoad.GetSpriteFromSheet(6, 11)
                },
                { "Trees", texLoad.GetSpriteFromSheet(8, 7)
                },
            };


            GenerateNew(startSize);
        }

        private void GenerateNew(int gridSize)
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    // Default to GrassTile as a placeholder, it will change based on noise value
                    Sprite newSprite = new Sprite(spriteMap["GrassTile"]);
                    Sprite objectSprite = null;
                    bool isGrass = true;
                    // Calculate noise value for this position
                    double noiseValue = perlinNoise.Noise(x * NoiseScale, y * NoiseScale, 0);

                    // Determine tile type based on noise value and assign the corresponding sprite
                    if (noiseValue <= 0.05f) // Lower values for water
                    {
                        newSprite = new Sprite(spriteMap["Water"]);
                        isGrass = false;
                    }
                    else if (noiseValue <= 0.5f) // Next level for grass/land
                    {
                        newSprite = new Sprite(spriteMap["GrassTile"]);
                    }
                    else if (noiseValue <= 0.6f) // Then trees
                    {
                        objectSprite = new Sprite(spriteMap["Trees"]);
                    }
                    else // And finally rocks at the highest values
                    {
                        objectSprite = new Sprite(spriteMap["Rocks"]);
                        isGrass = false;
                    }

                    var newTile = new OverworldTile(newSprite, new Vector2f(x * 16, y * 16));

                    // If there's an object sprite (trees or rocks), set its position and add to tile
                    if (objectSprite != null)
                    {
                        objectSprite.Position = newTile.Position;
                        newTile.Object = objectSprite;
                    }
                    else
                    {
                        if(isGrass == true)
                        {
                            if (rnd.Next(0, 100) < 55)
                            {
                                objectSprite = new Sprite(spriteMap["Trees"]);
                                objectSprite.Position = newTile.Position;
                                newTile.Object = objectSprite;
                            }
                        }
                        
                    }


                    OverworldTiles.Add(newTile);
                }
            }
        }




        public void Draw(RenderTexture renderTexture)
        {
            foreach (var tile in OverworldTiles)
            {
                renderTexture.Draw(tile.Sprite);

                if (tile.Object != null)
                {
                    renderTexture.Draw(tile.Object);
                }
            }
        }

        private bool ExistsTileAt(Vector2f pos)
        {
            return OverworldTiles.Any(t => t.Position == pos);
        }
    }
}
