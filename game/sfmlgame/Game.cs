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


        private RenderWindow _gameWindow;
        private RenderTexture gameRenderTexture;
        private RenderTexture uiRenderTexture;
        public View CAMERA;

        public UIManager UIManager;

        public Player PLAYER;
        private WorldManager world;

        public Clock GameClock = new Clock();

        private Shader CRTShader;

        public EntityManager EntityManager;

        public WaveManager WaveManager;

        public SoundManager SoundManager;

        public bool Debug = false;

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

            world = new WorldManager(18);

            PLAYER = new Player(GameAssets.Instance.TextureLoader.GetTexture("priestess_0", "Entities/priestess"), new Vector2f(16*16, 16*16), world);

            SoundManager = new SoundManager();

            var mode = VideoMode.FullscreenModes[0];
            _gameWindow = new RenderWindow(mode, "Game", Styles.Fullscreen); // Set window to fullscreen
            _gameWindow.SetFramerateLimit(60);
            _gameWindow.SetVerticalSyncEnabled(true);
            _gameWindow.Closed += StopGame;

            gameRenderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            gameRenderTexture.Clear(Color.Black);

            uiRenderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            uiRenderTexture.Clear(Color.Transparent);

            UIManager = new UIManager();

           
            

            AttachCamera(PLAYER);

            //world.GenerateAround(player.Sprite.Position, grasTile); // Now generate the world around the player

            CRTShader.SetUniform("resolution", new Vector2f(gameRenderTexture.Size.X, gameRenderTexture.Size.Y));

            world.ManageChunks(PLAYER.Sprite.Position);

            EntityManager = new EntityManager();

            WaveManager = new WaveManager();
            
        }

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
                WaveManager.StartWave();
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

                Vector2f point = PLAYER.Sprite.Position;
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
            lastPlayerChunkIndex = world.CalculateChunkIndex(PLAYER.Sprite.Position);

            //UI_DamageNumber test = new UI_DamageNumber(-1, new Vector2f(0, 0), 1000);
            //UIManager.AddComponent(test);
            UIBinding<string> fpsBinding = new UIBinding<string>(() => GetFPS());
            UI_Text fpsCounter = new UI_Text("FPS: ", 36, new Vector2f(10, _gameWindow.Size.Y - 50), fpsBinding);
            UIManager.AddComponent(fpsCounter);

            UIBinding<string> soundChannelsBinding = new UIBinding<string>(() => SoundManager.Instance.GetActiveChannels().ToString());
            UI_Text soundChannels = new UI_Text("Sound Channels: ", 36, new Vector2f(10, _gameWindow.Size.Y - 100), soundChannelsBinding);
            UIManager.AddComponent(soundChannels);

            while (_gameWindow.IsOpen)
            {
                _gameWindow.DispatchEvents();
                Update();
                Draw();
            }
        }

        public float DELTATIME => GameClock.Restart().AsSeconds();

        private void Update()
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

            UIManager.Update(frameTime);

            PLAYER.Update(frameTime);

            Vector2i currentPlayerChunkIndex = world.CalculateChunkIndex(PLAYER.Sprite.Position);
            if (currentPlayerChunkIndex != lastPlayerChunkIndex)
            {
                //world.Update(player.Sprite.Position, grasTile);
                lastPlayerChunkIndex = currentPlayerChunkIndex;
            }

            world.Update(PLAYER.Sprite.Position);

            UpdateEnemyWave();

            EntityManager.UpdateEntities(frameTime);

            //EntityManager.UpdateEntities(PLAYER, DELTATIME); WE DO THIS IN THE BACKGROUND NOW

            UpdateCameraPosition();
        }


        private void Draw()
        {
            // First, clear the RenderTexture and draw the game world and player onto it
            gameRenderTexture.Clear(Color.Black);
            uiRenderTexture.Clear(Color.Transparent);
            // Ensure the RenderTexture uses the camera's view for rendering the scene
            gameRenderTexture.SetView(CAMERA);
            world.Draw(gameRenderTexture);
            EntityManager.DrawEntities(gameRenderTexture, DELTATIME);
            gameRenderTexture.Draw(PLAYER.Sprite);

            // draw ui

            UIManager.Draw(uiRenderTexture);
            
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
            CAMERA = new View(entity.Sprite.Position, new Vector2f(640, 480)); // Window size as view size 640x480
        }

        private void UpdateCameraPosition()
        {
            if (CAMERA != null && PLAYER != null)
            {
                CAMERA.Center = PLAYER.Sprite.Position;
                _gameWindow.SetView(CAMERA);
            }
        }

        public Vector2f ConvertWorldToViewPosition(Vector2f worldPosition)
        {
            var realPos = _gameWindow.MapCoordsToPixel(worldPosition, CAMERA);
            // This uses the game window and the current view (CAMERA) to convert world coordinates to screen coordinates.
            return new Vector2f(realPos.X, realPos.Y);
        }
    }
}
