﻿using Microsoft.Xna.Framework;
using System;


namespace KeysToInsanity.Code.Entitys
{
    class Nurse : Character
    {

        //made chagnges
        //Place were NPC moves around- right boarder of patrol
        public float center;
        private float moveSpeed = 0.0f;
        private float moveDistance = 0.0f;
        private bool direction = true;
        private float damage = 5;

        public Nurse(Game game, float moveSpeed, float moveDistance,float XPos) :base(game,"nurse",new Point(22,22),1,0,true)
        { 
            // ****** DO NOT CHANGE MAKES IT WORK ********
            center = XPos;
            //********************************************
            this.moveDistance = moveDistance;
            this.moveSpeed = moveSpeed;
        }
       

        
        public override void Update(GameTime time)
        {
            //Console.WriteLine(direction);
            //Console.WriteLine("Center"+ center);
           // Console.WriteLine("XPOS" + getSpriteXPos());
            if (direction == true)
            {
                velocity = Velocity.FromCoordinates(moveSpeed, 0.0f);
                if (getSpriteXPos() > center + moveDistance)
                {
                    direction = false;
                }
            }
            else
            {
                if (getSpriteXPos() < center - moveDistance)
                {
                    direction = true;
                }velocity = Velocity.FromCoordinates(-moveSpeed, 0.0f);
                
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
                ((TheGentleman)collided).health -= damage;
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




