using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class InstructionScreen
    {
        public BasicSprite instruct;
        private BasicSprite returnButton;

        // See comment in CreditScreen class
        public InstructionScreen(Game game)
        {
            instruct = new BasicSprite(game, "Interface\\Instructions", false);
            instruct.spritePos = new Vector2(0, 100);

            returnButton = new BasicSprite(game, "Interface\\return", false);
            returnButton.spritePos = new Vector2(690, 20);
        }

        public void drawMenu(SpriteBatch s)
        {
            instruct.draw(s);
            returnButton.draw(s);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(returnButton.spritePos.ToPoint(), returnButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Paused;

            return KeysToInsanity.GameState.Instructions;
        }
    }
}

