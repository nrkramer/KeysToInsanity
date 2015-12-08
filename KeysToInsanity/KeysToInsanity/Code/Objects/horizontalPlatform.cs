using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class HorizontalPlatform : Platform
    {
        public float center = 0.0f;
        private float moveSpeed = 0.0f;
        private float moveDistance = 0.0f;
        private bool direction = true;

        //a type of platform that will move side to side
        public HorizontalPlatform(Game game,string file, float moveSpeed, float moveDistance) : base(game, file, true)
        {
            center = getSpriteXPos();
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
        }

        //allows the platform to move itself in the desired direction
        public override void Update(GameTime gameTime)
        {
            if (direction)
            {
                velocity = Velocity.FromCoordinates(moveSpeed, 0.0f);
                if (getSpriteXPos() > center + moveDistance)
                {
                    direction = false;
                }
            }
            else
            {
                velocity = Velocity.FromCoordinates(-moveSpeed, 0.0f);
                if (getSpriteXPos() < center - moveDistance)
                {
                    direction = true;
                }
            }
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            
            // for reasons, collision only happens half the time so we have to multiply the velocity by 2
            collided.updatePositionFromVelocity(Velocity.FromCoordinates(velocity.getX(), 0.0f));
        }
    }
}




