using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KeysToInsanity.Code.Interface
{

    class PauseScreen
    {

        
        BasicSprite logo;
        BasicSprite resume;
        BasicSprite help;
        BasicSprite exit;

        private Rectangle[] clickZones;

        public PauseScreen(Game game)
        {
            logo = new BasicSprite(game, "Interface\\logo", false);
            logo.spritePos = new Vector2(300, 20);

            resume = new BasicSprite(game, "Interface\\resume", false);
            resume.spritePos = new Vector2(350, 240);

            help = new BasicSprite(game, "Interface\\InstructionsButton", false);
            help.spritePos = new Vector2(300, 290);

            exit = new BasicSprite(game, "Interface\\exit", false);
            exit.spritePos = new Vector2(300, 340);

            // calculate clickZones
            clickZones = new Rectangle[3];
            clickZones[0] = new Rectangle(350, 240, 100, 20);
            clickZones[1] = new Rectangle(300, 290, 200, 20);
            clickZones[2] = new Rectangle(300, 340, 200, 20);
           

        }


        public int Update(GameTime time, MouseState state)
        {
            // mouse stuff
            for (int i = 0; i < 3; i++)
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
            logo.draw(s);
            resume.draw(s);
            help.draw(s);
            exit.draw(s);
            
        }
    }
}