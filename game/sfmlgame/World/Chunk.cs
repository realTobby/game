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

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    Vector2f tilePosition = new Vector2f(Position.X * chunkSize * tileSize + x * tileSize, Position.Y * chunkSize * tileSize + y * tileSize);

                    var newTile = new WorldTile(tileTexture, tilePosition); // Assuming Tile constructor is (Texture, Vector2f)

                    if (x == chunkSize/2 && y == chunkSize/2)
                    {
                        newTile.Object = new WorldTile(GameAssets.Instance.GetTileSprite(TileType.Station), tilePosition);
                        newTile.Object.Sprite.Position = tilePosition;


                    }

                    tiles.Add(newTile);

                }
            }



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
