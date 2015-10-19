using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        //public Velocity gravity = Velocity.FromDirection(-90.0f, -9.8f);
        public float gravity = 1.0f;
        public bool jumping = false;
        public float jumpTime = 0.0f;
        private bool grounded = false;

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            float downVel = gravity * (frameTime - jumpTime);

            //gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {
                
                if (downVel <= 9.8f)
                {
                    if (i.ToString() == "KeysToInsanity.Code.TheGentleman")
                    {
                        if (!((TheGentleman)i).isJumping() && grounded)
                            i.velocity.setY(0.0f);
                        else
                    i.velocity.setY(i.velocity.getY() + downVel);
                }
                }
                //Console.WriteLine(gravity * Velocity.FromCoordinates(frameTime, frameTime));
            }
            grounded = false;
        }

        public void resetTime(GameTime time)
        {
            jumpTime = (float)time.TotalGameTime.TotalSeconds;
            grounded = true;
        }
    }
}
