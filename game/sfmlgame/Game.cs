using SFML.Graphics;
using SFML.Window;
using SFML.System;
using sfmlgame.Entities;
using sfmlgame.Assets;
using sfmlgame.World;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;

namespace sfmlgame
{


    public class Game
    {
        private static readonly Lazy<Game> _instance = new Lazy<Game>(() => new Game());
        public static Game Instance => _instance.Value;


        private RenderWindow _gameWindow;
        private RenderTexture renderTexture;
        private View camera;

        public Player PLAYER;
        private WorldManager world;

        public Clock GameClock = new Clock();

        private Shader CRTShader;

        public EntityManager EntityManager;

        public WaveManager WaveManager;

        public SoundManager SoundManager;

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

            renderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            renderTexture.Clear(Color.Black);

            AttachCamera(PLAYER);

            //world.GenerateAround(player.Sprite.Position, grasTile); // Now generate the world around the player

            CRTShader.SetUniform("resolution", new Vector2f(renderTexture.Size.X, renderTexture.Size.Y));

            world.ManageChunks(PLAYER.Sprite.Position);

            EntityManager = new EntityManager();

            WaveManager = new WaveManager();
            
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
            renderTexture.Clear(Color.Black);
            // Ensure the RenderTexture uses the camera's view for rendering the scene
            renderTexture.SetView(camera);
            world.Draw(renderTexture);


            EntityManager.DrawEntities(renderTexture, DELTATIME);


            renderTexture.Draw(PLAYER.Sprite);

            // draw ui


            renderTexture.Display(); // Finalize the scene on the RenderTexture

            _gameWindow.Clear();

            // Before drawing the scene with the shader, reset to the default view
            _gameWindow.SetView(_gameWindow.DefaultView);

            // Create a sprite that uses the RenderTexture's texture (the rendered scene)
            Sprite renderSprite = new Sprite(renderTexture.Texture);

            // Apply the shader while drawing this sprite, which represents the entire scene
            _gameWindow.Draw(renderSprite, new RenderStates(CRTShader));

            _gameWindow.Display();
        }

        // This method attaches the camera to an entity, e.g., the player
        public void AttachCamera(Player entity)
        {
            // Create a new view centered around the entity's position with the desired size
            camera = new View(entity.Sprite.Position, new Vector2f(640, 480)); // Window size as view size 640x480
        }

        private void UpdateCameraPosition()
        {
            if (camera != null && PLAYER != null)
            {
                camera.Center = PLAYER.Sprite.Position;
                _gameWindow.SetView(camera);
            }
        }
    }
}
