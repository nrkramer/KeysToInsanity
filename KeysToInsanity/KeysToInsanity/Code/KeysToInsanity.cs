using KeysToInsanity.Code;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KeysToInsanity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class KeysToInsanity : Game
    {
        // Some debug values
        public static bool DRAW_BOUNDING_BOXES = false; // Draw bounding boxes on all sprites
        public static Texture2D BOUNDING_BOX;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private BasicBackground background; // background
        private List<BasicSprite> staticSprites = new List<BasicSprite>(); // Our other sprites
        private BasicSprite theGentleman; // Our main character sprite
        private BasicInput input; // Our input handler

        public KeysToInsanity()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (DRAW_BOUNDING_BOXES)
            {
                BOUNDING_BOX = new Texture2D(GraphicsDevice, 1, 1);
                BOUNDING_BOX.SetData(new[] { Color.White });
            }

            // Gentleman
            theGentleman = new TheGentleman(this,"TopHat.png");

            // static sprites
            background = new BasicBackground(this, "Test_Background");
            BasicSprite platform = new BasicSprite(this, "platform");
            platform.spritePos = new Point(300, 280);
            platform.spriteSize = new Point(150, 30);

            staticSprites.Add(platform);

            /* for now, the input is created here, however later we will want
               to create it earlier in order to provide input before everything is loaded
            */
            input = new BasicInput(this, theGentleman);

            // TODO: use this.Content to load your game content here
            // ^ this is now being done in our Basic classes
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.defaultKeyboardHandler();
            for (int i = 0; i < staticSprites.Count; i++)
            {
                // all-in-one collision detection/handling for input slip
                theGentleman.updatePositionFromVelocity(RectangleCollision.collisionWithSlip(theGentleman, staticSprites[i]));
            }

            if (theGentleman.spritePos.X < 0)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_RIGHT);
                theGentleman.spritePos = new Point(GraphicsDevice.Viewport.Width - theGentleman.spriteSize.X, theGentleman.spritePos.Y);
            } else if (theGentleman.spritePos.X + theGentleman.spriteSize.X > GraphicsDevice.Viewport.Width)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_LEFT);
                theGentleman.spritePos = new Point(0, theGentleman.spritePos.Y);
            } else if (theGentleman.spritePos.Y + theGentleman.spriteSize.Y > GraphicsDevice.Viewport.Height)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_UP);
                theGentleman.spritePos = new Point(theGentleman.spritePos.X, 0);
            } else if (theGentleman.spritePos.Y < 0)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_DOWN);
                theGentleman.spritePos = new Point(theGentleman.spritePos.X, GraphicsDevice.Viewport.Height - theGentleman.spriteSize.Y);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            background.draw(spriteBatch);
            for (int i = 0; i < staticSprites.Count; i++)
            {
                staticSprites[i].draw(spriteBatch);
            }
            theGentleman.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
