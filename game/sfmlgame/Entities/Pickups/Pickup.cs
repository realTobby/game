using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sfmlgame.Entities.Pickups
{
    public class Pickup : Entity
    {
        public Pickup(Sprite sprite, Vector2f initialPosition) : base(sprite, initialPosition)
        {
            base.animateSpriteComponent.SetScale(.7f);



            CanCheckCollision = true;

            SetPosition(initialPosition);
        }

        public Pickup(string category, string entityName, int frameCount, Vector2f initialPosition) : base(category, entityName, frameCount, initialPosition)
        {
            base.animateSpriteComponent.SetScale(.7f);



            CanCheckCollision = true;

            SetPosition(initialPosition);
        }

        public Pickup(Texture texture, int rows, int columns, Time frameDuration, Vector2f initialPosition) : base(texture, rows, columns, frameDuration, initialPosition)
        {
            base.animateSpriteComponent.SetScale(.7f);



            CanCheckCollision = true;

            SetPosition(initialPosition);
        }

        public virtual void PickItUp()
        {

        }

        public virtual int PickItUpInt()
        {
            PickItUp();
            return 0;
        }

        public override void ResetFromPool(Vector2f position)
        {
            throw new NotImplementedException();
        }
    }
}
