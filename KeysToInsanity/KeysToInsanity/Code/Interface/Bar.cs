using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class Bar: BasicSprite
    {
        public Color shade = Color.White;

        private float _level = 0.0f; // 0 - width
        private float _levelUpdate = 0.0f; // for animation 0 - width
        public float level // set to 0 to 100
        {
            set
            {
                _level = (value / 100.0f) * 202.0f;
            }
            get { return _level; }
        }

        public Bar (Game game, string bar): base(game, bar, false) { }

        public override void draw(SpriteBatch s)
        {
            // override the draw method to be able to make the insanity bar "fill in"
            s.Draw(spriteTex,
               new Rectangle(spritePos.ToPoint(), spriteSize),
               new Rectangle(new Point(0, 0), new Point((int)((spriteSize.X / 202.0f) * spriteTex.Bounds.Width), spriteTex.Bounds.Height)), shade, 0.0f,
               new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }

        // animate bar in the direction of the level
        public void Update(GameTime time)
        {
            if (_levelUpdate < _level)
            {
                _levelUpdate += 1.0f;
            } else if (_levelUpdate > _level)
            {
                _levelUpdate -= 1.0f;
            } else // (_levelUpdate == level)
            {

            }

            spriteSize = new Point((int)_levelUpdate, 32);
        }
    }
}
