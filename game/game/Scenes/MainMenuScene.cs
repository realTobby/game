using game.Managers;
using game.UI;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class MainMenuScene : Scene
    {
        private UIManager uiManager;

        public MainMenuScene()
        {
            uiManager = new UIManager();
            // add UI components to the manager

            var centerPos = new Vector2f(Game.Instance.GetRenderWindow().Size.X / 2, Game.Instance.GetRenderWindow().Size.Y / 2);

            centerPos.X = centerPos.X - 64;
            centerPos.Y = centerPos.Y - 32;

            UI_Button startGameButton = new UI_Button(
                                                centerPos,
                                                string.Empty,
                                                16,
                                                128,
                                                64,
                                                new SFML.Graphics.Sprite(TextureLoader.Instance.GetTexture("startGameButton", "UI")));

            startGameButton.ClickAction += StartGame;

            uiManager.AddComponent(startGameButton);


        }

        public override void LoadContent()
        {
            // load resources
        }

        public override void Update(float deltaTime)
        {
            uiManager.Update();
        }

        public override void Draw(float deltaTime)
        {
            uiManager.Draw();
        }

        public override void UnloadContent()
        {
            // unload resources
        }

        public void StartGame()
        {
            //game.Controllers.Game.Instance.
            Game.Instance.SceneTransition(new GameScene());
        }

    }
}
