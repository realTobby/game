using sfmglame.Helpers;
using SFML.Graphics;
using SFML.System;
using sfmlgame.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.UI
{
    public class UI_PowerUpButton : UI_Button
    {
        public Ability AbilityUpgrade;

        private Sprite uiAbilityIcon;

        private UI_Text abilityName;

        private RectangleShape iconBorder = new RectangleShape();

        public UI_PowerUpButton(Vector2f pos, string buttonText, int textSize, int width, int height, Sprite buttonSprite) : base(pos, buttonText, textSize, width, height, buttonSprite)
        {

            iconBorder = new RectangleShape(new Vector2f(150, 150));
            iconBorder.FillColor = Color.Transparent;
            iconBorder.OutlineColor = Color.Black;
            iconBorder.OutlineThickness = 2;

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(RenderTexture renderTexture)
        {
            base.Draw(renderTexture);

            
            renderTexture.Draw(uiAbilityIcon);


            renderTexture.Draw(iconBorder);
        }

        public void Reset(Ability availableAbility)
        {
            UniversalLog.LogInfo("New Option reset");
            //Game.Instance.UIManager.AddComponent(this);

            AbilityUpgrade = availableAbility;

            uiAbilityIcon = availableAbility.Icon;

            uiAbilityIcon.Position = new Vector2f(Position.X, Position.Y);

            iconBorder.Position = uiAbilityIcon.Position;

            base._text.SetText(AbilityUpgrade.Name, 36);

        }
    }
}
