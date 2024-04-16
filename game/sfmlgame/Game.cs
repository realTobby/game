using SFML.Graphics;
using SFML.Window;
using SFML.System;
using sfmlgame.Entities;
using sfmlgame.Assets;
using sfmlgame.World;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;
using sfmlgame.UI;
using sfmlgame.Scenes;
using Microsoft.VisualBasic;
using System.Reflection;
using sfmglame.Helpers;
using System.ComponentModel;

namespace sfmlgame
{
    public class Game
    {
		private const int screen_w = 1920;
		private const int screen_h = 1080;
		private const int screen_b = 32;

        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());
        public static Game Instance => _instance.Value;

        public Player PLAYER;
        public Vector2i lastPlayerChunkIndex;


        public static string GameVersion = "0.1.1";

        private Clock shaderClock = new Clock();

        private RenderWindow _gameWindow;
        private RenderTexture gameRenderTexture;
        private RenderTexture uiRenderTexture;

        public RenderWindow GetWindow()
        {
            return _gameWindow;
        }

        public Clock GameClock = new Clock();
        public UIManager UIManager;
        private Shader CRTShader;
        public bool Debug = false;
        public bool GamePaused = false;

        

        private SceneManager SceneManager;

        
        

        public EntityManager EntityManager;
        public WaveManager WaveManager;
        public WorldManager World;

        private void StopGame()
        {
            _gameWindow.Close();
        }


        private void LoadCRTShader()
        {
            string shaderPath = "Assets/Shaders/CRT.frag";
            if (!File.Exists(shaderPath))
            {
                throw new FileNotFoundException($"The shader file was not found: {shaderPath}");
            }

            // If the file exists, try to load it
            CRTShader = new Shader(null, null, shaderPath);
        }

        public void SceneTransition(Scene nextScene)
        {
            SceneManager.PopScene();

            SceneManager.PushScene(nextScene);
        }


        public Game()
        {

            LoadCRTShader();

            SceneManager = new SceneManager();

			int i=0;

			VideoMode mode = VideoMode.FullscreenModes[0];

			foreach(var related_mode in VideoMode.FullscreenModes)
			{
				UniversalLog.LogInfo("Mode[" + $"{i} " + "] " + $"{related_mode}");

				if(related_mode.BitsPerPixel == screen_b && related_mode.Width == screen_w && related_mode.Height == screen_h)
				{
					mode = related_mode;
				}

				i++;
			}

            mode = VideoMode.FullscreenModes[4];

			UniversalLog.LogInfo("");
			UniversalLog.LogInfo("Current Fullscreen Mode is ... ");
			UniversalLog.LogInfo($"{mode}");

            // [Vector2u] X(867) Y(1001)
            _gameWindow = new RenderWindow(mode, "Game", Styles.Fullscreen); // Set window to fullscreen
            _gameWindow.SetFramerateLimit(60);
            _gameWindow.SetVerticalSyncEnabled(true);

            _gameWindow.Closed += StopGame;

            gameRenderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            gameRenderTexture.Clear(Color.Black);

            uiRenderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            uiRenderTexture.Clear(Color.Transparent);

            UIManager = new UIManager();



            World = new WorldManager(18);

            shaderClock.Restart();

            //world.GenerateAround(player.Sprite.Position, grasTile); // Now generate the world around the player

            CRTShader.SetUniform("resolution", new Vector2f(gameRenderTexture.Size.X, gameRenderTexture.Size.Y));
            CRTShader.SetUniform("overallBrightness", 0.9f);

            _gameWindow.SetMouseCursorVisible(false);

            Texture cursorTexture = GameAssets.Instance.TextureLoader.GetTexture("handIcon", "Sprites");
            cursorSprite = new Sprite(cursorTexture);

            Texture cursorTextureClicked = GameAssets.Instance.TextureLoader.GetTexture("handIconClicked", "Sprites");
            clickedCursorSprite = new Sprite(cursorTextureClicked);

            EntityManager = new EntityManager();

            PLAYER = new Player(GameAssets.Instance.TextureLoader.GetTexture("priestess_0", "Entities/priestess"), new Vector2f(16 * 16, 16 * 16), World);
            lastPlayerChunkIndex = World.CalculateChunkIndex(PLAYER.GetPosition());



            

        }

        Sprite cursorSprite;

        Sprite clickedCursorSprite;


        private Clock fpsClock = new Clock(); // A clock to keep track of time between frames
        private int frameCount = 0; // A counter for the frames
        private float lastTime = 0; // The last time FPS was calculated

        private int LastCalculatedFPS = 0;

        private string GetFPS()
        {
            return LastCalculatedFPS.ToString();
        }

        private Clock waveTimer = new Clock();
        private float waveCooldown = 10f;

        private void StopGame(object? sender, EventArgs e)
        {
            // UNLOAD SCENES

            SceneManager.Clear();



            _gameWindow.Close();
        }

        

