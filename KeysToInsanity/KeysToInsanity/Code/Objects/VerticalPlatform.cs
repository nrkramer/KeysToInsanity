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
        public VerticalPlatform(Game game, float moveSpeed, float moveDistance) : base(game, "platform", true)
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
            collided.spritePos = new Vector2(collided.spritePos.X, collided.spritePos.Y + velocity.getY());
        }
    }
}




