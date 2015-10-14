using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class horizontalplatform : BasicSprite
    {

        private int center;

        public horizontalplatform(Game game) : base(game, "platform", true)
        {
            spriteSize = new Point(150, 50); 
        }

        public void Update(GameTime gameTime, SpriteContainer platformsThatMove) {
            foreach (BasicSprite z in platformsThatMove)
            {
                float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
                if (getSpriteXPos() <= center + 50)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 2.5f);
                }
                else if (getSpriteXPos() >= center - 50) 
                {
                    this.velocity = Velocity.FromDirection(0.0f, -2.5f);
                }
            }
        }
    }
}

