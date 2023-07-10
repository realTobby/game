using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Models
{
    public class DrawableEntity
    {
        public AnimatedSprite AnimatedSprite;

        //public Sprite Sprite;

        public DrawableEntity(string category, string entityName, int frameCount)
        {
            //Sprite = new Sprite(texture);
            AnimatedSprite = new AnimatedSprite(category, entityName, frameCount);
        }





    }
}
