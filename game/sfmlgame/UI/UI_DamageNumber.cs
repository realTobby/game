
using SFML.Graphics;
using SFML.System;
using sfmlgame;
using sfmlgame.UI;

namespace sfmlgame.UI
{
    public class UI_DamageNumber : UIComponent
    {
        private UI_Text damageText;
        private float duration;
        private float elapsedTime;
        private Vector2f uiPosition; // Store the original world position
        private float riseSpeed = 20.0f; // Adjust the speed of rising to your liking

        Random rnd = new Random();

        private Vector2f worldPosition;

        public UI_DamageNumber(int damageAmount, Vector2f worldPosition, float duration = 2.0f) : base(worldPosition)
        {
            this.worldPosition = worldPosition;
            //UniversalLog.LogInfo("newDamagerNumber");
            this.uiPosition = Game.Instance.ConvertWorldToViewPosition(worldPosition);
            this.Position = this.uiPosition; // Ensure the base position is also updated
            this.duration = duration;
            elapsedTime = 0;

            // Initialize UI_Text without setting its position here
            string textNumber = "";//damageAmount.ToString();
            UIBinding<string> damageBinding = new UIBinding<string>(() => damageAmount.ToString());
            damageText = new UI_Text(textNumber, 0, this.uiPosition, damageBinding);
            damageText.SetColor(new Color(0, 0, 0, 255)); // Start fully opaque
            damageText.SetBold(true);
            damageText.SetSize(35);
            damageText.Position = this.uiPosition;
            damageText.SetPosition(this.uiPosition);

            if(rnd.Next(0,100) == 0)
            {
                damageText.SetColor(new Color(231, 41, 41, 255));
            }

            //GameScene.Instance._uiManager.AddComponent(this);
        }

        public void ResetFromPool(Vector2f worldPos)
        {
            this.worldPosition = worldPos; // Store the original world position
            this.uiPosition = Game.Instance.ConvertWorldToViewPosition(worldPos); // Convert and store the initial screen position
            SetPosition(this.uiPosition); // You might not need this line if uiPosition is solely used for rendering
            damageText.SetPosition(this.uiPosition);
            elapsedTime = 0;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive) return;

            elapsedTime += deltaTime;

            // Apply the "rising" effect directly to the worldPosition.
            // This simulates the damage number rising in the game world, not the screen or UI.
            worldPosition.Y -= riseSpeed * deltaTime;

            this.uiPosition = Game.Instance.ConvertWorldToViewPosition(worldPosition);

            damageText.SetPosition(uiPosition);

            // The fade logic remains unchanged.
            // No need to convert to screen position here; it will be handled during rendering.
            if (elapsedTime >= duration)
            {
                IsActive = false; // Consider pooling or hiding for efficiency
            }
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!IsActive) return;
            // Render the damage text. The conversion ensures it appears at the correct screen position.
            damageText.Draw(renderTexture);
        }

    }
}
