using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace KeysToInsanity.Code
{
    class TheGentleman : AnimatedSprite
    {

        private BasicInput input;
        public TheGentleman(Game game) : base(game, "Gentleman", new Point(17,35), 2, 0.25, true)
        {
            input = new BasicInput(game, this);
        }

        public void handleInput(GameTime time)
        {
            input.defaultKeyboardHandler();
            updateWithAnimation(time, 0);
        }

        public override void onCollide(BasicSprite s)
        {
        }

        public override void draw(SpriteBatch s)
        {
            base.draw(s);
            if (KeysToInsanity.DRAW_MOVEMENT_VECTORS)
                drawMovementVector(s);
            // Custom Gentleman drawing code.
        }

        private void drawMovementVector(SpriteBatch s)
        {
            Rectangle bounds = KeysToInsanity.MOVEMENT_VECTOR.Bounds;
            s.Draw(KeysToInsanity.MOVEMENT_VECTOR,
                new Rectangle((spritePos + new Vector2(spriteSize.X / 2.0f, spriteSize.Y / 2.0f)).ToPoint(), spriteSize),
                KeysToInsanity.MOVEMENT_VECTOR.Bounds, Color.Red, (float)velocity.getRotation(),
                new Vector2(bounds.Width / 2, bounds.Height / 2), SpriteEffects.None, 1.0f);
        }
        //use an update method to move ai in methods.
    }
}
