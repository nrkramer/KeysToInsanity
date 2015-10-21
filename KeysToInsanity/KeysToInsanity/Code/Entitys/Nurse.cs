using Microsoft.Xna.Framework;
using System;


namespace KeysToInsanity.Code.Entitys
{
    class Nurse : Character
    {
 
        //Place were NPC moves around- right boarder of patrol
        private float center;
        //Far border of patrol
        private float patrolDistance;
        //How fast it is coming back
        private float patrolSpeed;
        
        private bool direction = true;

        public Nurse(Game game, float center, float patrol, float speed) : base(game, "nurse", new Point(22, 22), 1, 0, true)
        {
            this.center = center;
            patrolDistance = patrol;
            patrolSpeed = speed;
        }

        
        public override void Update(GameTime time)
        {
           // Console.WriteLine("Sprite Pos is" + getSpriteXPos());
           // Console.WriteLine("Center is" + center);
            
            if (direction == true)
            {

                velocity = Velocity.FromDirection(0.0f, patrolSpeed);
                if (getSpriteXPos() > center + patrolDistance)
            {
                direction = false;
                //Console.WriteLine(direction);
            }
            }
            else
            {
                velocity = Velocity.FromDirection(0.0f, -patrolSpeed);
            }
            if (getSpriteXPos() < center - patrolDistance)
            {
                direction = true;
            }
            
            updateWithAnimation(time, 0);
                                                                                      
        }
        //Overriding nurse to get it to cause damage || to get it to stop at obstacles
        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            //Seeing if nurse hit the Gentleman
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                //call method to handle damage and effects
            }
            //Checking to see if nurse hit a floor
            if (collided.collidable)
            {
                if (data.Width <= 0)
                {
                    velocity = Velocity.FromCoordinates(-velocity.getX(), 0.0f);
                }
                
            }
        }

        //Custom animation for nurse
        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 22, 22), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    }
}




