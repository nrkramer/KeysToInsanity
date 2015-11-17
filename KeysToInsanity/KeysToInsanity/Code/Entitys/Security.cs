using KeysToInsanity.Code.Entitys;
using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    class Security : Character
    {

        private float center;
        private float moveSpeed;
        private float moveDistance;
        private bool direction;
        private float damage = 10;


        public Security(Game game, float moveSpeed, float moveDistance, float XPos) :base(game,"security",new Point(22,22),1,0,true)
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
            //Seeing if security hit the Gentleman
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                ((TheGentleman)collided).health -= damage;
            }
            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    velocity = Velocity.FromCoordinates(-velocity.getX(), 0.0f);
                }


            }
        }

        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 22, 22), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    }
}
