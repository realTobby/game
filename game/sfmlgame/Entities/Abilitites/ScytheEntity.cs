
using sfmglame.Helpers;
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
        public int MaxHit = 4;
        public Clock killTimer = new Clock();
        public Entity target;
        private float rotationSpeed = 360f; // Degrees per second
        private float amplitude = 15f; // Amplitude of the sine wave
        private float frequency = 0.1f; // Slower frequency for the sine wave
        private float angleOffset = 0f; // For calculating sine wave offset

        public ScytheEntity(Vector2f initialPosition, Enemy targetEnemy)
            : base("Scythe", initialPosition, new Sprite(GameAssets.Instance.TextureLoader.GetTexture("scythe", "Entities/Abilities")))
        {
            SetScale(0.7f);
            CanCheckCollision = true;
            SetTarget(targetEnemy);
            SetPosition(initialPosition);
            Damage = 3; // Assuming scythe is more powerful
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            if (!IsActive) return;
            base.Draw(renderTexture, deltaTime);
        }

        public void SetTarget(Entity enemyTarget)
        {
            target = enemyTarget;
            killTimer.Restart();
            IsActive = true;
            SoundManager.Instance.PlaySliceEffect();
        }

        public override void ResetFromPool(Vector2f position)
        {
            base.ResetFromPool(position);
            killTimer.Restart();
            SetScale(0.7f);
            MaxHit = 4;
        }

        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            base.Update(player, deltaTime);

            if (target != null)
            {
                var direction = target.GetPosition() - GetPosition();
                var distance = Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
                var normalizedDirection = new Vector2f((float)(direction.X / distance), (float)(direction.Y / distance));

                // Sine wave offset
                angleOffset += deltaTime * frequency;
                float sineOffset = (float)Math.Sin(angleOffset * Math.PI * 2) * amplitude;

                // Rotate around itself
                float rotation = rotationSpeed * deltaTime;
                SetRotation(GetRotation() + rotation);

                // Update position with sine wave
                Vector2f sineWaveOffset = new Vector2f(-normalizedDirection.Y, normalizedDirection.X) * sineOffset;
                SetPosition(GetPosition() + normalizedDirection * 6f + sineWaveOffset); // Move towards the target

                if (target == player)
                {
                    SetTarget(Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition()));

                    if (target == null)
                    {
                        SetTarget(player);
                    }
                }

                if (target.IsActive == false)
                {
                    SetTarget(Game.Instance.EntityManager.FindNearestEnemy(player.GetPosition()));


                    if(target == null)
                    {
                        SetTarget(player);
                    }

                }
            }
        }

        public override void CollidedWith(Entity collision)
        {
            if(collision.GetType() == typeof(TestEnemy))
            {
                //UniversalLog.LogInfo("Current Scale: " + GetScale());
                // randomize scale
                SetScale(GetScale() - 0.05f);
                base.animateSpriteComponent.FlipSprite(Random.Shared.NextBool());
                if(GetScale() <= 0f)
                {
                    IsActive = false;
                }
            }

            

        }
    }
}
