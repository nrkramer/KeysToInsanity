using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class horizontalPlatform : BasicSprite
    {

        private float center;

        public horizontalPlatform(Game game) : base(game, "platform", false)
        {
            spriteSize = new Point(150, 50); 
        }

        public void Update(GameTime gameTime, SpriteContainer SidewaysPlatforms) {
                center = getSpriteXPos();
                
                if (getSpriteXPos() == center)
                {
                    velocity = Velocity.FromDirection(0.0f, 1.0f);
                }
                else if(getSpriteXPos() >= center)
                {
                    velocity = Velocity.FromDirection(0.0f, 2.5f);
                }
                else 
                {
                    velocity = Velocity.FromDirection(0.0f, -2.5f);
                }
            }
        }
    }


