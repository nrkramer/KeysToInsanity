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

        protected void nurseUpdate()
        {
            // However, the tank's behavior is more complicated than the mouse's, and so
            // the decision making process is a little different. 

            // First we have to use the current state to decide what the thresholds are
            // for changing state, as described in the doc.

            float nurseChaseThreshold = nurseChaseDistance;
            float nurseCaughtThreshold = nurseCaughtDistance;
            // if the tank is idle, he prefers to stay idle. we do this by making the
            // chase distance smaller, so the tank will be less likely to begin chasing
            // the cat.
            if (nurseState == nurseAiState.wandering)
            {
                nurseChaseThreshold -= nurseHysteresis / 2;
            }
            // similarly, if the tank is active, he prefers to stay active. we
            // accomplish this by increasing the range of values that will cause the
            // tank to go into the active state.
            else if (nurseState == nurseAiState.chasing)
            {
                nurseChaseThreshold += nurseHysteresis / 2;
                nurseCaughtThreshold -= nurseHysteresis / 2;
            }
            // the same logic is applied to the finished state.
            else if (nurseState == nurseAiState.caught)
            {
                nurseCaughtThreshold += nurseHysteresis / 2;
            }

            // Second, now that we know what the thresholds are, we compare the tank's 
            // distance from the cat against the thresholds to decide what the tank's
            // current state is.
            float distanceFromCat = Vector2.Distance(nursePosition, );
            if (distanceFromCat > nurseChaseThreshold)
            {
                // just like the mouse, if the tank is far away from the cat, it should
                // idle.
                nurseState = nurseAiState.wandering;
            }
            else if (distanceFromCat > nurseCaughtThreshold)
            {
                nurseState = nurseAiState.chasing;
            }
            else
            {
                nurseState = nurseAiState.caught;
            }

            // Third, once we know what state we're in, act on that state.
            float currentNurseSpeed;
            if (nurseState == nurseAiState.chasing)
            {
                // the tank wants to chase the cat, so it will just use the TurnToFace
                // function to turn towards the cat's position. Then, when the tank
                // moves forward, he will chase the cat.
                tankOrientation = TurnToFace(tankPosition, catPosition, tankOrientation,
                    TankTurnSpeed);
                currentNurseSpeed = MaxNurseSpeed;
            }
            else if (nurseState == nurseAiState.wandering)
            {
                // wander works just like the mouse's.
                Wander(tankPosition, ref tankWanderDirection, ref tankOrientation,
                    TankTurnSpeed);
                currentNurseSpeed = .25f * MaxNurseSpeed;
            }
            else
            {
                // this part is different from the mouse. if the tank catches the cat, 
                // it should stop. otherwise it will run right by, then spin around and
                // try to catch it all over again. The end result is that it will kind
                // of "run laps" around the cat, which looks funny, but is not what
                // we're after.
                currentNurseSpeed = 0.0f;
            }

            // this calculation is also just like the mouse's: we construct a heading
            // vector based on the tank's orientation, and then make the tank move along
            // that heading.
            Vector2 heading = new Vector2(
                (float)Math.Cos(tankOrientation), (float)Math.Sin(tankOrientation));
            nursePosition += heading * currentNurseSpeed;
        }

       

    }
}
