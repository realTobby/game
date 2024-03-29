using game.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.OverworldScene
{
    public class Chunk
    {
        // infinite gaming baby
        public List<OverworldTile> Tiles = new List<OverworldTile>();

        public void SetChunksInactive()
        {
            // we remove these chunks for them rendering pipeline

        }

    }
}
