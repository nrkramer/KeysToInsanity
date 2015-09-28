using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public Velocity gravity = Velocity.FromDirection(270, -9.8f);

        public void Update(GameTime gameTime, List<BasicSprite> spritesToPhysics)
        {
            foreach(BasicSprite i in spritesToPhysics)
            {
               // float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                i.velocity = i.velocity + gravity;
            }
        }
    }
}
