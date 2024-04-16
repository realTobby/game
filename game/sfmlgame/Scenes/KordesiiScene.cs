using SFML.Graphics;
using SFML.System;
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

        public float WaitTime = 5f;

        public float CurrentWait = 0f;

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {

        }

        public override void LoadContent()
        {
            var windowSize = new Vector2f(1920, 1080);
            var centerPos = windowSize / 2;

            kordesiiTitle = new UI_Text("kordesii", 128, centerPos);
            kordesiiTitle.SetBold(true);
            kordesiiTitle.SetColor(Color.White);
            // Correctly center the title based on its actual width
            kordesiiTitle.SetPosition(new Vector2f(centerPos.X - kordesiiTitle.Width / 2, centerPos.Y-kordesiiTitle.Height/2)); // Re-adjust X to be truly centered

            Game.Instance.UIManager.AddComponent(kordesiiTitle);

        }

        public override void UnloadContent()
        {
            Game.Instance.UIManager.RemoveComponent(kordesiiTitle);
        }

        public override void Update(float deltaTime)
        {
            CurrentWait += deltaTime;

            if(CurrentWait > WaitTime)
            {
                Game.Instance.SceneTransition(new MainMenuScene());
            }
        }
    }
}
