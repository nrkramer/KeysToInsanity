using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace KeysToInsanity.Code.Environment
{
    class LightEffect : BasicSprite
    {
        public LightEffect(Game game, string effect, Color color) : base(game, effect, false)
        {
            this.color = color;
        }

        public override void draw(SpriteBatch s)
        {
            if ((spriteTex != null) && !hidden)
                s.Draw(spriteTex, new Rectangle(spritePos.ToPoint(), spriteSize), color * opacity);
        }
    }
}
