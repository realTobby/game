using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
using sfmlgame.Entities;
using sfmlgame.UI;
using sfmlgame.World;
using System.Numerics;


namespace sfmlgame.Scenes
{
    public class GameScene : Scene
    {

        public UI_PlayerInfo PlayerInfo;
        public UI_PowerupMenu MainPowerUpMenu;

        public UI_DebugMenu debugMenu;

        public Music backgroundMusic = new Music("Assets/BGM/SuperHero_original.ogg");


        public override void Draw(RenderTexture renderTexture, float deltaTime)
        {
            renderTexture.SetView(Game.Instance.CAMERA);

            Game.Instance.World.Draw(renderTexture);
            Game.Instance.PLAYER.Draw(renderTexture, deltaTime);
        }

        public override void LoadContent()
        {
            backgroundMusic.Loop = true;
            //backgroundMusic.Play();

            //Game.Instance.World.ManageChunks(new Vector2i(0,0));

            AttachCamera(Game.Instance.PLAYER);

            MainPowerUpMenu = new UI_PowerupMenu((Vector2f)Game.Instance.GetWindow().Size / 2, 800, 800);
            Game.Instance.UIManager.AddComponent(MainPowerUpMenu);

            debugMenu = new UI_DebugMenu(new Vector2f(10, 10));
            Game.Instance.UIManager.AddComponent(debugMenu);

            PlayerInfo = new UI_PlayerInfo(new Vector2f(Game.Instance.GetWindow().Size.X - 525, Game.Instance.GetWindow().Size.Y/2-250));
            Game.Instance.UIManager.AddComponent(PlayerInfo);

            Game.Instance.PLAYER.Stats.OnPlayerLevelUp += MainPowerUpMenu.OpenWindow;

            Game.Instance.PLAYER.Stats.LevelUp(1);

        }


        public override void UnloadContent()
        {
            backgroundMusic.Stop();

            Game.Instance.World.Clear();

            Game.Instance.CAMERA = new View();



            //MainPowerUpMenu = new UI_PowerupMenu((Vector2f)Game.Instance.GetWindow().Size / 2, 800, 800);
            Game.Instance.UIManager.RemoveComponent(MainPowerUpMenu);

            //var debugMenu = new UI_DebugMenu(new Vector2f(10, 10));
            Game.Instance.UIManager.RemoveComponent(debugMenu);

            Game.Instance.UIManager.RemoveComponent(PlayerInfo);

            Game.Instance.PLAYER.Stats.OnPlayerLevelUp -= MainPowerUpMenu.OpenWindow;

            Game.Instance.EntityManager.Clear();

            Game.Instance.PLAYER.SetPosition(new Vector2f(0, 0));

            //Game.Instance.GetWindow().SetView(Game.Instance.CAMERA);

            

        }

        public override void Update(float deltaTime)
        {
            Game.Instance.PLAYER.Update(deltaTime);

            Vector2i currentPlayerChunkIndex = Game.Instance.World.CalculateChunkIndex(Game.Instance.PLAYER.GetPosition());
            if (currentPlayerChunkIndex != Game.Instance.lastPlayerChunkIndex)
            {
                //world.Update(player.Sprite.Position, grasTile);
                Game.Instance.lastPlayerChunkIndex = currentPlayerChunkIndex;
            }

            Game.Instance.World.Update(Game.Instance.PLAYER.GetPosition());


            //EntityManager.UpdateEntities(PLAYER, DELTATIME); WE DO THIS IN THE BACKGROUND NOW

            UpdateCameraPosition(deltaTime);
        }

        private void UpdateCameraPosition(float deltaTime)
        {
            if (Game.Instance.CAMERA != null && Game.Instance.PLAYER != null)
            {
                // Update the original camera position to follow the player.
                // This ensures that the camera follows the player's current position before applying the shake offset.
                Game.Instance.originalCameraPosition = Game.Instance.PLAYER.GetPosition();

                if (Game.Instance.shakeDuration > 0)
                {
                    // Generate random offsets within the shake intensity range.
                    float offsetX = (float)(Random.Shared.NextDouble() * 2 - 1) * Game.Instance.shakeIntensity;
                    float offsetY = (float)(Random.Shared.NextDouble() * 2 - 1) * Game.Instance.shakeIntensity;
                    // Apply the shake by offsetting the original camera position (which follows the player).
                    Game.Instance.CAMERA.Center = new Vector2f(Game.Instance.originalCameraPosition.X + offsetX, Game.Instance.originalCameraPosition.Y + offsetY);

                    Game.Instance.shakeDuration -= deltaTime; // Decrease the shake duration.
                }
                else
                {
                    // If not shaking, simply follow the player.
                    Game.Instance.CAMERA.Center = Game.Instance.originalCameraPosition;
                }

                // Apply the updated camera position to the game window.
                Game.Instance.GetWindow().SetView(Game.Instance.CAMERA);
            }
        }

        // This method attaches the camera to an entity, e.g., the player
        public void AttachCamera(Player entity)
        {
            // Create a new view centered around the entity's position with the desired size
            Game.Instance.CAMERA = new View(entity.GetPosition(), new Vector2f(640, 480)); // Window size as view size 640x480
        }

    }
}
