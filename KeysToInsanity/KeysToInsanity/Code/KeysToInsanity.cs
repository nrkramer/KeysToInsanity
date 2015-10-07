using KeysToInsanity.Code;
using KeysToInsanity.Code.Interactive_Objects;
using KeysToInsanity.Code.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace KeysToInsanity
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class KeysToInsanity : Game
    {
        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }
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
        private TheGentleman theGentleman; // Our main character sprite
        private HUD hud;

        private BasicInput input; // Our input handler

        private Physics physics = new Physics();

        private Sound testSound;

        //Used for the menu ADR
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D pauseButton;
        private Texture2D resumeButton;
        private Texture2D loadingScreen;
        
        //Used for position of the menu ADR        
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 rusumeButtonPosition;
        //Setting constants for the menu items
        private const float OrbWidth = 50f;
        private const float OrbHeight = 50f;
        private float speed = 1.5f;
        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;
        private GameState gameState;

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
            //Enabling mouse pointer
            IsMouseVisible = true;

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

            //set the gamestate to the start menu
            gameState = GameState.StartMenu;

            //Get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            base.Initialize();
        }

        public void testEvents(object caller)
        {
            if (caller.ToString() == "KeysToInsanity.Code.Interactive_Objects.Key")
            {
                Console.WriteLine("A Key was picked up!");
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
            //Loading the games menu buttons for menu screen ADR
            startButton = Content.Load<Texture2D>("start");
            exitButton = Content.Load<Texture2D>("exit");


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
            theGentleman.spritePos = new Vector2(370, 790);

            // Heads up display (HUD)
            hud = new HUD(this, GraphicsDevice);

            // static sprites - test code. To be replaced by a level loader (XML maybe)
            background = new BasicBackground(this, "padded_background");
            BasicSprite leftWall = new BasicSprite(this, "padded_wall_left", true);
            leftWall.spritePos = new Vector2(0, 0);
            leftWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height);
            BasicSprite rightWall = new BasicSprite(this, "padded_wall_right", true);
            rightWall.spritePos = new Vector2(GraphicsDevice.Viewport.Width - 30, 0);
            rightWall.spriteSize = new Point(30, GraphicsDevice.Viewport.Height);
            BasicSprite door = new BasicSprite(this, "closed_door_left_metal", false);
            door.spritePos = new Vector2(GraphicsDevice.Viewport.Width - 35, GraphicsDevice.Viewport.Height - 140);
            door.spriteSize = new Point(25, 120);
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
            door.addTo(staticSprites);

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
                mouseState = Mouse.GetState();
                if(previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
               /* For when we have a loading manager
               if(gameState == GameState.Playing && isLoading)
               {
                LoadGame();
                isLoading = false;
               }
               */

                theGentleman.handleInput(gameTime); // input
                physics.Update(gameTime, characterSprites); // physics
                RectangleCollision.update(characterSprites, staticSprites); // collision

                if (theGentleman.spritePos.X < 0) // background slide
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_RIGHT);
                    theGentleman.spritePos = new Vector2(GraphicsDevice.Viewport.Width - theGentleman.spriteSize.X, theGentleman.spritePos.Y);
                } else if (theGentleman.spritePos.X + theGentleman.spriteSize.X > GraphicsDevice.Viewport.Width)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_LEFT);
                    theGentleman.spritePos = new Vector2(0, theGentleman.spritePos.Y);
                } else if (theGentleman.spritePos.Y + theGentleman.spriteSize.Y > GraphicsDevice.Viewport.Height)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_UP);
                    theGentleman.spritePos = new Vector2(theGentleman.spritePos.X, 0);
                } else if (theGentleman.spritePos.Y < 0)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_DOWN);
                    theGentleman.spritePos = new Vector2(theGentleman.spritePos.X, GraphicsDevice.Viewport.Height - theGentleman.spriteSize.Y);
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

                spriteBatch.Begin();

            //Checks if gameState is at StartMenu, draws the start menu
               if(gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            }


            //checks if the gameState is at playing, draws the game
            if (gameState == GameState.Playing)
            {

                background.draw(spriteBatch);
                foreach (BasicSprite s in staticSprites)
                {
                    s.draw(spriteBatch);
                }
                theGentleman.draw(spriteBatch);
                hud.draw(spriteBatch);
            }
                spriteBatch.End();

                base.Draw(gameTime);

            
            
        }

    void MouseClicked(int x, int y)
        {
            //Creates a rectangle around where the mouse clicked
            Rectangle mouseClickR = new Rectangle(x,y,10,10);

            //Checks the start menu
            if(gameState == GameState.StartMenu)
            {
                Rectangle startButtonR = new Rectangle((int)startButtonPosition.X,
                    (int)exitButtonPosition.Y, 100, 20);
                Rectangle exitButtonR = new Rectangle((int)exitButtonPosition.X,
                    (int)exitButtonPosition.Y, 100, 20);
                //Checking if start button was clicked
                if(mouseClickR.Intersects(startButtonR))
                {
                    //gameState.Loading;
                    gameState = GameState.Playing;
                
                    //For when we have a loading manager
                   // isLoading = true;
                }
                //Player clicked exit button
                else if(mouseClickR.Intersects(exitButtonR))
                {
                    Exit();
                }
            }
        }

    }
}
