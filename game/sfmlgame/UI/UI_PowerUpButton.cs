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

        public UI_PowerUpButton(Vector2f pos, string buttonText, int textSize, int width, int height, Color color) : base(pos, buttonText, textSize, width, height, color)
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
            AbilityUpgrade = availableAbility;

            uiAbilityIcon = availableAbility.Icon;

            // Calculate the center position for the icon
            FloatRect iconBounds = uiAbilityIcon.GetLocalBounds();
            Vector2f iconSize = new Vector2f(iconBounds.Width, iconBounds.Height);
            Vector2f iconCenterPos = new Vector2f(Position.X + (_width - iconSize.X) / 2.0f,
                                                  Position.Y + (_height - iconSize.Y) / 2.0f);

            // Set the icon's position to the calculated center position
            uiAbilityIcon.Position = iconCenterPos;

            // Also center the iconBorder if needed
            FloatRect borderBounds = iconBorder.GetLocalBounds();
            Vector2f borderSize = new Vector2f(borderBounds.Width, borderBounds.Height);
            Vector2f borderCenterPos = new Vector2f(Position.X + (_width - borderSize.X) / 2.0f,
                                                    Position.Y + (_height - borderSize.Y) / 2.0f);
            iconBorder.Position = borderCenterPos;

            // Update the text as before
            base._text.SetText(AbilityUpgrade.Name, 36);
        }

    }
}
