using KeysToInsanity.Code.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class LevelSwitcher
    {
        private BasicSprite background; // Level switcher logo and level pics and text (background is transparent)
        private BasicSprite layer2; // level row background
        private BasicSprite levelHover; // mouse hover background
        private BasicSprite levelCover; // cover to hide locked levels
        private BasicSprite arrowUp; // scroll up
        private BasicSprite arrowDown; // scroll down

        private uint totalLevels = 3; // uint is unsigned int
        private uint level = 0;
        private uint unlocked = 1;

        private bool showHighlight = false;

        private Rectangle[] clickZones;
        private Vector2[] coverPositions;

        private int deltaY = 0;
        private int delta = 0;

        public LevelSwitcher(Game game, uint startLevel, uint totalLevels)
        {
            // load sprites
            background = new BasicSprite(game, "Interface\\LevelSwitcher", false);
            background.spritePos = new Vector2(0, 0);
            background.spriteSize = new Point(800, 850);

            layer2 = new BasicSprite(game, "Interface\\levelSwitcherLayer2", false);
            layer2.spritePos = new Vector2(0, 0);
            layer2.spriteSize = new Point(720, 131);

            levelHover = new BasicSprite(game, "Interface\\levelHover", false); // don't draw this until mouse is over a target area
            levelHover.spriteSize = new Point(720, 131);

            levelCover = new BasicSprite(game, "Interface\\levelCover", false);
            levelCover.spriteSize = new Point(150, 100);

            arrowUp = new BasicSprite(game, "arrow", false);
            arrowUp.spritePos = new Vector2(game.GraphicsDevice.Viewport.Width - 30, 0);
            arrowUp.spriteSize = new Point(30, 60);

            arrowDown = new BasicSprite(game, "arrow", false);
            arrowDown.spritePos = new Vector2(game.GraphicsDevice.Viewport.Width - 30, game.GraphicsDevice.Viewport.Height - 60);
            arrowDown.spriteSize = new Point(30, 60);

            level = startLevel;
            this.totalLevels = totalLevels;

            // calculate clickZones
            clickZones = new Rectangle[totalLevels];
            for (int i = 0; i < totalLevels; i++)
            {
                // i * (height of rectangle + space between them)
                clickZones[i] = new Rectangle(50, 126 + (i * (135 + 15)), 720, 131);
            }

            setUnlockedLevels(1);
        }

        public int Update(GameTime time, MouseState state)
        {
            deltaY = state.ScrollWheelValue;
            if (deltaY < -400)
                deltaY = -400;
            if (deltaY > 10)
                deltaY = 10;

            if (delta > deltaY)
                delta -= 20;
            if (delta < deltaY)
                delta += 20;

            // position stuff
            background.spritePos = new Vector2(background.spritePos.X, delta);
            
            // mouse stuff
            for(int i = 0; i < unlocked; i++)
            {
                Rectangle inScreenRect = clickZones[i];
                inScreenRect.Y = inScreenRect.Y + delta;
                if (inScreenRect.Contains(state.Position))
                {
                    levelHover.spritePos = inScreenRect.Location.ToVector2();
                    showHighlight = true;

                    // user selected a level
                    if ((state.LeftButton == ButtonState.Pressed))
                        return i; // selected this level

                    i = clickZones.Length; // break out of loop cleanly
                } else
                {
                    showHighlight = false;
                }
            }

            return -1;
        }

        public void setUnlockedLevels(uint amount)
        {
            unlocked = amount;
            coverPositions = new Vector2[totalLevels - unlocked];
            // calculate cover regions
            for(int i = (int)totalLevels - (int)unlocked - 1; i >= 0; i--)
            {
                coverPositions[i] = new Vector2(70, 140 + ((totalLevels - i - 1) * (100 + 50)));
            }
        }

        public void draw(SpriteBatch s)
        {
            // draw order must always be this
            for (int i = 0; i < clickZones.Length; i++)
            {
                layer2.spritePos = new Vector2(clickZones[i].X, clickZones[i].Y + delta);
                layer2.draw(s);
            }
            if (showHighlight)
                levelHover.draw(s);
            background.draw(s);
            for (int i = (int)totalLevels - (int)unlocked - 1; i >= 0; i--)
            {
                levelCover.spritePos = new Vector2(coverPositions[i].X, coverPositions[i].Y + delta);
                levelCover.draw(s);
            }
            arrowUp.drawRotated(s, 0.0f);
            arrowDown.drawRotated(s, 180.0f);
        }
    }
}
