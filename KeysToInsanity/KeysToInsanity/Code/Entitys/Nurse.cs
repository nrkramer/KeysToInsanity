using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KeysToInsanity.Code
{
    class Nurse : AnimatedSprite
    {
 
        //Place were NPC moves around
        private int center;
        

        public Nurse(Game game, int posX, int posY) : base(game, "nurse", new Point(posX, posY), 1, .25, false)
        {
            //Setting the nurse posX to be center
            center = posX;
        }

        protected  void Update()
        {
            
            //Deciding if we need to move to the left
            if( getSpriteXPos() <= center+50)
            {
                this.velocity = Velocity.FromDirection(0.0f, 2.5f);
            }else if ( getSpriteXPos() >=center-50) //Or to the right
            {
                this.velocity = Velocity.FromDirection(0.0f, -2.5f);
            }


        }
        //Overriding nurse to get it to cause damage || to get it to stop at obstacles
        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {

            base.onCollide(collided, data, time);
            //Seeing if nurse hit the Gentleman
            if(collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //call method to handle damage and effects
            }
            //Checking to see if nurse hit a floor
            if (collided.collidable)
            {if(data.Width <= 0)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 0.0f);
                }
                
            }
        }
    }
}
