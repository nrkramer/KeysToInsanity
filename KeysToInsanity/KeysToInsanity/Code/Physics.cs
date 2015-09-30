using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public Velocity gravity = Velocity.FromDirection(-90.0f, -9.8f);

        public void Update(GameTime gameTime, SpriteContainer spritesToPhysics)
        {
            //Console.WriteLine();
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            foreach (BasicSprite i in spritesToPhysics)
            {
                i.velocity = i.velocity + gravity;
                //Console.WriteLine(gravity * Velocity.FromCoordinates(frameTime, frameTime));
            }
        }
    }
}
