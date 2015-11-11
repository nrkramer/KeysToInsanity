using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class Start
    {
        private BasicSprite logo;
        private BasicSprite startButton; 
        private BasicSprite aboutButton;
        private BasicSprite creditsButton;
        private BasicSprite exitButton;

        private bool showHighlight = false;

        private Rectangle[] clickZones;
        private Vector2[] coverPositions;

        public Start(Game game)
        {
            // load sprites
            logo= new BasicSprite(game, "Interface\\logo", false);
            logo.spritePos = new Vector2(300, 20);

            startButton= new BasicSprite(game, "Interface\\start", false);
            startButton.spritePos = new Vector2(350, 240);

            aboutButton = new BasicSprite(game, "Interface\\aboutButton", false);
            aboutButton.spritePos = new Vector2(350, 290);

            creditsButton = new BasicSprite(game, "Interface\\start", false);
            creditsButton.spritePos = new Vector2(350, 340);

            exitButton = new BasicSprite(game, "Interface\\start", false);
            exitButton.spritePos = new Vector2(350, 390);



            // calculate clickZones
            clickZones = new Rectangle[4];
            for (int i = 0; i < 4; i++)
            {
                // i * (height of rectangle + space between them)
                clickZones[i] = new Rectangle(50, 126 + (i * (131 + 15)), 720, 131);
            }           
        }

        public int Update(GameTime time, MouseState state)
        {
            // mouse stuff
            for (int i = 0; i < 4; i++)
            {
                if (clickZones[i].Contains(state.Position))
                {
                    // user selected a level
                    if ((state.LeftButton == ButtonState.Pressed))
                        return i; // selected this level

                    i = clickZones.Length; // break out of loop cleanly
                }
                else
                {
                    showHighlight = false;
                }
            }

            return -1;
        }

     

        public void draw(SpriteBatch s)
        {
            logo.draw(s);
            startButton.draw(s);
            aboutButton.draw(s);
            creditsButton.draw(s); 
        }
    }
}
