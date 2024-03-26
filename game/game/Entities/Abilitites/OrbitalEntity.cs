using game.Entities;
using game.Entities.Enemies;
using game.Helpers;
using game.Managers;
using game.Models;
using SFML.Graphics;
using SFML.System;
using System;
using System.Linq;

namespace game.Entities.Abilitites
{
    public class OrbitalEntity : AbilityEntity
    {
        private Player orbitCenterPlayer;
        private float orbitSpeed;
        private float orbitRadius;
        private float currentAngle;

        public int MaxHit = 1;
        Random rnd = new Random();
        SpriteSheetLoader texLoad = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

        public OrbitalEntity(Player player, Vector2f initialPosition, float orbitSpeed, float orbitRadius)
            : base("OrbitalEntity", initialPosition, TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f))
        {
            this.orbitCenterPlayer = player;
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;

            // Calculate initial angle based on the initial position
            Vector2f direction = initialPosition - player.Position;
            this.currentAngle = MathF.Atan2(direction.Y, direction.X);

            SetPosition(initialPosition);
            Damage = 2; // Set according to your game's needs

            CanCheckCollision = true;

            MaxHit = player.Level / 2;

            base.animateSpriteComponent = new Models.AnimatedSprite(texLoad.GetSpriteFromSheet(12, 34), initialPosition);
        }

        public void SetStats(float orbitSpeed, float orbitRadius)
        {
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;
            MaxHit = 1;
            CanCheckCollision = true;
        }

        public override void Update()
        {
            SetScale(Random.Shared.NextFloat(1f, 3f));


            float deltaTime = base.GetDeltaTime();
            currentAngle += orbitSpeed * deltaTime; // Update the angle to move along the orbit

            // Calculate new position
            Vector2f newPosition = new Vector2f(
                orbitCenterPlayer.Position.X + MathF.Cos(currentAngle) * orbitRadius,
                orbitCenterPlayer.Position.Y + MathF.Sin(currentAngle) * orbitRadius
            );

            SetPosition(newPosition);

            base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height));

            var enemy = CheckCollisionWithEnemy();
            if(enemy != null)
            {
                MaxHit--;
                enemy.AbilityCollision(this);

            }

            if(MaxHit <= 0)
            {
                IsActive = false;
            }

            base.Update();
        }

        private Enemy CheckCollisionWithEnemy()
        {

            foreach (Enemy enemy in EntityManager.Instance.Enemies.ToList().Where(x => x.IsActive))
            {
                if (CheckCollision(enemy))
                {
                    return enemy;
                }
            }

            return null;
        }

    }
}
