using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.UI
{
    

    public class UI_DebugMenu : UIComponent
    {
        RectangleShape backgroundShape;

        UI_Text debugHeaderText;

        UI_Button debugLevelUp;

        UI_Button debugTeleport;

        UI_Button debugSpawnWave;

        UI_Group debugEntitySpawn;

        public UI_DebugMenu(Vector2f position) : base(position)
        {
            backgroundShape = new RectangleShape(new Vector2f(300, 500));
            backgroundShape.Position = position;

            debugHeaderText = new UI_Text("Debug-Menu", 36, new Vector2f(position.X + 10, position.Y + 10));
            debugHeaderText.SetBold(true);

            debugLevelUp = new UI_Button(new Vector2f(position.X+10, position.Y+60), "Gain Level", 40, 280, 64, Color.Magenta);
            debugLevelUp.ClickAction = () =>
            {
                Game.Instance.PLAYER.LevelUp(1);
            };

            debugTeleport = new UI_Button(new Vector2f(position.X + 10, position.Y + 84+60), "Teleport", 40, 280, 64, Color.Yellow);
            debugTeleport.ClickAction = () =>
            {
                Game.Instance.PLAYER.SetPosition(new Vector2f(0, 0));
            };

            debugSpawnWave = new UI_Button(new Vector2f(position.X + 10, position.Y + 84 + 60 + 84), "Spawn Wave", 40, 280, 64, Color.Green);
            debugSpawnWave.ClickAction = () =>
            {
                Game.Instance.WaveManager.StartWave();
            };

            debugEntitySpawn = new UI_Group(new Vector2f(position.X+10, position.Y + 84 + 60 + 84 + 84), "Spawn");

            // create child for simple enemy spawn
            UI_Button childSpawnSimpleEnemy = new UI_Button(new Vector2f(debugEntitySpawn.Position.X + debugEntitySpawn.baseButton._width + 5, debugEntitySpawn.Position.Y), "Enemy", 36, debugEntitySpawn.baseButton._width, debugEntitySpawn.baseButton._height, new Color(50, 168, 164, 255));
            childSpawnSimpleEnemy.ClickAction = () =>
            {
                var enemy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y-100), 1);
                //debugEntitySpawn.IsOpen = false;
            };
            debugEntitySpawn.AddChild(childSpawnSimpleEnemy);

            UI_Button childSpawnMonsterPack = new UI_Button(new Vector2f(debugEntitySpawn.Position.X + debugEntitySpawn.baseButton._width + 5, debugEntitySpawn.Position.Y+64), "MonsterPack", 36, debugEntitySpawn.baseButton._width, debugEntitySpawn.baseButton._height, new Color(155, 89, 182, 255));
            childSpawnMonsterPack.ClickAction = () =>
            {
                MonsterFactory.SpawnMonsterPack(Game.Instance.PLAYER.Level*2);
                //debugEntitySpawn.IsOpen = false;s
            };

            debugEntitySpawn.AddChild(childSpawnMonsterPack);



        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!Game.Instance.Debug) return;

            renderTexture.Draw(backgroundShape);

            debugHeaderText.Draw(renderTexture);

            debugLevelUp.Draw(renderTexture);
            debugTeleport.Draw(renderTexture);

            debugSpawnWave.Draw(renderTexture);

            debugEntitySpawn.Draw(renderTexture);
        }

        public override void Update(float deltaTime)
        {
            if (!Game.Instance.Debug) return;

            debugHeaderText.Update(deltaTime);

            debugLevelUp.Update(deltaTime);
            debugTeleport.Update(deltaTime);

            debugSpawnWave.Update(deltaTime);

            debugEntitySpawn.Update(deltaTime);
        }
    }
}
