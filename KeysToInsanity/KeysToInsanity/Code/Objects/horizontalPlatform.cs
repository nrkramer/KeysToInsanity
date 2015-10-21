using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class HorizontalPlatform : Platform
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
        public HorizontalPlatform(Game game, float moveSpeed, float moveDistance) : base(game, "platform", true)
        {
            center = getSpriteXPos();
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
            direction = true;
        }

        //allows the platform to move itself in the desired direction
        public override void Update(GameTime gameTime)
        {
                if (direction == true)
                {
                    velocity = Velocity.FromCoordinates(-moveSpeed, 0.0f);
                    if (getSpriteXPos() > center + moveDistance)
                    {
                        direction = false;
                    }
                    else
                    {
                        velocity = Velocity.FromDirection(moveSpeed, 0.0f);
                    }
                    if (getSpriteXPos() < center - moveDistance)
                    {
                        direction = true;
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




