using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class HatHanger : BasicSprite
    {
        public HatHanger(Game game) : base(game, "hat hanger 2", false)
        {
            spriteSize = new Point(100, 200);
        }

        public override void onCollide(BasicSprite s)
        {

        }
    }
}
