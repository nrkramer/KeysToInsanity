using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{

    /*Will try to create a cone of sit algoritim for this. Otherwise dogs will be really fast*/
    class AttackDog : AnimatedSprite
    {

        private int center;

        public AttackDog(Game game, int PosX, int PosY) : base(game, "TopHat", new Point(PosX, PosY), 1, .25, false)
        {
            center = PosX;
        }

        protected void Update()
        {
            if (getSpriteXPos() <= center + 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 6.0f);
            }
            if (getSpriteXPos() >= center - 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 6.0f);
            }
            /*if(TheGentleman.getSpriteXPos() >= center)
            {

            }*/
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
