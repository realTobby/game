using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Tile
    {
        public Sprite Sprite;
        public TileType Type;
        public int PosX;
        public int PosY;
    }

    public enum TileType
    {
        Grass,
        Rock
    }

    public class IsometricGrid
    {
        private RenderWindow _window;

        public int GridWidth = 10;
        public int GridHeight = 10;

        private Texture grassyTile;
        private Texture rockyTile;

        public Dictionary<Vector2i, Tile> GridBase = new Dictionary<Vector2i, Tile>();

        private Vector2f tileSize;

        Vector2f CartesianToIsometric(int x, int y)
        {
            float isoX = (x - y) * tileSize.X / 2;
            float isoY = (x + y) * tileSize.Y / 2;
            return new Vector2f(isoX, isoY);
        }

        Vector2i IsoToCartesian(int x, int y)
        {
            var isoPos = new Vector2f(x, y);

            float cartX = (isoPos.X / tileSize.X + (isoPos.Y / tileSize.Y)) / 2;
            float cartY = (isoPos.Y / tileSize.Y - (isoPos.X / tileSize.X)) / 2;
            return new Vector2i((int)cartX, (int)cartY);
        }

        public IsometricGrid(RenderWindow window)
        {
            _window = window;

            grassyTile = new Texture("Assets/grassyTile.png");
            rockyTile = new Texture("Assets/rockyTile.png");
            tileSize = new Vector2f(grassyTile.Size.X+1, grassyTile.Size.Y+1);

            InitBaseGrid();


        }

        private void InitBaseGrid()
        {
            for (int y = 0; y < GridHeight; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    if (GameManager.Instance.RANDOM.Next(100) > 50)
                    {
                        GridBase[new Vector2i(x, y)] = new Tile() { PosX = x, PosY = y, Type = TileType.Rock, Sprite = new Sprite(rockyTile) };
                    }
                    else
                    {
                        //GridBase.Add(new Tile() { PosX = x, PosY = y, Type = TileType.Grass, Sprite = new Sprite(grassyTile) });
                        GridBase[new Vector2i(x, y)] = new Tile() { PosX = x, PosY = y, Type = TileType.Grass, Sprite = new Sprite(grassyTile) };
                    }
                }
            }
        }

        public void Draw()
        {
            int radius = 25;

            // Calculate the center tile position
            var centerTilePos = IsoToCartesian((int)GameManager.Instance.Player.Position.X, (int)GameManager.Instance.Player.Position.Y);

            int minX = centerTilePos.X - (int)Math.Ceiling(radius / 2.0f);
            int maxX = centerTilePos.X + (int)Math.Floor(radius / 2.0f);
            int minY = centerTilePos.Y - (int)Math.Ceiling(radius / 2.0f);
            int maxY = centerTilePos.Y + (int)Math.Floor(radius / 2.0f);

            // Generate and draw the tiles within the range
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var pos = new Vector2i(x, y);

                    // Only add tile if it doesn't already exist
                    if (!GridBase.ContainsKey(pos))
                    {
                        GridBase[pos] = GenerateTile(x, y);
                    }
                }
            }

            // Draw the tiles
            foreach (var tile in GridBase.Values)
            {
                var isoPos = CartesianToIsometric(tile.PosX, tile.PosY);
                isoPos.X += (_window.Size.X - (GridWidth * tileSize.X)) / 2;
                isoPos.Y += (_window.Size.Y - (GridHeight * tileSize.Y)) / 2;

                tile.Sprite.Position = isoPos;

                float worldX = (tile.PosX - tile.PosY) * tileSize.X / 2;
                float worldY = (tile.PosX + tile.PosY) * tileSize.Y / 2;

                tile.Sprite.Position = new Vector2f(worldX, worldY);

                _window.Draw(tile.Sprite);
            }
        }

        //public void Draw()
        //{
        //    // Generate more tiles
        //    int radius = 50; // The radius around the player position in which we generate new tiles
        //    var playerGridPos = IsoToCartesian((int)GameManager.Instance.Player.Position.X, (int)GameManager.Instance.Player.Position.Y);

        //    int minX = (int)playerGridPos.X - radius / 2;
        //    int maxX = (int)playerGridPos.X + radius / 2;
        //    int minY = (int)playerGridPos.Y - radius / 2;
        //    int maxY = (int)playerGridPos.Y + radius / 2;

        //    for (int x = minX; x <= maxX; x++)
        //    {
        //        for (int y = minY; y <= maxY; y++)
        //        {
        //            var pos = CartesianToIsometric(x, y);

        //            if(GridBase.ContainsKey(new Vector2i(x,y)) == false)
        //            {
        //                if (GetTileAt(pos) == null)
        //                {
        //                    //GridBase[pos] = GenerateTile(x, y);
        //                    GridBase.Add(new Vector2i(x,y), GenerateTile(x, y));
        //                }
        //            }


        //        }
        //    }

        //    // Draw the tiles
        //    foreach (var tile in GridBase.Values)
        //    {
        //        var isoPos = CartesianToIsometric(tile.PosX, tile.PosY);
        //        isoPos.X += (_window.Size.X - (GridWidth * tileSize.X)) / 2;
        //        isoPos.Y += (_window.Size.Y - (GridHeight * tileSize.Y)) / 2;

        //        tile.Sprite.Position = isoPos;

        //        float worldX = (tile.PosX - tile.PosY) * tileSize.X / 2;
        //        float worldY = (tile.PosX + tile.PosY) * tileSize.Y / 2;

        //        tile.Sprite.Position = new Vector2f(worldX, worldY);

        //        _window.Draw(tile.Sprite);
        //    }
        //}

        public Tile GetTileAt(Vector2f pos)
        {
            if (GridBase.TryGetValue(new Vector2i((int)pos.X, (int)pos.Y), out Tile tile))
            {
                return tile;
            }
            else
            {
                return null; // No tile found at the given position
            }
        }

        //public Vector2f GetCenterIsoPos()
        //{
        //    var centerTile = GetTileAt(new Vector2f(5,5));
        //    var isoPos = CartesianToIsometric(centerTile.PosX, centerTile.PosY);
        //    isoPos.X += (_window.Size.X - (GridWidth * tileSize.X)) / 2;
        //    isoPos.Y += (_window.Size.Y - (GridHeight * tileSize.Y)) / 2;
        //    return isoPos;
        //}

        public void ResetGrid()
        {
            GridBase.Clear();
            InitBaseGrid();
        }

        private Tile GenerateTile(int x, int y)
        {
            if (GameManager.Instance.RANDOM.Next(100) > 50)
            {
                return new Tile() { PosX = x, PosY = y, Type = TileType.Rock, Sprite = new Sprite(rockyTile) };
            }
            else
            {
                return new Tile() { PosX = x, PosY = y, Type = TileType.Grass, Sprite = new Sprite(grassyTile) };
            }
        }
    }
}
