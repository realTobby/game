using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Enemies;
using sfmlgame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.UI
{
    public class UI_DebugMenu : UIComponent
    {
        Enemy chunkyBoy;
        RectangleShape backgroundShape;

        UI_Text debugHeaderText;

        UI_Group debugButtons;
        UI_Button debugLevelUp;
        UI_Button debugTeleport;


        UI_Group debugEntitySpawn;
        

        public UI_DebugMenu(Vector2f position) : base(position)
        {

            var pos = new Vector2f(position.X, position.Y + 10);

            debugButtons = new UI_Group(pos, "debugButtons");
            debugButtons.IsOpen = true;
            debugButtons.HideBase = true;
            chunkyBoy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200), 999999);
            chunkyBoy.IsStatic = true;
            chunkyBoy.IsActive = false;

            backgroundShape = new RectangleShape(new Vector2f(300, 550));
            backgroundShape.Position = position;

            var gameTitle = new UI_Text("Twilight Requiem", 40, new Vector2f(position.X + 10, position.Y + 10));
            gameTitle.SetBold(true);
            debugButtons.AddChild(gameTitle);

            debugHeaderText = new UI_Text("Debug-Menu", 24, new Vector2f(position.X + 10, position.Y + 10));
            debugHeaderText.SetBold(true);
            debugButtons.AddChild(debugHeaderText);

            UIBinding<string> soundChannelsBinding = new UIBinding<string>(() => SoundManager.Instance.GetActiveChannels().ToString());
            UI_Text soundChannels = new UI_Text("Sound Channels: ", 36, new Vector2f(10, 10), soundChannelsBinding);
            debugButtons.AddChild(soundChannels);

            UIBinding<string> entityCountBinding = new UIBinding<string>(() => Game.Instance.EntityManager.AllEntities.Count().ToString());
            UI_Text entityCountText = new UI_Text("Entity Count: ", 36, new Vector2f(10, 10), entityCountBinding);
            debugButtons.AddChild(entityCountText);

            debugLevelUp = new UI_Button(new Vector2f(position.X + 20, position.Y + 20), "Gain Level", 40, 280, 64, Color.Magenta);
            debugLevelUp.ClickAction = () =>
            {
                Game.Instance.PLAYER.LevelUp(1);
            };

            debugButtons.AddChild(debugLevelUp);

            debugTeleport = new UI_Button(new Vector2f(position.X + 20, position.Y + 20), "Teleport", 40, 280, 64, Color.Yellow);
            debugTeleport.ClickAction = () =>
            {
                Game.Instance.PLAYER.SetPosition(new Vector2f(0, 0));
            };
            debugButtons.AddChild(debugTeleport);

            //debugSpawnWave = new UI_Button(new Vector2f(position.X + 10, position.Y + 10), "Spawn Wave", 40, 280, 64, Color.Green);
            //debugSpawnWave.ClickAction = () =>
            //{
            //    Game.Instance.WaveManager.StartWave();
            //};
            //debugButtons.AddChild(debugSpawnWave);

            debugEntitySpawn = new UI_Group(new Vector2f(debugTeleport.Position.X, debugTeleport.Position.Y + debugTeleport.Height + 10), "Monster");
            AddDebugSpawnOptions();
            debugButtons.AddChild(debugEntitySpawn);

            var debugNPCGroup = new UI_Group(new Vector2f(debugEntitySpawn.Position.X, debugEntitySpawn.Position.Y + debugEntitySpawn.baseButton.Height + 10), "NPC");
            
            var placeholderNPC1 = new UI_Button(pos, "Placeholder NPC1", 40, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            placeholderNPC1.ClickAction = () =>
            {
                Game.Instance.PLAYER.LevelUp(1);
                debugNPCGroup.IsOpen = false;
            };

            debugNPCGroup.AddChild(placeholderNPC1);

            debugButtons.AddChild(debugNPCGroup);


            
        }

        private void InitSpawnOptions()
        {
            // Initialize UI_Group without setting positions for children here
            
        }

        private void AddDebugSpawnOptions()
        {
            // Adding children without manually setting positions
            UI_Button childSpawnSimpleEnemy = new UI_Button(base.Position, "Enemy", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            childSpawnSimpleEnemy.ClickAction = () =>
            {
                var enemy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 100), 1);
                debugEntitySpawn.IsOpen = false;
            };
                debugEntitySpawn.AddChild(childSpawnSimpleEnemy);

            UI_Button childSpawnMonsterPack = new UI_Button(base.Position, "MonsterPack", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            childSpawnMonsterPack.ClickAction = () =>
            {
                MonsterFactory.SpawnMonsterPack(Game.Instance.PLAYER.Level * 2);
                debugEntitySpawn.IsOpen = false;
            };
            debugEntitySpawn.AddChild(childSpawnMonsterPack);

            UI_Button spawnChunkyBoy = new UI_Button(base.Position, "ChunkyBoy", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            spawnChunkyBoy.ClickAction = () =>
            {
                var newChunky = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200), 5000);
                newChunky.IsStatic = true;
                debugEntitySpawn.IsOpen = false;
                //chunkyBoy.IsActive = !chunkyBoy.IsActive;
                //chunkyBoy.SetPosition(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200));
            };
            debugEntitySpawn.AddChild(spawnChunkyBoy);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!Game.Instance.Debug) return;

            renderTexture.Draw(backgroundShape);
            debugButtons.Draw(renderTexture);
        }

        public override void Update(float deltaTime)
        {
            if (!Game.Instance.Debug) return;

            debugButtons.Update(deltaTime);
        }
    }
}
