using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    // Only difference between this class and BasicSprite is that this will always fill the screen
    class BasicBackground : BasicSprite
    {
        public BasicBackground(Game game, string file) : base(game, file)
        {
            spriteSize = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        // Load sprite from existing texture
        public BasicBackground(Texture2D tex) : base(tex)
        {
        }

        // "override" overrides the inherited draw function
        public override void draw(SpriteBatch s)
        {
            // set the sprite size to the size of the drawing viewport
            spriteSize.X = s.GraphicsDevice.Viewport.Width;
            spriteSize.Y = s.GraphicsDevice.Viewport.Height;
            s.Draw(spriteTex, new Rectangle(spritePos, spriteSize), new Color(1.0f, 1.0f, 1.0f));
        }
    }
}
