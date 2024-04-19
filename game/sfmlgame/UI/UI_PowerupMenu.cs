
using SFML.Graphics;
using SFML.System;
using sfmlgame.Abilities;
using sfmlgame.Assets;


namespace sfmlgame.UI
{
    public class UI_PowerupMenu : UIComponent
    {
        RectangleShape backgroundShape;

        UI_PowerUpButton leftOption;
        UI_PowerUpButton rightOption;

        //UI_Button closeButton;

        public bool IsMenuOpen = false;

        public UI_PowerupMenu(Vector2f position, float width, float height) : base(position)
        {

            // Create a rectangle shape with the desired dimensions
            backgroundShape = new RectangleShape(new Vector2f(width, height));
            backgroundShape.FillColor = new Color(0, 0, 0, 90);
            backgroundShape.Position = position;
            // Center the origin if necessary
            backgroundShape.Origin = new Vector2f(width / 2f, height / 2f);


            //var closeButtonPos = new Vector2f(850, 845);

            //closeButton = new UI_Button(closeButtonPos, "Close", 46, 45, 25, Color.Cyan);
            ///closeButton.ClickAction += CloseButton_ClickAction;

            leftOption = new UI_PowerUpButton(new Vector2f(position.X-250, position.Y), string.Empty, 0, 250, 250, Color.Red);

            rightOption = new UI_PowerUpButton(new Vector2f(position.X+ 250, position.Y), string.Empty, 0, 250, 250, Color.Blue);


            

            

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
            if (!IsMenuOpen) return;
            //closeButton.Update(deltaTime);
            leftOption.Update(deltaTime);
            rightOption.Update(deltaTime);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!IsMenuOpen) return;
            renderTexture.Draw(backgroundShape);
            leftOption.Draw(renderTexture);
            rightOption.Draw(renderTexture);
            //closeButton.Draw(renderTexture);
        }
    }
}
