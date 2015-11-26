using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KeysToInsanity.Code.Interface
{

    class PauseScreen
    {
        private Rectangle resumeR = new Rectangle(350, 240, 100, 20);
        private Rectangle helpR = new Rectangle(350, 290, 100, 20);
        private Rectangle exitR = new Rectangle(350, 340, 100, 20);

        private BasicSprite logo;
        private BasicSprite resume;
        private BasicSprite help;
        private BasicSprite exit;


        // See comment in CreditScreen class
        public PauseScreen(Game game)
        {
            logo = new BasicSprite(game, "Interface\\logo", false);
            logo.spritePos = new Vector2(300, 20);

            resume = new BasicSprite(game, "Interface\\resume", false);
            resume.spritePos = new Vector2(300, 240);

            help = new BasicSprite(game, "Interface\\InstructionsButton", false);
            help.spritePos = new Vector2(300, 290);

            exit = new BasicSprite(game, "Interface\\exit", false);
            exit.spritePos = new Vector2(300, 340);
        }
        
        public void drawMenu(SpriteBatch s)
        {
            logo.draw(s);
            resume.draw(s);
            help.draw(s);
            exit.draw(s);
            
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (resumeR.Contains(pos))
                return KeysToInsanity.GameState.Playing;
            else if (helpR.Contains(pos))
                return KeysToInsanity.GameState.Instructions;
            else if (exitR.Contains(pos))
                return KeysToInsanity.GameState.Exit;
            return KeysToInsanity.GameState.Paused;
        }
    }
}