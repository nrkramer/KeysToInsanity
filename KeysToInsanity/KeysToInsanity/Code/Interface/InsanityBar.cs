using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class InsanityBar: BasicSprite
    {

        public InsanityBar (Game game): base(game, "insanity_bar_color", false)
        {

        }

        public override void draw(SpriteBatch s)
        {
            //trying to override the draw method to be able to make the insanity bar "fill in"
            s.Draw(spriteTex,
               new Rectangle(spritePos.ToPoint(), spriteSize),
               new Rectangle(new Point(0, 0), new Point(spriteSize.X+9, spriteTex.Bounds.Height)), Color.White, 0.0f,
               new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }
    }
}
