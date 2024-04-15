using SFML.Graphics;
using SFML.Window;
using SFML.System;
using sfmlgame.Entities;
using sfmlgame.Assets;
using sfmlgame.World;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;
using sfmlgame.UI;

namespace sfmlgame
{


    public class Game
    {
        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());
        public static Game Instance => _instance.Value;

        private Clock shaderClock = new Clock();

        private RenderWindow _gameWindow;
        private RenderTexture gameRenderTexture;
        private RenderTexture uiRenderTexture;

        public RenderWindow GetWindow()
        {
            return _gameWindow;
        }

        public View CAMERA;

        public UIManager UIManager;

        public Player PLAYER;
        private WorldManager world;

        public Clock GameClock = new Clock();

        private Shader CRTShader;

        public EntityManager EntityManager;

        public WaveManager WaveManager;

        public UI_PowerupMenu MainPowerUpMenu;

        public bool Debug = false;

        public bool GamePaused = false;

        private float shakeDuration = 0f;
        private float shakeIntensity = 2f;
        private Vector2f originalCameraPosition;

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

        public Game()
        {

            LoadCRTShader();

            EntityManager = new EntityManager();

            world = new WorldManager(18);

            

            var mode = VideoMode.FullscreenModes[0];
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



            shaderClock.Restart();



            //world.GenerateAround(player.Sprite.Position, grasTile); // Now generate the world around the player

            CRTShader.SetUniform("resolution", new Vector2f(gameRenderTexture.Size.X, gameRenderTexture.Size.Y));
            CRTShader.SetUniform("overallBrightness", 0.9f);

            _gameWindow.SetMouseCursorVisible(false);

            Texture cursorTexture = GameAssets.Instance.TextureLoader.GetTexture("handIcon", "Sprites");
            cursorSprite = new Sprite(cursorTexture);

            Texture cursorTextureClicked = GameAssets.Instance.TextureLoader.GetTexture("handIconClicked", "Sprites");
            clickedCursorSprite = new Sprite(cursorTextureClicked);

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

        private void UpdateEnemyWave()
        {
            if (waveTimer.ElapsedTime.AsSeconds() > waveCooldown)
            {
                waveTimer.Restart();
                GenerateNewWave();
                //WaveManager.StartWave();
                waveCooldown = 10f;
            }
        }

        private void GenerateNewWave()
        {
            //UniversalLog.LogInfo("trying to generate new wave...");
            if (EntityManager.Enemies.Where(x => x.IsActive).Count() < 50)
            {
                //UniversalLog.LogInfo("can create new wave!");
                EnemyWave wave = new EnemyWave(1f, 25f);

                Vector2f point = PLAYER.GetPosition();
                float radius = 300f;

                // TEST: wave.AddSpawnPosition(new Vector2f(100, 100));

                for (int i = 0; i < 10; i++)
                {
                    var radians = 2 * MathF.PI / 10 * i;
                    var vertical = MathF.Sin(radians);
                    var horizontal = MathF.Cos(radians);

                    var spawnDir = new Vector2f(horizontal, vertical);
                    var spawnPos = point + spawnDir * radius;
                    wave.AddSpawnPosition(new Vector2f(spawnPos.X, spawnPos.Y));
                }
                WaveManager.AddWave(wave);
            }
        }


        private void StopGame(object? sender, EventArgs e)
        {
            _gameWindow.Close();
        }

        private Vector2i lastPlayerChunkIndex;

        public void Run()
        {
           
            //UI_DamageNumber test = new UI_DamageNumber(-1, new Vector2f(0, 0), 1000);
            //UIManager.AddComponent(test);
            UIBinding<string> fpsBinding = new UIBinding<string>(() => GetFPS());
            UI_Text fpsCounter = new UI_Text("FPS: ", 36, new Vector2f(10, _gameWindow.Size.Y - 50), fpsBinding);
            UIManager.AddComponent(fpsCounter);

            UIBinding<string> soundChannelsBinding = new UIBinding<string>(() => SoundManager.Instance.GetActiveChannels().ToString());
            UI_Text soundChannels = new UI_Text("Sound Channels: ", 36, new Vector2f(10, _gameWindow.Size.Y - 100), soundChannelsBinding);
            UIManager.AddComponent(soundChannels);

            UIBinding<string> entityCountBinding = new UIBinding<string>(() => EntityManager.AllEntities.Count().ToString());
            UI_Text entityCountText = new UI_Text("Entity Count: ", 36, new Vector2f(10, _gameWindow.Size.Y - 150), entityCountBinding);
            UIManager.AddComponent(entityCountText);

            PLAYER = new Player(GameAssets.Instance.TextureLoader.GetTexture("priestess_0", "Entities/priestess"), new Vector2f(16 * 16, 16 * 16), world);
            lastPlayerChunkIndex = world.CalculateChunkIndex(PLAYER.GetPosition());
            AttachCamera(PLAYER);

            MainPowerUpMenu = new UI_PowerupMenu((Vector2f)_gameWindow.Size/2, 800, 800);
            UIManager.AddComponent(MainPowerUpMenu);

            var debugMenu = new UI_DebugMenu(new Vector2f(10,10));
            UIManager.AddComponent(debugMenu);

            world.ManageChunks(PLAYER.GetPosition());

            

            WaveManager = new WaveManager();

            PLAYER.LevelUp(1);

            

            while (_gameWindow.IsOpen)
            {
                _gameWindow.DispatchEvents();
                
                Update();
                Draw();
            }

            

        }

        public float DELTATIME => GameClock.Restart().AsSeconds();

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

            UIManager.Update(frameTime);

            Vector2i mousePosition = Mouse.GetPosition(_gameWindow); // Get the mouse position relative to the window
            cursorSprite.Position = new Vector2f(mousePosition.X, mousePosition.Y);

            clickedCursorSprite.Position = new Vector2f(mousePosition.X, mousePosition.Y);

            // change sprite to clicked if click

            //CRTShader.SetUniform("crtSpeed", crtTime); // This will be a continuously increasing value

            if (GamePaused)
            {
                return;
            }

            PLAYER.Update(frameTime);

            Vector2i currentPlayerChunkIndex = world.CalculateChunkIndex(PLAYER.GetPosition());
            if (currentPlayerChunkIndex != lastPlayerChunkIndex)
            {
                //world.Update(player.Sprite.Position, grasTile);
                lastPlayerChunkIndex = currentPlayerChunkIndex;
            }

            world.Update(PLAYER.GetPosition());

            UpdateEnemyWave();

            EntityManager.UpdateEntities(frameTime);

            //EntityManager.UpdateEntities(PLAYER, DELTATIME); WE DO THIS IN THE BACKGROUND NOW

            UpdateCameraPosition(frameTime);

            

            

        }


