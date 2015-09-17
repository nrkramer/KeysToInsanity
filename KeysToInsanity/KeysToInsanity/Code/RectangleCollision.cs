using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class RectangleCollision
    {
        public static bool willCollide(BasicSprite s1, BasicSprite s2)
        {
            return new Rectangle(s1.getUpdatePosition(), s1.spriteSize).Intersects(new Rectangle(s2.getUpdatePosition(), s2.spriteSize));
        }
    }
}
