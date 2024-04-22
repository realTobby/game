using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class WorldGenerator : TileMap
{
	private const int ChunkSize = 16; // Size of each chunk (16x16 tiles)
	private Dictionary<Vector2I, Chunk> chunks = new Dictionary<Vector2I, Chunk>();

	private Vector2I lastLoadedChunk = Vector2I.Zero;

	public override void _Ready()
	{
		GD.Print("World Generator Ready. Creating World...");

		UpdateActiveChunks(new Vector2I(0, 0));
	}

	public override void _Draw()
	{
		foreach (var chunkKey in chunks.Keys)
		{
			DrawChunkOutline(chunkKey);
		}
	}

	private void DrawChunkOutline(Vector2I chunkPosition)
	{
		float scale = Transform.Scale.X; // Assuming uniform scaling for simplicity
		int effectiveChunkPixelSize = (int)(ChunkSize * 16 * scale);

		Vector2 topLeft = new Vector2(chunkPosition.X * effectiveChunkPixelSize, chunkPosition.Y * effectiveChunkPixelSize);
		Vector2 size = new Vector2(effectiveChunkPixelSize, effectiveChunkPixelSize);

		DrawRect(new Rect2(topLeft, size), new Color(1, 0, 0)); // Draw a red outline
	}


	public void LoadChunk(Vector2I chunkPos)
	{
		//GD.Print("Loading Chunk at: " + chunkPos.ToString());
		var chunk = SetChunkTiles(chunkPos, new Vector2I(0, 0));
		if(chunk != null)
			chunks.Add(chunkPos, chunk);
	}

	public void UnloadChunk(Vector2I chunkPos)
	{
		
		Vector2I emptyAtlasCoords = new Vector2I(-1, -1); // Atlas coordinates for the tile

		RemoveChunkAt(chunkPos);	

		SetChunkTiles(chunkPos, emptyAtlasCoords);
	}

	private void RemoveChunkAt(Vector2I chunkPos)
	{
		if(chunks.ContainsKey(chunkPos))
		{
			chunks.Remove(chunkPos);
		}
	}

	private Chunk SetChunkTiles(Vector2I chunkPos, Vector2I tileAtlasCoords)
	{
		Chunk newChunk = new Chunk(chunkPos);

		for (int x = 0; x < ChunkSize; x++)
		{
			for (int y = 0; y < ChunkSize; y++)
			{
				
				Vector2I tilePos = new Vector2I(chunkPos.X * ChunkSize + x, chunkPos.Y * ChunkSize + y);
				SetCell(0, tilePos,sourceId: 0, atlasCoords: tileAtlasCoords);
				//GD.Print("SetCell " + tilePos.ToString());

			}
		}

		return newChunk;
		
	}

	public void UpdatePlayerPosition(Vector2 playerPosition)
	{
		// Assuming tileSize represents the size of each tile in pixels
		int tileSize = 16; // Assuming each tile is 16x16 pixels

		// Adjust for TileMap scale
		float scale = Transform.Scale.X; // Assuming uniform scaling for simplicity

		// Calculate the effective chunk size in pixels considering both tile size and scale
		int effectiveChunkSize = (int)(ChunkSize * tileSize * scale);

		// Calculate the chunk position based on the scaled and tiled player position
		Vector2I chunkPos = new Vector2I((int)(playerPosition.X / effectiveChunkSize), (int)(playerPosition.Y/ effectiveChunkSize));

		if (chunkPos == lastLoadedChunk) return; // If the player hasn't moved to a new chunk, do nothing

		GD.Print("Updating player position to: " + playerPosition.ToString());
		lastLoadedChunk = chunkPos;
		GD.Print("Calculated chunk position: " + chunkPos.ToString());

		UpdateActiveChunks(chunkPos); // Update chunks around the new player position
	}




	private void UpdateActiveChunks(Vector2I newChunkPos)
	{
		GD.Print("Unloading Chunks");
		// Deactivate unused chunks
		foreach (var chunk in chunks.Values.ToList())
		{
			UnloadChunk(chunk.Origin);
		}

		GD.Print("Loading Chunks");
		// Load or activate chunks around the player
		for (int dx = -1; dx <= 1; dx++)
		{
			for (int dy = -1; dy <= 1; dy++)
			{
				Vector2I pos = new Vector2I(newChunkPos.X + dx, newChunkPos.Y + dy);
				LoadChunk(pos);
			}
		}

		
	}
}
