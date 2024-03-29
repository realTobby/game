using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Reflection.Metadata.Ecma335;

namespace sfmlgame
{
    

    public class Game
    {
        private RenderWindow _gameWindow;
        private RenderTexture renderTexture;
        private View camera;

        private Player player;
        private World world;

        public Clock GameClock = new Clock();

        public TextureLoader textureLoader;

        private Shader CRTShader;

        private Sprite grasTile;

        private SpriteSheetLoader spriteLoad;

        public Game()
        {
            textureLoader = new TextureLoader();

            spriteLoad = new SpriteSheetLoader("Assets/Sprites/spritesheet.png");

            grasTile = spriteLoad.GetSpriteFromSheet(0, 0);

            string shaderPath = "Assets/Shaders/CRT.frag";
            if (!File.Exists(shaderPath))
            {
                throw new FileNotFoundException($"The shader file was not found: {shaderPath}");
            }

            // If the file exists, try to load it
            CRTShader = new Shader(null, null, shaderPath);


            world = new World(16);

            player = new Player(textureLoader.GetTexture("priestess_0", "Entities/priestess"), new Vector2f(0, 0), world);

            var mode = VideoMode.FullscreenModes[0];
            _gameWindow = new RenderWindow(mode, "Game", Styles.Fullscreen); // Set window to fullscreen
            _gameWindow.SetFramerateLimit(60);
            _gameWindow.SetVerticalSyncEnabled(true);
            _gameWindow.Closed += StopGame;

            renderTexture = new RenderTexture(_gameWindow.Size.X, _gameWindow.Size.Y);
            renderTexture.Clear(Color.Black);

            AttachCamera(player);

            //world.GenerateAround(player.Sprite.Position, grasTile); // Now generate the world around the player

            CRTShader.SetUniform("resolution", new Vector2f(renderTexture.Size.X, renderTexture.Size.Y));

            
        }

        private void StopGame(object? sender, EventArgs e)
        {
            _gameWindow.Close();
        }

        private Vector2i lastPlayerChunkIndex;

        public void Run()
        {
            lastPlayerChunkIndex = world.CalculateChunkIndex(player.Sprite.Position);

            while (_gameWindow.IsOpen)
            {
                _gameWindow.DispatchEvents();
                Update();
                Draw();
            }
        }



        private void Update()
        {
            float deltaTime = GameClock.Restart().AsSeconds();
            player.Update(deltaTime);

            Vector2i currentPlayerChunkIndex = world.CalculateChunkIndex(player.Sprite.Position);
            if (currentPlayerChunkIndex != lastPlayerChunkIndex)
            {
                //world.Update(player.Sprite.Position, grasTile);
                lastPlayerChunkIndex = currentPlayerChunkIndex;
            }

            world.Update(player.Sprite.Position, grasTile);


            UpdateCameraPosition();
        }


        private void Draw()
        {
            // First, clear the RenderTexture and draw the game world and player onto it
            renderTexture.Clear(Color.Black);
            // Ensure the RenderTexture uses the camera's view for rendering the scene
            renderTexture.SetView(camera);
            world.Draw(renderTexture);
            renderTexture.Draw(player.Sprite);
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
            if (camera != null && player != null)
            {
                camera.Center = player.Sprite.Position;
                _gameWindow.SetView(camera);
            }
        }
    }
}
