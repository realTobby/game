using game.Controllers;
using game.Helpers;
using game.Models;
using game.OverworldScene;
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

        private Dictionary<Vector2i, Chunk> chunks = new Dictionary<Vector2i, Chunk>();
        // Keep existing properties...

        public int ChunkSize = 16;

        // Generate or get a chunk at a specific position
        public Chunk GetOrCreateChunk(Vector2i chunkPosition)
        {
            if (!chunks.ContainsKey(chunkPosition))
            {
                Chunk newChunk = GenerateChunk(chunkPosition);
                chunks.Add(chunkPosition, newChunk);
                return newChunk;
            }
            return chunks[chunkPosition];
        }

        private const int TileSize = 16; // Size of a tile in pixels

        private Chunk GenerateChunk(Vector2i chunkPosition)
        {
            Chunk chunk = new Chunk();

            // Calculate the world position of the top-left tile of the chunk
            int startX = chunkPosition.X * ChunkSize * TileSize;
            int startY = chunkPosition.Y * ChunkSize * TileSize;

            for (int x = 0; x < ChunkSize; x++)
            {
                for (int y = 0; y < ChunkSize; y++)
                {
                    // Calculate the world position for each tile within the chunk
                    int worldX = startX + x * TileSize;
                    int worldY = startY + y * TileSize;

                    // Use your existing logic to determine the tile type based on the world position
                    // Here's a simplified version of that logic adapted for this context
                    Sprite newSprite = new Sprite(spriteMap["GrassTile"]);
                    double noiseValue = perlinNoise.Noise(worldX * NoiseScale, worldY * NoiseScale, 0);

                    if (noiseValue <= 0.05f) // Water
                    {
                        newSprite = new Sprite(spriteMap["Water"]);
                    }
                    else if (noiseValue <= 0.5f) // Grass
                    {
                        newSprite = new Sprite(spriteMap["GrassTile"]);
                    }
                    else if (noiseValue <= 0.6f) // Trees
                    {
                        newSprite = new Sprite(spriteMap["Trees"]);
                    }
                    else // Rocks
                    {
                        newSprite = new Sprite(spriteMap["Rocks"]);
                    }

                    // Create the tile and set its position
                    OverworldTile tile = new OverworldTile(newSprite, new Vector2f(worldX, worldY));
                    chunk.Tiles.Add(tile);
                }
            }

            return chunk;
        }

        // Method to unload a chunk not needed anymore
        public void UnloadChunk(Vector2i chunkPosition)
        {
            if (chunks.ContainsKey(chunkPosition))
            {
                //chunks.Select(() => SetChunkInactive)
                // Further cleanup if necessary
            }
        }

        // load every fucking sprite in memory
        SpriteSheetLoader texLoad = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

        public Dictionary<string, Sprite> spriteMap = new Dictionary<string, Sprite>();

        Random rnd = new Random();

        // Define tile types
        public enum TileType { Grass, Water, Rocks, Trees }

        // Noise parameters
        private const float NoiseScale = 0.5f;
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


            //GenerateNew(startSize);
        }

        //private void GenerateNew(int gridSize)
        //{
        //    for (int x = 0; x < gridSize; x++)
        //    {
        //        for (int y = 0; y < gridSize; y++)
        //        {
        //            // Default to GrassTile as a placeholder, it will change based on noise value
        //            Sprite newSprite = new Sprite(spriteMap["GrassTile"]);
        //            Sprite objectSprite = null;
        //            bool isGrass = true;
        //            // Calculate noise value for this position
        //            double noiseValue = perlinNoise.Noise(x * NoiseScale, y * NoiseScale, 0);

        //            // Determine tile type based on noise value and assign the corresponding sprite
        //            if (noiseValue <= 0.05f) // Lower values for water
        //            {
        //                newSprite = new Sprite(spriteMap["Water"]);
        //                isGrass = false;
        //            }
        //            else if (noiseValue <= 0.5f) // Next level for grass/land
        //            {
        //                newSprite = new Sprite(spriteMap["GrassTile"]);
        //            }
        //            else if (noiseValue <= 0.6f) // Then trees
        //            {
        //                objectSprite = new Sprite(spriteMap["Trees"]);
        //            }
        //            else // And finally rocks at the highest values
        //            {
        //                objectSprite = new Sprite(spriteMap["Rocks"]);
        //                isGrass = false;
        //            }

        //            var newTile = new OverworldTile(newSprite, new Vector2f(x * 16, y * 16));

        //            // If there's an object sprite (trees or rocks), set its position and add to tile
        //            if (objectSprite != null)
        //            {
        //                objectSprite.Position = newTile.Position;
        //                newTile.Object = objectSprite;
        //            }
        //            else
        //            {
        //                if(isGrass == true)
        //                {
        //                    if (rnd.Next(0, 100) < 55)
        //                    {
        //                        objectSprite = new Sprite(spriteMap["Trees"]);
        //                        objectSprite.Position = newTile.Position;
        //                        newTile.Object = objectSprite;
        //                    }
        //                }

        //            }


        //            OverworldTiles.Add(newTile);
        //        }
        //    }
        //}




        public void Draw(RenderTexture renderTexture, ViewCamera camera)
        {
            // Clear the renderTexture to prepare for new drawing
            renderTexture.Clear(Color.Transparent);

            // Set the renderTexture's view to match the camera's view
            renderTexture.SetView(camera.view);


            FloatRect viewBounds = camera.GetViewBounds();

            // Define how much extra space you want to include around the view (in tiles)
            float extraMargin = 50; // This could be any value depending on how many extra tiles you want

            // Expand the viewBounds by the extraMargin
            FloatRect expandedBounds = new FloatRect(
                viewBounds.Left - extraMargin,
                viewBounds.Top - extraMargin,
                viewBounds.Width + (extraMargin * 2),
                viewBounds.Height + (extraMargin * 2)
            );

            // Iterate through each loaded chunk
            foreach (var chunkEntry in chunks)
            {
                Chunk chunk = chunkEntry.Value;

                // Iterate through each tile in the chunk
                foreach (var tile in chunk.Tiles)
                {
                    // Now, we simply draw without checking bounds since the view is doing the work for us
                    renderTexture.Draw(tile.Sprite);

                    if (tile.Object != null)
                    {
                        renderTexture.Draw(tile.Object);
                    }
                }
            }
        }


        private bool ExistsTileAt(Vector2f pos)
        {
            return OverworldTiles.Any(t => t.Position == pos);
        }
    }
}
