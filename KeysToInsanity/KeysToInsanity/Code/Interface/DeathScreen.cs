using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class DeathScreen : BasicSprite
    {
   
        
            private SpriteContainer deathSprites = new SpriteContainer();

            public DeathScreen(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                    game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                    game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
            {
                BasicSprite  DeathText= new BasicSprite(game, "Interface\\DeathText", false);
                DeathText.spritePos = new Vector2(300, 100);

                BasicSprite ReturnToStart = new BasicSprite(game, "Interface\\returnStart", false);
                ReturnToStart.spritePos = new Vector2(250, 240);

                BasicSprite restartCP = new BasicSprite(game, "Interface\\returnCheck", false);
                restartCP.spritePos = new Vector2(250, 290);

                BasicSprite restartL = new BasicSprite(game, "Interface\\restartLevel", false);
                restartL.spritePos = new Vector2(250, 340);

                BasicSprite chooseL = new BasicSprite(game, "Interface\\chooseLevel", false);
                chooseL.spritePos = new Vector2(250, 390);

                BasicSprite exit = new BasicSprite(game, "Interface\\exit", false);
                exit.spritePos = new Vector2(350, 440);

                DeathText.addTo(deathSprites);
                restartCP.addTo(deathSprites);
                restartL.addTo(deathSprites);
                ReturnToStart.addTo(deathSprites);
                chooseL.addTo(deathSprites);
                exit.addTo(deathSprites);
            }

            public void drawMenu(SpriteBatch spriteBatch)
            {
                foreach (BasicSprite s in deathSprites)
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

