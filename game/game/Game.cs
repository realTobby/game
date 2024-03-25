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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Game
    {
        [AllowNull]
        private static Game _instance;
        public static Game Instance => _instance;

        private RenderWindow _gameWindow;
        private SceneManager sceneManager;
        public RenderWindow GetRenderWindow() => _gameWindow;
        public Clock GameClock = new Clock();
        public TextureLoader TextureLoader = new TextureLoader();
        Music backgroundMusic = new Music("Assets/BGM/SuperHero_original.ogg");

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
            _gameWindow.Closed += (sender, e) => StopGame();

            sceneManager = new SceneManager();
            sceneManager.PushScene(new MainMenuScene());
        }

        private void StopGame()
        {
            EntityManager.Instance.StopUpdatingEntities();
            _gameWindow.Close();
        }

        private void StartBackGroundMusic()
        {
            backgroundMusic.Loop = true;
            backgroundMusic.Volume = 20;
            backgroundMusic.Play();
        }

        public float DeltaTime => GameClock.ElapsedTime.AsSeconds();

        public void Run()
        {
            //StartBackGroundMusic();

            while (_gameWindow.IsOpen)
            {
                _gameWindow.DispatchEvents();
                _gameWindow.Clear(Color.Black);

                float deltaTime = GameClock.ElapsedTime.AsSeconds();
               

                sceneManager.Update(deltaTime);
                sceneManager.Draw(deltaTime);

                GameClock.Restart();

                _gameWindow.Display();
            }
        }
    }
}
