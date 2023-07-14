using game.Entities.Pickups;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class GemManager
    {
        private Random rnd = new Random();

        public List<Gem> ActiveGems = new List<Gem>();

        public void Update()
        {
            if(ActiveGems.Count > 200)
            {
                int totalXP = 0;

                Vector2f randomPos = ActiveGems[rnd.Next(0, ActiveGems.Count)].Position;

                foreach(var item in ActiveGems.ToList())
                {
                    totalXP = totalXP + item.XPGAIN;
                    EntityManager.Instance.RemoveEntity(item);
                }

                ActiveGems.Clear();
                MaxiGem maxiGem = new MaxiGem(randomPos, totalXP);
                EntityManager.Instance.AddEntity(maxiGem);
            }
        }


    }
}
