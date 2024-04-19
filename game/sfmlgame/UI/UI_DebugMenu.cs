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

        UI_List debugButtons;
        UI_Button debugLevelUp;
        UI_Button debugTeleport;

        UI_List debugEntitySpawnButtonList;

        public UI_DebugMenu(Vector2f position) : base(position)
        {
            Width = 400;
            Height = 550;

            var pos = new Vector2f(position.X, position.Y + 10);

            debugButtons = new UI_List(new Vector2f(GetCenterX, GetCenterY-Height/2));
            chunkyBoy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200), 999999);
            chunkyBoy.IsStatic = true;
            chunkyBoy.IsActive = false;

            backgroundShape = new RectangleShape(new Vector2f(Width, Height));
            backgroundShape.FillColor = Color.Black;
            backgroundShape.Position = position;

            var gameTitle = new UI_Text("Twilight Requiem", 40, position);
            gameTitle.SetBold(true);
            debugButtons.AddChild(gameTitle, new Vector2f(0,25));

            debugHeaderText = new UI_Text("Debug-Menu", 24, position);
            debugHeaderText.SetBold(true);
            debugButtons.AddChild(debugHeaderText);

            UIBinding<string> soundChannelsBinding = new UIBinding<string>(() => SoundManager.Instance.GetActiveChannels().ToString());
            UI_Text soundChannels = new UI_Text("Sound Channels: ", 36, new Vector2f(10, 10), soundChannelsBinding);
            soundChannels.SetColor(Color.Black, Color.White, 3.5f);
            debugButtons.AddChild(soundChannels);

            UIBinding<string> entityCountBinding = new UIBinding<string>(() => Game.Instance.EntityManager.AllEntities.Count().ToString());
            UI_Text entityCountText = new UI_Text("Entity Count: ", 36, new Vector2f(10, 10), entityCountBinding);
            entityCountText.SetColor(Color.Black, Color.White, 3.5f);
            debugButtons.AddChild(entityCountText);

            debugLevelUp = new UI_Button(position, "Gain Level", 40, 280, 64, Color.Magenta);
            debugLevelUp.ClickAction = () =>
            {
                Game.Instance.PLAYER.LevelUp(1);
            };

            debugButtons.AddChild(debugLevelUp);

            debugTeleport = new UI_Button(position, "Teleport", 40, 280, 64, Color.Yellow);
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

            debugEntitySpawnButtonList = new UI_List(new Vector2f(0,0));
            debugEntitySpawnButtonList.Hide = true;
            UI_Button debugEntitySpawnButton = new UI_Button(position, "Entities", 40, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            debugButtons.AddChild(debugEntitySpawnButton);
            // Position the spawn button list to the right of the Entities button
            float margin = 10f; // Margin between the buttons
            Vector2f debugEntitySpawnButtonListPosition = new Vector2f(
                debugEntitySpawnButton.Position.X + debugEntitySpawnButton.Width + margin,
                debugEntitySpawnButton.Position.Y
            );
            debugEntitySpawnButtonList.SetPosition(debugEntitySpawnButtonListPosition);
            debugEntitySpawnButton.ClickAction = () =>
            {
                debugEntitySpawnButtonList.Hide = !debugEntitySpawnButtonList.Hide;
            };
            
            
            AddDebugSpawnOptions(debugEntitySpawnButtonList);
             
            //var debugNPCGroup = new UI_List(new Vector2f(debugEntitySpawnButtonList.Position.X+debugEntitySpawnButtonList., debugEntitySpawnButtonList.Position.Y + debugEntitySpawnButtonList.baseButton.Height + 10), "NPC", true);

            //var placeholderNPC1 = new UI_Button(pos, "Placeholder NPC1", 40, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            //placeholderNPC1.ClickAction = () =>
            //{
            //    Game.Instance.EntityManager.CreateNPC(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 100));
            //    debugNPCGroup.IsOpen = false;
            //};

            //debugNPCGroup.AddChild(placeholderNPC1);

            //debugButtons.AddChild(debugNPCGroup, new Vector2f(25, 25));



        }

        private void InitSpawnOptions()
        {
            // Initialize UI_Group without setting positions for children here
            
        }

        private void AddDebugSpawnOptions(UI_List list)
        {
            // Adding children without manually setting positions
            UI_Button childSpawnSimpleEnemy = new UI_Button(base.Position, "Enemy", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            childSpawnSimpleEnemy.ClickAction = () =>
            {
                var enemy = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 100), 1);
                list.Hide = true;
            };
            list.AddChild(childSpawnSimpleEnemy);

            UI_Button childSpawnMonsterPack = new UI_Button(base.Position, "MonsterPack", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            childSpawnMonsterPack.ClickAction = () =>
            {
                MonsterFactory.SpawnMonsterPack(Game.Instance.PLAYER.Level * 2);
                list.Hide = true;
            };
            list.AddChild(childSpawnMonsterPack);

            UI_Button spawnChunkyBoy = new UI_Button(base.Position, "ChunkyBoy", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            spawnChunkyBoy.ClickAction = () =>
            {
                var newChunky = Game.Instance.EntityManager.CreateEnemy(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200), 5000);
                newChunky.IsStatic = true;
                list.Hide = true;
                //chunkyBoy.IsActive = !chunkyBoy.IsActive;
                //chunkyBoy.SetPosition(new Vector2f(Game.Instance.PLAYER.GetPosition().X, Game.Instance.PLAYER.GetPosition().Y - 200));
            };
            list.AddChild(spawnChunkyBoy);

            UI_Button spawnParticleEffect = new UI_Button(base.Position, "Particle Effect", 36, 280, 64, RandomExtensions.GenerateRandomPastelColor());
            spawnParticleEffect.ClickAction = () =>
            {
                Game.Instance.EntityManager.CreateDamageParticle(Game.Instance.PLAYER.GetPosition());
            };
            list.AddChild(spawnParticleEffect);

        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!Game.Instance.Debug) return;

            renderTexture.Draw(backgroundShape);
            debugButtons.Draw(renderTexture);

            if (debugEntitySpawnButtonList.Hide) return;

            debugEntitySpawnButtonList.Draw(renderTexture);

        }

        public override void Update(float deltaTime)
        {
            if (!Game.Instance.Debug) return;

            debugButtons.Update(deltaTime);

            debugEntitySpawnButtonList.Update(deltaTime);
        }
    }
}
