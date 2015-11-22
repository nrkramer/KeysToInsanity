using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class WinScreen
    {
        BasicSprite winText;
        BasicSprite returnToStart;
        BasicSprite exit;


        public WinScreen(Game game)
        {
            winText = new BasicSprite(game, "Interface\\WinText", false);
            winText.spritePos = new Vector2(300, 100);
            returnToStart = new BasicSprite(game, "Interface\\returnStart", false);
            returnToStart.spritePos = new Vector2(250, 240);

            exit = new BasicSprite(game, "Interface\\exit", false);
            exit.spritePos = new Vector2(300, 280);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(returnToStart.spritePos.ToPoint(), returnToStart.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.StartMenu;
            if (new Rectangle(exit.spritePos.ToPoint(), returnToStart.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Exit;

            return KeysToInsanity.GameState.Win;
        }

        public void drawMenu(SpriteBatch s)
        {
            winText.draw(s);
            returnToStart.draw(s);
            exit.draw(s);
 
        }

    }
}
