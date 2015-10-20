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
        private bool neverStop = false;

        
        public horizontalPlatform(Game game) : base(game, "platform", true)
        {
            spriteSize = new Point(150, 50);
        }

        public void Update(GameTime gameTime, SpriteContainer SidewaysPlatforms) {
            foreach (BasicSprite x in SidewaysPlatforms)
            {
                x.velocity.setY(0.0f);
                center = x.getSpriteXPos();                
                    if (x.getSpriteXPos() >= center)
                    {
                    x.velocity = Velocity.FromCoordinates(1.0f, 0.0f);
                    }
                    else if (x.velocity.getX() < center)
                    {
                    x.velocity = Velocity.FromCoordinates(1.0f, 0.0f);
                    }
                }
            }

            }
        }
    


