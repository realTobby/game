using game.Entities;
using game.Entities.Enemies;
using game.Entities.Pickups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class EntityManager
    {
        private static EntityManager _instacne;
        public static EntityManager Instance => _instacne;

        private GemManager gemManager;

        private List<Entity> entities;

        public EntityManager()
        {
            if (_instacne == null) _instacne = this;

            entities = new List<Entity>();
            gemManager = new GemManager();
        }

        public List<Entity> Entities => entities;

        public void AddEntity(Entity entity)
        {
            if(entity.GetType() == typeof(Gem) || entity.GetType() == typeof(MaxiGem))
            {
                gemManager.ActiveGems.Add((Gem)entity);
            }

            entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if(entity.GetType() == typeof(Gem) || entity.GetType() == typeof(MaxiGem))
            {
                gemManager.ActiveGems.Remove((Gem)entity);
            }

            entities.Remove(entity);
        }

        public void UpdateEntities(float deltaTime)
        {
            gemManager.Update();

            foreach (Entity entity in entities.ToList())
            {
                entity.Update();
            }



        }

        public void DrawEntities(float deltaTime)
        {
            foreach (Entity entity in entities.ToList())
            {
                entity.Draw(deltaTime);
            }
        }

        public void UpdateEnemyEntities(Player player, float deltaTime)
        {
            foreach (Enemy enemy in GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) }))
            {
                enemy.Update(player, deltaTime);
            }
        }



    }
}
