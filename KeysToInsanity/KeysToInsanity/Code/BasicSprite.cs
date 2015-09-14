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
        protected Point spriteSize;

        // Load sprite from file, requires you pass in a game instance for content loading
        public BasicSprite(Game game, string file)
        {
            try {
                spriteTex = game.Content.Load<Texture2D>(file); // load the texture

                spritePos = new Point(0, 0); // initial position
                spriteSize = spriteTex.Bounds.Size; // get the size from the texture size
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
        }

        // "virtual" allows the method to be overriden by subclasses
        public virtual void draw(SpriteBatch s)
        {
            if (spriteTex != null)
                s.Draw(spriteTex, spritePos.ToVector2(), new Color(1.0f, 1.0f, 1.0f));
        }
    }
}
