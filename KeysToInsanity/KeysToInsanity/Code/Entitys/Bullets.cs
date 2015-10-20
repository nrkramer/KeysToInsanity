using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Bullets : AnimatedSprite
    {
        const int maxDistance = 300;
        public Vector2 startPosition;
        public Vector2 speed;
        public Vector2 direction;
        public bool visible = false;

        Bullets(Game game) : base(game,"bullet",new Point(16,16),1,.25,true)
        {

        }

        public void update(GameTime time)
        {
           /* if (Vector2.Distance(startPosition,Position) > maxDistance)
            {
                visible = false;
            }
            if(visible == true)
            {

            }*/
        }
    }
}
