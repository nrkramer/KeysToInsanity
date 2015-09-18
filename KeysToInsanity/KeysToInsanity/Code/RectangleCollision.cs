using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class RectangleCollision
    {
        public static bool willCollide(BasicSprite s1, BasicSprite s2)
        {
            return new Rectangle(s1.getUpdatePosition(), s1.spriteSize).Intersects(new Rectangle(s2.getUpdatePosition(), s2.spriteSize));
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
                float vf1, vf2;
                Velocity v1 = Velocity.FromCoordinates(s1.velocity.getDirection().X, 0.0f);
                Velocity v2 = Velocity.FromCoordinates(s2.velocity.getDirection().X, 0.0f);
                if (new Rectangle(s1.getUpdatePositionFromVelocity(v1), s1.spriteSize).Intersects(new Rectangle(s2.getUpdatePositionFromVelocity(v2), s2.spriteSize)))
                    vf1 = 0.0f;
                else
                    vf1 = v1.getDirection().X;

                v1 = Velocity.FromCoordinates(0.0f, s1.velocity.getDirection().Y);
                v2 = Velocity.FromCoordinates(0.0f, s2.velocity.getDirection().Y);
                if (new Rectangle(s1.getUpdatePositionFromVelocity(v1), s1.spriteSize).Intersects(new Rectangle(s2.getUpdatePositionFromVelocity(v2), s2.spriteSize)))
                    vf2 = 0.0f;
                else
                    vf2 = v1.getDirection().Y;

                return Velocity.FromCoordinates(vf1, vf2);
            } else
            {
                return s1.velocity;
            }

        }
    }
}
