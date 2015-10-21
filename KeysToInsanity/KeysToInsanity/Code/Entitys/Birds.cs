using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Birds : Character
    {
        private float center;

        //basic floating enemie that will "swoop" and attack the player
        public Birds(Game game) : base(game, "TopHat", new Point(72, 71), 1, .25, true)
        {
            center = getSpriteXPos();
        }

        public override void Update(GameTime time)
        {
            if (getSpriteXPos() <= center + 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 2.0f);
            }
            if (getSpriteXPos() >= center - 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 2.0f);
            }
            
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {

            }

            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 0.0f);
                }
            }
        }
    }
}
