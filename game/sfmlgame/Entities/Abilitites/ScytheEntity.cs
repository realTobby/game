using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Abilitites;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;

namespace sfmlgame.Entities.Abilities
{
    public class ScytheEntity : AbilityEntity
    {
        private Player playerTarget;
        public bool AtPlayer = false;
        public int MaxHit = 4;
        public Clock killTimer = new Clock();
        public Entity target;
        private float rotationSpeed = 420f; // Degrees per second
        private float amplitude = 2f; // Amplitude of the sine wave
        private float frequency = 0.5f; // Frequency of the sine wave
        private float angleOffset = 0f; // For calculating sine wave offset
        private bool isReturning = false; // State of returning to player
        private const float approachThreshold = 30f; // Distance at which to start orbiting

        public bool IsParked = false;

        private float orbitRadius = 25f;
        private float currentAngle = 0;
        private float orbitSpeed = -4f;


        public ScytheEntity(Vector2f initialPosition, Enemy targetEnemy)
            : base("Scythe", initialPosition, new Sprite(GameAssets.Instance.TextureLoader.GetTexture("scythe", "Entities/Abilities")))
        {
            SetScale(0.466f);
            CanCheckCollision = true;
            SetTarget(targetEnemy);
            SetPosition(initialPosition);
            Damage = 3;
        }

        public void SetTarget(Entity enemyTarget)
        {
            if(enemyTarget != playerTarget)
            {
                IsParked = false;
            }

            target = enemyTarget;
            isReturning = false; // Reset returning state
            killTimer.Restart();
            IsActive = true;
            SoundManager.Instance.PlaySliceEffect();
        }

        public override void Update(Player player, float deltaTime)
        {
            playerTarget = player;

            if (!IsActive) return;

            base.Update(player, deltaTime);

            if(IsParked)
            {
                currentAngle += orbitSpeed * deltaTime; // Update the angle to move along the orbit
                // orbit arround player
                float sineValue = MathF.Sin(Environment.TickCount * frequency * 0.001f);
                // Calculate new position
                Vector2f newPosition = new Vector2f(
                    player.GetPosition().X + MathF.Cos(currentAngle) * orbitRadius,
                    player.GetPosition().Y + MathF.Sin(currentAngle) * orbitRadius
                );
                base.SetPosition(newPosition);
            }
            else
            {
                var direction = target.GetPosition() - GetPosition();
                var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));
                SetPosition(GetPosition() + normalizedDirection * 300f * deltaTime);
            }

            // Rotate the scythe around itself
            float rotation = rotationSpeed * deltaTime;
            SetRotation(GetRotation() + rotation);
        }

        public override void CollidedWith(Entity collision)
        {
            if (collision.GetType() == typeof(TestEnemy))
            {
                IsParked = true;
                SetTarget(playerTarget);
            }
        }
    }
}
