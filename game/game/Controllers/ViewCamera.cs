using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace game.Controllers
{
    public class ViewCamera
    {
        private RenderTexture renderTexture;

        float cameraSpeed = 150f;
        Random random = new Random();

        public View view;
        public Vector2f TargetPosition;
        public bool IsFlyToggled = false;
        private bool isShaking = false;
        private float shakeDuration = 0f;
        private float shakeIntensity = 0f;
        private float shakeTimer = 0f;
        private Vector2f originalCenter;

        public ViewCamera()
        {
            renderTexture = new RenderTexture(1000, 1000);
            

            view = new View(new FloatRect(0, 0, 800, 600));

            originalCenter = view.Center;
        }


        public void Update(Vector2f targetPos, Shader shader)
        {
            TargetPosition = targetPos;
            TrackPlayer();

            if (isShaking)
            {
                UpdateShake();
            }

            //Game.Instance.GetRenderWindow().SetView(view);

            // Clear the RenderTexture
            renderTexture.Clear(Color.Black);

            // Set the view of the RenderTexture
            renderTexture.SetView(view);

            // Draw everything to the RenderTexture instead of the window
            // sceneManager.Draw(renderTexture, deltaTime); // Example, replace with actual drawing code

            // Display to finalize the texture
            renderTexture.Display();

            // Now draw the texture to the window with the shader applied
            Sprite sceneSprite = new Sprite(renderTexture.Texture);
            shader.SetUniform("resolution", new Vector2f(Game.Instance.GetRenderWindow().Size.X, Game.Instance.GetRenderWindow().Size.Y));
            //sceneSprite.Position = new Vector2f(500, 0);
            //sceneSprite.Position = new Vector2f(sceneSprite.Position.X+renderTexture.Size.X/2, sceneSprite.Position.Y);

            

            Game.Instance.GetRenderWindow().Draw(sceneSprite, new RenderStates(shader));

            // Now, set the view back for the window
            //Game.Instance.GetRenderWindow().SetView(view);
        }

        public void ShakeCamera(float intensity, float duration)
        {
            isShaking = true;
            shakeIntensity = intensity;
            shakeDuration = duration;
            shakeTimer = 0f;
            originalCenter = view.Center;
        }

        private void TrackPlayer()
        {
            // Always update the center to follow the player
            Vector2f roundedPosition = new Vector2f((float)Math.Round(TargetPosition.X), (float)Math.Round(TargetPosition.Y));
            view.Center = roundedPosition;

            // If shaking, save the original center before applying the shake offset
            if (isShaking)
            {
                originalCenter = view.Center;
            }
        }

        private void UpdateShake()
        {
            if (shakeTimer < shakeDuration)
            {
                shakeTimer += Game.Instance.DeltaTime;

                // Calculate a random offset for the shake
                float shakeOffsetX = (float)(random.NextDouble() - 0.5) * shakeIntensity;
                float shakeOffsetY = (float)(random.NextDouble() - 0.5) * shakeIntensity;

                // Apply the shake offset to the current center position
                view.Center = originalCenter + new Vector2f(shakeOffsetX, shakeOffsetY);

                if (shakeTimer >= shakeDuration)
                {
                    // When shaking is done, we don't need to reset the view center
                    // because it's already being set in TrackPlayer each frame
                    isShaking = false;
                }
            }
        }

        public Vector2f ConvertWorldToViewPosition(Vector2f worldPosition, View view)
        {
            // Calculate the position relative to the view
            Vector2f viewCenter = view.Center;
            Vector2f viewSize = view.Size;
            Vector2f viewTopLeft = viewCenter - (viewSize / 2f);

            return worldPosition - viewTopLeft;
        }

        public FloatRect GetViewBounds()
        {
            var viewCenter = view.Center;
            var viewSize = view.Size;
            var halfSize = new Vector2f(viewSize.X / 2f, viewSize.Y / 2f);
            return new FloatRect(viewCenter - halfSize, viewSize);
        }


    }
}
