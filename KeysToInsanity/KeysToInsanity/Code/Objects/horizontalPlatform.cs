using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class horizontalPlatform : Platform
    {

        private float center;
        private bool neverStop = false;
        private float moveSpeed;
        private float moveDistance;
        private bool p2Flag;
        private bool p1Flag;
        private bool direction = true;
        private float p1;
        private float p2;

        //a type of platform that will move side to side
        public horizontalPlatform(Game game) : base(game, "platform", true)
        {
            spriteSize = new Point(150, 50);
            center = getSpriteXPos();
            moveDistance = 50f;
            moveSpeed = 1.0f;
            p1Flag = false;
            p2Flag = false;
            direction = true;
        }

        //allows the platform to move itself in the desired direction
        public void Update(GameTime gameTime, SpriteContainer SidewaysPlatforms)
        {
            foreach (BasicSprite x in SidewaysPlatforms)
            {
                p1 = center - moveDistance;
                p2 = center + moveDistance;
                if (direction == true)
                {
                    x.velocity = Velocity.FromCoordinates(0.0f, -moveSpeed);
                    if (getSpriteXPos() > center + moveDistance)
                    {
                        direction = false;
                    }
                    else
                    {
                        velocity = Velocity.FromDirection(0.0f, -moveSpeed);
                    }
                    if (getSpriteXPos() < center - moveDistance)
                    {
                        direction = true;
                    }
                }
            }
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            collided.spritePos = new Vector2(collided.spritePos.X + velocity.getX(), collided.spritePos.Y);
        }
    }
}




