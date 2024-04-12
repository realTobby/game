using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Entities.Particles
{
    public class DamageParticle : Entity
    {
        private Vector2f velocity;
        private float lifespan = 1.0f; // lifespan of 1 second
        private float originalLifespan;
        private Vector2f creationVelocity;

        public DamageParticle(Texture particleTexture, Vector2f initialPosition, Vector2f velocity)
            : base(particleTexture, 1, 1, Time.FromSeconds(1), initialPosition)
        {
            creationVelocity = velocity;
            originalLifespan = lifespan; // Store the original lifespan for scaling calculations

            float randomRotation = Random.Shared.NextFloat(0, 360);
            SetRotation(randomRotation);

            SetPosition(initialPosition);
            this.velocity = velocity;
            IsActive = true;
        }

        public override void ResetFromPool(Vector2f position)
        {
            SetPosition(position);
            this.velocity = creationVelocity;
            lifespan = originalLifespan; // Reset lifespan when reusing
        }

        public override void Update(Player player, float deltaTime)
        {
            if (!IsActive) return;

            // Update particle position
            var pos = GetPosition();
            SetPosition(pos += velocity * deltaTime);

            // Update lifespan and check for deactivation
            lifespan -= deltaTime;
            if (lifespan <= 0)
            {
                IsActive = false; // Deactivate the particle after its lifespan is over
                return;
            }

            // Decelerate the particle by 10% per frame
            velocity *= 0.9f;

            // Scale down the particle based on its remaining lifespan
            float scale = lifespan / originalLifespan; // Calculates the current scale as a fraction of the remaining life
            SetScale(scale); // Apply the calculated scale to both axes

            base.Update(player, deltaTime);
        }
    }
}
