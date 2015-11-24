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
    // Now it allows sliding, parallax. Soon to allow fading as well.
    class ParallaxBackground : BasicSprite
    {
        private KeysToInsanity.Boundary sliding = KeysToInsanity.Boundary.None;
        private Texture2D background2Texture = null;
        private Rectangle background2Rectangle = Rectangle.Empty;

        private Texture2D previousBackgroundTexture;
        private Texture2D previousBackgroundTexture2 = null;
        private Rectangle previousRectangle = Rectangle.Empty;
        private Rectangle previousRectangle2 = Rectangle.Empty;

        private int parallax = 500;

        public bool slide = false;

        public ParallaxBackground(Game game, string file) : base(game, file, false)
        {
            spriteSize = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        // for parallax background
        public ParallaxBackground(Game game, string file1, string file2) : base(game, file1, false)
        {
            spriteSize = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            background2Texture = game.Content.Load<Texture2D>(file2);
            background2Rectangle = new Rectangle(-parallax, -parallax, spriteSize.X + parallax * 2, spriteSize.Y + parallax * 2);
        }

        // Load sprite from existing texture
        public ParallaxBackground(Texture2D tex) : base(tex, false) { }

        // slides this background in, drawing the previous one behind it
        public void slideIn(KeysToInsanity.Boundary direction, ParallaxBackground previousBackground)
        {
            if (sliding == KeysToInsanity.Boundary.None)
            {
                previousBackgroundTexture = previousBackground.spriteTex;
                previousBackgroundTexture2 = previousBackground.background2Texture;
                previousRectangle = new Rectangle(0, 0, spriteSize.X, spriteSize.Y);
                previousRectangle2 = new Rectangle(0, 0, spriteSize.X + parallax, spriteSize.Y + parallax);
                sliding = direction;
                switch(sliding)
                {
                    case KeysToInsanity.Boundary.Left:
                        spritePos = new Vector2(spritePos.X + spriteSize.X, spritePos.Y);
                        background2Rectangle = new Rectangle(previousRectangle2.X + previousRectangle2.Width, previousRectangle2.Y, spriteSize.X + parallax, spriteSize.Y + parallax);
                        break;
                    case KeysToInsanity.Boundary.Right:
                        spritePos = new Vector2(spritePos.X - spriteSize.X, spritePos.Y);
                        background2Rectangle = new Rectangle(previousRectangle2.X - previousRectangle2.Width, previousRectangle2.Y, spriteSize.X + parallax, spriteSize.Y + parallax);
                        break;
                    case KeysToInsanity.Boundary.Top:
                        spritePos = new Vector2(spritePos.X, spritePos.Y + spriteSize.Y);
                        background2Rectangle = new Rectangle(previousRectangle2.X, previousRectangle2.Y + previousRectangle2.Height, spriteSize.X + parallax, spriteSize.Y + parallax);
                        break;
                    case KeysToInsanity.Boundary.Bottom:
                        spritePos = new Vector2(spritePos.X, spritePos.Y - spriteSize.Y);
                        background2Rectangle = new Rectangle(previousRectangle2.X, previousRectangle2.Y - previousRectangle2.Height, spriteSize.X + parallax, spriteSize.Y + parallax);
                        break;
                    default:
                        break;
                }
                slide = true;
            }
        }

        public void Update(GameTime time)
        {
            int step = 20; // adjust the value here for background1 speed
            int step2 = 32; // adjust the value here for background2 speed
            switch (sliding)
            {
                case KeysToInsanity.Boundary.Left:
                    spritePos = new Vector2(spritePos.X - step, spritePos.Y);
                    background2Rectangle.X -= step2;
                    previousRectangle.X -= step;
                    previousRectangle2.X -= step2;
                    if (spritePos.X <= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                        //background2Rectangle.Location = new Point(-parallax);
                    }
                    break;
                case KeysToInsanity.Boundary.Right:
                    spritePos = new Vector2(spritePos.X + step, spritePos.Y);
                    background2Rectangle.X += step2;
                    previousRectangle.X += step;
                    previousRectangle2.X += step2;
                    if (spritePos.X >= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                        //background2Rectangle.Location = new Point(-parallax);
                    }
                    break;
                case KeysToInsanity.Boundary.Top:
                    spritePos = new Vector2(spritePos.X, spritePos.Y - step);
                    background2Rectangle.Y -= step2;
                    previousRectangle.Y -= step;
                    previousRectangle2.Y -= step2;
                    if (spritePos.Y <= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                        //background2Rectangle.Location = new Point(-parallax);
                    }
                    break;
                case KeysToInsanity.Boundary.Bottom:
                    spritePos = new Vector2(spritePos.X, spritePos.Y + step);
                    background2Rectangle.Y += step2;
                    previousRectangle.Y += step;
                    previousRectangle2.Y += step2;
                    if (spritePos.Y >= 0)
                    {
                        sliding = KeysToInsanity.Boundary.None;
                        slide = false;
                        spritePos = new Vector2(0, 0);
                        //background2Rectangle.Location = new Point(-parallax);
                    }
                    break;
                default:
                    break;
            }
        }

        // "override" overrides the inherited draw function
        public override void draw(SpriteBatch s)
        {
            // draw this background's base
            Rectangle bgR = new Rectangle(spritePos.ToPoint(), spriteSize);
            s.Draw(spriteTex, bgR, Color.White);
            // draw previous background's base
            if (slide)
                s.Draw(previousBackgroundTexture, previousRectangle, Color.White);
            // draw this background's parallax
            if (background2Texture != null)
                s.Draw(background2Texture, background2Rectangle, Color.White);
            // draw previous backgroun's parallax
            if (slide)
                if (previousBackgroundTexture2 != null)
                    s.Draw(previousBackgroundTexture2, previousRectangle2, Color.White);

            if (KeysToInsanity.DRAW_BOUNDING_BOXES)
            {
                drawBorder(s, bgR, 2, Color.Yellow);
                drawBorder(s, previousRectangle, 2, Color.Yellow);
                drawBorder(s, background2Rectangle, 2, Color.YellowGreen);
                drawBorder(s, previousRectangle2, 2, Color.YellowGreen);
            }
        }
    }
}
