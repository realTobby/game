using game.Controllers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class OverworldManager
    {
        public List<OverworldTile> OverworldTiles = new List<OverworldTile>();

        public Vector2i StartCenterPos = new Vector2i();

        public OverworldManager(int startSize)
        {
            
            InitOverworld();
            GenerateNew(startSize / 2*-1, startSize / 2*-1, startSize);
        }

        private void InitOverworld()
        {
            OverworldTiles.Clear();
        }

        private void GenerateNew(int startX, int startY, int startGridSize)
        {
            Random rnd = new Random();

            StartCenterPos = new Vector2i(startGridSize / 2, startGridSize / 2);
            // generate a 10x10 grid of tiles
            for (int x = startX; x < startX + startGridSize; x++)
            {
                for (int y = startY; y < startY + startGridSize; y++)
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

                        newTile.Object.Position = new Vector2f(newTile.Position.X+24, newTile.Position.Y-2);
                    }

                    OverworldTiles.Add(newTile);
                }
            }
        }

        public void Draw(RenderWindow _window)
        {
            foreach(var tile in OverworldTiles)
            {
                _window.Draw(tile.Sprite);

                

            }

            foreach(var tile in OverworldTiles)
            {
                if (tile.Object != null)
                {
                    _window.Draw(tile.Object);
                }
            }

        }

        private bool ExistsTileAt(Vector2f pos)
        {
            if(OverworldTiles.Any(t => t.Position == pos))
            {
                return true;
            }
            return false;
        }

        public void OnPlayerMove(Vector2f playerPos)
        {
            Vector2i gridPosition = new Vector2i(
                (int)(playerPos.X / 16),
                (int)(playerPos.Y / 16)
            );

            int startX = gridPosition.X - 10;
            int endX = gridPosition.X + 10;
            int startY = gridPosition.Y - 10;
            int endY = gridPosition.Y + 10;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startX; y < endX; y++)
                {
                    if(!ExistsTileAt(new Vector2f(x,y)))
                    {
                        var newTile = new OverworldTile(TextureLoader.Instance.GetTexture("grassyTile", "Tiles"), new Vector2f(x * 16, y * 16));

                        OverworldTiles.Add(newTile);
                    }

                    
                }
            }




        }


    }
}
