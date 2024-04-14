using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;
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

        public UI_DebugMenu(Vector2f position) : base(position)
        {
            backgroundShape = new RectangleShape(new Vector2f(300, 500));
            backgroundShape.Position = position;

            debugHeaderText = new UI_Text("Debug-Menu", 24, new Vector2f(position.X + 10, position.Y + 10));

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
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!Game.Instance.Debug) return;

            renderTexture.Draw(backgroundShape);

            debugHeaderText.Draw(renderTexture);

            debugLevelUp.Draw(renderTexture);
            debugTeleport.Draw(renderTexture);
        }

        public override void Update(float deltaTime)
        {
            if (!Game.Instance.Debug) return;

            debugHeaderText.Update(deltaTime);

            debugLevelUp.Update(deltaTime);
            debugTeleport.Update(deltaTime);
        }
    }
}
