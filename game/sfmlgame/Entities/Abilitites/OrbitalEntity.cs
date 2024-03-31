
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
            : base("OrbitalEntity", initialPosition, new Sprite(GameAssets.Instance.GetTileSprite(TileType.Skull)))
        {
            this.orbitCenterPlayer = player;
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;

            // Calculate initial angle based on the initial position
            Vector2f direction = initialPosition - player.Sprite.Position;
            this.currentAngle = MathF.Atan2(direction.Y, direction.X);

           
            Damage = 1; // Set according to your game's needs

            CanCheckCollision = true;

            MaxHit = 5;

            //base.animateSpriteComponent = new AnimatedSprite(GameAssets.GetTile(TileType.Skull), initialPosition);

            SetPosition(initialPosition);
        }

        public void SetStats(float orbitSpeed, float orbitRadius)
        {
            this.orbitSpeed = orbitSpeed;
            this.orbitRadius = orbitRadius;
            MaxHit = 5;
            CanCheckCollision = true;
        }

        public override void Update(Player player, float deltaTime)
        {
            if (MaxHit <= 0)
            {
                IsActive = false;
            }

            // base.SetScale(Random.Shared.NextFloat(1f, 3f));

            currentAngle += orbitSpeed * deltaTime; // Update the angle to move along the orbit

            // Calculate new position
            Vector2f newPosition = new Vector2f(
                player.Sprite.Position.X + MathF.Cos(currentAngle) * orbitRadius,
                player.Sprite.Position.Y + MathF.Sin(currentAngle) * orbitRadius
            );

            SetPosition(newPosition);

            //base.SrtHitBoxDimensions(new FloatRect(GetPosition().X, GetPosition().Y, HitBoxDimensions.Width, HitBoxDimensions.Height));

            

            base.Update(player, deltaTime);
        }

        public override void CollidedWith(Entity collision)
        {
            if (collision.GetType().IsSubclassOf(typeof(Enemy)))
            {
                UniversalLog.LogInfo("orbital entry");
                MaxHit--;
            }
            
        }

    }
}
