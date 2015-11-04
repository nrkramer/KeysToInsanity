using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace KeysToInsanity.Code
{
    public class TheGentleman : AnimatedSprite
    {
        private RenderTarget2D renderTarget; // override default spriteTex (which is a Texture2D)
        private Effect effect;
        private BasicInput input;

        private GraphicsDevice gd;

        public int jumps = 2;
        private float _health = 100.0f;
        public float health {
            set { _health = value; KeysToInsanity.hud.updateHealth(value); }
            get { return _health; }
        }

        public TheGentleman(Game game) : base(game, "Samus_fixed", new Point(32, 48), 4, 0.1, true)
        {
            gd = game.GraphicsDevice;

            renderTarget = new RenderTarget2D(gd,
                spriteSize.X,
                spriteSize.Y);

            input = new BasicInput(game, this);
            effect = game.Content.Load<Effect>("Shaders\\Test_shader.mgfx");
        }

        public void handleInput(GameTime time)
        {
            //how The Gentleman is able to know where to move
            input.defaultKeyboardHandler();
            float xVelocity = velocity.getX();
            float yVelocity = velocity.getY();

            if (input.rightDown(input.kb))
            {
                xVelocity = 5.0f;
                updateWithAnimation(time, 1);
            }
            else if (input.leftDown(input.kb))
            {
                xVelocity = -5.0f;
                updateWithAnimation(time, 2);
            }
            else
            {
                xVelocity = 0.0f;
                updateWithAnimation(time, 0);
            }

            //allows the game to know when to apply gravity
            if (input.spaceDownOnce() && (jumps > 0))
            {
                KeysToInsanity.physics.resetTime(time);
                yVelocity = -10.0f;
                jumps -= 1;
            }

            velocity = Velocity.FromCoordinates(xVelocity, yVelocity);
        }

        public override void onCollide(BasicSprite s, Rectangle data, GameTime time)
        {
            base.onCollide(s, data, time);
        }

        public void onFallOutOfBounds()
        {
            health -= 10;
        }

        public void renderGentleman(SpriteBatch s)
        {
            gd.SetRenderTarget(renderTarget);
            gd.Clear(Color.Transparent);

            s.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Rectangle spriteBox = new Rectangle(spritePos.ToPoint(), spriteSize);
            s.Draw(spriteTex, Vector2.Zero, currentAnimation.CurrentRectangle, new Color(1.0f, 1.0f, 1.0f)); // add source rectangle

            s.End();
            //spriteTex = renderTarget;
            gd.SetRenderTarget(null);
        }

        public override void draw(SpriteBatch s)
        {
            // Custom Gentleman drawing code.
            //effect.CurrentTechnique.Passes[0].Apply();
            s.Draw(renderTarget, new Rectangle(spritePos.ToPoint(), spriteSize), Color.White);

            if (KeysToInsanity.DRAW_MOVEMENT_VECTORS)
                drawMovementVector(s);
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

        // custom animation loading for gentleman... could get complicated
        protected override void loadAnimations()
        {
            // idle
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 32, 48), TimeSpan.FromSeconds(1.0));

            // run right
            Animation runRight = new Animation();
            runRight.AddUniformStrip(new Rectangle(1, 48, 320, 48), new Point(34, 48), TimeSpan.FromSeconds(0.05));

            // run left
            Animation runLeft = new Animation();
            runLeft.AddUniformStrip(new Rectangle(1, 96, 320, 48), new Point(34, 48), TimeSpan.FromSeconds(0.05));

            // fall right
            Animation fallRight = new Animation();
            fallRight.AddUniformStrip(new Rectangle(1, 144, 320, 48), new Point(34, 48), TimeSpan.FromSeconds(0.02));

            // fall left
            Animation fallLeft = new Animation();
            fallLeft.AddUniformStrip(new Rectangle(1, 192, 320, 48), new Point(34, 48), TimeSpan.FromSeconds(0.02));

            animations.Add(idle);
            animations.Add(runRight);
            animations.Add(runLeft);
            animations.Add(fallRight);
            animations.Add(fallLeft);
        }


    }
}
