using KeysToInsanity.Code;
using KeysToInsanity.Code.Interactive_Objects;
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
        private TheGentleman theGentleman; // Our main character sprite
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
            theGentleman = new TheGentleman(this);
            theGentleman.spritePos = new Point(100, 100);

            // static sprites - test code. To be replaced by a level loader (XML maybe)
            background = new BasicBackground(this, "padded_background");
            BasicSprite leftWall = new BasicSprite(this, "padded_wall_left", true);
            leftWall.spritePos = new Point(0, 0);
            leftWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height);
            BasicSprite rightWall = new BasicSprite(this, "padded_wall_right", true);
            rightWall.spritePos = new Point(GraphicsDevice.Viewport.Width - 30, 0);
            rightWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height);
            BasicSprite floor = new BasicSprite(this, "padded_floor", true);
            floor.spritePos = new Point(0, GraphicsDevice.Viewport.Height - 30);
            floor.spriteSize = new Point(GraphicsDevice.Viewport.Width, 30);
            Key key = new Key(this);
            key.spritePos = new Point(30, GraphicsDevice.Viewport.Height - 80);
            HatHanger hanger = new HatHanger(this);
            hanger.spritePos = new Point(550, GraphicsDevice.Viewport.Height - 220);
            BasicSprite bed = new BasicSprite(this, "bed", false);
            bed.spritePos = new Point(250, GraphicsDevice.Viewport.Height - 150);
            bed.spriteSize = new Point(200, 150);

            staticSprites.Add(floor);
            staticSprites.Add(rightWall);
            staticSprites.Add(leftWall);
            staticSprites.Add(key);
            staticSprites.Add(hanger);
            staticSprites.Add(bed);

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
            theGentleman.handleInput(gameTime);
            for (int i = 0; i < staticSprites.Count; i++)
            {
                // all-in-one collision detection/handling for input slip
                theGentleman.velocity = RectangleCollision.collisionWithSlip(theGentleman, staticSprites[i]);
            }
            theGentleman.updatePosition();

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
