using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using System.Collections.Generic;

namespace sfmlgame.World
{
    public class Chunk
    {
        public bool IsActive { get; private set; }
        public Vector2i Position { get; private set; } // Position in the chunk grid
        private List<WorldTile> tiles = new List<WorldTile>(); // List of tiles in the chunk
        private int tileSize;
        private int chunkSize = 32; // Number of tiles per side in the chunk

        public Chunk(Vector2i position, int tileSize)
        {
            this.tileSize = tileSize;
            Reset(position, null); // Null passed initially for texture, assuming tiles are set later
            indexText = new Text($"({Position.X}, {Position.Y})", GameAssets.Instance.pixelFont1, 19);

            debugOutline = new RectangleShape(new Vector2f(chunkSize * tileSize, chunkSize * tileSize));
        }

        // Activates or deactivates the chunk
        public void SetActive(bool active)
        {
            IsActive = active;
        }

        // Resets the chunk for reuse, including setting its new position and reinitializing its tiles
        public void Reset(Vector2i newPosition, Sprite tileTexture)
        {
            Position = newPosition;
            tiles.Clear();

            double scale = 0.1; // Adjust for more or less frequent terrain changes
            double z = 0; // Keep constant if you want a 2D noise

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    double noiseValue = WorldManager.perlin.Noise((Position.X * chunkSize + x) * scale, (Position.Y * chunkSize + y) * scale, z);
                    TileType type = DetermineTileTypeBasedOnNoise(noiseValue);
                    Vector2f tilePosition = new Vector2f(Position.X * chunkSize * tileSize + x * tileSize, Position.Y * chunkSize * tileSize + y * tileSize);

                    // Get the corresponding sprite for the determined tile type
                    Sprite sprite = GameAssets.Instance.GetTileSprite(type);
                    var newTile = new WorldTile(sprite, tilePosition);
                    tiles.Add(newTile);
                }
            }

            AddWaterBodies();


        }

        public const int MaxWaterSeedsPerChunk = 2;

        public void AddWaterBodies()
        {
            double scale = 0.1; // Increased scale for more granularity
            double threshold = 0.3; // Adjusted threshold
            int maxRadius = 5; // Reduced radius size
            Random rnd = new Random();
            HashSet<Vector2i> waterSeeds = new HashSet<Vector2i>(); // To ensure we don't create overlapping seeds

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    // Convert chunk indices to world position
                    int worldX = Position.X * chunkSize + x;
                    int worldY = Position.Y * chunkSize + y;

                    // Use world position for noise calculation
                    double noiseValue = WorldManager.perlin.Noise(worldX * scale, worldY * scale, 0);
                    if (noiseValue > threshold && waterSeeds.Count < MaxWaterSeedsPerChunk) // Limit the number of seeds
                    {
                        // This tile is a seed for a water body
                        int radius = rnd.Next(2, maxRadius); // Reduced range for variability
                        for (int dx = -radius; dx <= radius; dx++)
                        {
                            for (int dy = -radius; dy <= radius; dy++)
                            {
                                // Calculate the position of the neighbor tile
                                int neighborX = worldX + dx;
                                int neighborY = worldY + dy;
                                // Check if the neighbor is within the radius
                                if (Math.Sqrt(dx * dx + dy * dy) <= radius)
                                {
                                    // Convert neighbor world position back to chunk indices
                                    int targetX = neighborX - (Position.X * chunkSize);
                                    int targetY = neighborY - (Position.Y * chunkSize);
                                    if (targetX >= 0 && targetX < chunkSize && targetY >= 0 && targetY < chunkSize)
                                    {
                                        SetTileTypeAtPosition(targetX, targetY, TileType.WaterTile);
                                    }
                                }
                            }
                        }
                        // Add the seed to the set
                        waterSeeds.Add(new Vector2i(worldX, worldY));
                    }
                }
            }
        }




        private void SetTileTypeAtPosition(int targetX, int targetY, TileType waterTile)
        {
            // Get the corresponding sprite for the determined tile type
            Sprite waterTileSprite = GameAssets.Instance.GetTileSprite(waterTile);

            var tileToChange = tiles.Where(x => x.Sprite.Position.X == targetX && x.Sprite.Position.Y == targetY).FirstOrDefault();

            tiles.Remove(tileToChange);

            if(tileToChange != null)
            {
                tiles.Add(new WorldTile(waterTileSprite, new Vector2f(targetX, targetY)));
            }
        }

        private TileType DetermineTileTypeBasedOnNoise(double noiseValue)
        {
            // Adjust the ranges to make grass more common and water less common.
            // The noiseValue will mostly fall into the range for grass, with smaller ranges for water (to simulate ponds or lakes)
            if (noiseValue < 0.3) return TileType.Grass; // Most of the terrain is grass
            else if (noiseValue < 0.5) return TileType.Grass; // More grass
            else if (noiseValue < 0.55) return TileType.Rock; // Introduce some rocks
                                                              // You can continue with other terrains, ensuring grass remains predominant
            else return TileType.TreeObject; // Default to other types of terrain for higher noise values
        }


        // Updates the chunk, if there's any dynamic content or logic to process
        public void Update(float deltaTime)
        {
            if (!IsActive) return;

            // Update logic for tiles or entities within the chunk
        }

        // Draws the chunk's content to the given render target
        public void Draw(RenderTexture target)
        {
            if (IsActive)
            {

                foreach (var tile in tiles)
                {
                    tile.Draw(target);
                }


            }
            if(Game.Instance.Debug)
                DrawDebug(target);
        }

        RectangleShape debugOutline;
        Text indexText;

        public void DrawDebug(RenderTexture target)
        {
            if (!IsActive) return;

            // Update positions first before drawing
            debugOutline.Position = new Vector2f(Position.X * chunkSize * tileSize, Position.Y * chunkSize * tileSize);
            indexText.Position = new Vector2f(Position.X * chunkSize * tileSize, Position.Y * chunkSize * tileSize);
            indexText.DisplayedString = $"({Position.X}, {Position.Y})"; // Ensure the text is updated every draw call

            // Set styles (this could be moved to the constructor or Reset method to avoid setting it every frame)
            debugOutline.OutlineThickness = 1f;
            debugOutline.OutlineColor = Color.Red;
            debugOutline.FillColor = Color.Transparent;
            indexText.Color = Color.Black;
            indexText.OutlineColor = Color.White;
            indexText.OutlineThickness = 1;

            // Now, draw the debug elements
            target.Draw(debugOutline);
            target.Draw(indexText);

            //Console.WriteLine($"Drawing debug for chunk at position: {Position}");
        }

        public bool ContainsPosition(Vector2f position)
        {
            float left = Position.X * chunkSize * tileSize;
            float top = Position.Y * chunkSize * tileSize;
            float right = left + chunkSize * tileSize;
            float bottom = top + chunkSize * tileSize;

            return position.X >= left && position.X < right && position.Y >= top && position.Y < bottom;
        }

    }
}
