﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{

    class PauseScreen : BasicSprite
    {

        private SpriteContainer pauseSprites = new SpriteContainer();

        public PauseScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            BasicSprite logo = new BasicSprite(game, "Interface\\logo", false);
            logo.spritePos = new Vector2(300, 20);

            BasicSprite resume = new BasicSprite(game, "Interface\\resume", false);
            resume.spritePos = new Vector2(350, 240);

            BasicSprite help = new BasicSprite(game, "Interface\\help", false);
            help.spritePos = new Vector2(350, 290);

            BasicSprite exit = new BasicSprite(game, "Interface\\exit", false);
            exit.spritePos = new Vector2(350, 340);

            logo.addTo(pauseSprites);
            resume.addTo(pauseSprites);
            help.addTo(pauseSprites);
            exit.addTo(pauseSprites);

        }

        // gd.SetRenderTarget(null) clears the back buffer
        public void drawMenu(SpriteBatch spriteBatch)
        {

            foreach (BasicSprite s in pauseSprites)
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