using game.Controllers;
using game.Entities;
using game.Managers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.UI
{
    public class UI_PowerupMenu : UIComponent
    {
        RectangleShape backgroundShape;

        UI_Button closeButton;

        public bool IsMenuOpen = false;

        public UI_PowerupMenu(Vector2f position) : base(position)
        {
        }

        public UI_PowerupMenu(Vector2f position, View view) : base(position, view)
        {

            backgroundShape = new RectangleShape(new Vector2f(100, 100));
            backgroundShape.FillColor = Color.Red;
            backgroundShape.Position = position;

            closeButton = new UI_Button(position, "Close", 10, 50, 32, new SFML.Graphics.Sprite(TextureLoader.Instance.GetTexture("startGameButton", "UI")), view);
            closeButton.ClickAction += CloseButton_ClickAction;

        }

        public void OpenWindow()
        {
            GameManager.Instance.IsGamePaused = true;
            IsMenuOpen = true;
        }

        private void CloseButton_ClickAction()
        {
            GameManager.Instance.IsGamePaused = false;
            IsMenuOpen = false;
        }

        public override void Update()
        {
            closeButton.Update();
        }

        public override void Draw()
        {
            if(GameManager.Instance.IsGamePaused == true && IsMenuOpen == true)
            {
                base.StartUIDraw();

                // Update the position relative to the camera view
                Vector2f offsetPosition = Position + cameraView.Center - cameraView.Size / 2f;
                backgroundShape.Position = offsetPosition;
                Game.Instance.GetRenderWindow().Draw(backgroundShape);

                
                closeButton.Draw();

                base.EndUIDraw();
            }

            
        }
    }
}
