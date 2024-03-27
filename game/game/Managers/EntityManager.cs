using game.Entities.Enemies;
using game.Entities.Pickups;
using game.Entities;
using game.Scenes;
using SFML.System;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq;
using game.Entities.Abilitites;
using game.Abilities;

namespace game.Managers
{
    public class EntityManager
    {
        private static readonly Lazy<EntityManager> _instance = new Lazy<EntityManager>(() => new EntityManager());
        private static readonly object _lock = new object();

        private GemManager gemManager;

        private Lazy<List<Entity>> allEntities = new Lazy<List<Entity>>(() => new List<Entity>());

        private Thread updateThread;
        private bool isUpdating;

        public static EntityManager Instance => _instance.Value;

        public int GetEnemyPoolSize => AllEntities.Where(x => x.GetType() == typeof(TestEnemy)).Count();
        public int GetDisbaledEnemyCount => AllEntities.Where(x => x.GetType() == typeof(TestEnemy) && x.IsActive == false).Count();

        private EntityManager()
        {
            gemManager = new GemManager();
            updateThread = new Thread(UpdateLoop) { IsBackground = true };
        }

        public IEnumerable<Entity> AllEntities
        {
            get
            {
                lock (_lock)
                {
                    return allEntities.Value.ToList();
                }
            }
        }

        public List<Enemy> Enemies
        {
            get
            {
                lock (_lock)
                {
                    // Use LINQ to filter and cast entities to Enemy.
                    return AllEntities.ToList().OfType<Enemy>().ToList();
                }
            }
        }

        public List<Entity> noEnemyEntities
        {
            get
            {
                lock (_lock)
                {
                    return AllEntities.ToList().OfType<Entity>().Where(x => !(x is Enemy)).ToList();
                }
            }
        }

        public List<AbilityEntity> abilityEntities
        {
            get
            {
                lock (_lock)
                {
                    return AllEntities.ToList().OfType<AbilityEntity>().ToList();
                }
            }
        }

        public List<Gem> gemEntities
        {
            get
            {
                lock (_lock)
                {
                    return AllEntities.ToList().OfType<Gem>().ToList();
                }
            }
        }


        public void StartUpdatingEntities()
        {
            lock (_lock)
            {
                if (!isUpdating)
                {
                    isUpdating = true;
                    updateThread.Start();
                }
            }
        }

        public void StopUpdatingEntities()
        {
            lock (_lock)
            {
                if (isUpdating)
                {
                    isUpdating = false;
                    updateThread.Join();
                }
            }
        }

        
        private void UpdateLoop()
        {
            var clock = new Clock();

            while (isUpdating)
            {
                lock (_lock)
                {
                    var deltaTime = clock.Restart().AsSeconds();

                    gemManager.Update();

                    foreach (var entity in noEnemyEntities)
                    {
                        entity?.Update();
                    }

                    foreach (Enemy enemy in Enemies)
                    {
                        enemy.Update();
                    }
                }

                Thread.Sleep(5);
            }
        }

        public void DrawEntities(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            lock (_lock)
            {
                foreach (var entity in AllEntities)
                {
                    entity?.Draw(renderTexture, deltaTime);
                }
            }
        }

        public bool EntityExists(Entity entityToCheck)
        {
            lock (_lock)
            {
                return AllEntities.Contains(entityToCheck);
            }
        }

        public Enemy CreateEnemy(Vector2f pos)
        {
            // get free enemy from pool
            var freeEnemy = AllEntities.Where(x => x.IsActive == false && x.GetType() == typeof(Enemy)).FirstOrDefault() as Enemy;
            
            if(freeEnemy == null)
            {
                freeEnemy = new TestEnemy(pos, 25);
                lock(_lock)
                {
                    allEntities.Value.Add(freeEnemy);
                }
                
            }
            else
            {
                freeEnemy.ResetFromPool(pos);
            }

            return freeEnemy;
        }

        public AbilityEntity CreateAbilityEntity(Vector2f pos, Type abilityType)
        {
            // Check for an inactive AbilityEntity of the matching type
            AbilityEntity freeAbilityEntity = AllEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == abilityType) as AbilityEntity;

            if (freeAbilityEntity == null)
            {
                // If no inactive entities are found, create a new one based on the type
                if (abilityType == typeof(FireballEntity))
                {
                    // Assuming FireballEntity has a constructor that takes Vector2f position
                    freeAbilityEntity = new FireballEntity(pos, null); // You might need to adjust this based on your constructors
                }
                
                if(abilityType == typeof(ThunderStrikeEntity))
                {
                    freeAbilityEntity = new ThunderStrikeEntity(pos);
                }

                if(abilityType == typeof(OrbitalEntity))
                {
                    freeAbilityEntity = new OrbitalEntity(GameScene.Instance.player, pos, 0, 0);
                    //freeAbilityEntity.IsActive = true;
                }

                if(freeAbilityEntity != null)
                {
                    lock(_lock)
                    {
                        allEntities.Value.Add(freeAbilityEntity);
                    }
                    
                }

            }
            else
            {
                // If an inactive entity is found, reset it for reuse
                freeAbilityEntity.ResetFromPool(pos);
            }

            

            // Activate the entity
            //freeAbilityEntity.IsActive = true;
            return freeAbilityEntity;
        }

        public Gem CreateGem(float XP, Vector2f pos)
        {
            Gem freeGem = noEnemyEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(Gem)) as Gem;

            if(freeGem == null)
            {
                freeGem = new Gem(pos);
                lock(_lock)
                {
                    allEntities.Value.Add(freeGem);
                }
                
            }
           
            freeGem.ResetFromPool(pos);

            return freeGem;
        }

        public void AddMaxGemEntity(MaxiGem maxiGem)
        {
            lock(_lock)
            {
                allEntities.Value.Add(maxiGem);
            }
            
        }
    }
}
