using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class CreditScreen
    {
        private BasicSprite credits;
        private BasicSprite returnButton;

       
        
        public CreditScreen(Game game)
        {
            credits = new BasicSprite(game, "Interface\\creditsPage", false);
            credits.spritePos = new Vector2(10, 100);

            returnButton = new BasicSprite(game, "Interface\\return", false);
            returnButton.spritePos = new Vector2(690, 20);
        }

        public void drawMenu(SpriteBatch s)
        {
            credits.draw(s);
            returnButton.draw(s);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(returnButton.spritePos.ToPoint(), returnButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.StartMenu;

            return KeysToInsanity.GameState.Credits;
        }
    }
}
