using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Entities
{
    public class Particle
    {
        public Vector2f Position;
        public Vector2f Velocity;
        public float Lifetime;
    }

    public class ParticleSystem
    {
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();

        // Spawn particles with an initial position, a spread in velocity, and a given color
        public void SpawnDamageParticles(Vector2f position, int amount, float spread, float lifetime)
        {
            for (int i = 0; i < amount; i++)
            {
                float angle = (float)(random.NextDouble() * 2 * Math.PI);
                float speed = (float)(spread * random.NextDouble());
                Vector2f velocity = new Vector2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);

                particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Lifetime = (float)(lifetime * (1.0f + random.NextDouble() * 0.5f)) // Add some randomness to the lifetime
                });
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle particle = particles[i];

                // Reduce the lifetime
                particle.Lifetime -= deltaTime;

                if (particle.Lifetime <= 0)
                {
                    particles.RemoveAt(i); // Remove dead particles
                }
                else
                {
                    // Update particle position
                    particle.Position += particle.Velocity * deltaTime;
                    // You can also add here some gravity effect if you want
                }
            }
        }

        public void Draw(View cameraView)
        {

            RenderWindow window = Game.Instance.GetRenderWindow();
            foreach (var particle in particles.ToList())
            {
                // Update the position relative to the camera view
                Vector2f offsetPosition = particle.Position + cameraView.Center - cameraView.Size / 2f;

                // Convert world coordinates to view coordinates
                Vector2f viewPos = GameScene.Instance._viewCamera.ConvertWorldToViewPosition(particle.Position, cameraView);

                // Draw the particle as a small circle or a point
                CircleShape shape = new CircleShape(4) // Radius of the particle
                {
                    Position = viewPos + offsetPosition,
                    FillColor = Color.Red
                };
                window.Draw(shape);
            }
        }
    }
}
