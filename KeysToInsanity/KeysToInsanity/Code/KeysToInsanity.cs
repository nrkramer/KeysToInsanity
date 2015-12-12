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
            Paused,
            About,
            Help,
            Instructions,
            Credits,
            Death,
            Exit,
            Win,
            Checkpoint
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
        public static RenderTarget2D OFFSCREEN;
        public static Texture2D OFFSCREEN_TEX;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private LevelLoader loader;

        // All of the sprites, menus, and stuff that is essential to have here
        private TheGentleman theGentleman; // Our main character sprite
        private bool enteredStageFromStart = true;
        public static HUD hud;
        private StartScreen startMenu;
        private PauseScreen pauseMenu;
        private CreditScreen creditScreen;
        private AboutScreen aboutScreen;
        private InstructionScreen instructScreen;
        private DeathScreen yourdead;
        private LevelSwitcher chooseLevelMenu;
        private WinScreen winScreen;
        private bool showHelp = false;

        private string[] levelXMLs; // CHANGE THE FOLLOWING STUFF FOR TESTING
        private uint unlockedLevels = 5; // CHANGE THIS TO CHANGE THE AMOUNT OF UNLOCKED LEVELS
        private uint currentLevel = 4; // CHANGE THIS TO CHANGE THE CURRENT LEVEL
        private uint stageIndex = 0; // CHANGE THIS TO CHANGE THE CURRENT STAGE
        private GameState gameState = GameState.StartMenu; // CHANGE THIS TO CHANGE WHICH STATE YOU WANT TO TEST

        private BasicInput input; // Our input handler

        public static Physics physics = new Physics();

        private Sound testSound;
        private Sound landedOnGround;

        //Setting constants for the menu items
        MouseState mouseState;
        MouseState previousMouseState;
        KeyboardState keyboardState;
        private bool gotKey;

        // Checkpoint logic
        private bool inCheckpoint = false;
        private bool wasOutsideCheckpoint = false;
        private uint checkpointStageIndex = 0;
        private HatHanger currentCheckpoint = null;
        private Vector2 checkpointPosition = new Vector2(300, 400);
        private float checkpointHealth = 100.0f; // gentleman's health at checkpoint
        private float checkpointInsanity = 0.0f; // insanity at checkpoint

        // Insanity logic
        private float insanity = 0.0f;

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

            //Get input states
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            keyboardState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // offscreen render target
            OFFSCREEN = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            OFFSCREEN_TEX = new Texture2D(GraphicsDevice, OFFSCREEN.Width, OFFSCREEN.Height);

            levelXMLs = Directory.GetFiles("Content\\Levels\\", "*.xml");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Loading the games menu buttons for menu screen
            startMenu = new StartScreen(this);
            pauseMenu = new PauseScreen(this);
            aboutScreen = new AboutScreen(this);
            instructScreen = new InstructionScreen(this);
            creditScreen = new CreditScreen(this);
            chooseLevelMenu = new LevelSwitcher(this, (uint)levelXMLs.Length);
            chooseLevelMenu.setUnlockedLevels(unlockedLevels);
            yourdead = new DeathScreen(this);
            winScreen = new WinScreen(this);

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
            theGentleman.collisionCallback += new CollisionEventHandler(collisionEvents);

            // Heads up display (HUD)
            hud = new HUD(this);

            // Load level
            loader = new LevelLoader(this, levelXMLs[currentLevel - 1], hud);
            loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key
            theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
            foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                s.opacity = 0.0f;

            input = new BasicInput(this, theGentleman);

            landedOnGround = new Sound(this, "SoundFX\\TheGentleman\\LandedOnFloor");
            landedOnGround.pitch = 1.0f;

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
            }

            if (caller.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                if (collided.collidable)
                {
                    if (data.Height >= 1.0f)
                    {
                        if (theGentleman.inAir)
                          landedOnGround.play(false);

                        theGentleman.inAir = false;
                        theGentleman.jumps = 2;
                        theGentleman.velocity.setY(0.0f);
                        theGentleman.spritePos = new Vector2(theGentleman.spritePos.X, collided.spritePos.Y - theGentleman.spriteSize.Y);
                    }

                    if (collided.ToString() == "KeysToInsanity.Code.Interactive_Objects.Hazard")
                    {
                        theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                        theGentleman.jumps = 0;
                    }
                }
                if (collided.ToString() == "KeysToInsanity.Code.Nurse") // collided with Nurse
                {
                    theGentleman.health -= 10;
                }
                
                if (collided.ToString() == "KeysToInsanity.Code.Interactive_Objects.HatHanger")
                {
                    currentCheckpoint = (HatHanger)collided;
                    inCheckpoint = true;
                    checkpointPosition = currentCheckpoint.spritePos;
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
            // mouse info
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            showHelp = false;
            bool mouseClicked = false;
            Point mouseCoords = new Point(mouseState.X, mouseState.Y);
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                mouseClicked = true;
            }
            previousMouseState = mouseState;

            // global keyboard switches
            if (keyboardState.IsKeyDown(Keys.P))
            {
                gameState = GameState.Paused;

            } else if (keyboardState.IsKeyDown(Keys.H))
            {
                // show help
                showHelp = true;
            }
            if(gameState == GameState.Exit)
            {
                Exit();
            }

            if (gameState == GameState.StartMenu)
            {
                if (mouseClicked)
                    gameState = startMenu.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Paused)
            {
                if (mouseClicked)
                    gameState = pauseMenu.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.About)
            {
                if (mouseClicked)
                    gameState = aboutScreen.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Death)
            {
                if (mouseClicked)
                    gameState = yourdead.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Instructions)
            {
                if (mouseClicked)
                    gameState = instructScreen.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Win)
            {
                if (mouseClicked)
                    gameState = winScreen.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Credits)
            {
                if (mouseClicked)
                    gameState = creditScreen.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Checkpoint)
            {
                theGentleman.spritePos = new Vector2(checkpointPosition.X, checkpointPosition.Y + theGentleman.spriteSize.Y);
                stageIndex = checkpointStageIndex;
                theGentleman.health = checkpointHealth;
                insanity = checkpointInsanity;
                gameState = GameState.Playing;
            }
            else if (gameState == GameState.Playing)
            {
                if (theGentleman.health <= 0)
                {
                    gameState = GameState.Death;
                    theGentleman.health = 100.0f;
                    insanity = 0.0f;
                    stageIndex = 0;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                }

                inCheckpoint = false;

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

                    theGentleman.Update(gameTime); // gentleman input

                    foreach (Character c in loader.level.stages[stageIndex].characters)
                    {
                        c.Update(gameTime);
                    }

                    insanity += 0.01f;
                    hud.updateInsanity(insanity);
                    hud.Update(gameTime);

                    // gentleman physics
                    physics.UpdateGentlemanPhysics(gameTime, theGentleman);

                    // non-gentleman character physics
                    physics.Update(gameTime, loader.level.stages[stageIndex].characters);
                    physics.Update(gameTime, loader.level.stages[stageIndex].gravitySprites);

                    // collision for gentleman against static sprites
                    RectangleCollision.update(theGentleman, loader.level.stages[stageIndex].collidables, gameTime);

                    // collision for non-gentleman characters
                    RectangleCollision.update(loader.level.stages[stageIndex].characters, loader.level.stages[stageIndex].statics, gameTime);

                    // collision for sprites that have gravity, and are collidable
                    RectangleCollision.update(loader.level.stages[stageIndex].gravitySprites, loader.level.stages[stageIndex].statics, gameTime);

                    // CHECKPOINT LOGIC
                    if (currentCheckpoint != null)
                        currentCheckpoint.Update(gameTime);

                    if (wasOutsideCheckpoint && inCheckpoint)
                    {
                        if (currentCheckpoint != null)
                            currentCheckpoint.playAnimation = true;
                        checkpointStageIndex = stageIndex;
                        checkpointHealth = theGentleman.health;
                        checkpointInsanity = insanity;
                        // position is handled in the collision handler
                    }

                    if (!inCheckpoint)
                        wasOutsideCheckpoint = true;
                    else
                        wasOutsideCheckpoint = false;

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
                            // play sounds
                            if (stageIndex > 0)
                                loader.level.stages[stageIndex - 1].stopSounds();
                            loader.level.stages[stageIndex].playSounds();
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
                            if (unlockedLevels == currentLevel)
                                unlockedLevels++;
                        chooseLevelMenu.setUnlockedLevels(unlockedLevels);
                        insanity = 0.0f;
                        theGentleman.health = 100.0f;
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
                        // play sounds
                        if (stageIndex < loader.level.stages.Length)
                            loader.level.stages[stageIndex + 1].stopSounds();
                        loader.level.stages[stageIndex].playSounds();
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
            } else if (gameState == GameState.ChooseLevel) // choose level
            {
                int i = chooseLevelMenu.Update(gameTime, mouseState);
                if (i >= 0)
                {
                    loader.level.stopMusic();
                    loader = new LevelLoader(this, levelXMLs[i], hud); // load new level
                    currentLevel = (uint)i + 1; // calculate current level
                    stageIndex = 0; // set to first stage
                    // re-position Gentleman
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                    // no longer has a key
                    gotKey = false;
                    // re-assign key collision
                    loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key
                    // reset health and insanity
                    theGentleman.health = 100.0f;
                    insanity = 0.0f;
                    hud.resetHUD(); // reset HUD
                    // fade in stuff
                    foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                        s.opacity = 0.0f;
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
            GraphicsDevice.SetRenderTarget(OFFSCREEN);

            hud.drawHUD(spriteBatch); // render HUD texture
            theGentleman.renderGentleman(spriteBatch);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            //Checks if gameState is at StartMenu, draws the start menus
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
                    // draw stage
                    loader.level.stages[stageIndex].drawStage(spriteBatch);
                    // draw gentleman
                    theGentleman.draw(spriteBatch);
                    // draw lights
                    loader.level.stages[stageIndex].drawLights(spriteBatch);
                    // draw hud
                    hud.draw(spriteBatch);
                    
                }
                if (showHelp)
                    instructScreen.instruct.draw(spriteBatch);
            }
            else if (gameState == GameState.About) // about
            {
                aboutScreen.drawMenu(spriteBatch);
            }
            else if (gameState == GameState.Help) // help
            {
                instructScreen.drawMenu(spriteBatch);
            }
            else if (gameState == GameState.Credits) // credits
            {
                creditScreen.drawMenu(spriteBatch);
            }
            else if (gameState == GameState.ChooseLevel) // choose level
            {
                chooseLevelMenu.draw(spriteBatch);
            }
            else if (gameState == GameState.Instructions)
            {
                instructScreen.drawMenu(spriteBatch);
            }
            else if(gameState == GameState.Death)
            {
                yourdead.drawMenu(spriteBatch);
            }
            else if(gameState == GameState.Win)
            {
                winScreen.drawMenu(spriteBatch);
            }
            spriteBatch.End();

            /*GraphicsDevice.SetRenderTarget(null);
            OFFSCREEN_TEX = OFFSCREEN;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DEFAULT_SHADER.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(OFFSCREEN, Vector2.Zero, Color.White);
            spriteBatch.End();*/

            base.Draw(gameTime);
        }
    }
}
