using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    public class Physics
    {
        public float gravity = 5.0f;
        public float jumpTime = 0.0f;

        public void UpdateGentlemanPhysics(GameTime gameTime, TheGentleman sprite)
        {
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float downVel = gravity * (frameTime - jumpTime);

            if (downVel >= 2.0f) // terminal velocity
                downVel = 0.5f;

            sprite.velocity.setY(sprite.velocity.getY() + downVel);
        }
        
        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float downVel = gravity * frameTime;

            // gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {
                if (downVel >= 2.0f)
                    downVel = 0.5f;

                i.velocity.setY(i.velocity.getY() + downVel);
            }
        }

        public void resetTime(GameTime time)
        {
            // when the Gentleman hits the ground we can make sure to turn off gravity by resetting the timer, 
            // which makes gravity zero until he is in the air again
            jumpTime = (float)time.TotalGameTime.TotalSeconds;
        }
    }
}
