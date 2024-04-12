using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities.Enemies;

namespace sfmlgame.Entities.Abilitites
{
    public class OrbitalEntity : AbilityEntity
    {
        private Player orbitCenterPlayer;
        private float orbitSpeed;
        private float baseOrbitRadius; // The base radius of the orbit
        private float orbitRadius; // The actual orbit radius that will change
        private float currentAngle;

        // New variables for sine-wave pattern
        private float radiusAmplitude = 20f; // The maximum change in orbit radius
        private float radiusFrequency = 2f; // How fast the orbit radius changes

        public int MaxHit = 1;
        Random rnd = new Random();

        public OrbitalEntity(Player player, Vector2f initialPosition, float orbitSpeed, float orbitRadius)
            : base("OrbitalEntity", initialPosition, new Sprite(GameAssets.Instance.GetTileSprite(TileType.Skull)))
        {
            this.orbitCenterPlayer = player;
            this.orbitSpeed = orbitSpeed;
            this.baseOrbitRadius = orbitRadius;
            this.orbitRadius = orbitRadius;

            Vector2f direction = initialPosition - player.GetPosition();
            this.currentAngle = MathF.Atan2(direction.Y, direction.X);

            Damage = 1; // Set according to your game's needs
            MaxHit = 5;
            CanCheckCollision = true;
            SetPosition(initialPosition);
        }

        public void SetStats(float orbitSpeed, float orbitRadius)
        {
            this.orbitSpeed = orbitSpeed;
            this.baseOrbitRadius = orbitRadius;
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

            currentAngle += orbitSpeed * deltaTime; // Update the angle to move along the orbit

            // Modulate the orbit radius in a sine-wave pattern
            float sineValue = MathF.Sin(Environment.TickCount * radiusFrequency * 0.001f);
            orbitRadius = baseOrbitRadius + (sineValue * radiusAmplitude);

            // Calculate new position
            Vector2f newPosition = new Vector2f(
                player.GetPosition().X + MathF.Cos(currentAngle) * orbitRadius,
                player.GetPosition().Y + MathF.Sin(currentAngle) * orbitRadius
            );
           base. SetPosition(newPosition);

            // Oscillate the scale based on the sine value
            float scale = 1 + MathF.Abs(sineValue); // Oscillates the scale value between 1 and 2
            base.SetScale(scale); // Adjust the scale of the entity

            // Rotate the entity around itself at the same speed as the orbit speed
            float rotationAngle = currentAngle * (180 / MathF.PI) * 2; // Convert radians to degrees
            base.SetRotation(rotationAngle); // Assuming SetRotation takes the rotation angle in degrees

            base.Update(player, deltaTime);
        }


        public override void CollidedWith(Entity collision)
        {
            if (collision.GetType().IsSubclassOf(typeof(Enemy)))
            {
                //if(collision.CanCheckCollision) MaxHit--;
                //UniversalLog.LogInfo("orbital entry");

            }
        }
    }
}
