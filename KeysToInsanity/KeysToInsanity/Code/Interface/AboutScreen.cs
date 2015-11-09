﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class AboutScreen : BasicSprite
    {       
        private SpriteContainer aboutSprites = new SpriteContainer();

        public AboutScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            BasicSprite about = new BasicSprite(game, "aboutPage", false);
            about.spritePos = new Vector2(500, 400);

            BasicSprite returnButton = new BasicSprite(game, "return", false);
            returnButton.spritePos = new Vector2(580, 780);


            about.addTo(aboutSprites);
            returnButton.addTo(aboutSprites);
            
        }

        public void drawMenu(SpriteBatch spriteBatch)
        {

            foreach (BasicSprite s in aboutSprites)
            {
                s.draw(spriteBatch);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTex, new Rectangle(spritePos.ToPoint(), spriteSize), Color.White);
        }
    }
}
