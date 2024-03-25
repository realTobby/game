using game.Helpers;
using game.Scenes;
using SFML.Graphics;
using SFML.System;
using System;

namespace game.UI
{
    public class UI_DamageNumber : UIComponent
    {
        private UI_Text damageText;
        private float duration;
        private float elapsedTime;
        private Vector2f worldPosition; // Store the original world position
        private float riseSpeed = 20.0f; // Adjust the speed of rising to your liking

        public UI_DamageNumber(int damageAmount, Vector2f worldPosition, View view, float duration = 2.0f) : base(worldPosition, view)
        {
            UniversalLog.LogInfo("newDamagerNumber");
            this.worldPosition = worldPosition; // Save the world position
            this.duration = duration;
            elapsedTime = 0;

            // Initialize UI_Text without setting its position here
            UIBinding<string> damageBinding = new UIBinding<string>(() => damageAmount.ToString());
            damageText = new UI_Text(damageAmount.ToString(), 24, new Vector2f(0, 0), view, damageBinding);
            damageText.SetColor(new Color(0, 0, 0, 255)); // Start fully opaque
            damageText.SetBold(true);
            damageText.SetSize(24);

            //GameScene.Instance._uiManager.AddComponent(this);
        }

        public override void Update()
        {
            elapsedTime += Game.Instance.DeltaTime;

            // Make the damage number rise over time
            worldPosition.Y -= riseSpeed * Game.Instance.DeltaTime;

            // Fade out the damage number over time
            byte alpha = (byte)(255 - (elapsedTime / duration) * 255);
            damageText.SetColor(new Color(0, 0, 0, alpha));

            if (elapsedTime >= duration)
            {
                GameScene.Instance._uiManager.RemoveComponent(this);
            }
        }

        public override void Draw()
        {
            // Convert the world position to view-relative position every draw call
            Vector2f viewRelativePosition = ConvertWorldToViewPosition(worldPosition, Game.Instance.GetRenderWindow().GetView());

            // Update the UI_Text component's position before drawing
            damageText.Position = viewRelativePosition;

            damageText.Draw();
        }

        private Vector2f ConvertWorldToViewPosition(Vector2f worldPosition, View view)
        {
            // Calculate the position relative to the view
            Vector2f viewCenter = view.Center;
            Vector2f viewSize = view.Size;
            Vector2f viewTopLeft = viewCenter - (viewSize / 2f);

            return worldPosition - viewTopLeft;
        }
    }
}
