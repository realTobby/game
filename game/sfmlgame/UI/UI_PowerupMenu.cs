
using SFML.Graphics;
using SFML.System;
using sfmlgame.Abilities;
using sfmlgame.Assets;


namespace sfmlgame.UI
{
    public class UI_PowerupMenu : UIComponent
    {
        Sprite backgroundShape;

        UI_PowerUpButton leftOption;
        UI_PowerUpButton rightOption;

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

            leftOption = new UI_PowerUpButton(new Vector2f(position.X-250, 600), string.Empty, 0, 250, 500, new SFML.Graphics.Sprite(TextureLoader.Instance.GetTexture("powrupButton", "UI")));

            rightOption = new UI_PowerUpButton(new Vector2f(position.X+250, 600), string.Empty, 0, 250, 500, new SFML.Graphics.Sprite(TextureLoader.Instance.GetTexture("powrupButton", "UI")));


            

            

        }

        private void ReShuffleOptions()
        {
            AbilityFactory abilityFactory = new AbilityFactory();
            leftOption.Reset(abilityFactory.CreateRandomAbility(Game.Instance.PLAYER));
            rightOption.Reset(abilityFactory.CreateRandomAbility(Game.Instance.PLAYER));

            leftOption.ClickAction += ChooseOptionLeft;
            rightOption.ClickAction += ChooseOptionRight;

        }

        private void ChooseOptionLeft()
        {
            Game.Instance.PLAYER.Abilities.Add(leftOption.AbilityUpgrade);

            CloseWindow();
        }


        private void ChooseOptionRight()
        {
            Game.Instance.PLAYER.Abilities.Add(rightOption.AbilityUpgrade);
            CloseWindow();
        }

        public void OpenWindow()
        {
            //GameManager.Instance.IsGamePaused = true;
            ReShuffleOptions();



            Game.Instance.GamePaused = true;
            IsMenuOpen = true;
        }

        private void CloseButton_ClickAction()
        {
            //GameManager.Instance.IsGamePaused = false;
            CloseWindow();
        }

        private void CloseWindow()
        {
            Game.Instance.GamePaused = false;
            IsMenuOpen = false;
        }

        public override void Update(float deltaTime)
        {
            closeButton.Update(deltaTime);
            leftOption.Update(deltaTime);
            rightOption.Update(deltaTime);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!IsMenuOpen) return;
            renderTexture.Draw(backgroundShape);
            leftOption.Draw(renderTexture);
            rightOption.Draw(renderTexture);
            closeButton.Draw(renderTexture);
        }
    }
}
