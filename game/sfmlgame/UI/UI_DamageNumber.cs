
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

        public UI_DamageNumber(int damageAmount, Vector2f worldPosition, float duration = 2.0f) : base(worldPosition)
        {
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

            if(rnd.Next(0,100) == 0)
            {
                damageText.SetColor(new Color(231, 41, 41, 255));
            }

            //GameScene.Instance._uiManager.AddComponent(this);
        }

        public void ResetFromPool(Vector2f pos)
        {
            SetPosition(Game.Instance.ConvertWorldToViewPosition(pos));
            damageText.Position = uiPosition;
            elapsedTime = 0;
        }

        public override void Update(float deltaTime)
        {
            if (!base.IsActive) return;

            elapsedTime += deltaTime;

            this.uiPosition.Y -= riseSpeed * deltaTime; // Example update

            // IMPORTANT: Convert again in case the world position changes or if you want the damage number to "follow" a moving target
            this.uiPosition = Game.Instance.ConvertWorldToViewPosition(Position);

            damageText.SetPosition(this.uiPosition); // Update the UI_Text's position

            // Fade out the damage number over time
            byte alpha = (byte)(255 - (elapsedTime / duration) * 255);
            damageText.SetColor(new Color(0, 0, 0, alpha));

            if (elapsedTime >= duration)
            {
                Game.Instance.UIManager.RemoveComponent(this);
            }
        }

        public override void Draw(RenderTexture renderTexture)
        {
            if (!base.IsActive) return;

            damageText.Draw(renderTexture);
        }

    }
}
