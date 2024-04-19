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
