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
        private new RenderTarget2D spriteTex;
        private GraphicsDevice gd;
        private SpriteContainer creditSprites = new SpriteContainer();

        public CreditScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            BasicSprite credits = new BasicSprite(game, "creditsPage", false);
            credits.spritePos = new Vector2(500, 400);

            BasicSprite returnButton = new BasicSprite(game, "return", false);
            returnButton.spritePos = new Vector2(580, 780);

            credits.addTo(creditSprites);
            returnButton.addTo(creditSprites);
        }

        public void drawStart(SpriteBatch spriteBatch)
        {
            gd.SetRenderTarget(spriteTex);
            gd.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            foreach (BasicSprite s in creditSprites)
            {
                s.draw(spriteBatch);
            }

            spriteBatch.End();

            gd.SetRenderTarget(null);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTex, new Rectangle(spritePos.ToPoint(), spriteSize), Color.White);
        }
    }
}
