using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Cars : Character
    {
        private float center;
        private float moveSpeed;
        private float moveDistance;
        private bool direction;
        private float damage = 100;


        public Cars (Game game, float moveSpeed, float moveDistance, float XPos) :base(game,"TopHat",new Point(22,22),1,0,true)
        {
            // ****** DO NOT CHANGE MAKES IT WORK ********
            center = XPos;
            //********************************************

            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
        }



        public override void Update(GameTime time)
        {
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
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                ((TheGentleman)collided).health -= damage;
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
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 72, 71), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    }
}
