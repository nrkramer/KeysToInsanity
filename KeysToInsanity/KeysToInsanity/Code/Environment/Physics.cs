using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        //public Velocity gravity = Velocity.FromDirection(-90.0f, -9.8f);
        public float gravity = 9.8f;
        public bool jumping = false;
        public float jumpTime = 0.0f;

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();

            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;

            //gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {
                    i.velocity.setY(i.velocity.getY() + (gravity * frameTime - jumpTime));
            }
            Console.WriteLine(frameTime - jumpTime);
                //Console.WriteLine(gravity * Velocity.FromCoordinates(frameTime, frameTime));
        }

        public void resetTime(GameTime time)
        {
            jumpTime = (float)time.TotalGameTime.TotalSeconds;
        }
    }
}
