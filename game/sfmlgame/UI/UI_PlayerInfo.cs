using SFML.Graphics;
using SFML.System;
using System.Drawing;
namespace sfmlgame.UI
{
    public class UI_PlayerInfo : UIComponent
    {
        RectangleShape backgroundShape;

        public bool IsOpen = false;

        UI_List playerStatsGroup;

        public UI_PlayerInfo(Vector2f position) : base(position)
        {

            base.SetPosition(position);

            backgroundShape = new RectangleShape(new Vector2f(500, 500));
            backgroundShape.FillColor = SFML.Graphics.Color.Black;
            backgroundShape.Position = position;

            base.Width = (int)backgroundShape.Size.X;
            base.Height = (int)backgroundShape.Size.Y;

            playerStatsGroup = new UI_List(new Vector2f(GetCenterX, GetCenterY-Width/2));

            UI_Text title = new UI_Text("- Info -", 45, new Vector2f(GetCenterX, GetCenterY));
            playerStatsGroup.AddChild(title, new Vector2f(0, 50));

            UIBinding<string> playerLevelTextBinding = new UIBinding<string>(() => Game.Instance.PLAYER.Stats.Level.ToString());
            UI_Text playerLEvelText = new UI_Text("Level: ", 45, new Vector2f(0, 0), playerLevelTextBinding);
            playerStatsGroup.AddChild(playerLEvelText, new Vector2f(0, 100));

            UIBinding<string> playerXPTextBinding = new UIBinding<string>(() => Game.Instance.PLAYER.Stats.XP.ToString());
            UI_Text playerXPText = new UI_Text("XP: ", 45, new Vector2f(0, 0), playerXPTextBinding);
            playerStatsGroup.AddChild(playerXPText);

            UIBinding<string> playerHPTextBinding = new UIBinding<string>(() => Game.Instance.PLAYER.Stats.MaxHP.ToString());
            UI_Text playerHPText = new UI_Text("HP: ", 45, new Vector2f(0, 0), playerHPTextBinding);
            playerStatsGroup.AddChild(playerHPText);

            UIBinding<string> playerDamageTextBinding = new UIBinding<string>(() => Game.Instance.PLAYER.Stats.Damage.ToString());
            UI_Text playerDamageText = new UI_Text("Damage: ", 45, new Vector2f(0, 0), playerDamageTextBinding);
            playerStatsGroup.AddChild(playerDamageText);

            UIBinding<string> playerSpeedTextBinding = new UIBinding<string>(() => Game.Instance.PLAYER.Stats.MovementSpeed.ToString());
            UI_Text playerSpeedText = new UI_Text("Speed: ", 45, new Vector2f(0, 0), playerSpeedTextBinding);
            playerStatsGroup.AddChild(playerSpeedText);



        }

        public override void Draw(RenderTexture renderTexture)
        {
            if(IsOpen)
            {
                renderTexture.Draw(backgroundShape);

                playerStatsGroup.Draw(renderTexture);   

            }
        }

        public override void Update(float deltaTime)
        {
            if (!IsOpen) return;
        }
    }
}
