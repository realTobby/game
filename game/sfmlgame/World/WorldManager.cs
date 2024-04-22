using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using System.Collections.Generic;

namespace sfmlgame.World
{
    public class WorldManager
    {
        public static PerlinNoise perlin = new PerlinNoise(187133610);

        public int chunkSize = 32; // Number of tiles per side in a chunk
        public int tileSize = 16; // Size of each tile in pixels
        private Dictionary<Vector2i, Chunk> activeChunks = new Dictionary<Vector2i, Chunk>();
        private Queue<Chunk> chunkPool = new Queue<Chunk>();
        public Vector2i CurrentChunkIndex;

        public WorldManager(int poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                chunkPool.Enqueue(new Chunk(new Vector2i(-1, -1), tileSize));
            }
            // Trigger initial load
            CurrentChunkIndex = CalculateChunkIndex(new Vector2f(0,0));
            LoadOrUnloadChunks();
        }

        public void Update(Vector2f playerPosition)
        {
            // Determine the player's current chunk index based on their position
            Vector2i playerChunkIndex = CalculateChunkIndex(playerPosition);

            // Check if the player has moved to a different chunk
            if (playerChunkIndex != CurrentChunkIndex)
            {
                // Calculate the direction in which the player has moved across chunk boundaries
                Vector2i chunkMoveDirection = playerChunkIndex - CurrentChunkIndex;

                // Re-center the active chunks around the new playerChunkIndex
                ReCenterChunks(playerChunkIndex, chunkMoveDirection);

                // Update the player's position to be within the new central chunk
                Vector2f playerPositionInNewChunk = new Vector2f(
                    playerPosition.X - chunkMoveDirection.X * chunkSize * tileSize,
                    playerPosition.Y - chunkMoveDirection.Y * chunkSize * tileSize
                );

                // Ensure the player's position is updated in the game instance
                Game.Instance.PLAYER.SetPosition(playerPositionInNewChunk);

                // Update the current chunk index
                CurrentChunkIndex = playerChunkIndex;
            }

            // Load new chunks and unload old chunks based on the updated CurrentChunkIndex
            LoadOrUnloadChunks();
        }


        private void ReCenterChunks(Vector2i newCenterChunkIndex, Vector2i chunkMoveDirection)
        {
            // Calculate the set of chunks that should now be active around the new center chunk
            HashSet<Vector2i> requiredChunks = CalculateRequiredChunks(newCenterChunkIndex);

            // Create a new dictionary to hold the re-centered active chunks
            var reCenteredActiveChunks = new Dictionary<Vector2i, Chunk>();

            // Go through each required chunk position
            foreach (Vector2i requiredChunkIndex in requiredChunks)
            {
                // Calculate the position of the chunk in the old activeChunks dictionary
                Vector2i oldChunkPosition = requiredChunkIndex - chunkMoveDirection;

                if (activeChunks.TryGetValue(oldChunkPosition, out Chunk chunk))
                {
                    // Move the chunk to the new required position
                    reCenteredActiveChunks[requiredChunkIndex] = chunk;
                }
                else
                {
                    // If the chunk wasn't active before, activate it now
                    ActivateChunk(requiredChunkIndex);
                    reCenteredActiveChunks[requiredChunkIndex] = activeChunks[requiredChunkIndex];
                }
            }

            // Replace the old activeChunks dictionary with the re-centered one
            activeChunks = reCenteredActiveChunks;

            // Unload chunks that are no longer in the required set
            foreach (var chunkIndex in activeChunks.Keys.Except(requiredChunks).ToList())
            {
                DeactivateChunk(chunkIndex);
            }
        }


        public void Draw(RenderTexture target)
        {
            // This needs to draw chunks based on their relation to the center chunk (0,0)
            foreach (KeyValuePair<Vector2i, Chunk> entry in activeChunks)
            {
                Vector2f position = new Vector2f(
                    (entry.Key.X - CurrentChunkIndex.X) * chunkSize * tileSize,
                    (entry.Key.Y - CurrentChunkIndex.Y) * chunkSize * tileSize
                );
                entry.Value.Draw(target, position);
            }
        }

