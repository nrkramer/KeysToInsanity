using KeysToInsanity.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace KeysToInsanity
{
    class BasicSprite
    {

        public Texture2D spriteTex {
            get; }
        public Point spritePos {
            get; set; }
        public Point spriteSize;
        public Velocity velocity;

        // Load sprite from file, requires you pass in a game instance for content loading
        public BasicSprite(Game game, string file)
        {
            try {
                spriteTex = game.Content.Load<Texture2D>(file); // load the texture

                spritePos = new Point(0, 0); // initial position
                spriteSize = spriteTex.Bounds.Size; // get the size from the texture size
                velocity = Velocity.Zero;
                
            } catch (ContentLoadException e)
            {
                Console.WriteLine(file + " not loaded. Probably can't be found.\n");
            }
        }

        // Load sprite from existing texture
        public BasicSprite(Texture2D tex)
        {
            spriteTex = tex;
            spritePos = new Point(0, 0);
            spriteSize = spriteTex.Bounds.Size;
            velocity = Velocity.Zero;
        }

        // updates the position based on the current velocity
        public void updatePosition()
        {
            spritePos = getUpdatePosition();
        }

        // returns the position based on the current velocity without updating the sprites position
        public Point getUpdatePosition()
        {
            return spritePos + velocity.getDirection().ToPoint();
        }

        // "virtual" allows the method to be overriden by subclasses
        public virtual void draw(SpriteBatch s)
        {
            if (spriteTex != null)
            {
                Rectangle spriteBox = new Rectangle(spritePos, spriteSize);
                s.Draw(spriteTex, spriteBox, new Color(1.0f, 1.0f, 1.0f));
                if (KeysToInsanity.DRAW_BOUNDING_BOXES)
                    drawBorder(s, spriteBox, 2, Color.Red);
            }
        }

        private void drawBorder(SpriteBatch s, Rectangle box, int borderWidth, Color color)
        {
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Top, borderWidth, box.Height), color); // Left
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Right, box.Top, borderWidth, box.Height), color); // Right
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Top, box.Width, borderWidth), color); // Top
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Bottom, box.Width, borderWidth), color); // Bottom
        }
    }
}
