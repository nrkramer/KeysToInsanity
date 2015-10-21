using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Entitys
{
    class Cop : Character
    {
        //Cops position
        private float center;      
        private bool direction;
        private float range;



        public Cop(Game game, float posX) : base(game, "TopHat", new Point(22, 22), 1, 0, true)
        {
            //Setting the cop posX to be center
            center = posX;            
            direction = true;

        }

        


        public override void Update(GameTime time)
        {
           /* if (theGentleman.getSpriteXPos() > center+range)
            {
                //turn right, fire
            }else if(TheGentleman.getSpriteXPos() < center-range)
            {
                //turn left, fire
            }*/
            


            

            updateWithAnimation(time, 0);

        }
        //Overriding NPC get it to cause damage || to get it to stop at obstacles
        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {

            base.onCollide(collided, data, time);
            //Seeing if NPC hit the Gentleman
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //call method to handle damage and effects
            }
            //Checking to see if Cop hit a floor
            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    velocity = Velocity.FromCoordinates(-this.velocity.getX(), 0.0f);
                }

            }
        }

        //Custom animation for Cop
        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 72, 71), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    
}

}
