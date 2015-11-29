using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    class RectangleCollision
    {
        private static Rectangle data = new Rectangle();

        public static void update(BasicSprite character, SpriteContainer staticSprites, GameTime time)
        {
            for (int i = staticSprites.Count - 1; i >= 0; i--)
            {
                if (willCollide(character, staticSprites[i], time))
                {
                    character.velocity = collisionWithSlip(character, staticSprites[i], time);
                    character.onCollide(staticSprites[i], data, time);
                    staticSprites[i].onCollide(character, data, time);
                }
            }
            character.updatePosition();
        }

        public static void update(SpriteContainer characterSprites, SpriteContainer staticSprites, GameTime time)
        {
            foreach (BasicSprite cs in characterSprites)
            {
                for (int i = staticSprites.Count - 1; i >= 0; i--)
                {
                    // all-in-one collision detection/handling for input slip
                    if (willCollide(cs, staticSprites[i], time))
                    {
                        cs.velocity = collisionWithSlip(cs, staticSprites[i], time);
                        cs.onCollide(staticSprites[i], data, time);
                        staticSprites[i].onCollide(cs, data, time);
                    }
                }
                cs.updatePosition();
            }
        }

        public static bool willCollide(BasicSprite s1, BasicSprite s2, GameTime time)
        {
            if (s1 != s2)
            {
                Rectangle r = Rectangle.Intersect(new Rectangle(s1.getUpdatePosition().ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePosition().ToPoint(), s2.spriteSize));
                if ((r != Rectangle.Empty))
                {
                    if (!(s1.collidable && s2.collidable))
                    {
                        s1.onCollide(s2, r, time);
                        s2.onCollide(s1, r, time);
                    }
                    return true;
                }
            }
            return false;
        }

        public static Vector2 collisionDirection(BasicSprite s1, BasicSprite s2, GameTime time)
        {
            //returns the direction of how one collision box is colliding with another (i.e. The Gentleman hits the ground going straight down and the game recognizes that)
            if (willCollide(s1, s2, time))
                return (s2.velocity - s1.velocity).getDirection();
            else
                return Vector2.Zero;
        }

        public static Velocity collisionWithSlip(BasicSprite s1, BasicSprite s2, GameTime time)
        {
            if (s1.collidable && s2.collidable)
            {
                double vfx, vfy;
                Velocity v1 = Velocity.FromCoordinates(s1.velocity.getX(), 0.0f);
                Velocity v2 = Velocity.FromCoordinates(s2.velocity.getX(), 0.0f);
                Rectangle collision = Rectangle.Intersect(new Rectangle(s1.getUpdatePositionFromVelocity(v1).ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePositionFromVelocity(v2).ToPoint(), s2.spriteSize));
                if (collision != Rectangle.Empty)
                    vfx =  v1.getX() - collision.Width * Math.Sign(v1.getX());
                else
                    vfx = v1.getX();

                if (Math.Abs(vfx) < 1.0f)
                    vfx = 0.0f;

                data.X = collision.X;
                data.Width = (Math.Sign(v1.getX())) * collision.Width;

                v1 = Velocity.FromCoordinates((float)vfx, s1.velocity.getY());
                v2 = Velocity.FromCoordinates((float)vfx, s2.velocity.getY());

                collision = Rectangle.Intersect(new Rectangle(s1.getUpdatePositionFromVelocity(v1).ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePositionFromVelocity(v2).ToPoint(), s2.spriteSize));
                if (collision != Rectangle.Empty)
                    vfy = v1.getY() - (collision.Height * Math.Sign(v1.getY()));
                else
                    vfy = v1.getY();

                if (Math.Abs(vfy) < 1.0f) // stupid floats
                    vfy = 0.0f;

                data.Y = collision.Y;
                data.Height = Math.Sign(v1.getY()) * collision.Height;

                return Velocity.FromCoordinates((float)vfx, (float)vfy);
            }
            else
            {
                return s1.velocity;
            }
        }
    }
}
