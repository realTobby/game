
using SFML.Graphics;
using SFML.System;
using sfmlgame.Assets;


namespace sfmlgame.UI
{
    public class UI_PowerupMenu : UIComponent
    {
        Sprite backgroundShape;

        UI_Button closeButton;

        public bool IsMenuOpen = false;

        public UI_PowerupMenu(Vector2f position) : base(position)
        {

            backgroundShape = new Sprite(GameAssets.Instance.TextureLoader.GetTexture("menuBackground", "UI"));
            backgroundShape.Position = position;
            backgroundShape.Origin = new Vector2f(backgroundShape.Texture.Size.X/2, backgroundShape.Texture.Size.Y/2);



            var closeButtonPos = new Vector2f(850, 845);

            closeButton = new UI_Button(closeButtonPos, "Close", 46, 45, 25, new SFML.Graphics.Sprite(TextureLoader.Instance.GetTexture("button", "UI")));
            closeButton.ClickAction += CloseButton_ClickAction;

        }

        public void OpenWindow()
        {
            //GameManager.Instance.IsGamePaused = true;
            Game.Instance.GamePaused = true;
            IsMenuOpen = true;
        }

        private void CloseButton_ClickAction()
        {
            //GameManager.Instance.IsGamePaused = false;
            Game.Instance.GamePaused = false;
            IsMenuOpen = false;
        }

        public override void Update(float deltaTime)
        {
            closeButton.Update(deltaTime);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!IsMenuOpen) return;
            renderTexture.Draw(backgroundShape);
            closeButton.Draw(renderTexture);
        }
    }
}
