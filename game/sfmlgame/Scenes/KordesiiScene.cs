using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Managers;
using sfmlgame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Scenes
{
    public class KordesiiScene : Scene
    {

        private UI_Text kordesiiTitle;

        private float animationTime = 0f;
        private Vector2f startPosition;
        private Vector2f endPosition;
        private bool transitionStarted = false;

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {

        }

        public override void LoadContent()
        {
            var windowSize = new Vector2f(1920, 1080);
            var centerPos = windowSize / 2;

            kordesiiTitle = new UI_Text("kordesii", 300, centerPos);
            kordesiiTitle.SetBold(true);
            kordesiiTitle.SetColor(Color.Black, Color.White, 3.5f);
            // Correctly center the title based on its actual width
            kordesiiTitle.SetPosition(new Vector2f(centerPos.X - kordesiiTitle.Width / 2, centerPos.Y - kordesiiTitle.Height)); // Re-adjust X to be truly centered

            Game.Instance.UIManager.AddComponent(kordesiiTitle);

            startPosition = new Vector2f(centerPos.X, 0); // Start from top of the screen
            endPosition = new Vector2f(centerPos.X - kordesiiTitle.Width / 2, centerPos.Y - kordesiiTitle.Height / 2);

            kordesiiTitle.SetPosition(startPosition);
            kordesiiTitle.Opacity = 0; // Start transparent
        }

        public override void UnloadContent()
        {
            Game.Instance.UIManager.RemoveComponent(kordesiiTitle);
        }

        public override void Update(float deltaTime)
        {
            animationTime += deltaTime;

            float moveDuration = 3.0f;
            float jiggleDuration = 1.0f;
            float fadeOutDuration = 1.0f;
            float totalDuration = moveDuration + jiggleDuration + fadeOutDuration;

            if (animationTime <= moveDuration)
            {
                // Fade in while moving down
                kordesiiTitle.Opacity = (animationTime / moveDuration) * 255;
                float progress = animationTime / moveDuration;
                kordesiiTitle.SetPosition(RandomExtensions.Lerp(startPosition, endPosition, progress));
                kordesiiTitle.Scale = 1.0f + progress * 0.5f; // Grow while moving
            }
            else if (animationTime <= moveDuration + jiggleDuration)
            {
                // Jiggle effect
                float jiggleProgress = (animationTime - moveDuration) / jiggleDuration;
                float jiggleScale = 1.5f - Math.Abs(0.5f - jiggleProgress) * 1.0f; // Bouncy effect
                kordesiiTitle.Scale = jiggleScale;
            }
            else if (animationTime <= totalDuration)
            {
                // Fade out
                float fadeProgress = (animationTime - moveDuration - jiggleDuration) / fadeOutDuration;
                kordesiiTitle.Opacity = 255 - (fadeProgress * 255);
            }

            if (animationTime > totalDuration && !transitionStarted)
            {
                transitionStarted = true;
                Game.Instance.SceneTransition(new MainMenuScene());
            }
        }


    }
}
