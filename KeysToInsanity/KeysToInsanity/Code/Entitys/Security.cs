using KeysToInsanity.Code.Entitys;
using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    class Security : Character
    {

        private float center;
        private float patrolSpeed;
        private float patrolDistance;
        private bool direction;
        public Security(Game game,float posX) : base(game, "TopHat",new Point (72,71),1,.25, false)
        {
            //Setting Xpos
            center = posX;
            patrolDistance = 50.0f;
            patrolSpeed = 0.50f;
            direction = true;
        }

        public override void Update(GameTime time)
        {
            // Console.WriteLine("Sprite Pos is" + getSpriteXPos());
            // Console.WriteLine("Center is" + center);

            if (direction == true)
            {

                velocity = Velocity.FromDirection(0.0f, patrolSpeed);
                if (getSpriteXPos() > center + patrolDistance)
                {
                    direction = false;
                    //Console.WriteLine(direction);
                }
            }
            else
            {
                velocity = Velocity.FromDirection(0.0f, -patrolSpeed);
            }
            if (getSpriteXPos() < center - patrolDistance)
            {
                direction = true;
                //Console.WriteLine("Center+50");
            }

            updateWithAnimation(time, 0);

        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            //Seeing if security hit the Gentleman
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //call method to handle damage and effects
            }
            //Checking to see if security hit a object
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
