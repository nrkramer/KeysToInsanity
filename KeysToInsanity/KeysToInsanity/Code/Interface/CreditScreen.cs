using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class CreditScreen : BasicSprite
    {
        private SpriteContainer creditSprites = new SpriteContainer();

        public CreditScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            BasicSprite credits = new BasicSprite(game, "creditsPage", false);
            credits.spritePos = new Vector2(10, 100);

            BasicSprite returnButton = new BasicSprite(game, "return", false);
            returnButton.spritePos = new Vector2(690, 20);

            credits.addTo(creditSprites);
            returnButton.addTo(creditSprites);
        }

        public void drawMenu(SpriteBatch spriteBatch)
        {

            foreach (BasicSprite s in creditSprites)
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
