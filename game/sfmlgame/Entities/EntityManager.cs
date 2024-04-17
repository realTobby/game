using System.Collections.Concurrent;
using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Abilities;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Enemies;
using sfmlgame.Entities.NPCs;
using sfmlgame.Entities.Particles;
using sfmlgame.Entities.Pickups;

namespace sfmlgame.Entities
{
    public class EntityManager
    {
        private ConcurrentBag<Entity> allEntities = new ConcurrentBag<Entity>();

        // Removed the separate thread for updating entities. Consider using Task or ThreadPool for background operations if needed.

        public EntityManager()
        {
        }

        public void Clear()
        {
            allEntities.Clear();
        }

        // Simplified properties for accessing entities
        public IEnumerable<Entity> AllEntities => allEntities;

        public IEnumerable<Enemy> Enemies => allEntities.OfType<Enemy>();

        public IEnumerable<DamageParticle> Particles => allEntities.OfType<DamageParticle>();

        public IEnumerable<AbilityEntity> AbilityEntities => allEntities.OfType<AbilityEntity>();

        // Simplified property for non-enemy entities
        public IEnumerable<Pickup> Pickups => allEntities.OfType<Pickup>();

        public IEnumerable<NPC> NPCs => allEntities.OfType<NPC>();

        //public IEnumerable<ChunkTrapTrigger> TRAPS => allEntities.OfType<ChunkTrapTrigger>();

        // Method to start any background tasks for updating entities, if necessary.
        // Consider using async/await with Tasks for any heavy or IO-bound operations.

        // Removed the UpdateLoop method. Updates are now handled more efficiently in the main game loop.

        public void UpdateEntities(float frameTime)
        {
            //float lastFrameDeltaTime = Game.Instance.DELTATIME;

            Parallel.ForEach(Pickups, pickup =>
            {
                pickup.Update(Game.Instance.PLAYER, frameTime);
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

            Parallel.ForEach(Particles, particle =>
            {
                particle.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });

            Parallel.ForEach(NPCs, npc =>
            {
                npc.Update(Game.Instance.PLAYER, frameTime);
                // Implement other entity-specific updates as needed
            });

            //Parallel.ForEach(TRAPS, trap =>
            //{
            //    trap.Update(Game.Instance.PLAYER, frameTime);
            //    // Implement other entity-specific updates as needed
            //});
        }

        //public void AddEntity(Entity entity)
        //{
        //    allEntities.Add(entity);
        //}

        public void DrawEntities(SFML.Graphics.RenderTexture renderTexture, float frameTime)
        {

            foreach(var pickup in Pickups)
            {
                pickup.Draw(renderTexture, frameTime);
            }

            foreach(var enemy in Enemies)
            {
                enemy.Draw(renderTexture, frameTime);
            }

            foreach(var ability in AbilityEntities)
            {
                ability.Draw(renderTexture, frameTime);
            }

            foreach(var particle in Particles)
            {
                particle.Draw(renderTexture, frameTime);
            }

            foreach (var npc in NPCs)
            {
                npc.Draw(renderTexture, frameTime);
            }

            //foreach (var trap in TRAPS)
            //{
            //    trap.Draw(renderTexture, frameTime);
            //}

            //Thread.Sleep(5);
        }

        public bool EntityExists(Entity entityToCheck)
        {
            // Efficiently checks for existence without locking
            return allEntities.Contains(entityToCheck);
        }

        public Enemy CreateEnemy(Vector2f pos, int HP)
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

            freeEnemy.SetHP(HP);

            return freeEnemy;
        }

        public DamageParticle CreateDamageParticle(Vector2f pos)
        {
            var freeParticle = Particles.FirstOrDefault(x => !x.IsActive && x is DamageParticle) as DamageParticle;

            Vector2f particleVelocity = new Vector2f(Random.Shared.NextFloat(-1f, 1f), Random.Shared.NextFloat(-1f, 1f));
            particleVelocity = (particleVelocity) * Random.Shared.NextFloat(-500, 500f); // Random speed

            if (freeParticle == null)
            {
                freeParticle = new DamageParticle(GameAssets.Instance.TextureLoader.GetTexture("damageParticle", "Entities/Particles"), pos, particleVelocity);
                allEntities.Add(freeParticle);
            }
            else
            {
                freeParticle.ResetFromPool(pos);
                freeParticle.IsActive = true;
            }

            return freeParticle;

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

        public BombEntity CreateBombEntity(Vector2f spawnPos, Vector2f targetPos)
        {
            BombEntity freeBombEntity = AbilityEntities.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(BombEntity)) as BombEntity;

            if (freeBombEntity == null)
            {
                freeBombEntity = new BombEntity("BOMB", spawnPos, targetPos);

                allEntities.Add(freeBombEntity);
               // Game.Instance.PLAYER.SetPosition(targetPos);
            }
            else
            {
                freeBombEntity.ResetFromPool(spawnPos);
            }

            

            return freeBombEntity;
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

                
                if(abilityType == typeof(ScytheEntity))
                {
                    freeAbilityEntity = new ScytheEntity(Game.Instance.PLAYER.GetPosition(), null);
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
            Gem freeGem = Pickups.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(Gem)) as Gem;

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
            Magnet freeMagnet = Pickups.FirstOrDefault(x => !x.IsActive && x.GetType() == typeof(Magnet)) as Magnet;

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
            //UniversalLog.LogInfo("magnetizing");
            foreach(var gem in Pickups.Where(x => x.IsActive && x.GetType() == typeof(Gem)))
            {
                gem.IsMagnetized = true;
            }
        }

        //public ChunkTrapTrigger CreateTrapTrigger(Vector2f pos)
        //{
        //    ChunkTrapTrigger trap = TRAPS.FirstOrDefault(x => !x.IsActive);
        //    if (trap == null)
        //    {
        //        // Assuming a default sprite for traps; customize as needed
        //        int width = Random.Shared.Next(75, 250);
        //        int height = Random.Shared.Next(75, 250);

        //        var perfectSizeButtonSprite = new RenderTexture((uint)width, (uint)height);
        //        perfectSizeButtonSprite.Clear(SFML.Graphics.Color.Transparent); // Optional, if you want transparency
        //        perfectSizeButtonSprite.Display(); // Finish rendering

        //        // Create a new texture from the RenderTexture
        //        var _buttonTexture = new Texture(perfectSizeButtonSprite.Texture);
        //        var _buttonSprite = new Sprite(_buttonTexture);

        //        trap = new ChunkTrapTrigger(pos, _buttonSprite);
        //        allEntities.Add(trap);
        //    }
        //    else
        //    {
        //        trap.ResetFromPool(pos);
        //    }

        //    return trap;
        //}

        public NPC CreateNPC(Vector2f pos)
        {
            NPC freeNPC = NPCs.FirstOrDefault(x => !x.IsActive);
            if(freeNPC == null)
            {
                freeNPC = new NPC(pos);
                allEntities.Add(freeNPC);
            }
            else
            {
                freeNPC.ResetFromPool(pos);
            }
            return freeNPC;
        }

    }
}
