using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    public class Velocity
    {
        public const double DEG_TO_RAD = Math.PI / 180.0;
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
            v.direction = new Vector2(((float)Math.Cos(directionDegrees * DEG_TO_RAD) * speed), (float)Math.Sin(directionDegrees * DEG_TO_RAD) * speed);
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

        public double getRotation()
        {
            return Math.Atan2(direction.Y, direction.X);
        }

        public float getSpeed()
        {
            return speed;
        }

        public float getY()
        {
            return direction.Y;
        }

        public float getX()
        {
            return direction.X;
        }

        public void setY(float ySpeed)
        {
            direction.Y = ySpeed;
        }

        public void setX(float xSpeed)
        {
            direction.X = xSpeed;
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

        public static Velocity operator *(Velocity v1, Velocity v2)
        {
            return Velocity.FromCoordinates(v1.direction.X * v2.direction.X, v1.direction.Y * v2.direction.Y);
        }
    }
}
