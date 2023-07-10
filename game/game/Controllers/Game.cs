using game.Entities;
using game.Managers;
using game.Models;
using game.Scenes;
using game.UI;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Controllers
{
    public class Game
    {
        private static Game _instance;
        public static Game Instance => _instance;

        private RenderWindow _gameWindow;

        private SceneManager sceneManager;

        public RenderWindow GetRenderWindow() => _gameWindow;

        public Clock GameClock = new Clock();

        //public float GetDeltaTime() => GameClock.ElapsedTime.AsSeconds();

        public TextureLoader TextureLoader = new TextureLoader();

        public void SceneTransition(Scene nextScene)
        {
            sceneManager.PushScene(nextScene);
        }

        public Game()
        {
            if (_instance == null) _instance = this;

            var mode = new VideoMode(800, 600);
            _gameWindow = new RenderWindow(mode, "Game");

            _gameWindow.SetFramerateLimit(60);

            _gameWindow.Closed += (sender, e) => _gameWindow.Close();

            sceneManager = new SceneManager();

            sceneManager.PushScene(new MainMenuScene());
            //sceneManager.PushScene(new GameScene());
        }

        public void Run()
        {
            // Load the music
            Music backgroundMusic = new Music("Assets/BGM/Venus.wav");

            // Set the music to loop
            backgroundMusic.Loop = true;
            backgroundMusic.Volume = 35;
            backgroundMusic.Play();

            while (_gameWindow.IsOpen)
            {
                // Process events
                _gameWindow.DispatchEvents();

                // Clear screen
                _gameWindow.Clear(SFML.Graphics.Color.Black);

                float deltaTime = GameClock.ElapsedTime.AsSeconds();

                Console.WriteLine("Delta Time: " + deltaTime + " seconds");

                GameClock.Restart();

                sceneManager.Update(deltaTime);
                sceneManager.Draw();

                GameClock.Restart();

                // Update the window
                _gameWindow.Display();
            }
        }
    }
}
