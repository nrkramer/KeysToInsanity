using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class DeathScreen
    {

       private BasicSprite DeathText;
       private BasicSprite ReturnToStart;
       private BasicSprite restartCP;
       private BasicSprite restartL;
       private BasicSprite chooseL;
       private BasicSprite exit;

        public DeathScreen(Game game)
        {
            DeathText= new BasicSprite(game, "Interface\\DeathText", false);
            DeathText.spritePos = new Vector2(300, 100);

            ReturnToStart = new BasicSprite(game, "Interface\\returnStart", false);
            ReturnToStart.spritePos = new Vector2(250, 240);

            restartCP = new BasicSprite(game, "Interface\\returnCheck", false);
            restartCP.spritePos = new Vector2(250, 290);

            restartL = new BasicSprite(game, "Interface\\restartLevel", false);
            restartL.spritePos = new Vector2(250, 340);

            chooseL = new BasicSprite(game, "Interface\\chooseLevel", false);
            chooseL.spritePos = new Vector2(250, 390);

            exit = new BasicSprite(game, "Interface\\exit", false);
            exit.spritePos = new Vector2(300, 440);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(ReturnToStart.spritePos.ToPoint(), ReturnToStart.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.StartMenu;

            if (new Rectangle(restartCP.spritePos.ToPoint(), restartCP.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Checkpoint;

            if (new Rectangle(restartL.spritePos.ToPoint(), restartL.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Playing;

            if (new Rectangle(chooseL.spritePos.ToPoint(), chooseL.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.ChooseLevel;

            if (new Rectangle(exit.spritePos.ToPoint(), exit.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.Exit;

            return KeysToInsanity.GameState.Death;
        }

        public void drawMenu(SpriteBatch s)
        {
            DeathText.draw(s);
            ReturnToStart.draw(s);
            restartCP.draw(s);
            restartL.draw(s);
            chooseL.draw(s);
            exit.draw(s);
        }
    }
    }

