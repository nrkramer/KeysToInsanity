using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class VerticalPlatform : Platform
    {
        public float center = 0.0f;
        private float moveSpeed = 0.0f;
        private float moveDistance = 0.0f;
        private bool direction = true;

        //a type of platform that will move up to down
        public VerticalPlatform(Game game, string file,float moveSpeed, float moveDistance) : base(game, file, true)
        {
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
        }

        //allows the platform to move itself in the desired direction
        public override void Update(GameTime gameTime)
        {
            if (direction)
            {
                velocity = Velocity.FromCoordinates(0.0f, moveSpeed);
                if (getSpriteYPos() > center + moveDistance)
                {
                    direction = false;
                }
            }
            else
            {
                velocity = Velocity.FromCoordinates(0.0f, -moveSpeed);
                if (getSpriteYPos() < center - moveDistance)
                {
                    direction = true;
                }
            }
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            
            // only if on top
            if (data.Height >= 1.0f)
            {
                collided.updatePositionFromVelocity(Velocity.FromCoordinates(0.0f, collided.velocity.getY() + velocity.getY()));
            }
        }
    }
}




