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
         
        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();

            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;

            //gravity is applied to every sprite in the game here so that there is a universal gravity
            foreach (BasicSprite i in spritesToPhysics)
            {
                if (i.velocity.getY() >= 0)
                {
                    i.velocity.setY(i.velocity.getY() + (gravity * frameTime));
                    if (i.velocity.getY() >= 10.0f)
                    {
                        i.velocity.setY(10.0f);
                    }
                }

                //Console.WriteLine(gravity * Velocity.FromCoordinates(frameTime, frameTime));
            }
        }
        }
    }
