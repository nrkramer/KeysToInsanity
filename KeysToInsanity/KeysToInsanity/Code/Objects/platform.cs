using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Objects
{

    class platform : BasicSprite
    {
        private string direction;
        /*Takes in movement, direction the platform is moving--- uses direction in Update.
        SizeX & SizeY set size of the platform. Speed sets how fast the platform will move*/
        platform(Game game, Vector2 position, int sizeX, int sizeY) : base(game, "platform", false)
        {
            spriteSize = new Point(sizeX, sizeY);
            
        }

        public void Update()
        {
          
        }
    }
}

