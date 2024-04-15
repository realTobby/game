using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.Entities.Enemies;
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
        UI_Button debugSpawnWave; // entry button for debugEntitySpawn stuff
        

        public UI_DebugMenu(Vector2f position) : base(position)
        {

            var pos = new Vector2f(position.X, position.Y + 10);

            debugButtons = new UI_Group(pos, "debugButtons");
            debugButtons.IsOpen = true;
            debugButtons.HideBase = true;
            chunkyBoy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200), 999999);
            chunkyBoy.IsStatic = true;
            chunkyBoy.IsActive = false;

            backgroundShape = new RectangleShape(new Vector2f(300, 500));
            backgroundShape.Position = position;

            debugHeaderText = new UI_Text("Debug-Menu", 36, new Vector2f(position.X + 10, position.Y + 10));
            debugHeaderText.SetBold(true);

            //debugButtons.AddChild(debugHeaderText);

            debugLevelUp = new UI_Button(new Vector2f(position.X + 20, position.Y + 20), "Gain Level", 40, 280, 64, Color.Magenta);
            debugLevelUp.ClickAction = () =>
            {
                Game.Instance.PLAYER.LevelUp(1);
            };

            debugButtons.AddChild(debugLevelUp);


            debugTeleport = new UI_Button(new Vector2f(position.X + 10, position.Y + 10), "Teleport", 40, 280, 64, Color.Yellow);
            debugTeleport.ClickAction = () =>
            {
                Game.Instance.PLAYER.SetPosition(new Vector2f(0, 0));
            };
            debugButtons.AddChild(debugTeleport);

            debugSpawnWave = new UI_Button(new Vector2f(position.X + 10, position.Y + 10), "Spawn Wave", 40, 280, 64, Color.Green);
            debugSpawnWave.ClickAction = () =>
            {
                Game.Instance.WaveManager.StartWave();
            };
            debugButtons.AddChild(debugSpawnWave);

            // Initialize UI_Group without setting positions for children here
            debugEntitySpawn = new UI_Group(new Vector2f(debugSpawnWave.Position.X, debugSpawnWave.Position.Y+debugSpawnWave._height+10), "Spawn");
            
            debugButtons.AddChild(debugEntitySpawn);

            AddDebugSpawnOptions();

        }

        private void AddDebugSpawnOptions()
        {
            // Adding children without manually setting positions
            UI_Button childSpawnSimpleEnemy = new UI_Button(base.Position, "Enemy", 36, 280, 64, new Color(50, 168, 164, 255));
            childSpawnSimpleEnemy.ClickAction = () =>
            {
                var enemy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 100), 1);
            };
            debugEntitySpawn.AddChild(childSpawnSimpleEnemy);

            UI_Button childSpawnMonsterPack = new UI_Button(base.Position, "MonsterPack", 36, 280, 64, new Color(155, 89, 182, 255));
            childSpawnMonsterPack.ClickAction = () =>
            {
                MonsterFactory.SpawnMonsterPack(Game.Instance.PLAYER.Level * 2);
            };
            debugEntitySpawn.AddChild(childSpawnMonsterPack);

            UI_Button spawnChunkyBoy = new UI_Button(base.Position, "ChunkyBoy", 36, 280, 64, new Color(170, 240, 209, 255));
            spawnChunkyBoy.ClickAction = () =>
            {
                chunkyBoy.IsActive = !chunkyBoy.IsActive;
                chunkyBoy.SetPosition(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200));
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
