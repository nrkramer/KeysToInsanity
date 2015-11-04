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
        private int _level = 0; // 0 - 202
        public int level // set to 0 to 100
        {
            set
            {
                _level = (int)((value / 100.0f) * 202.0f);
                spriteSize = new Point(_level, 32);
            }
            get { return _level; }
        }

        public InsanityBar (Game game, string bar): base(game, bar, false) { }

        public override void draw(SpriteBatch s)
        {
            // override the draw method to be able to make the insanity bar "fill in"
            s.Draw(spriteTex,
               new Rectangle(spritePos.ToPoint(), spriteSize),
               new Rectangle(new Point(0, 0), new Point((int)((spriteSize.X / 202.0f) * spriteTex.Bounds.Width), spriteTex.Bounds.Height)), Color.White, 0.0f,
               new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }

        public void Update(GameTime time)
        {
            int frameTime = (int)time.TotalGameTime.TotalMilliseconds;
            if ((frameTime / 100) <= 202)
                level = frameTime / 100;
        }
    }
}
