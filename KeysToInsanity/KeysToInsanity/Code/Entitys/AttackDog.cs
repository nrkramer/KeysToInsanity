using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{

    /*Will try to create a line of sit algoritim for this. Otherwise dogs will be really fast*/
    class AttackDog : Character
    {

        private float center;
        private float moveSpeed;
        private float moveDistance;
        private bool direction;
        private float damage = 20;

       
        public AttackDog(Game game, float moveSpeed, float moveDistance, float XPos) :base(game,"dogs",new Point(47,27),1,0,true)
        {
            // ****** DO NOT CHANGE MAKES IT WORK ********
            center = XPos;
            //********************************************
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
        }



        public override void Update(GameTime time)
        {
            Console.WriteLine(direction);
            Console.WriteLine("Center" + center);
            Console.WriteLine("XPOS" + getSpriteXPos());
            if (direction == true)
            {
                velocity = Velocity.FromCoordinates(moveSpeed, 0.0f);
                if (getSpriteXPos() > center + moveDistance)
                {
                    direction = false;
                }
            }
            else
            {
                if (getSpriteXPos() < center - moveDistance)
                {
                    direction = true;
                }
                velocity = Velocity.FromCoordinates(-moveSpeed, 0.0f);

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
                TheGentleman tg = (TheGentleman)collided;
                if (!tg.invincible)
                {
                   
                    tg.health -= damage;
                }
            }

            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    velocity = Velocity.FromCoordinates(-velocity.getX(), 0.0f);
                }

            }
        }

        //Custom load for AttackDog
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
