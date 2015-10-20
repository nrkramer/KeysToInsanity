using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Rats : AnimatedSprite
    {
        private float center;
        //Basic fast NPC, will try to hit player, no line of sight algorithim
        public Rats(Game game) : base(game, "TopHat", new Point(72, 71), 1, .25, false)
        {
            center = getSpriteXPos();
        }

        protected void Update()
        {
            if (getSpriteXPos() <= center + 75)
            {
                this.velocity = Velocity.FromDirection(0.0f, 4.8f);
            }
            if (getSpriteXPos() >= center - 75)
            {
                this.velocity = Velocity.FromDirection(0.0f, 4.8f);
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
