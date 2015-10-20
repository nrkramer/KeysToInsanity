using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace KeysToInsanity.Code
{
    class TheGentleman : AnimatedSprite
    {
        private BasicInput input;

        private bool jumping = false;

        public TheGentleman(Game game) : base(game, "SuperMetroidSamus", new Point(30, 56), 4, 0.1, true)
        {
            input = new BasicInput(game, this);
        }

        public void handleInput(GameTime time)
        {
            //how The Gentleman is able to know where to move
            input.defaultKeyboardHandler();
            if (input.rightDown(input.kb))
                updateWithAnimation(time, 1);
            else if (input.leftDown(input.kb))
                updateWithAnimation(time, 2);
            else
                updateWithAnimation(time, 0);

            //allows the game to know when to apply gravity
            if (input.spaceDown(input.kb))
            {
                jumping = true;
            }
        }

        //checks to see if The Gentleman is in the air or not
        public bool isJumping()
        {
            return jumping;
        }

        public override void onCollide(BasicSprite s, Rectangle data, GameTime time)
        {
            base.onCollide(s, data, time);
        }

        public override void draw(SpriteBatch s)
        {
            base.draw(s);
            if (KeysToInsanity.DRAW_MOVEMENT_VECTORS)
                drawMovementVector(s);
            // Custom Gentleman drawing code.
        }

        //development tool to allow us to see how the Gentleman is moving through a vector arrow
        private void drawMovementVector(SpriteBatch s)
        {
            Rectangle bounds = KeysToInsanity.MOVEMENT_VECTOR.Bounds;
            s.Draw(KeysToInsanity.MOVEMENT_VECTOR,
                new Rectangle((spritePos + new Vector2(spriteSize.X / 2.0f, spriteSize.Y / 2.0f)).ToPoint(), spriteSize),
                KeysToInsanity.MOVEMENT_VECTOR.Bounds, Color.Red, (float)velocity.getRotation(),
                new Vector2(bounds.Width / 2, bounds.Height / 2), SpriteEffects.None, 1.0f);
        }
        //use an update method to move ai in methods.

        // custom animation loading for gentleman... could get complicated
        protected override void loadAnimations()
        {
            // idle
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 30, 55), TimeSpan.FromSeconds(1.0));

            // run right
            Animation runRight = new Animation();
            runRight.AddUniformStrip(new Rectangle(0, 280, 420, 45), new Point(42, 55), TimeSpan.FromSeconds(0.05));

            // run left
            Animation runLeft = new Animation();
            runLeft.AddUniformStrip(new Rectangle(550, 225, 420, 45), new Point(42, 55), TimeSpan.FromSeconds(0.05));

            // fall right
            //Animation fallLeft = new Animation();
            //fallLeft.AddUniformHeightStrip(new Rectangle(368, 183, 327, 33), new int[9] {30, 39, 44, 44, 33, 30, 38, 40, 27}, TimeSpan.FromSeconds(0.05));

            animations.Add(idle);
            animations.Add(runRight);
            animations.Add(runLeft);
            //animations.Add(fallLeft);
        }


    }
}
