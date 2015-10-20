using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KeysToInsanity.Code.Entitys
{
    class Nurse : AnimatedSprite
    {
 
        //Place were NPC moves around
        private float center;
        private float patrolDistance;
        private float patrolSpeed;
        Random random = new Random();

        public Nurse(Game game,float posX) : base(game, "nurse", new Point(22, 22), 1, 0, true)
        {
            //Setting the nurse posX to be center
            center = posX;
            patrolDistance = 50f;
            patrolSpeed = 1.0f;
        }

        public Nurse(Game game, float posX, float patrol, float speed):base(game,"nurse",new Point(22,22),1,0,true)
        {
            center = posX;
            patrolDistance = patrol;
            patrolSpeed = speed;

        }

        
        public  void Update(GameTime time)
        {
            int sign = random.Next(0, 2);
            if(getSpriteXPos() == center) {
                
                velocity = Velocity.FromDirection(0.0f, 45f);
            }
            //Deciding if we need to move to the left
            if( getSpriteXPos() <= center+patrolDistance)
            {
               // velocity = Velocity.FromDirection(0.0f, patrolSpeed);
            }else if ( getSpriteXPos() >=center-patrolDistance) //Or to the right
            {
                velocity = Velocity.FromDirection(0.0f, -patrolSpeed);
            }
           
            updateWithAnimation(time, 0);


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
                    this.velocity = Velocity.FromCoordinates(-this.velocity.getX(),0.0f);
                }
                
            }
        }
        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 22, 22), TimeSpan.FromSeconds(1.0));
            animations.Add(idle);
        }
    }
}
