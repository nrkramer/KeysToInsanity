using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class Box : BasicSprite
    {


        public Box(Game game) : base(game, "box", false)
        {
            spriteSize = new Point(25, 25);

        }

        public override void onCollide(BasicSprite collided)
        {
            base.onCollide(collided);
            // The Gentleman pushs into a box
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //Want position to move
            }
        }
    }
}
