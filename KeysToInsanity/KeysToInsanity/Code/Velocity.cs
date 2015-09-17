using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class Velocity
    {
        public static Velocity Zero { get { return Velocity.FromCoordinates(0.0f, 0.0f); } }
        private Vector2 direction;
        private float speed;

        // The constructor is made private so that factory methods are forced to be used
        private Velocity() { }

        // These are "factory" methods, they are used when you want to use parameters of the same type and amount
        // for class constructors
        // They are used by calling ClassName.FromMethod(PARAMETERS)
        // for example: Velocity myVelocity = Velocity.FromCoordinates(2, -2); which creates a velocity going towards 2, -2
        // or Velocity myVelocity = Velocity.FromDirection(270, 30); which creates a velocity going straight down at a speed of 30
        public static Velocity FromDirection(float directionDegrees, float speed)
        {
            Velocity v = new Velocity();
            v.direction = new Vector2((float)Math.Cos(directionDegrees) * speed, (float)Math.Sin(directionDegrees) * speed);
            v.speed = speed;
            return v;
        }

        public static Velocity FromCoordinates(float x, float y)
        {
            Velocity v = new Velocity();
            v.direction = new Vector2(x, y);
            v.speed = v.direction.Length();
            return v;
        }

        public Vector2 getDirection()
        {
            return direction;
        }

        public float getSpeed()
        {
            return speed;
        }

        public override String ToString()
        {
            return direction.ToString() + ", Speed:[" + Convert.ToString(speed) + "]";
        }

        // this is an operator function, in which we can define what '+', '-', '*', and '/' do
        public static Velocity operator +(Velocity v1, Velocity v2)
        {
            return Velocity.FromCoordinates(v1.direction.X + v2.direction.X, v1.direction.Y + v2.direction.Y);
        }

        public static Velocity operator -(Velocity v1, Velocity v2)
        {
            return Velocity.FromCoordinates(v1.direction.X - v2.direction.X, v1.direction.Y - v2.direction.Y);
        }
    }
}
