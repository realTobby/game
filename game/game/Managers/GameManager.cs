using game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Managers
{
    public class GameManager
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        private EntityManager entityManager;
        public WaveManager _waveManager;

        private GameManager()
        {
            entityManager = new EntityManager();
            _waveManager = new WaveManager();
        }

        public List<Entity> GetEntities()
        {
            return entityManager.Entities;
        }

        public List<Entity> GetEntities(Type[] specificEntityTypes)
        {
            return entityManager.Entities
                         .Where(x => specificEntityTypes.Contains(x.GetType()))
                         .ToList();
        }

        public void AddEntity(Entity entity)
        {
            entityManager.AddEntity(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            entityManager.RemoveEntity(entity);
        }

        public void Update(float deltaTime)
        {
            entityManager.UpdateEntities(deltaTime);
        }

        public void Draw(float deltaTime)
        {
            entityManager.DrawEntities(deltaTime);
        }
    }
}
