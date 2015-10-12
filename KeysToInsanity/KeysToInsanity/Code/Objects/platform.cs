using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class platform : BasicSprite
    {
        
        
        platform(Game game, Vector2 position, int sizeX, int sizeY) : base(game, "platform", false)
        {
            spriteSize = new Point(sizeX, sizeY);
            
        }

        public void Update()
        {
          
        }
    }
}

