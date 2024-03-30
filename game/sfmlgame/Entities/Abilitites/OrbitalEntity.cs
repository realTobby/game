
using game.Entities.Enemies;
using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Framework;
using static SFML.Window.Joystick;

namespace sfmlgame.Entities.Abilitites
{
    public class OrbitalEntity : AbilityEntity
    {
        private Player orbitCenterPlayer;
        private float orbitSpeed;
        private float orbitRadius;
        private float currentAngle;

        public int MaxHit = 1;
        Random rnd = new Random();

        public OrbitalEntity(Player player, Vector2f initialPosition, float orbitSpeed, float orbitRadius)
            : base("OrbitalEntity", initialPosition, GameAssets.Instance.TextureLoader.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f))
        {
            this.orbitCenterPlayer = player;
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;

            // Calculate initial angle based on the initial position
            Vector2f direction = initialPosition - player.Sprite.Position;
            this.currentAngle = MathF.Atan2(direction.Y, direction.X);

            SetPosition(initialPosition);
            Damage = 2; // Set according to your game's needs

            CanCheckCollision = true;

            MaxHit = 5;

            base.animateSpriteComponent = new AnimatedSprite(GameAssets.GetTile(TileType.Skull), initialPosition);
        }

        public void SetStats(float orbitSpeed, float orbitRadius)
        {
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;
            MaxHit = 1;
            CanCheckCollision = true;
        }

        public override void Update(Player player, float deltaTime)
        {
           // base.SetScale(Random.Shared.NextFloat(1f, 3f));

            currentAngle += orbitSpeed * deltaTime; // Update the angle to move along the orbit

            // Calculate new position
            Vector2f newPosition = new Vector2f(
                orbitCenterPlayer.Sprite.Position.X + MathF.Cos(currentAngle) * orbitRadius,
                orbitCenterPlayer.Sprite.Position.Y + MathF.Sin(currentAngle) * orbitRadius
            );

            SetPosition(newPosition);

            base.SrtHitBoxDimensions(new FloatRect(Position.X, Position.Y, HitBoxDimensions.Width, HitBoxDimensions.Height));

            if(MaxHit <= 0)
            {
                IsActive = false;
            }

            base.Update(player, deltaTime);
        }

        public override void CollidedWith(Entity collision)
        {
            // remove from MaxHit
            MaxHit--;
        }

        private Enemy CheckCollisionWithEnemy()
        {

            foreach (Enemy enemy in Game.Instance.EntityManager.Enemies.ToList().Where(x => x.IsActive))
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
