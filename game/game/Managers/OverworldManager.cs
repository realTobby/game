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

        public OverworldManager(int startSize)
        {
            GenerateNew(startSize);
        }

        private void GenerateNew(int gridSize)
        {
            Random rnd = new Random();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    var newTile = new OverworldTile(TextureLoader.Instance.GetTexture("grassyTile", "Tiles"), new Vector2f(x * 32, y * 32));

                    if (rnd.Next(100) > 80)
                    {
                        if (rnd.Next(100) > 50)
                        {
                            newTile.Object = new Sprite(TextureLoader.Instance.GetTexture("tree", "Objects"));
                        }
                        else
                        {
                            newTile.Object = new Sprite(TextureLoader.Instance.GetTexture("tree2", "Objects"));
                        }
                    }

                    OverworldTiles.Add(newTile);
                }
            }
        }

        public void Draw()
        {
            foreach (var tile in OverworldTiles)
            {
                Game.Instance.GetRenderWindow().Draw(tile.Sprite);

                if (tile.Object != null)
                {
                    Game.Instance.GetRenderWindow().Draw(tile.Object);
                }
            }
        }

        private bool ExistsTileAt(Vector2f pos)
        {
            return OverworldTiles.Any(t => t.Position == pos);
        }

        public void OnPlayerMove(Vector2f playerPos)
        {
            int tileSize = 32; // Assuming the tile size is 32x32
            int viewRange = 10;
            Vector2i gridPosition = new Vector2i(
                (int)(playerPos.X / tileSize),
                (int)(playerPos.Y / tileSize)
            );

            int startX = gridPosition.X - viewRange;
            int endX = gridPosition.X + viewRange;
            int startY = gridPosition.Y - viewRange;
            int endY = gridPosition.Y + viewRange;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    if (!ExistsTileAt(new Vector2f(x * tileSize, y * tileSize)))
                    {
                        var newTile = new OverworldTile(TextureLoader.Instance.GetTexture("grassyTile", "Tiles"), new Vector2f(x * tileSize, y * tileSize));

                        OverworldTiles.Add(newTile);
                    }
                }
            }
        }
    }
}
