using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;
using System.Numerics;

namespace sfmlgame.Entities.Abilities
{
    public class ScytheEntity : AbilityEntity
    {
        public bool AtPlayer = false;

        public int MaxHit = 4;
        public Clock killTimer = new Clock();
        public Entity target;
        private float rotationSpeed = 420f; // Degrees per second
        private float amplitude = 2f; // Amplitude of the sine wave
        private float frequency = 0.5f; // Frequency of the sine wave
        private float angleOffset = 0f; // For calculating sine wave offset
        private bool isOrbiting = false; // State of movement
        private const float approachThreshold = 30f; // Distance at which to start orbiting

        public ScytheEntity(Vector2f initialPosition, Enemy targetEnemy)
            : base("Scythe", initialPosition, new Sprite(GameAssets.Instance.TextureLoader.GetTexture("scythe", "Entities/Abilities")))
        {
            SetScale(0.66f);
            CanCheckCollision = true;
            SetTarget(targetEnemy);
            SetPosition(initialPosition);
            Damage = 3;
        }

        public void SetTarget(Entity enemyTarget)
        {
            target = enemyTarget;
            isOrbiting = false; // Reset orbiting state
            killTimer.Restart();
            IsActive = true;
            SoundManager.Instance.PlaySliceEffect();
        }

        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            base.Update(player, deltaTime);


            


            // Ensure there is always a target
            if (target == null || !target.IsActive)
            {

                SetTarget(Game.Instance.EntityManager.FindNearestEnemy(GetPosition()));

                if(target == null) SetTarget(player); // Set player as fallback target


            }
            else
            {
                if(target.GetType() == typeof(TestEnemy))
                {
                    AtPlayer = false;
                }
                
            }

            

            var direction = target.GetPosition() - GetPosition();
            var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));

            if (distance <= approachThreshold)
            {
                isOrbiting = true;
            }
            else
            {
                isOrbiting = false; // Reset to direct movement if distance is too great
            }

            if (isOrbiting)
            {
                // Sine wave offset for orbiting
                angleOffset += deltaTime * frequency;
                float sineOffset = (float)Math.Sin(angleOffset * Math.PI * 2) * amplitude;
                Vector2f orbitDirection = new Vector2f(-normalizedDirection.Y, normalizedDirection.X);
                SetPosition(target.GetPosition() + orbitDirection * sineOffset);
            }
            else
            {
                // Move directly towards the target
                SetPosition(GetPosition() + normalizedDirection * 250f * deltaTime);
            }

            // Rotate the scythe around itself
            float rotation = rotationSpeed * deltaTime;
            SetRotation(GetRotation() + rotation);
        }


        public override void CollidedWith(Entity collision)
        {
            if(collision.GetType() == typeof(TestEnemy))
            {
                
                SetTarget(Game.Instance.PLAYER);
            }

            if(collision.GetType() == typeof(Player))
            {
                AtPlayer = true;
            }
        }

    }


}
