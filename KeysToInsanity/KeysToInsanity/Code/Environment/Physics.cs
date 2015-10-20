using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public float gravity = 4.9f;
        public float jumpTime = 0.0f;
        private bool grounded = false;

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float downVel = gravity * (frameTime - jumpTime);


            //gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {

                if (downVel <= 9.8f)
                {
                    if (i.ToString() == "KeysToInsanity.Code.TheGentleman")
                    {
                        if ((((TheGentleman)i).isJumping() == false) && (grounded == true))
                        {
                            i.velocity.setY(0.0f);
                        }
                        else
                        {
                            i.velocity.setY(i.velocity.getY() + downVel);
                            
                        }
                    }
                    else
                    {
                        if (grounded == true)
                        {
                            i.velocity.setY(0.0f);
                        }
                        else
                        {
                            i.velocity.setY(i.velocity.getY() + downVel);
                        }
                    }
                }
            }
            grounded = false;
        }


        public void resetTime(GameTime time)
        {
            //when the Gentleman hits the ground we can make sure to turn off gravity by resetting the timer, which makes gravity zero until he is in the air again
            jumpTime = (float)time.TotalGameTime.TotalSeconds;
            grounded = true;
        }
    }
}
