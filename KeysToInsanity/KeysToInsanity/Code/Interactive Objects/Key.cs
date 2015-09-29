using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class Key : BasicSprite
    {
        public Key(Game game) : base(game, "key", false)
        {
            spriteSize = new Point(50, 50);
        }

        public override void onCollide(BasicSprite collided)
        {
        }
    }
}
