using game.Entities;
using game.Entities.Abilitites;
using game.Helpers;
using game.Managers;
using game.Scenes;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace game.Abilities
{
    public class OrbitalAbility : Ability
    {
        private Player player;
        private float circleSpeed;
        private float circleRadius;
        private int entityCount; // Number of entities to spawn

        private bool IsCurrentlyActive = false;
        private List<OrbitalEntity> orbitals;

        public OrbitalAbility(Player player, float cooldown, float circleSpeed, float circleRadius, int entityCount)
            : base("OrbitalAbility", 0, cooldown) // Assuming the ability itself does no direct damage
        {
            this.player = player;
            this.circleSpeed = circleSpeed;
            this.circleRadius = circleRadius;
            this.entityCount = entityCount;

            orbitals = new List<OrbitalEntity>();

        }

        public override void Activate()
        {
            if(!IsCurrentlyActive)
            {
               
                entityCount = player.Level * 2;

                float angleIncrement = 360f / entityCount; // Divide the circle into equal parts based on entity count
                for (int i = 0; i < entityCount; i++)
                {
                    float angle = angleIncrement * i * (MathF.PI / 180);
                    Vector2f spawnPosition = new Vector2f(
                        player.Position.X + MathF.Cos(angle) * circleRadius,
                        player.Position.Y + MathF.Sin(angle) * circleRadius
                    );
                    OrbitalEntity orbitalEntity = new OrbitalEntity(player, spawnPosition, circleSpeed, circleRadius);

                    orbitalEntity.SetScale(Random.Shared.NextFloat(1f, 3f));
                    
                    EntityManager.Instance.AddEntity(orbitalEntity);

                    orbitals.Add(orbitalEntity);
                }


                IsCurrentlyActive = true;
                abilityClock.Restart();
            }

            
        }

        public override void Update()
        {
            if (IsCurrentlyActive)
            {
                // Remove any orbital entities that have been destroyed or are no longer active
                orbitals.RemoveAll(orbital => !EntityManager.Instance.EntityExists(orbital));

                // If there are no more orbitals, set the ability to inactive
                if (orbitals.Count == 0)
                {
                    IsCurrentlyActive = false;
                }
            }
        }
    }
}



