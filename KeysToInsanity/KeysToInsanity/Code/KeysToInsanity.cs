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
using System.IO;
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
            ChooseLevel,
            Playing,
            loading,
            Paused,
            About,
            Help,
            Credits,
            Death
            
        }

        public enum Boundary
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            Bottom = 4
        }

        // Some debug/default values
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
        public static HUD hud;
        private StartScreen startMenu;
        private PauseScreen pauseMenu;
        private CreditScreen creditScreen;
        private AboutScreen aboutScreen;
        private DeathScreen yourdead;
        private LevelSwitcher chooseLevelMenu;

        private string[] levelXMLs;
        private uint unlockedLevels = 1;

        private BasicInput input; // Our input handler

        public static Physics physics = new Physics();

        private Sound testSound;
        private Sound landedOnGround;

        //private horizontalPlatform platformH;

        //Setting constants for the menu items
        MouseState mouseState;
        MouseState previousMouseState;
        private GameState gameState;
        private bool gotKey;

        // Checkpoint logic
        private bool inCheckpoint = false;
        private bool enteredCheckpoint = false;

        // Insanity logic
        private float insanity = 0.0f;

        //Vector2 Scale = Vector2.One;

        public delegate void CollisionEventHandler(BasicSprite caller, BasicSprite collided, Rectangle data, GameTime time);

        public KeysToInsanity()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;    // set this value to the desired width of your window
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
            gameState = GameState.Playing;

            //Get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            levelXMLs = Directory.GetFiles(Content.RootDirectory + "\\Levels\\", "*.xml");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Loading the games menu buttons for menu screen
            startMenu = new StartScreen(this);
            pauseMenu = new PauseScreen(this);
            aboutScreen = new AboutScreen(this);
            creditScreen = new CreditScreen(this);
            yourdead = new DeathScreen(this);

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
            loader = new LevelLoader(this, "Content\\Levels\\Level2.xml", hud);
            loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key

            input = new BasicInput(this, theGentleman);

            //testSound = new Sound(this, "SoundFX\\Music\\Op9No2Session");
            //testSound.play(true);

            //landedOnGround = new Sound(this, "SoundFX\\TheGentleman\\LandedOnFloor");

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
                        theGentleman.inAir = false;
                        theGentleman.jumps = 2;
                        theGentleman.velocity = Velocity.FromCoordinates(theGentleman.velocity.getX(), 0.0f);
                        physics.resetTime(time);
                        //landedOnGround.play(false);
                    }
                if (collided.ToString() == "KeysToInsanity.Code.Nurse") // collided with Nurse
                {
                    theGentleman.health -= 10;
                }
                if (collided.ToString() == "KeysToInsanity.Code.Interactive_Objects.HatHanger")
                {
                    if (inCheckpoint)
                        enteredCheckpoint = false;
                    else
                        enteredCheckpoint = true;

                    inCheckpoint = true;
                }
            }
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
                //if (theGentleman.health <= 0)
                // game over

                // pause game logic while background is sliding
                if (loader.level.stages[stageIndex].background.slide == true)
                {
                    loader.level.stages[stageIndex].background.Update(gameTime);
                }
                else
                {
                    // Animated statics
                    foreach (AnimatedSprite s in loader.level.stages[stageIndex].animatedStatics)
                    {
                        s.updateWithAnimation(gameTime, 0);
                    }

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

                    insanity += 0.02f;
                    hud.updateInsanity(insanity);
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
                        if (stageIndex < loader.level.stages.Length)
                        {
                            enteredStageFromStart = true;
                            // slide background
                            loader.level.stages[stageIndex].background.slideIn(loader.level.stages[stageIndex].start, loader.level.stages[stageIndex - 1].background);
                            // fade in stuff
                            foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                                s.opacity = 0.0f;
                            // put gentleman in start position
                            theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                            physics.resetTime(gameTime);
                        }
                    }

                    // check gentleman has completed the level
                    if (stageIndex >= loader.level.stages.Length)
                    {
                        gameState = GameState.ChooseLevel;
                        if (unlockedLevels < levelXMLs.Length)
                            unlockedLevels++;
                        chooseLevelMenu.setUnlockedLevels(unlockedLevels);
                        base.Update(gameTime);
                        return;
                    }

                    // check gentleman has past the start stage boundary
                    if (checkStageBoundary(loader.level.stages[stageIndex].start))
                    {
                        stageIndex--;
                        enteredStageFromStart = false;
                        // slide background
                        loader.level.stages[stageIndex].background.slideIn(loader.level.stages[stageIndex].end, loader.level.stages[stageIndex + 1].background);
                        // fade in stuff
                        foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                            s.opacity = 0.0f;
                        // put gentleman in end position
                        theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].endX, loader.level.stages[stageIndex].endY);
                    }

                    // fade in lights
                    foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                    {
                        if (s.opacity < 1.0f)
                            s.opacity += 0.01f;
                        else
                            s.opacity = 1.0f;
                    }

                    // check gentleman has fallen somewhere he shouldnt have
                    if (checkOpposingBoundaries(loader.level.stages[stageIndex].start, loader.level.stages[stageIndex].end))
                    {
                        theGentleman.health -= 10;
                        if (enteredStageFromStart)
                            theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                        else
                            theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].endX, loader.level.stages[stageIndex].endY);
                    }

                    // check for key
                    if (gotKey)
                        if (stageIndex == loader.level.stageWithDoor)
                            loader.level.stages[stageIndex].door.setOpen(true);
                }
            } else if (gameState == GameState.ChooseLevel)
            {
                int i = chooseLevelMenu.Update(gameTime, mouseState);
                if (i >= 0)
                {
                    loader = new LevelLoader(this, levelXMLs[i], hud);
                    stageIndex = 0;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                    gotKey = false;
                    loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key
                    hud.updateHealth(100.0f);
                    hud.updateInsanity(0.0f);
                    gameState = GameState.Playing;
                }
            }

                base.Update(gameTime);
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
            hud.drawHUD(spriteBatch); // render HUD texture
            theGentleman.renderGentleman(spriteBatch);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //DEFAULT_SHADER.CurrentTechnique.Passes[0].Apply();

            //Checks if gameState is at StartMenu, draws the start menu
            if (gameState == GameState.StartMenu)
            {
                startMenu.drawMenu(spriteBatch);
            } else if (gameState == GameState.Paused) // paused
            {
                pauseMenu.drawMenu(spriteBatch);
            } else if (gameState == GameState.Playing) // playing
            {
                if (loader.level != null)
                {
                    Stage s = loader.level.stages[stageIndex];
                    if (s != null)
                    {
                        s.background.draw(spriteBatch);

                        foreach (AnimatedSprite ast in s.animatedStatics)
                        {
                            ast.draw(spriteBatch);
                        }

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
            } else if(gameState == GameState.About) // about
            {
                aboutScreen.drawMenu(spriteBatch);
            } else if(gameState == GameState.Help) // help
            {
                aboutScreen.drawMenu(spriteBatch);
            } else if(gameState == GameState.Credits) // credits
            {
                creditScreen.drawMenu(spriteBatch);
            } else if(gameState == GameState.ChooseLevel) // choose level
            {
                chooseLevelMenu.draw(spriteBatch);
            }
            if(gameState == GameState.Death)
            {
                yourdead.drawMenu(spriteBatch);
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
                //Rectangle for start button
                Rectangle startR = new Rectangle(350,
                    240,100, 20);                                                                                                                 

                Rectangle aboutR = new Rectangle(350, 290, 100, 20);

                Rectangle creditR = new Rectangle(350, 340, 100, 20);

                Rectangle exitR = new Rectangle(350,
                    390, 100, 20);
                //Checking if start button was clicked
                if (mouseClickR.Intersects(startR))
                {

                    gameState = GameState.Playing;


                }else if(mouseClickR.Intersects(aboutR))
                {
                    gameState = GameState.About;
                }else if (mouseClickR.Intersects(creditR))
                {
                    gameState = GameState.Credits;
                }else if (mouseClickR.Intersects(exitR)) //Player clicked exit button
                {
                    Exit();
                }
            }
            else if (gameState == GameState.Paused)
            {
                Rectangle resumeR = new Rectangle(350,
                    240, 100, 20);
                Rectangle helpR = new Rectangle(350, 290, 100, 20);
                Rectangle exitR = new Rectangle(350,340, 100, 20);
                    
                //Checking if start button was clicked
                if (mouseClickR.Intersects(resumeR))
                {
                    gameState = GameState.Playing;
                }else if(mouseClickR.Intersects(helpR))
                {
                    gameState = GameState.Help;
                }  else if (mouseClickR.Intersects(exitR))//Player clicked exit button
                {
                    Exit();
                }
            }
            
            else if(gameState == GameState.Help || gameState == GameState.About)
            {
              Rectangle returnR = new Rectangle(690,20,100,20);
                if(mouseClickR.Intersects(returnR) && gameState == GameState.Help)
                {
                    gameState = GameState.Paused;
                } else if (mouseClickR.Intersects(returnR)&& gameState == GameState.About)
                {
                    gameState = GameState.StartMenu;
                }  
            }
       
            else if (gameState == GameState.Credits)
            {
                Rectangle returnR = new Rectangle(690, 20, 100, 20);
                if(mouseClickR.Intersects(returnR))
                {
                    gameState = GameState.StartMenu;
                }
            }
            else if (gameState == GameState.Death)
            {
                Rectangle returnStartR = new Rectangle(250, 240, 300, 20);
                Rectangle restartCPR = new Rectangle(250, 290, 300, 20);
                Rectangle restartLR = new Rectangle(250, 340, 300, 20);
                Rectangle chooseLR = new Rectangle(250, 390, 300, 20);
                Rectangle exitR = new Rectangle(350, 440, 100, 20);

                if (mouseClickR.Intersects(returnStartR))
                {
                    gameState = GameState.StartMenu;
                }else if(mouseClickR.Intersects(restartCPR))
                {

                }else if(mouseClickR.Intersects(restartLR))
                {

                }else if (mouseClickR.Intersects(chooseLR))
                {

                }else if(mouseClickR.Intersects(exitR))
                {
                    Exit();
                }
            }
        }

    }
}
