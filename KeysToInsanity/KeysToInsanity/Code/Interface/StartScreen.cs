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



            // calculate clickZones
            clickZones = new Rectangle[5];
            clickZones[0] = new Rectangle(300,240,200,20);
            clickZones[1] = new Rectangle(350,290,100,20);
            clickZones[2] = new Rectangle(300,340,300,20);
            clickZones[3] = new Rectangle(350,390,100,20);
            clickZones[4] = new Rectangle(300,440,200,20);

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
            logo.draw(s);
            startButton.draw(s);
            aboutButton.draw(s);
            instructButton.draw(s);
            creditsButton.draw(s);
            exitButton.draw(s);
        }
    }
}


