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
    class BasicBackground : BasicSprite
    {
        public enum SLIDE_DIRECTION
        {
            NO_SLIDE = 0,
            SLIDE_LEFT = 1,
            SLIDE_RIGHT = 2,
            SLIDE_UP = 3,
            SLIDE_DOWN = 4
        };

        private SLIDE_DIRECTION sliding = SLIDE_DIRECTION.NO_SLIDE;
        private Timer slide_timer = new Timer();

        public BasicBackground(Game game, string file) : base(game, file, false)
        {
            spriteSize = new Point(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            slide_timer.Elapsed += new ElapsedEventHandler(slide_timer_event);
            slide_timer.Interval = 5; // Adjust this for faster animation updates
        }

        // Load sprite from existing texture
        public BasicBackground(Texture2D tex) : base(tex, false)
        {
            slide_timer.Elapsed += new ElapsedEventHandler(slide_timer_event);
            slide_timer.Interval = 5; // Adjust this for faster animation updates
        }

        public void slide(SLIDE_DIRECTION direction)
        {
            if (sliding == SLIDE_DIRECTION.NO_SLIDE)
            {
                sliding = direction;
                // create a timer to update the position and draw the next background chunk
                slide_timer.Start();
            }
        }

        private void slide_timer_event(object source, ElapsedEventArgs e)
        {
            switch(sliding)
            {
                case SLIDE_DIRECTION.SLIDE_LEFT:
                    spritePos = new Vector2(spritePos.X - 15, spritePos.Y); // adjust the value here for animation speed
                    if (spritePos.X < -spriteSize.X)
                    {
                        sliding = SLIDE_DIRECTION.NO_SLIDE;
                        slide_timer.Stop();
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case SLIDE_DIRECTION.SLIDE_RIGHT:
                    spritePos = new Vector2(spritePos.X + 15, spritePos.Y); // adjust the value here for animation speed
                    if (spritePos.X > spriteSize.X)
                    {
                        sliding = SLIDE_DIRECTION.NO_SLIDE;
                        slide_timer.Stop();
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case SLIDE_DIRECTION.SLIDE_UP:
                    spritePos = new Vector2(spritePos.X, spritePos.Y - 10); // adjust the value here for animation speed
                    if (spritePos.Y < -spriteSize.Y)
                    {
                        sliding = SLIDE_DIRECTION.NO_SLIDE;
                        slide_timer.Stop();
                        spritePos = new Vector2(0, 0);
                    }
                    break;
                case SLIDE_DIRECTION.SLIDE_DOWN:
                    spritePos = new Vector2(spritePos.X, spritePos.Y + 10); // adjust the value here for animation speed
                    if (spritePos.Y > spriteSize.Y)
                    {
                        sliding = SLIDE_DIRECTION.NO_SLIDE;
                        slide_timer.Stop();
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
                case SLIDE_DIRECTION.SLIDE_DOWN:
                    drawBackground(s, spriteTex, new Rectangle(new Vector2(spritePos.X, spritePos.Y - spriteSize.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case SLIDE_DIRECTION.SLIDE_LEFT:
                    drawBackground(s, spriteTex, new Rectangle(new Vector2(spritePos.X + spriteSize.X, spritePos.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case SLIDE_DIRECTION.SLIDE_RIGHT:
                    drawBackground(s, spriteTex, new Rectangle(new Vector2(spritePos.X - spriteSize.X, spritePos.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
                    break;
                case SLIDE_DIRECTION.SLIDE_UP:
                    drawBackground(s, spriteTex, new Rectangle(new Vector2(spritePos.X, spritePos.Y + spriteSize.Y).ToPoint(), spriteSize), new Color(1.0f, 1.0f, 1.0f));
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
