using Microsoft.Xna.Framework;

namespace KeysToInsanity.Code
{
    class TheDoctor : AnimatedSprite
    {

        private int center;

        public TheDoctor(Game game, int posX, int posY) : base(game, "TopHat", new Point (posX,posY), 1,.25, false)
        {
            center = posX;
        }

        protected void Update()
        {
            
            if (getSpriteXPos() <= center + 150)
            {
                this.velocity = Velocity.FromDirection(0.0f, 4.5f);
            }
            else if (getSpriteXPos() >= center - 150)
            {
                this.velocity = Velocity.FromDirection(0.0f, -4.5f);
            }
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data,time);
            //Seeing if security hit the Gentleman
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //call method to handle damage and effects
            }
            //Checking to see if security hit a object
            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 0.0f);
                }

            }
        }
    }
}
