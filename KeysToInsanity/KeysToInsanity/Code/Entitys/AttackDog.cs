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

        private float center;

        public AttackDog(Game game) : base(game, "dogs", new Point(47,27 ), 3, .25, true)
        {
            center = getSpriteXPos();
        }

        public void Update(GameTime time)
        {
            
            /*
            G1 |   C     | G2
            If Genteman is at G1, the dog needs to attack Gentleman, but not go past boundary.
            So the control statement has to be less than or equal to C plus 100. 
            Likewise if the Gentleman is at G2, the dog has to get him,but not go past the boundry. 
            So  it is greater than or equal to center plus 100.
            */
            
            /*if (KeysToInsanity.theGentleman.getSpriteXPos() <= center+100)
            {
                this.velocity = Velocity.FromDirection(0.0f, 6.0f);
            }
            if (KeysToInsanity.theGentleman.SpritePos.get() >= center-100)
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

            updateWithAnimation(time, 0);
            updateWithAnimation(time, 1);
            updateWithAnimation(time, 2);
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

        protected override void loadAnimations()
        {
            Animation run1 = new Animation();
            run1.AddFrame(new Rectangle(0, 0, 47, 27), TimeSpan.FromSeconds(1.0));
            animations.Add(run1);

            Animation run2 = new Animation();
            run2.AddFrame(new Rectangle(47, 0, 47, 27), TimeSpan.FromSeconds(1.0));
            animations.Add(run2);

            Animation run3 = new Animation();
            run3.AddFrame(new Rectangle(94, 0, 47, 27), TimeSpan.FromSeconds(1.0));
            animations.Add(run3);
        }
    }
}
