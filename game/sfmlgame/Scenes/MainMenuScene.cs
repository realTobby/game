using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.UI;

namespace sfmlgame.Scenes
{
    public class MainMenuScene : Scene
    {
        UI_Button startGameButton;
        Sprite backgroundSprite = new Sprite(GameAssets.Instance.TextureLoader.GetTexture("background", "TitleScreen"));
        UI_Text gameTitle;

        UI_Text gameVersion;

        private float gameTitleHue = 0f;  // Initial hue value

        public MainMenuScene()
        {


        }

        public override void LoadContent()
        {
            backgroundSprite.Position = new Vector2f(0, 0);

            var windowSize = new Vector2f(1920, 1080);
            var centerPos = windowSize / 2;

            

            // Setting up the game title at the top center of the screen
            var titlePos = new Vector2f(centerPos.X, 25); // Y position is arbitrary here for top margin
            gameTitle = new UI_Text("Twilight Requiem", 128, titlePos);
            gameTitle.SetBold(true);
            gameTitle.SetColor(Color.Cyan);
            // Correctly center the title based on its actual width
            gameTitle.SetPosition(new Vector2f(centerPos.X-gameTitle.Width/2, 25)); // Re-adjust X to be truly centered

            Game.Instance.UIManager.AddComponent(gameTitle);

            // Setup the start game button
            startGameButton = new UI_Button(
                new Vector2f(centerPos.X - 64, centerPos.Y - 32), // Minor adjustment from center, may adjust as needed
                "Start Game",
                26,
                128,
                64,
                RandomExtensions.GenerateRandomPastelColor());
            startGameButton.ClickAction += StartGame;
            Game.Instance.UIManager.AddComponent(startGameButton);

            gameVersion = new UI_Text("v" + Game.GameVersion, 40, new Vector2f(0,0));
            gameVersion.SetBold(true);
            gameVersion.SetColor(Color.Red);
            gameVersion.SetPosition(new Vector2f(windowSize.X-gameVersion.Width-10, windowSize.Y-gameVersion.Height-10));

            Game.Instance.UIManager.AddComponent(gameVersion);
        }

        public override void UnloadContent()
        {
            Game.Instance.UIManager.RemoveComponent(gameTitle);
            Game.Instance.UIManager.RemoveComponent(startGameButton);
            Game.Instance.UIManager.RemoveComponent(gameVersion);
        }

        public void StartGame()
        {
            Game.Instance.SceneTransition(new GameScene());
        }

        public override void Update(float deltaTime)
        {
            // Change the hue value at a constant rate
            gameTitleHue += deltaTime * 0.2f;  // Adjust speed as needed
            if (gameTitleHue > 1f) gameTitleHue -= 1f;  // Wrap hue around if it exceeds 1

            // Convert the current hue to an RGB color with full saturation and lightness
            Color rainbowColor = RandomExtensions.HSLToRGB(gameTitleHue, 1.0f, 0.5f);
            gameTitle.SetColor(rainbowColor);
        }

        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            renderTexture.SetView(renderTexture.DefaultView);  // Reset to default view
            renderTexture.Draw(backgroundSprite);
        }
    }
}
