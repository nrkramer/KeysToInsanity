using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public Velocity gravity = Velocity.FromDirection(270, -9.8f);
        public Velocity jump = Velocity.FromDirection(90, 10.0f);
        public bool hasJumped = false;

        public void Update(GameTime gameTime, List<BasicSprite> spritesToPhysics)
        {

            foreach(BasicSprite i in spritesToPhysics)
        {
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                i.velocity = i.velocity + gravity;

            
            }
    }
    }
}
