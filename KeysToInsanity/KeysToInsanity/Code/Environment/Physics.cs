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
        const float hysteresisAmount = 1.0f;

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float downVel = gravity * (frameTime - jumpTime) * hysteresisAmount;
            

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
                            grounded = false;
                        }
                        else
                        {
                            i.velocity.setY(i.velocity.getY() + downVel);
                            
                        }
                    }
                }
            }
        }

        public void resetTime(GameTime time)
        {
            jumpTime = (float)time.TotalGameTime.TotalSeconds;
            grounded = true;
        }
    }
}
