using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class StartScreen : BasicSprite
    {     
        private SpriteContainer startSprites = new SpriteContainer();

        public StartScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            BasicSprite logo = new BasicSprite(game, "logo", false);
            logo.spritePos = new Vector2(300, 20);

            BasicSprite start = new BasicSprite(game, "start", false);
            start.spritePos = new Vector2(350, 240);

            BasicSprite about = new BasicSprite(game, "aboutButton", false);
            about.spritePos = new Vector2(350, 290);

            BasicSprite credits = new BasicSprite(game, "creditsButton", false);
            credits.spritePos = new Vector2(350, 340);

            BasicSprite exit = new BasicSprite(game, "exit", false);
            exit.spritePos = new Vector2(350, 390);

            logo.addTo(startSprites);
            start.addTo(startSprites);
            about.addTo(startSprites);
            credits.addTo(startSprites);
            exit.addTo(startSprites);
        }

        public void drawMenu(SpriteBatch spriteBatch)
        {
            foreach (BasicSprite s in startSprites)
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