using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class StartScreen
    {     
        private BasicSprite logo;
        private BasicSprite startButton;
        private BasicSprite aboutButton;
        private BasicSprite instructButton;
        private BasicSprite creditsButton;
        private BasicSprite exitButton;
        //REMOVE WHEN READY TO RELEASE GAME
        private BasicSprite chooseLevel;

        private Rectangle[] clickZones;

        public StartScreen(Game game)
        {
            // load sprites
            logo = new BasicSprite(game, "Interface\\logo", false);
            logo.spritePos = new Vector2(300, 20);

            startButton = new BasicSprite(game, "Interface\\start", false);
            startButton.spritePos = new Vector2(300, 240);

            aboutButton = new BasicSprite(game, "Interface\\aboutButton", false);
            aboutButton.spritePos = new Vector2(350, 290);

            instructButton = new BasicSprite(game, "Interface\\InstructionsButton", false);
            instructButton.spritePos = new Vector2(300, 340);

            creditsButton = new BasicSprite(game, "Interface\\creditsButton", false);
            creditsButton.spritePos = new Vector2(350, 390);

            exitButton = new BasicSprite(game, "Interface\\exit", false);
            exitButton.spritePos = new Vector2(300, 440);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(startButton.spritePos.ToPoint(), startButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Playing;

            if (new Rectangle(aboutButton.spritePos.ToPoint(), startButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.About;

            if (new Rectangle(instructButton.spritePos.ToPoint(), startButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Instructions;

            //if (new Rectangle(exitButton.spritePos.ToPoint(), startButton.spriteSize).Contains(pos))
                //exit();

            return KeysToInsanity.GameState.StartMenu; 
        }

        public void drawMenu(SpriteBatch s)
        {
            logo.draw(s);
            startButton.draw(s);
            aboutButton.draw(s);
            instructButton.draw(s);
            creditsButton.draw(s);
            exitButton.draw(s);
        }
    }
}


