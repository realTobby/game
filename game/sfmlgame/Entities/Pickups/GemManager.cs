
using SFML.System;


namespace sfmlgame.Entities.Pickups
{
    public class GemManager
    {
        private Random rnd = new Random();

        public List<Gem> ActiveGems = new List<Gem>();

        public void Update()
        {
            if (ActiveGems.Count > 200)
            {
                int totalXP = 0;

                Vector2f randomPos = ActiveGems[rnd.Next(0, ActiveGems.Count)].GetPosition();

                foreach (var item in ActiveGems.ToList())
                {
                    totalXP = totalXP + item.XPGAIN;
                    item.IsActive = false;
                    //EntityManager.Instance.RemoveEntity(item);
                }

                ActiveGems.Clear();
                MaxiGem maxiGem = new MaxiGem(randomPos, totalXP);
                Game.Instance.EntityManager.AddMaxGemEntity(maxiGem);
            }
        }



    }
}
