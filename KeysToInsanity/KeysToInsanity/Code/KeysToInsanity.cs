using KeysToInsanity.Code;
using KeysToInsanity.Code.Entitys;
using KeysToInsanity.Code.Interactive_Objects;
using KeysToInsanity.Code.Interface;
using KeysToInsanity.Code.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Threading;

namespace KeysToInsanity
{
    /// <summary>
    /// This is the main type for our game.
    /// </summary>
    public class KeysToInsanity : Game
    {
        enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }
        // Some debug values
        public static bool DRAW_BOUNDING_BOXES = true; // Draw bounding boxes on all sprites
        public static bool DRAW_MOVEMENT_VECTORS = false;
        public static Texture2D BOUNDING_BOX;
        public static Texture2D MOVEMENT_VECTOR;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private BasicBackground background; // background
        private SpriteContainer staticSprites = new SpriteContainer();
        private SpriteContainer characterSprites = new SpriteContainer(); // characters (nurses, gentleman, etc...)
        private SpriteContainer hPlatforms = new SpriteContainer(); // horizontally moving platforms
        private SpriteContainer vPlatforms = new SpriteContainer(); // vertically moving platforms
        private SpriteContainer lightEffects = new SpriteContainer(); // light effects
        private TheGentleman theGentleman; // Our main character sprite
        private Nurse nurse;
        private AttackDog dog;
        private HUD hud;

        private Door testDoor;

        private BasicInput input; // Our input handler

        private Physics physics = new Physics();

        private Sound testSound;

        private horizontalPlatform platformH;

        //Used for the menu
        private BasicSprite startButton;
        private BasicSprite exitButton;
        private BasicSprite logo;
        private BasicSprite resume;

        //Used for position of the menu        
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 logoPosition;
        private Vector2 resumePosition;
        const float aspectX= 800/1920;
        
        //Setting constants for the menu items       
        MouseState mouseState;
        MouseState previousMouseState;
        private GameState gameState;
        private bool gotKey;

        Vector2 Scale = Vector2.One;

        public delegate void CollisionEventHandler(BasicSprite caller, BasicSprite collided, Rectangle data, GameTime time);

        public KeysToInsanity()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
           
            
            Content.RootDirectory = "Content";


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


            //set the gamestate to the start menu
            gameState = GameState.StartMenu;

