using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KeysToInsanity.Code
{
    class Nurse : AnimatedSprite
    {
  
        enum nurseAiState
        {
            //For when a nurse is chasing the player
            chasing,
            //For when a nurse has caught a player
            caught,
            //For when a nurse can't see a player
            wandering
        }

        //How fast a nurse can move
        const float nurseSpeed = 5.0f;

        //How far a nurse can chase the player
        const float nurseChaseDistance = 50.0f;

        //How far a nurse will stop after catching the player
        const float nurseCaughtDistance = 60.0f;

        //Helps to smooth the movement of the AI
        const float nurseHysteresis = 50.0f;

        //Sets nurse state to defualt
        nurseAiState nurseState = nurseAiState.wandering;
       
        private Vector2 nursePosition;

        public Nurse(Game game) : base(game, "TopHat", new Point(72, 71), 1, .25, false)
        {
            
        }

        protected  void Update()
        {
            nursePosition = getUpdatePosition();
            nurseUpdate();

           
        }

        private void nurseUpdate()
        {
           /* int nurseCenterX, nurseRightCenterY;
            nurseCenterX = pongoidY + (texPongoid.Height / 2);
            nurseRightCenterY = paddleRightY + (texPaddleRight.Height / 2);

            // Move left or right? Or not at all?
            if (nurseCenterX > (paddleRightY + texPaddleRight.Height / 2)) // Need to move left
            {
                paddleRightY += ySpeedP2;
            }
            else if (pongoidCenterY < paddleRightY + texPaddleRight.Height / 2) // Move right
            {
                paddleRightY -= ySpeedP2;
            }*/
        }



    }
}
