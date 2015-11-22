using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace KeysToInsanity.Code
{
    // Only difference between this class and BasicSprite is that this will always fill the screen
    // Now it allows sliding. Soon to allow fading as well.
    class BasicBackground : BasicSprite
    {
        private KeysToInsanity.Boundary sliding = KeysToInsanity.Boundary.None;
        private Texture2D previousBackgroundTexture;

        public bool slide = false;

        public BasicBackground(Game game, string file) : base(game, file, false)
        {
            spriteSize = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        // for parallax background
        public BasicBackground(Game game, string file1, string file2) : base(game, file1, false)
        {

        }

        // Load sprite from existing texture
        public BasicBackground(Texture2D tex) : base(tex, false) { }

        // slides this background in, drawing the previous one behind it
        public void slideIn(KeysToInsanity.Boundary direction, BasicBackground previousBackground)
        {
            if (sliding == KeysToInsanity.Boundary.None)
            {
                previousBackgroundTexture = previousBackground.spriteTex;
                sliding = direction;
                switch(sliding)
                {
                    case KeysToInsanity.Boundary.Left:
                        spritePos = new Vector2(spritePos.X + spriteSize.X, spritePos.Y);
                        break;
                    case KeysToInsanity.Boundary.Right:
                        spritePos = new Vector2(spritePos.X - spriteSize.X, spritePos.Y);
                        break;
                    case KeysToInsanity.Boundary.Top:
                        spritePos = new Vector2(spritePos.X, spritePos.Y + spriteSize.Y);
                        break;
                    case KeysToInsanity.Boundary.Bottom:
                        spritePos = new Vector2(spritePos.X, spritePos.Y - spriteSize.Y);
                        break;
                    default:
                        break;
                }

                slide = true;
            }
        }

        public void Update(GameTime time)
        {
            int step = 20;
            switch (sliding)
            {
                case KeysToInsanity.Boundary.Left:
                    spritePos = new Vector2(spritePos.X - step, spritePos.Y); // adjust the value here for animation speed
                    if (spritePos.X <= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case KeysToInsanity.Boundary.Right:
                    spritePos = new Vector2(spritePos.X + step, spritePos.Y); // adjust the value here for animation speed
                    if (spritePos.X >= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case KeysToInsanity.Boundary.Top:
                    spritePos = new Vector2(spritePos.X, spritePos.Y - step); // adjust the value here for animation speed
                    if (spritePos.Y <= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case KeysToInsanity.Boundary.Bottom:
                    spritePos = new Vector2(spritePos.X, spritePos.Y + step); // adjust the value here for animation speed
                    if (spritePos.Y >= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                default:
                    break;
            }
        }

        // "override" overrides the inherited draw function
        public override void draw(SpriteBatch s)
        {
            // set the sprite size to the size of the drawing viewport
            spriteSize.X = s.GraphicsDevice.Viewport.Width;
            spriteSize.Y = s.GraphicsDevice.Viewport.Height;
            drawBackground(s, spriteTex, new Rectangle(spritePos.ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
            // draw the next chunk of background
            switch(sliding)
            {
                case KeysToInsanity.Boundary.Bottom:
                    drawBackground(s, previousBackgroundTexture, new Rectangle(new Vector2(spritePos.X, spritePos.Y + spriteSize.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case KeysToInsanity.Boundary.Left:
                    drawBackground(s, previousBackgroundTexture, new Rectangle(new Vector2(spritePos.X - spriteSize.X, spritePos.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case KeysToInsanity.Boundary.Right:
                    drawBackground(s, previousBackgroundTexture, new Rectangle(new Vector2(spritePos.X + spriteSize.X, spritePos.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case KeysToInsanity.Boundary.Top:
                    drawBackground(s, previousBackgroundTexture, new Rectangle(new Vector2(spritePos.X, spritePos.Y - spriteSize.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                default:
                    break;
            }
        }

        private void drawBackground(SpriteBatch s, Texture2D tex, Rectangle r, Color c)
        {
            s.Draw(tex, r, null, c, 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
            if (KeysToInsanity.DRAW_BOUNDING_BOXES)
                drawBorder(s, r, 2, Color.Yellow);
        }
    }
}
