using game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
            _waveManager = new WaveManager();
            entityManager = EntityManager.Instance;
        }

        public List<Entity> GetEntities()
        {
            return EntityManager.Instance.Entities;
        }

        public List<Entity> GetEntities(Type[] baseTypes)
        {
            return EntityManager.Instance.Entities
                .Where(x => x != null && baseTypes.Any(baseType => baseType.IsAssignableFrom(x.GetType())))
                .ToList();
        }


        public bool EntityExists(Entity entity)
        {
            return EntityManager.Instance.Entities.Contains(entity);
        }


        public void Draw(float deltaTime)
        {
            EntityManager.Instance.DrawEntities(deltaTime);
        }

        public void StopGame()
        {
            entityManager.StopUpdatingEntities();
        }

        public void Dispose()
        {
            StopGame();
        }
    }
}