        public void Run()
        {
            //UI_DamageNumber test = new UI_DamageNumber(-1, new Vector2f(0, 0), 1000);
            //UIManager.AddComponent(test);
            UIBinding<string> fpsBinding = new UIBinding<string>(() => GetFPS());
            UI_Text fpsCounter = new UI_Text("FPS: ", 36, new Vector2f(10, _gameWindow.Size.Y - 50), fpsBinding);
            fpsCounter.SetColor(Color.White);
            UIManager.AddComponent(fpsCounter);

            CAMERA = new View();

            SceneTransition(new KordesiiScene());

            while (_gameWindow.IsOpen)
            {
                _gameWindow.DispatchEvents();
                _gameWindow.Clear(Color.Black);

                Update();
                Draw();

                _gameWindow.Display();
                _gameWindow.DispatchEvents();
                
                
            }

            

        }

        public float DELTATIME => GameClock.Restart().AsSeconds();

        public View CAMERA;

        public float shakeDuration = 0f;
        public float shakeIntensity = 2f;
        public Vector2f originalCameraPosition;

        public void ShakeCamera(float duration)
        {
            originalCameraPosition = CAMERA.Center;
            shakeDuration = duration;
        }

        private void Update()
        {
            float frameTime = DELTATIME;
            float crtTime = shaderClock.ElapsedTime.AsSeconds();
            frameCount++;
            float currentTime = fpsClock.ElapsedTime.AsSeconds();
            float deltaTime = currentTime - lastTime;

            // Calculate FPS every second
            if (deltaTime >= 1.0f)
            {
                int fps = frameCount;
                frameCount = 0;
                lastTime += deltaTime;
                LastCalculatedFPS = fps;
            }



            Vector2i mousePosition = Mouse.GetPosition(_gameWindow); // Get the mouse position relative to the window
            cursorSprite.Position = new Vector2f(mousePosition.X, mousePosition.Y);

            clickedCursorSprite.Position = new Vector2f(mousePosition.X, mousePosition.Y);

            // change sprite to clicked if click

            //CRTShader.SetUniform("crtSpeed", crtTime); // This will be a continuously increasing value

            UIManager.Update(frameTime);

            if (GamePaused)
            {
                return;
            }

            SceneManager.Update(frameTime);

            EntityManager.UpdateEntities(frameTime);

            

        }


        private void Draw()
        {
            //_gameWindow.Clear();

            float frameTime = DELTATIME;

            frameCount++;
            float currentTime = fpsClock.ElapsedTime.AsSeconds();
            float deltaTime = currentTime - lastTime;

            //// Calculate FPS every second
            if (deltaTime >= 1.0f)
            {
                int fps = frameCount;
                frameCount = 0;
                lastTime += deltaTime;
                LastCalculatedFPS = fps;
            }

            //// First, clear the RenderTexture and draw the game world and player onto it
            gameRenderTexture.Clear(Color.Black);
            uiRenderTexture.Clear(Color.Transparent);
            //// Ensure the RenderTexture uses the camera's view for rendering the scene

            SceneManager.Draw(gameRenderTexture, frameTime);

            EntityManager.DrawEntities(gameRenderTexture, frameTime);

            //// draw ui

            UIManager.Draw(uiRenderTexture);

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                uiRenderTexture.Draw(clickedCursorSprite);
            }
            else
            {
                uiRenderTexture.Draw(cursorSprite);
            }

            gameRenderTexture.Display(); // Finalize the scene on the RenderTexture
            uiRenderTexture.Display();
            

            //// Before drawing the scene with the shader, reset to the default view
            _gameWindow.SetView(_gameWindow.DefaultView);

            //// Create a sprite that uses the RenderTexture's texture (the rendered scene)
            Sprite gameRenderSprite = new Sprite(gameRenderTexture.Texture);
            Sprite uiRenderSprite = new Sprite(uiRenderTexture.Texture);

            

            //// Apply the shader while drawing this sprite, which represents the entire scene
            _gameWindow.Draw(gameRenderSprite, new RenderStates(CRTShader));
            _gameWindow.Draw(uiRenderSprite, new RenderStates(CRTShader));

            //_gameWindow.Display();
        }

        

        

        public Vector2f ConvertWorldToViewPosition(Vector2f worldPosition)
        {
            var realPos = _gameWindow.MapCoordsToPixel(worldPosition, CAMERA);
            // This uses the game window and the current view (CAMERA) to convert world coordinates to screen coordinates.
            return new Vector2f(realPos.X, realPos.Y);
        }

        public Vector2f ConvertViewToWorldPosition(Vector2i viewPosition)
        {
            var realPos = _gameWindow.MapPixelToCoords(viewPosition);
            // This uses the game window and the current view (CAMERA) to convert world coordinates to screen coordinates.
            return new Vector2f(realPos.X, realPos.Y);
        }
    }
}
