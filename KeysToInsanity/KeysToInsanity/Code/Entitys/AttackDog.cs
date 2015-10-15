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
            
            /*
            G1 |   C     | G2
            If Genteman is at G1, the dog needs to attack Gentleman, but not go past boundary.
            So the control statement has to be less than or equal to C plus 100. 
            Likewise if the Gentleman is at G2, the dog has to get him,but not go past the boundry. 
            So  it is greater than or equal to center plus 100.
            */
            
            /*if (TheGentleman.theGentleman.getSpriteXPos() <= center+100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 6.0f);
            }
            if (TheGentleman.theGentleman.SpritePos.get() >= center-100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 6.0f);
            }*/

            if (getSpriteXPos() <= center + 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 1.0f);
            }
            if (getSpriteXPos() >= center - 100)
            {
                this.velocity = Velocity.FromDirection(0.0f, -1.0f);
            }
            
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //Will use damage class to deduct gentleman
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
