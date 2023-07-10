using game.Entities;
using game.Managers;
using game.Models;
using game.UI;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Scenes
{
    public class GameScene : Scene
    {
        private UIManager _uiManager;
        private OverworldManager _overworldManager;
        private ViewCamera _viewCamera;

        private Player player;
        private AnimatedSprite testThunder;
        private Enemy follower;

        public List<AnimatedSprite> animatedSprites = new List<AnimatedSprite>();

        

        public GameScene()
        {
            _uiManager = new UIManager();
            // add UI components to the manager

            _overworldManager = new OverworldManager(100);

            player = new Player();

            _viewCamera = new ViewCamera();

            //testThunder = new AnimatedSprite(TextureLoader.Instance.GetTexture("thunderStrike", "VFX"), 1, 13, Time.FromSeconds(0.1f));
            //testThunder.IsSingleShotAnimation = true;
            //animatedSprites.Add(testThunder);

            follower = new Enemy(player.Position, player.Position, 0, Time.FromSeconds(.5f));

            //AnimatedSprite explosion = new AnimatedSprite(TextureLoader.Instance.GetTexture("EXPLOSION", "VFX"), 1, 12, Time.FromSeconds(0.1f));
            //animatedSprites.Add(explosion);

            _uiManager = new UIManager();

            follower.OnSpawnThunder += SpawnThunder;

        }

        private void SpawnThunder(ThunderStrike obj)
        {
            animatedSprites.Add(obj);
        }

        public override void LoadContent()
        {
            // load resources if needed

        }

        public override void Update()
        {
            _uiManager.Update();
            player.Update();
            _viewCamera.Update(player.Position);
            follower.Update(player.Position);
            
        }

        public override void Draw()
        {
            _uiManager.Draw();
            _overworldManager.Draw();
            player.Draw();
            HandleAnimations();
        }

        public override void UnloadContent()
        {
            // unload resources
        }

        private void HandleAnimations()
        {
            foreach (var sprite in animatedSprites.ToList())
            {
                if (sprite.IsFinished)
                {
                    animatedSprites.Remove(sprite);
                }
                else
                {
                    sprite.Update();
                    sprite.Draw();
                }
            }
        }

    }
}
