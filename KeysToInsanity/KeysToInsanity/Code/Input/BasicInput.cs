using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KeysToInsanity.Code
{
    // this class basically manages any sprites that you want to move with user interaction
    class BasicInput
    {
        private BasicSprite sprite;
        private Game game;
        public KeyboardState IKBS; // immediate keyboard state
        public KeyboardState OKBS; // original keyboard state

        public BasicInput(Game game, BasicSprite sprite)
        {
            this.game = game;
            this.sprite = sprite;
        }

        public virtual void defaultKeyboardHandler()
        {
            GamePadButtons b = GamePad.GetState(PlayerIndex.One).Buttons; // gamepad controller support (may get rid of)
            IKBS = Keyboard.GetState();

            // if either escape is pressed, or, on a gamepad, back is pressed
            if (escDown(IKBS) || gamepadBackPressed(b))
                game.Exit();

            if (sprite != null)
            {
                // here, you can change how fast the sprite moves
                int xVelocity = 5;
                int yVelocity = 20;

                int xDiff = 0;
                int yDiff = 0;

                if (leftDown(IKBS))
                    xDiff -= xVelocity;
                if (rightDown(IKBS))
                    xDiff += xVelocity;
                if (upDown(IKBS))
                    yDiff -= yVelocity;
                if (downDown(IKBS))
                    yDiff += yVelocity;
                if (spaceDown(IKBS))
                    yDiff += -4*yVelocity;
               

                //Velocity jumpVelocity = Velocity.FromDirection(90, yDiff);
                sprite.velocity = Velocity.FromCoordinates(xDiff, yDiff); //+ jumpVelocity;
            }
            OKBS = IKBS;
        }

        public bool leftDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Left);
        }

        public bool rightDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Right);
        }

        public bool upDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Up);
        }

        public bool downDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Down);
        }

        public bool escDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Escape);
        }

        public bool spaceDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Space) && !OKBS.IsKeyDown(Keys.Space);
        }

        public bool gamepadBackPressed(GamePadButtons b)
        {
            return (b.Back == ButtonState.Pressed);
        }

    }
}
