using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class platform : BasicSprite
    {
        public bool moving = false;

        public platform(Game game) : base(game, "platform", true)
        {
            spriteSize = new Point(100, 35);
        }
       
        public void Update(GameTime gameTime, SpriteContainer platformsThatMove)
        { 
            float frameTime = (float)gameTime.TotalGameTime.TotalSeconds;
            moving = true;
            

            foreach (BasicSprite z in platformsThatMove)
            {
                float newSpeed = z.getSpriteXPos();
                while (moving == true)
                {
                    if(frameTime <= 5.0) {
                        newSpeed = z.getSpriteXPos() - 1;
                    }else if(frameTime <= 10.0)
                    {
                        newSpeed = z.getSpriteXPos() + 1;
                    }
                    frameTime = 0;                 
                }
            }
        }
    }
}

