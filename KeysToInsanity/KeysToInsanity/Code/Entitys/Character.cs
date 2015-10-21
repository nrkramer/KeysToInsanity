using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Character : AnimatedSprite
    {
        public Character(Game game, string file, Point animatedSpriteSize, int spriteAnimations, double animationSpeed, bool collidable) : base(game, file, animatedSpriteSize, spriteAnimations, animationSpeed, collidable)
        {

        }

        // override this with custom update code
        public virtual void Update(GameTime time)
        {

        }
    }
}
