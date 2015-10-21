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
        private float patrolSpeed;
        private float patrolDistance;
        private bool direction;
        //Basic fast NPC, will try to hit player, no line of sight algorithim
        public Rats(Game game, float xPos) : base(game, "TopHat", new Point(72, 71), 1, .25, false)
        {
            center = xPos;
            patrolDistance = 100.0f;
            patrolSpeed = 5.0f;
            direction = true;
        }
        public Rats(Game game,float xPos,float patrol,float speed) : base(game, "TopHat",new Point(72,71),1,.25,false)
        {
            center = xPos;
            patrolSpeed = speed;
            patrolDistance = patrol;
            direction = true;
        }

        protected void Update(GameTime time)
        {
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
            updateWithAnimation(time,0);
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

        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 72, 71), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    }
}
