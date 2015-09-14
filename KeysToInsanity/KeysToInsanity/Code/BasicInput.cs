using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeysToInsanity.Code
{
    // this class basically manages any sprites that you want to move with user interaction
    class BasicInput
    {
        private BasicSprite sprite;
        private Game game;

        public BasicInput(Game game, BasicSprite sprite)
        {
            this.game = game;
            this.sprite = sprite;
        }

        public void defaultKeyboardHandler()
        {
            GamePadButtons b = GamePad.GetState(PlayerIndex.One).Buttons; // gamepad controller support (may get rid of)
            KeyboardState kb = Keyboard.GetState();

            // if either escape is pressed, or, on a gamepad, back is pressed
            if (escDown(kb) || gamepadBackPressed(b))
                game.Exit();

            if (sprite != null)
            {
                // here, you can change how fast the sprites move
                int xVelocity = 5;
                int yVelocity = 5;

                int xDiff = 0;
                int yDiff = 0;
                if (leftDown(kb))
                    xDiff -= xVelocity;
                if (rightDown(kb))
                    xDiff += xVelocity;
                if (upDown(kb))
                    yDiff -= yVelocity;
                if (downDown(kb))
                    yDiff += yVelocity;

                sprite.spritePos = new Point(sprite.spritePos.X + xDiff, sprite.spritePos.Y + yDiff);
            }
        }

        private bool leftDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Left);
        }

        private bool rightDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Right);
        }

        private bool upDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Up);
        }

        private bool downDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Down);
        }

        private bool escDown(KeyboardState kb)
        {
            return kb.IsKeyDown(Keys.Escape);
        }

        private bool gamepadBackPressed(GamePadButtons b)
        {
            return (b.Back == ButtonState.Pressed);
        }

    }
}
