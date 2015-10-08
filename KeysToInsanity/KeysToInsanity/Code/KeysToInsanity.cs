using KeysToInsanity.Code;
using KeysToInsanity.Code.Interactive_Objects;
using KeysToInsanity.Code.Interface;
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
        public static bool DRAW_MOVEMENT_VECTORS = false;
        public static Texture2D BOUNDING_BOX;
        public static Texture2D MOVEMENT_VECTOR;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private BasicBackground background; // background
        private SpriteContainer staticSprites = new SpriteContainer();
        private SpriteContainer characterSprites = new SpriteContainer(); // characters (nurses, gentleman, etc...)
        private SpriteContainer lightEffects = new SpriteContainer(); // light effects
        private TheGentleman theGentleman; // Our main character sprite
        private HUD hud;

        private Door testDoor;

        private BasicInput input; // Our input handler

        private Physics physics = new Physics();

        private Sound testSound;

        public delegate void GameEventHandler(object caller);
        //public event GameEventHandler gameEventHandeler;

        public KeysToInsanity()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           // graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
           // graphics.PreferredBackBufferHeight = 1080;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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

        public void testEvents(object caller)
        {
            if (caller.ToString() == "KeysToInsanity.Code.Interactive_Objects.Key")
            {
                Console.WriteLine("A Key was picked up!");
                testDoor.setOpen(true);
            }
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

            if (DRAW_MOVEMENT_VECTORS)
                MOVEMENT_VECTOR = Content.Load<Texture2D>("arrow");

            // Gentleman
            theGentleman = new TheGentleman(this);
            theGentleman.addTo(characterSprites);
            theGentleman.spritePos = new Vector2(370, 0);

            // Heads up display (HUD)
            hud = new HUD(this);

            // static sprites - test code. To be replaced by a level loader (XML maybe)
            background = new BasicBackground(this, "padded_background");
            BasicSprite leftWall = new BasicSprite(this, "padded_wall_left", true);
            leftWall.spritePos = new Vector2(0, 0);
            leftWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height);
            BasicSprite rightWall = new BasicSprite(this, "padded_wall_right", true);
            rightWall.spritePos = new Vector2(GraphicsDevice.Viewport.Width - 30, 0);
            rightWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height - 150);
            testDoor = new Door(this);
            testDoor.spritePos = new Vector2(GraphicsDevice.Viewport.Width - 25, GraphicsDevice.Viewport.Height - 150);
            testDoor.spriteSize = new Point(25, 120);
            BasicSprite floor = new BasicSprite(this, "padded_floor", true);
            floor.spritePos = new Vector2(0, GraphicsDevice.Viewport.Height - 30);
            floor.spriteSize = new Point(GraphicsDevice.Viewport.Width, 30);
            Key key = new Key(this, hud); // key requires a HUD to go to
            key.spritePos = new Vector2(30, GraphicsDevice.Viewport.Height - 80);
            key.eventCallback += new GameEventHandler(testEvents);
            HatHanger hanger = new HatHanger(this);
            hanger.spritePos = new Vector2(550, GraphicsDevice.Viewport.Height - 120);
            BasicSprite bed = new BasicSprite(this, "bed", false);
            bed.spritePos = new Vector2(350, GraphicsDevice.Viewport.Height - 60);
            bed.spriteSize = new Point(70, 55);

            floor.addTo(staticSprites);
            rightWall.addTo(staticSprites);
            leftWall.addTo(staticSprites);
            key.addTo(staticSprites);
            hanger.addTo(staticSprites);
            bed.addTo(staticSprites);
            testDoor.addTo(staticSprites);

            testDoor.doorLight.addTo(lightEffects);

            /* for now, the input is created here, however later we will want
               to create it earlier in order to provide input before everything is loaded
            */
            input = new BasicInput(this, theGentleman);

            //testSound = new Sound(this, "SoundFX/Music/Op9No2Session");
            //testSound.play(true);

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
            theGentleman.handleInput(gameTime); // input
            physics.Update(gameTime, characterSprites); // physics
            RectangleCollision.update(characterSprites, staticSprites); // collision

            if (theGentleman.spritePos.X < 0) // background slide
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_RIGHT);
            } else if (theGentleman.spritePos.X + theGentleman.spriteSize.X > GraphicsDevice.Viewport.Width)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_LEFT);
            } else if (theGentleman.spritePos.Y + theGentleman.spriteSize.Y > GraphicsDevice.Viewport.Height)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_UP);
            } else if (theGentleman.spritePos.Y < 0)
            {
                background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_DOWN);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            background.draw(spriteBatch);
            foreach (BasicSprite s in staticSprites)
            {
                s.draw(spriteBatch);
            }
            theGentleman.draw(spriteBatch);
            foreach (BasicSprite s in lightEffects)
            {
                s.draw(spriteBatch);
            }
            hud.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
