using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    class RectangleCollision
    {
        public static void update(SpriteContainer characterSprites, SpriteContainer staticSprites)
        {
            foreach (BasicSprite cs in characterSprites)
            {
                for (int i = staticSprites.Count - 1; i >= 0; i--)
                {
                    // all-in-one collision detection/handling for input slip
                    cs.velocity = collisionWithSlip(cs, staticSprites[i]);
                }
                cs.updatePosition();
            }
        }

        public static bool willCollide(BasicSprite s1, BasicSprite s2)
        {
            Rectangle r = Rectangle.Intersect(new Rectangle(s1.getUpdatePosition().ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePosition().ToPoint(), s2.spriteSize));
            if ((r != Rectangle.Empty))
            {
                if (!(s1.collidable && s2.collidable))
                {
                    s1.onCollide(s2, r);
                    s2.onCollide(s1, r);
                }
                return true;
            }
            else
                return false;
        }

        public static Vector2 collisionDirection(BasicSprite s1, BasicSprite s2)
        {
            if (willCollide(s1, s2))
                return (s2.velocity - s1.velocity).getDirection();
            else
                return Vector2.Zero;
        }

        public static Velocity collisionWithSlip(BasicSprite s1, BasicSprite s2)
        {
            if (willCollide(s1, s2))
            {
                Rectangle data = new Rectangle();
                if (s1.collidable && s2.collidable)
                {
                    float vf1, vf2;
                    Velocity v1 = Velocity.FromCoordinates(s1.velocity.getDirection().X, 0.0f);
                    Velocity v2 = Velocity.FromCoordinates(s2.velocity.getDirection().X, 0.0f);
                    Rectangle collision = Rectangle.Intersect(new Rectangle(s1.getUpdatePositionFromVelocity(v1).ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePositionFromVelocity(v2).ToPoint(), s2.spriteSize));
                    if (collision != Rectangle.Empty)
                        vf1 = (int)v1.getDirection().X + (Math.Sign(v1.getDirection().X) * -collision.Width);
                    else
                        vf1 = v1.getDirection().X;

                    data.X = collision.X;
                    data.Width = (Math.Sign(v1.getDirection().X)) * collision.Width;

                    v1 = Velocity.FromCoordinates(0.0f, s1.velocity.getDirection().Y);
                    v2 = Velocity.FromCoordinates(0.0f, s2.velocity.getDirection().Y);
                    collision = Rectangle.Intersect(new Rectangle(s1.getUpdatePositionFromVelocity(v1).ToPoint(), s1.spriteSize), new Rectangle(s2.getUpdatePositionFromVelocity(v2).ToPoint(), s2.spriteSize));
                    if (collision != Rectangle.Empty)
                        vf2 = (int)v1.getDirection().Y + (Math.Sign(v1.getDirection().Y) * -collision.Height);
                    else
                        vf2 = v1.getDirection().Y;

                    data.Y = collision.Y;
                    data.Height = Math.Sign(v1.getDirection().Y) * collision.Height;

                    //Console.WriteLine(data);
                    s1.onCollide(s2, data);
                    s2.onCollide(s1, data);

                    return Velocity.FromCoordinates(vf1, vf2);
                }
                else
                {
                    return s1.velocity;
                }
            } else
            {
                return s1.velocity;
            }
        }
    }
}