        public void ManageChunks(Vector2i newCenterChunkIndex)
        {
            HashSet<Vector2i> requiredChunks = CalculateRequiredChunks(newCenterChunkIndex);

            // Load new chunks or reuse existing ones from the pool
            foreach (Vector2i index in requiredChunks)
            {
                if (!activeChunks.ContainsKey(index))
                {
                    ActivateChunk(index);
                }
            }

            // Unload chunks not in the new set of required chunks
            var chunksToUnload = new List<Vector2i>(activeChunks.Keys.Except(requiredChunks));
            foreach (Vector2i index in chunksToUnload)
            {
                DeactivateChunk(index);
            }

            CurrentChunkIndex = newCenterChunkIndex; // Update current chunk index to the new center
        }


        // This method should calculate the set of chunks that need to be active based on the new chunk index
        private HashSet<Vector2i> CalculateRequiredChunks(Vector2i centerChunkIndex)
        {
            HashSet<Vector2i> requiredChunks = new HashSet<Vector2i>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    requiredChunks.Add(new Vector2i(centerChunkIndex.X + x, centerChunkIndex.Y + y));
                }
            }
            return requiredChunks;
        }

        public void Clear()
        {
            foreach (var chunk in activeChunks.Values)
            {
                chunk.SetActive(false); // Ensure all chunks are deactivated.
                chunkPool.Enqueue(chunk); // Return to the pool for reuse.
            }
            activeChunks.Clear(); // Clear the dictionary of active chunks.
        }

        public Vector2f AdjustPlayerPositionForChunkTransition(Vector2f playerPosition, Vector2i oldChunkIndex, Vector2i newChunkIndex)
        {
            // Der Unterschied in den Chunks in X- und Y-Richtung
            int deltaX = newChunkIndex.X - oldChunkIndex.X;
            int deltaY = newChunkIndex.Y - oldChunkIndex.Y;

            // Berechnen Sie die neue X- und Y-Position basierend auf dem Unterschied
            // und der aktuellen Position des Spielers innerhalb des Chunks
            float newX = playerPosition.X - deltaX * chunkSize * tileSize;
            float newY = playerPosition.Y - deltaY * chunkSize * tileSize;

            // Stellen Sie sicher, dass der Spieler am entgegengesetzten Rand des Chunks platziert wird
            if (deltaX != 0)
            {
                newX = -deltaX > 0 ? 0 : chunkSize * tileSize - tileSize;
            }
            if (deltaY != 0)
            {
                newY = -deltaY > 0 ? 0 : chunkSize * tileSize - tileSize;
            }

            return new Vector2f(newX, newY);
        }


        private void LoadOrUnloadChunks()
        {
            HashSet<Vector2i> requiredChunks = new HashSet<Vector2i>();
            int radius = 2; // Load chunks within this radius from the current chunk
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector2i index = new Vector2i(CurrentChunkIndex.X + x, CurrentChunkIndex.Y + y);
                    requiredChunks.Add(index);
                    if (!activeChunks.ContainsKey(index))
                    {
                        Console.WriteLine("Activating Chunk at: " + index);
                        ActivateChunk(index);
                    }
                }
            }

            var toUnload = new List<Vector2i>(activeChunks.Keys.Except(requiredChunks));
            foreach (var index in toUnload)
            {
                Console.WriteLine("Deactivating Chunk at: " + index);
                DeactivateChunk(index);
            }
        }


        private void ActivateChunk(Vector2i index)
        {
            Chunk chunk = chunkPool.Count > 0 ? chunkPool.Dequeue() : new Chunk(index, tileSize);
            chunk.Reset(index, GameAssets.Instance.GetTileSprite(TileType.Grass));
            activeChunks[index] = chunk;
            chunk.SetActive(true);
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
            int x = (int)(position.X / (chunkSize * tileSize));
            int y = (int)(position.Y / (chunkSize * tileSize));
            return new Vector2i(x, y);
        }
    }
}
