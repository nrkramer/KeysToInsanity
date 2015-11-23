using KeysToInsanity.Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace KeysToInsanity
{
    public class BasicSprite
    {
        public Texture2D spriteTex
        {
            get; set;
        }

        public Vector2 spritePos
        {
            get; set;
        }

        public Point spriteSize;
        public Velocity velocity;
        public bool collidable = false;
        public bool hidden = false;
        public Color color = Color.White;
        public float opacity = 1.0f;
        public float friction = 0.85f; // 15% friction for most objects
        protected Color borderColor = Color.Red;
        protected SpriteContainer container;
        public event KeysToInsanity.CollisionEventHandler collisionCallback;


        // Load sprite from file, requires you pass in a game instance for content loading
        // when subclassing BasicSprite you must create the same Constructors in the subclass with :base(parameters)
        // the parameters should match the ones here
        public BasicSprite(Game game, string file, bool collide)
        {
            try
            {
                spriteTex = game.Content.Load<Texture2D>(file); // load the texture
                spritePos = new Vector2(0, 0); // initial position
                spriteSize = spriteTex.Bounds.Size; // get the size from the texture size
                velocity = Velocity.Zero;
                collidable = collide;
                if (!collidable)
                    borderColor = Color.Blue;

            }
            catch (ContentLoadException)
            {
                Console.WriteLine(file + " not loaded. Probably can't be found.\n");
            }
        }

        // Load sprite from existing texture
        public BasicSprite(Texture2D tex, bool collide)
        {
            spriteTex = tex;
            spritePos = new Vector2(0, 0);
            spriteSize = spriteTex.Bounds.Size;
            velocity = Velocity.Zero;
            collidable = collide;
            if (!collidable)
                borderColor = Color.Blue;
        }

        // adds the sprite to a container - this is useful if the sprite
        // wishes to remove itself from that container, therefore not get drawn or something
        public void addTo(SpriteContainer container)
        {
            if (container != null)
                container.Add(this);
            this.container = container;
        }

        // updates the position based on the current velocity
        public void updatePosition()
        {
            spritePos = getUpdatePosition();
        }

        // returns the position based on the current velocity without updating the sprites position
        public Vector2 getUpdatePosition()
        {
            return spritePos + velocity.getDirection();
        }

        public void updatePositionFromVelocity(Velocity v)
        {
            spritePos = getUpdatePositionFromVelocity(v);
        }

        public Vector2 getUpdatePositionFromVelocity(Velocity v)
        {
            return spritePos + v.getDirection();
        }

        // gets collision data
        // first parameter is who i've collided with
        public virtual void onCollide(BasicSprite s, Rectangle data, GameTime time)
        {
            if (collisionCallback != null)
                collisionCallback(this, s, data, time);
        }

        public void applyFriction(BasicSprite s, Rectangle data)
        {
            if (s.collidable)
            {
                if (data.Height >= 1)
                {
                    velocity.setX(velocity.getX() / s.friction);
                }
            }
        }

        public float getSpriteXPos()
        {
            return spritePos.X;
        }

        public float getSpriteYPos()
        {
            return spritePos.Y;
        }

        public virtual void drawRotated(SpriteBatch s, float degrees)
        {
            s.Draw(spriteTex,
                new Rectangle((spritePos + new Vector2(spriteSize.X / 2.0f, spriteSize.Y / 2.0f)).ToPoint(), spriteSize),
                new Rectangle(new Point(0, 0), spriteSize), Color.White, degrees,
                new Vector2(spriteSize.X / 2.0f, spriteSize.Y / 2.0f), SpriteEffects.None, 1.0f);
        }

        // "virtual" allows the method to be overriden by subclasses
        public virtual void draw(SpriteBatch s)
        {
            Rectangle spriteBox = new Rectangle(spritePos.ToPoint(), spriteSize);
            if ((spriteTex != null) && !hidden)
            {
                s.Draw(spriteTex, spriteBox, null, color * opacity);
            }
            if (KeysToInsanity.DRAW_BOUNDING_BOXES)
                drawBorder(s, spriteBox, 2, borderColor);
        }

        protected void drawBorder(SpriteBatch s, Rectangle box, int borderWidth, Color color)
        {
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Top, borderWidth, box.Height), color); // Left
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Right, box.Top, borderWidth, box.Height), color); // Right
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Top, box.Width, borderWidth), color); // Top
            s.Draw(KeysToInsanity.BOUNDING_BOX, new Rectangle(box.Left, box.Bottom, box.Width, borderWidth), color); // Bottom
        }
    }
}
