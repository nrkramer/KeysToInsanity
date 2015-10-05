using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        //public Velocity gravity = Velocity.FromDirection(-90.0f, -9.8f);
        public float gravity = 9.8f;
        public bool onTheGround;

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            //gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {
                i.velocity.setY((i.velocity.getY() * 0.5f) + (gravity * frameTime));
                if (i.velocity.getY() >= 29.0f)
                   i.velocity.setY(29.0f);
                

               // Console.WriteLine(i.velocity.getY());
            }
        }
    }
}