        private void Draw()
        {
            float frameTime = DELTATIME;

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

            // First, clear the RenderTexture and draw the game world and player onto it
            gameRenderTexture.Clear(Color.Black);
            uiRenderTexture.Clear(Color.Transparent);
            // Ensure the RenderTexture uses the camera's view for rendering the scene
            gameRenderTexture.SetView(CAMERA);
            world.Draw(gameRenderTexture);
            EntityManager.DrawEntities(gameRenderTexture, frameTime);


            PLAYER.Draw(gameRenderTexture, frameTime);

            // draw ui

            UIManager.Draw(uiRenderTexture);

            if(Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                uiRenderTexture.Draw(clickedCursorSprite);
            }
            else
            {
                uiRenderTexture.Draw(cursorSprite);
            }

            gameRenderTexture.Display(); // Finalize the scene on the RenderTexture
            uiRenderTexture.Display();
            _gameWindow.Clear();

            // Before drawing the scene with the shader, reset to the default view
            _gameWindow.SetView(_gameWindow.DefaultView);

            // Create a sprite that uses the RenderTexture's texture (the rendered scene)
            Sprite gameRenderSprite = new Sprite(gameRenderTexture.Texture);
            Sprite uiRenderSprite = new Sprite(uiRenderTexture.Texture);

            // Apply the shader while drawing this sprite, which represents the entire scene
            _gameWindow.Draw(gameRenderSprite, new RenderStates(CRTShader));
            _gameWindow.Draw(uiRenderSprite, new RenderStates(CRTShader));

            _gameWindow.Display();
        }

        // This method attaches the camera to an entity, e.g., the player
        public void AttachCamera(Player entity)
        {
            // Create a new view centered around the entity's position with the desired size
            CAMERA = new View(entity.GetPosition(), new Vector2f(640, 480)); // Window size as view size 640x480
        }

        private void UpdateCameraPosition(float deltaTime)
        {
            if (CAMERA != null && PLAYER != null)
            {
                // Update the original camera position to follow the player.
                // This ensures that the camera follows the player's current position before applying the shake offset.
                originalCameraPosition = PLAYER.GetPosition();

                if (shakeDuration > 0)
                {
                    // Generate random offsets within the shake intensity range.
                    float offsetX = (float)(Random.Shared.NextDouble() * 2 - 1) * shakeIntensity;
                    float offsetY = (float)(Random.Shared.NextDouble() * 2 - 1) * shakeIntensity;
                    // Apply the shake by offsetting the original camera position (which follows the player).
                    CAMERA.Center = new Vector2f(originalCameraPosition.X + offsetX, originalCameraPosition.Y + offsetY);

                    shakeDuration -= deltaTime; // Decrease the shake duration.
                }
                else
                {
                    // If not shaking, simply follow the player.
                    CAMERA.Center = originalCameraPosition;
                }

                // Apply the updated camera position to the game window.
                _gameWindow.SetView(CAMERA);
            }
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
