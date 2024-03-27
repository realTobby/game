using game.Entities;
using game.Entities.Enemies;
using game.Entities.Pickups;
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

        public bool IsGamePaused = false;

        private EntityManager entityManager;

        public WaveManager _waveManager;

        public void MagnetizeGems()
        {
            foreach (Entity unknownEntity in EntityManager.Instance.noEnemyEntities.ToList())
            {
                if(unknownEntity.GetType() == typeof(Gem))
                {
                    unknownEntity.IsMagnetized = true;
                }
                
            }

            //foreach (Enemy enemy in GetEntities(new Type[1] { typeof(Enemy)}))
            //{
            //    enemy.IsMagnetized = true;
            //}
        }

        private GameManager()
        {
            _waveManager = new WaveManager();
            entityManager = EntityManager.Instance;
        }

        private static readonly object _lock = new object();

        public void Draw(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            EntityManager.Instance.DrawEntities(renderTexture, deltaTime);
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
