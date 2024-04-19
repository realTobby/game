
using SFML.Graphics;
using SFML.System;
using sfmlgame.Abilities;
using sfmlgame.Assets;

namespace sfmlgame.UI
{
    public class UI_PowerupMenu : UIComponent
    {
        RectangleShape backgroundShape;

        UI_PowerUpButton abilityUpgrade1;
        UI_PowerUpButton abilityUpgrade2;

        UI_Button statUpgrade;

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


            Width = (int)width;
            Height = (int)height;

            //var closeButtonPos = new Vector2f(850, 845);

            //closeButton = new UI_Button(closeButtonPos, "Close", 46, 45, 25, Color.Cyan);
            ///closeButton.ClickAction += CloseButton_ClickAction;

            abilityUpgrade1 = new UI_PowerUpButton(new Vector2f(position.X-250, position.Y), string.Empty, 0, 250, 250, Color.Red);

            abilityUpgrade2 = new UI_PowerUpButton(new Vector2f(position.X+ 250, position.Y), string.Empty, 0, 250, 250, Color.Blue);

            statUpgrade = new UI_Button(new Vector2f(GetCenterX, GetCenterY+Height/2), "Stat", 30, 150, 150, Color.Magenta);
            

            

        }

        private void ReShuffleOptions()
        {
            AbilityFactory abilityFactory = new AbilityFactory();
            abilityUpgrade1.Reset(abilityFactory.CreateRandomAbility(Game.Instance.PLAYER));
            abilityUpgrade2.Reset(abilityFactory.CreateRandomAbility(Game.Instance.PLAYER));

            abilityUpgrade1.ClickAction += ChooseOptionLeft;
            abilityUpgrade2.ClickAction += ChooseOptionRight;

            statUpgrade.ClickAction += ChooseStatUpgrade;

        }

        private void ChooseStatUpgrade()
        {
            Game.Instance.PLAYER.Stats.RandomStatUp();
        }

        private void ChooseOptionLeft()
        {
            Game.Instance.PLAYER.Abilities.Add(abilityUpgrade1.AbilityUpgrade);

            CloseWindow();
        }


        private void ChooseOptionRight()
        {
            Game.Instance.PLAYER.Abilities.Add(abilityUpgrade2.AbilityUpgrade);
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
            abilityUpgrade1.Update(deltaTime);
            abilityUpgrade2.Update(deltaTime);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!IsMenuOpen) return;
            renderTexture.Draw(backgroundShape);
            abilityUpgrade1.Draw(renderTexture);
            abilityUpgrade2.Draw(renderTexture);
            //closeButton.Draw(renderTexture);
        }
    }
}
