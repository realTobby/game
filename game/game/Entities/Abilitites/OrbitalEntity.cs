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
            : base("Fireball", initialPosition, TextureLoader.Instance.GetTexture("burning_loop_1", "Entities/Abilities"), 1, 8, Time.FromSeconds(0.1f))
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

            base.animateSpriteComponent = new Models.AnimatedSprite(texLoad.GetSpriteFromSheet(rnd.Next(0, 69), rnd.Next(0, 47)), initialPosition);
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

            Position = newPosition;


            var enemy = CheckCollisionWithEnemy();
            if(enemy != null)
            {
                MaxHit--;

                //ThunderStrikeEntity newOnHit = new ThunderStrikeEntity(Position);
                //EntityManager.Instance.AddEntity(newOnHit);

            }

            if(MaxHit == 0)
            {

                EntityManager.Instance.RemoveEntity(this);


                // spawn thunderstrike entity here
                //ThunderStrikeEntity newOnHit = new ThunderStrikeEntity(Position);
                //newOnHit.SetScale(Random.Shared.NextFloat(1,2));
                //EntityManager.Instance.AddEntity(newOnHit);

            }

            base.Update();
        }

        private Enemy CheckCollisionWithEnemy()
        {

            foreach (Enemy enemy in GameManager.Instance.GetEntities(new Type[] { typeof(Enemy) }).Cast<Enemy>().ToList())
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
