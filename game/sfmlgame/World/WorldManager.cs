using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace sfmlgame.World
{
    public class WorldManager
    {
        public static PerlinNoise perlin = new PerlinNoise(187133610);

        private Dictionary<Vector2i, Chunk> activeChunks = new Dictionary<Vector2i, Chunk>();
        private Queue<Chunk> chunkPool = new Queue<Chunk>();
        private int chunkSize = 32; // Size of a chunk (pixels)
        private int tileSize = 16; // Size of a tile (pixels)

        public WorldManager(int initialPoolSize)
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                chunkPool.Enqueue(new Chunk(new Vector2i(-1, -1), tileSize));
            }
        }

        public void Update(Vector2f playerPosition)
        {
            // New method to manage chunks based on the player's position
            //ManageChunks(playerPosition);
        }

        public void Draw(RenderTexture target)
        {
            foreach (var chunk in activeChunks.Values)
            {
                chunk.Draw(target);
            }

            // Optional: Draw debug information for each chunk
            //foreach (var chunk in activeChunks.Values)
            //{
            //    chunk.DrawDebug(target);
            //}
        }

        public void ManageChunks(Vector2f playerPosition)
        {
            Sprite tileTexture = GameAssets.Instance.GetTileSprite(TileType.Grass);

            // Determine the range of chunks that should be checked based on the player's position
            // This example checks a 3x3 area centered on the player's current chunk
            Vector2i playerChunkIndex = CalculateChunkIndex(playerPosition);

            //Console.WriteLine($"Attempting to activate chunk at: {playerChunkIndex}");

            HashSet<Vector2i> requiredChunks = new HashSet<Vector2i>();

            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    Vector2i nearChunkIndex = new Vector2i(playerChunkIndex.X + x, playerChunkIndex.Y + y);
                    if (!activeChunks.ContainsKey(nearChunkIndex))
                    {
                        ActivateChunk(nearChunkIndex, tileTexture);
                    }
                    requiredChunks.Add(nearChunkIndex);
                }
            }

            // Deactivate chunks that are no longer near the player
            var chunksToDeactivate = activeChunks.Keys.Except(requiredChunks).ToList();
            foreach (var index in chunksToDeactivate)
            {
                DeactivateChunk(index);
            }
        }

        private void ActivateChunk(Vector2i index, Sprite tileTexture)
        {

            Chunk chunk;
            if (chunkPool.Count > 0)
            {
                chunk = chunkPool.Dequeue();
            }
            else
            {
                chunk = new Chunk(index, tileSize);
            }
            chunk.Reset(index, tileTexture);
            activeChunks.Add(index, chunk);
            chunk.SetActive(true);
            //Console.WriteLine($"Successfully activated chunk at: {index}");
        }

        private void DeactivateChunk(Vector2i index)
        {
            if (activeChunks.TryGetValue(index, out Chunk chunk))
            {
                chunk.SetActive(false);
                chunkPool.Enqueue(chunk);
                activeChunks.Remove(index);
            }
        }

        public Vector2i CalculateChunkIndex(Vector2f position)
        {
            // Calculate chunk index by dividing the position by the total size of a chunk in pixels
            int xIndex = (int)Math.Floor(position.X / (chunkSize * tileSize));
            int yIndex = (int)Math.Floor(position.Y / (chunkSize * tileSize));
            return new Vector2i(xIndex, yIndex);
        }
    }
}
