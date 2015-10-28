using KeysToInsanity.Code;
using KeysToInsanity.Code.Base;
using KeysToInsanity.Code.Entitys;
using KeysToInsanity.Code.Environment;
using KeysToInsanity.Code.Interactive_Objects;
using KeysToInsanity.Code.Interface;
using KeysToInsanity.Code.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public enum GameState
        {
            StartMenu,
            Playing,
            Paused
        }

        public enum Boundary
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            Bottom = 4
        }

        // Some debug values
        public static bool DRAW_BOUNDING_BOXES = false; // Draw bounding boxes on all sprites
        public static bool DRAW_MOVEMENT_VECTORS = false;
        public static Texture2D BOUNDING_BOX;
        public static Texture2D MOVEMENT_VECTOR;
        public static Effect DEFAULT_SHADER;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private LevelLoader loader;
        private int stageIndex = 0;

        private TheGentleman theGentleman; // Our main character sprite
        private bool enteredStageFromStart = true;
        private HUD hud;

        //private Door testDoor;

        private BasicInput input; // Our input handler

        private Physics physics = new Physics();

        private Sound testSound;

        //private horizontalPlatform platformH;

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

        //Setting constants for the menu items
        MouseState mouseState;
        MouseState previousMouseState;
        private GameState gameState;
        private bool gotKey;

        //Vector2 Scale = Vector2.One;

        public delegate void CollisionEventHandler(BasicSprite caller, BasicSprite collided, Rectangle data, GameTime time);

        public KeysToInsanity()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
           if (!graphics.IsFullScreen)
            {
                //graphics.ToggleFullScreen();
            }
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
                Console.WriteLine("A Key was picked up!");
            }

            if (caller.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                if (collided.collidable)
                    if (Math.Abs(data.Height) >= 1.0f)
                    {
                        theGentleman.jumping = false;
                        theGentleman.velocity = Velocity.FromCoordinates(theGentleman.velocity.getX(), 0.0f);
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

            DEFAULT_SHADER = Content.Load<Effect>("Shaders\\DefaultShader.mgfx");

            // Gentleman
            theGentleman = new TheGentleman(this);
            theGentleman.spritePos = new Vector2(370, 300);
            theGentleman.collisionCallback += new CollisionEventHandler(collisionEvents);

            // Heads up display (HUD)
            hud = new HUD(this);

            // Load level
            loader = new LevelLoader(this, "Content\\Levels\\Level1.xml", hud);
            loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key

            input = new BasicInput(this, theGentleman);

            testSound = new Sound(this, "SoundFX\\Music\\Op9No2Session.wav");
            testSound.play(true);

            spriteBatch = new SpriteBatch(GraphicsDevice);
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
                foreach (Platform f in loader.level.stages[stageIndex].platforms)
                {
                    f.Update(gameTime);
                    f.updatePosition();
                }

                theGentleman.handleInput(gameTime); // input

                foreach (Character c in loader.level.stages[stageIndex].characters)
                {
                    c.Update(gameTime);
                }

                hud.Update(gameTime);

                // gentleman physics
                physics.UpdateGentlemanPhysics(gameTime, theGentleman);

                // non-gentleman character physics
                physics.Update(gameTime, loader.level.stages[stageIndex].characters);

                // collision for gentleman against static sprites
                RectangleCollision.update(theGentleman, loader.level.stages[stageIndex].collidables, gameTime);

                // collision for non-gentleman characters (doesn't check for key and doors)
                RectangleCollision.update(loader.level.stages[stageIndex].characters, loader.level.stages[stageIndex].statics, gameTime);

                // check gentleman has past the end stage boundary
                if (checkStageBoundary(loader.level.stages[stageIndex].end))
                {
                    stageIndex++;
                    enteredStageFromStart = true;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                    physics.resetTime(gameTime);
                }

                // check gentleman has past the start stage boundary
                if (checkStageBoundary(loader.level.stages[stageIndex].start))
                {
                    stageIndex--;
                    enteredStageFromStart = false;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].endX, loader.level.stages[stageIndex].endY);
                }

                // check gentleman has fallen somewhere he shouldnt have
                if (checkOpposingBoundaries(loader.level.stages[stageIndex].start, loader.level.stages[stageIndex].end))
                {
                    if (enteredStageFromStart)
                        theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                    else
                        theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].endX, loader.level.stages[stageIndex].endY);
                }

                // check for key
                if (gotKey)
                    if (stageIndex == loader.level.stageWithDoor)
                        loader.level.stages[stageIndex].door.setOpen(true);

                base.Update(gameTime);
            }
        }

        // magic. do not change
        public bool checkOpposingBoundaries(Boundary b1, Boundary b2)
        {
            if ((b1 == Boundary.Left && b2 == Boundary.Right) || (b2 == Boundary.Left && b1 == Boundary.Right))
                return checkStageBoundary(Boundary.Top) || checkStageBoundary(Boundary.Bottom);
            else if ((b1 == Boundary.Left && b2 == Boundary.Top) || (b2 == Boundary.Left && b1 == Boundary.Top))
                return checkStageBoundary(Boundary.Right) || checkStageBoundary(Boundary.Bottom);
            else if ((b1 == Boundary.Left && b2 == Boundary.Bottom) || (b2 == Boundary.Left && b1 == Boundary.Bottom))
                return checkStageBoundary(Boundary.Right) || checkStageBoundary(Boundary.Top);
            else if ((b1 == Boundary.Right && b2 == Boundary.Top) || (b2 == Boundary.Right && b1 == Boundary.Top))
                return checkStageBoundary(Boundary.Left) || checkStageBoundary(Boundary.Bottom);
            else if ((b1 == Boundary.Right && b2 == Boundary.Bottom) || (b2 == Boundary.Right && b1 == Boundary.Bottom))
                return checkStageBoundary(Boundary.Left) || checkStageBoundary(Boundary.Top);
            else if ((b1 == Boundary.Top && b2 == Boundary.Bottom) || (b2 == Boundary.Top && b1 == Boundary.Bottom))
                return checkStageBoundary(Boundary.Top) || checkStageBoundary(Boundary.Bottom);
            else return false;
        }

        // check the gentleman has left the stage
        public bool checkStageBoundary(Boundary b)
        {
            switch (b)
            {
                case Boundary.None:
                    return false;
                case Boundary.Left:
                    if (theGentleman.spritePos.X + theGentleman.spriteSize.X < loader.level.stages[stageIndex].stageX)
                        return true;
                    break;
                case Boundary.Right:
                    if (theGentleman.spritePos.X > loader.level.stages[stageIndex].stageWidth)
                        return true;
                    break;
                case Boundary.Top:
                    if (theGentleman.spritePos.Y + theGentleman.spriteSize.Y < loader.level.stages[stageIndex].stageY)
                        return true;
                    break;
                case Boundary.Bottom:
                    if (theGentleman.spritePos.Y > loader.level.stages[stageIndex].stageHeight)
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            //DEFAULT_SHADER.CurrentTechnique.Passes[0].Apply();

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
                if (loader.level != null)
                {
                    Stage s = loader.level.stages[stageIndex];
                    if (s != null)
                    {
                        s.background.draw(spriteBatch);
                        foreach (BasicSprite st in s.statics)
                        {
                            st.draw(spriteBatch);
                        }

                        foreach (BasicSprite pl in s.platforms)
                        {
                            pl.draw(spriteBatch);
                        }

                        if (s.key != null)
                            s.key.draw(spriteBatch);

                        if (s.door != null)
                            s.door.draw(spriteBatch);

                        foreach (AnimatedSprite sp in s.characters)
                {
                            sp.draw(spriteBatch);
                }

                        theGentleman.draw(spriteBatch);

                        foreach (LightEffect le in s.lights)
                {
                            le.draw(spriteBatch);
                }

                hud.draw(spriteBatch);
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void MouseClicked(int x, int y)
        {
            //Creates a rectangle around where the mouse clicked
            Rectangle mouseClickR = new Rectangle(x, y, 10, 10);
            if (graphics.IsFullScreen)
            {
                mouseClickR.X = (int)(mouseClickR.X * (GraphicsDevice.Viewport.Bounds.Width / (float)Window.ClientBounds.Width));
                mouseClickR.Y = (int)(mouseClickR.Y * (GraphicsDevice.Viewport.Bounds.Height / (float)Window.ClientBounds.Height));
            }

            //Checks the start menu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonR = new Rectangle((int)startButton.getSpriteXPos(),
                    (int)startButton.getSpriteYPos(),100, 20);
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
