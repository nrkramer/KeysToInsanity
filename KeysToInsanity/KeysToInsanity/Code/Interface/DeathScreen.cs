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


        private Rectangle[] clickZones;



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

            // calculate clickZones
            clickZones = new Rectangle[5];
            clickZones[0] = new Rectangle(250, 240, 300, 20);
            clickZones[1] = new Rectangle(250, 290, 300, 20);
            clickZones[2] = new Rectangle(250, 340, 300, 20);
            clickZones[3] = new Rectangle(250, 390, 300, 20);
            clickZones[4] = new Rectangle(300, 440, 200, 20);

        }

        public int Update(GameTime time, MouseState state)
        {
            // mouse stuff
            for (int i = 0; i < 5; i++)
            {
                if (clickZones[i].Contains(state.Position))
                {
                    // user a button
                    if ((state.LeftButton == ButtonState.Pressed))
                        return i; // selected button

                    i = clickZones.Length; // break out of loop cleanly
                }

            }

            return -1;
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