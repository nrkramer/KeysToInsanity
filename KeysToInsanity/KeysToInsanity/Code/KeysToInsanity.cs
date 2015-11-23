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
            Win
            
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
        private int levelsPlayed = 0;

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

        private string[] levelXMLs;
        private uint unlockedLevels = 5;

        private BasicInput input; // Our input handler

        public static Physics physics = new Physics();

        private Sound testSound;
        private Sound landedOnGround;

        //private horizontalPlatform platformH;

        //Setting constants for the menu items
        MouseState mouseState;
        MouseState previousMouseState;
        KeyboardState keyboardState;
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
            gameState = GameState.ChooseLevel;

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
            levelXMLs = Directory.GetFiles(Content.RootDirectory + "\\Levels\\", "*.xml");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Loading the games menu buttons for menu screen
            startMenu = new StartScreen(this);
            pauseMenu = new PauseScreen(this);
            aboutScreen = new AboutScreen(this);
            instructScreen = new InstructionScreen(this);
            creditScreen = new CreditScreen(this);
            chooseLevelMenu = new LevelSwitcher(this, 0, (uint)levelXMLs.Length);
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
            loader = new LevelLoader(this, "Content\\Levels\\Level1.xml", hud);
            loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key
            theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
            foreach (BasicSprite s in loader.level.stages[stageIndex].fadeIns)
                s.opacity = 0.0f;

            input = new BasicInput(this, theGentleman);

           //testSound = new Sound(this, "SoundFX\\Music\\Op9No2Session");
           //testSound.play(true);

            //landedOnGround = new Sound(this, "SoundFX\\campfire-1");
            //landedOnGround.play(true);

            //landedOnGround = new Sound(this, "SoundFX\\TheGentleman\\LandedOnFloor");//

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
                    if (data.Height >= 1.0f)
                    {
                        theGentleman.inAir = false;
                        theGentleman.jumps = 2;
                        theGentleman.velocity.setY(0.0f);
                        theGentleman.spritePos = new Vector2(theGentleman.spritePos.X, collided.spritePos.Y - theGentleman.spriteSize.Y);
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
            else if(gameState == GameState.Win)
            {
                if (mouseClicked)
                    gameState = winScreen.MouseClicked(mouseCoords);
            }
            else if (gameState == GameState.Playing)
            {
                if (theGentleman.health <= 0)
                {
                    gameState = GameState.Death;
                    insanity = 0.0f;
                    theGentleman.health = 100.0f;
                    stageIndex = 0;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                }

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

                    insanity += 0.02f;
                    hud.updateInsanity(insanity);
                    hud.Update(gameTime);

                    // gentleman physics
                    physics.UpdateGentlemanPhysics(gameTime, theGentleman);

                    // non-gentleman character physics
                    physics.Update(gameTime, loader.level.stages[stageIndex].characters);

                    // collision for gentleman against static sprites
                    RectangleCollision.update(theGentleman, loader.level.stages[stageIndex].collidables, gameTime);

                    // collision for non-gentleman characters
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
                    hud.removeKey();
                    loader = new LevelLoader(this, levelXMLs[i], hud);
                    stageIndex = 0;
                    theGentleman.spritePos = new Vector2(loader.level.stages[stageIndex].startX, loader.level.stages[stageIndex].startY);
                    gotKey = false;
                    loader.level.stages[loader.level.stageWithKey].key.collisionCallback += new CollisionEventHandler(collisionEvents); // collision callback for key
                    theGentleman.health = 100.0f;
                    insanity = 0.0f;
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

            base.Draw(gameTime);
        }
    }
}
