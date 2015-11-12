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
        private BasicSprite instruct;
        private BasicSprite returnButton;
        private Rectangle[] clickZones;

        public InstructionScreen(Game game)
        {
            instruct = new BasicSprite(game, "Interface\\InstructionsPage", false);
            instruct.spritePos = new Vector2(0, 100);

            returnButton = new BasicSprite(game, "Interface\\return", false);
            returnButton.spritePos = new Vector2(690, 20);

            clickZones = new Rectangle[1];
            clickZones[0] = new Rectangle(690, 20, 100, 20);

        }

        public int Update(GameTime time, MouseState state)
        {
            // mouse stuff
            for (int i = 0; i < 1; i++)
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
            instruct.draw(s);
            returnButton.draw(s);

        }
    }
}

