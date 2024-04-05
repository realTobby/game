using System.Collections.Concurrent;
using System.Threading.Tasks;
using game.Entities.Abilitites;
using game.Entities.Enemies;
using sfmglame.Helpers;
using SFML.System;
using sfmlgame.Abilities;
using sfmlgame.Entities;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Pickups;
using sfmlgame.Managers;

namespace sfmlgame.Entities
{
    public class EntityManager
    {
        private ConcurrentBag<Entity> allEntities = new ConcurrentBag<Entity>();

        // Removed the separate thread for updating entities. Consider using Task or ThreadPool for background operations if needed.

        public EntityManager()
        {
        }

        // Simplified properties for accessing entities
        public IEnumerable<Entity> AllEntities => allEntities;

        public IEnumerable<Enemy> Enemies => allEntities.OfType<Enemy>();

        public IEnumerable<AbilityEntity> AbilityEntities => allEntities.OfType<AbilityEntity>();

        // Simplified property for non-enemy entities
        public IEnumerable<Entity> NoEnemyEntities => allEntities.Where(x => !(x is Enemy));

        // Method to start any background tasks for updating entities, if necessary.
        // Consider using async/await with Tasks for any heavy or IO-bound operations.

        // Removed the UpdateLoop method. Updates are now handled more efficiently in the main game loop.

        public void UpdateEntities(float frameTime)
        {
            //float lastFrameDeltaTime = Game.Instance.DELTATIME;

            Parallel.ForEach(NoEnemyEntities.Where(a => (a is Pickup)), pickupEntity =>
            {
                pickupEntity.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });

            // Using parallel foreach for thread-safe iteration and potential performance improvement
            Parallel.ForEach(Enemies, enemy =>
            {
                enemy.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });

            Parallel.ForEach(AbilityEntities, abilityEntity =>
            {
                abilityEntity.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });
        }

        public void DrawEntities(SFML.Graphics.RenderTexture renderTexture, float deltaTime)
        {
            foreach (var entity in allEntities)
            {
                entity?.Draw(renderTexture, deltaTime);
            }
            //Thread.Sleep(5);
        }

        public bool EntityExists(Entity entityToCheck)
        {
            // Efficiently checks for existence without locking
            return allEntities.Contains(entityToCheck);
        }

        public Enemy CreateEnemy(Vector2f pos)
        {
            // Attempt to reuse a disabled enemy from the pool
            var freeEnemy = allEntities.FirstOrDefault(x => !x.IsActive && x is Enemy) as Enemy;

            if (freeEnemy == null)
            {
                freeEnemy = new TestEnemy(pos, 50);
                allEntities.Add(freeEnemy);
            }
            else
            {
                freeEnemy.ResetFromPool(pos);
            }

            return freeEnemy;
        }

        public Enemy FindNearestEnemy(Vector2f pos)
        {
            Enemy? nearestEnemy = null;
            float nearestDistance = float.MaxValue;
            int lowestHealth = int.MaxValue;

            foreach (Enemy enemy in Enemies)
            {
                if (enemy.HP <= lowestHealth && enemy.HP > 0) // Check if enemy has lower health and is not defeated
                {
                    float distance = CalculateDistance(pos, enemy.GetPosition());
                    if (distance < nearestDistance)
                    {
                        nearestEnemy = enemy;
                        nearestDistance = distance;
                        lowestHealth = enemy.HP;
                    }
                }
            }

            return nearestEnemy;
        }

        private float CalculateDistance(Vector2f a, Vector2f b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }


        public AbilityEntity CreateAbilityEntity(Vector2f pos, Type abilityType)
        {
            // Check for an inactive AbilityEntity of the matching type
            AbilityEntity freeAbilityEntity = AbilityEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == abilityType);

            if (freeAbilityEntity == null)
            {
                // If no inactive entities are found, create a new one based on the type
                if (abilityType == typeof(FireballEntity))
                {
                    // Assuming FireballEntity has a constructor that takes Vector2f position
                    freeAbilityEntity = new FireballEntity(pos, null); // You might need to adjust this based on your constructors
                    
                }

                if (abilityType == typeof(ThunderStrikeEntity))
                {
                    freeAbilityEntity = new ThunderStrikeEntity(pos);
                }

                if (abilityType == typeof(OrbitalEntity))
                {
                    freeAbilityEntity = new OrbitalEntity(Game.Instance.PLAYER, pos, 0, 0);
                    //freeAbilityEntity.IsActive = true;
                }

                if (freeAbilityEntity != null)
                {
                    allEntities.Add(freeAbilityEntity);
                }

            }
            else
            {
                // If an inactive entity is found, reset it for reuse
                freeAbilityEntity.ResetFromPool(pos);
            }



            // Activate the entity
            //freeAbilityEntity.IsActive = true; NEVER RETURN AN ALREADY ACTIVE ENTITY, IF THE WRONG LOGIC CATCHES THEM, THEY ARE INSTANTLY INACTIVE LOL
            return freeAbilityEntity;
        }

        public Gem CreateGem(float XP, Vector2f pos)
        {
            Gem freeGem = NoEnemyEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(Gem)) as Gem;

            if (freeGem == null)
            {
                freeGem = new Gem(pos);
                allEntities.Add(freeGem);

            }

            freeGem.ResetFromPool(pos);

            return freeGem;
        }

        public void AddMaxGemEntity(MaxiGem maxiGem)
        {
            allEntities.Add(maxiGem);
        }

        public Magnet CreateMagnet(Vector2f position)
        {
            Magnet freeMagnet = NoEnemyEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(Magnet)) as Magnet;

            if (freeMagnet == null)
            {
                freeMagnet = new Magnet(position);
                allEntities.Add(freeMagnet);

            }

            freeMagnet.ResetFromPool(position);

            return freeMagnet;
        }

        public void MagnetizeAllGems()
        {
            UniversalLog.LogInfo("magnetizing");
            foreach(var gem in NoEnemyEntities.Where(x => x.IsActive && x.GetType() == typeof(Gem)))
            {
                gem.IsMagnetized = true;
            }
        }
    }
}