            //Get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            base.Initialize();
        }

        public void collisionEvents(BasicSprite caller, BasicSprite collided, Rectangle data, GameTime time)
        {
            //collision detection so we can manipulate gravity to simulate real jumping
            if (caller.ToString() == "KeysToInsanity.Code.Interactive_Objects.Key")
            {
                gotKey = true;
                //Console.WriteLine("A Key was picked up!");
                testDoor.setOpen(true);
            }

            if (caller.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                if (collided.collidable)
                    if (Math.Abs(data.Height) >= 1.0f)
                    {
                        //Console.WriteLine("The Gentleman has collided with the ground.");
                        physics.resetTime(time);
                    }
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
            //Loading the games menu buttons for menu screen
            logo = new BasicSprite(this,"logo",false);
            logo.spritePos = new Vector2(300,20);
            startButton = new BasicSprite(this,"start",false);
            startButton.spritePos = new Vector2(350, 240);        
            exitButton = new BasicSprite(this,"exit",false);
            exitButton.spritePos = new Vector2(350, 290);
            resume = new BasicSprite(this,"resume",false);
            resume.spritePos = new Vector2(350, 240);

            //to help us understand how the bounding boxes are working and how the vectors are being affected on mostly just the Gentleman
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
            theGentleman.spritePos = new Vector2(370, 300);
            theGentleman.collisionCallback += new CollisionEventHandler(collisionEvents);
            nurse = new Nurse(this, 300);
            nurse.addTo(characterSprites);
            nurse.spritePos = new Vector2(300, 560);
            //dog = new AttackDog(this);
            // dog.addTo(characterSprites);
            //dog.spritePos = new Vector2(250, 100);             




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
            BasicSprite ceiling = new BasicSprite(this, "padded_ceiling", true);
            ceiling.spritePos = new Vector2(0, 0);
            ceiling.spriteSize = new Point(GraphicsDevice.Viewport.Width, 30);
            testDoor = new Door(this);
            testDoor.spritePos = new Vector2(GraphicsDevice.Viewport.Width - 25, GraphicsDevice.Viewport.Height - 150);
            testDoor.spriteSize = new Point(25, 120);
            BasicSprite floor = new BasicSprite(this, "padded_floor", true);
            floor.spritePos = new Vector2(0, GraphicsDevice.Viewport.Height - 30);
            floor.spriteSize = new Point(GraphicsDevice.Viewport.Width, 30);
            Key key = new Key(this, hud); // key requires a HUD to go to
            key.spritePos = new Vector2(30, GraphicsDevice.Viewport.Height - 80);
            key.collisionCallback += new CollisionEventHandler(collisionEvents);
            HatHanger hanger = new HatHanger(this);
            hanger.spritePos = new Vector2(550, GraphicsDevice.Viewport.Height - 120);
            BasicSprite bed = new BasicSprite(this, "bed", false);
            bed.spritePos = new Vector2(350, GraphicsDevice.Viewport.Height - 60);
            bed.spriteSize = new Point(70, 55);
            platformH = new horizontalPlatform(this);
            platformH.spritePos = new Vector2(349, GraphicsDevice.Viewport.Height - 200);

            //we add them to the SpriteContainers here
            floor.addTo(staticSprites);
            rightWall.addTo(staticSprites);
            leftWall.addTo(staticSprites);
            ceiling.addTo(staticSprites);
            key.addTo(staticSprites);
            hanger.addTo(staticSprites);
            bed.addTo(staticSprites);
            testDoor.addTo(staticSprites);
            platformH.addTo(staticSprites);
            testDoor.doorLight.addTo(lightEffects);

            /* for now, the input is created here, however later we will want
               to create it earlier in order to provide input before everything is loaded
            */
            input = new BasicInput(this, theGentleman);


            //Song testSound = Content.Load<Song>("Beethoven_5thSymphony.mp3");
            //MediaPlayer.Play(testSound);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Font1 = Content.Load<SpriteFont>("Fonts/Kootenay");
            //FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

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
            //allows the game to know when the user has used the mouse to start the game
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
            //pause menu            
            if (Keyboard.GetState().IsKeyDown(Keys.P) == true)
            {
                gameState = GameState.Paused;

            }
            if (gameState == GameState.Paused)
            {
                if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                {
                    MouseClicked(mouseState.X, mouseState.Y);
                }
                previousMouseState = mouseState;
            }
            else if (gameState == GameState.Playing)
            {
                nurse.Update(gameTime);
                //dog.Update(gameTime);
                theGentleman.handleInput(gameTime); // input
                physics.Update(gameTime, characterSprites); // physics
                RectangleCollision.update(characterSprites, staticSprites, gameTime); // collision
                hud.Update(gameTime);
                //platformH.Update(gameTime, hPlatforms); // horizontal movement for platforms
                //RectangleCollision.update(characterSprites, hPlatforms, gameTime);


                if (theGentleman.spritePos.X < 0) // background slide
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_RIGHT);
                }
                else if (theGentleman.spritePos.X + theGentleman.spriteSize.X > GraphicsDevice.Viewport.Width)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_LEFT);
                }
                else if (theGentleman.spritePos.Y + theGentleman.spriteSize.Y > GraphicsDevice.Viewport.Height)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_UP);
                }
                else if (theGentleman.spritePos.Y < 0)
                {
                    background.slide(BasicBackground.SLIDE_DIRECTION.SLIDE_DOWN);
                }


                base.Update(gameTime);
            }
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
            if (gameState == GameState.StartMenu)
            {
                logo.draw(spriteBatch);
                startButton.draw(spriteBatch);
                exitButton.draw(spriteBatch);
                
            }

            if (gameState == GameState.Paused)
            {
                logo.draw(spriteBatch);
                resume.draw(spriteBatch);
                exitButton.draw(spriteBatch);
            }
            //checks if the gameState is at playing, draws the game
            if (gameState == GameState.Playing)
            {

                background.draw(spriteBatch);
                foreach (BasicSprite s in staticSprites)
                {
                    s.draw(spriteBatch);
                }
                foreach (AnimatedSprite s in characterSprites)
                {
                    s.draw(spriteBatch);
                }
                /*foreach (BasicSprite s in hPlatforms)
                 {
                     s.draw(spriteBatch);
                 }*/

                //nurse.draw(spriteBatch);
                //dog.draw(spriteBatch);

                //allows us to make the light effects in the game
                foreach (BasicSprite s in lightEffects)
                {
                    s.draw(spriteBatch);
                }
                hud.draw(spriteBatch);

                if (gotKey == true)
                {
                    string output = "You got a key!";
                    // Vector2 FontOrigin = Font1.MeasureString(output) / 2;
                    // spriteBatch.DrawString(Font1, output, FontPos, Color.Red, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);



        }

        void MouseClicked(int x, int y)
        {
            //Creates a rectangle around where the mouse clicked
            Rectangle mouseClickR = new Rectangle(x, y, 10, 10);
            Console.WriteLine(mouseClickR);

            //Checks the start menu
            if (gameState == GameState.StartMenu)
            {
                
                Rectangle startButtonR = new Rectangle((int)startButton.getSpriteXPos(),
                    (int)startButton.getSpriteYPos(),100, 20);
                Console.WriteLine(startButton.getSpriteXPos() + "," + startButton.getSpriteYPos());
                Console.WriteLine(startButtonR);
                Rectangle exitButtonR = new Rectangle((int)exitButton.getSpriteXPos(),
                    (int)exitButton.getSpriteXPos(), 100, 20);
                //Checking if start button was clicked
                if (mouseClickR.Intersects(startButtonR))
                {

                    gameState = GameState.Playing;


                }
                //Player clicked exit button
                else if (mouseClickR.Intersects(exitButtonR))
                {
                    Exit();
                }
            }
            else if (gameState == GameState.Paused)
            {
                Rectangle resumeR = new Rectangle((int)resume.getSpriteXPos(),
                    (int)resume.getSpriteYPos(), 100, 20);
                Rectangle exitButtonR = new Rectangle((int)exitButton.getSpriteXPos(),
                    (int)exitButton.getSpriteYPos(), 100, 20);
                //Checking if start button was clicked
                if (mouseClickR.Intersects(resumeR))
                {

                    gameState = GameState.Playing;


                }
                //Player clicked exit button
                else if (mouseClickR.Intersects(exitButtonR))
                {
                    Exit();
                }
            }
        }

    }
}
