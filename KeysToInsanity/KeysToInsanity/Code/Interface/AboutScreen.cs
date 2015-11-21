using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interface
{
    class AboutScreen
    {       

        private BasicSprite about;
        private BasicSprite returnButton;
        private SpriteContainer aboutSprites = new SpriteContainer();

        public AboutScreen(Game game)
        {
            about = new BasicSprite(game, "Interface\\aboutPage", false);
            about.spritePos = new Vector2(0, 100);

            returnButton = new BasicSprite(game, "Interface\\return", false);
            returnButton.spritePos = new Vector2(690, 20);

            about.addTo(aboutSprites);
            returnButton.addTo(aboutSprites);
        }

        public void drawMenu(SpriteBatch spriteBatch)
        {
            foreach (BasicSprite s in aboutSprites)
                s.draw(spriteBatch);
        }

        public KeysToInsanity.GameState MouseClicked(Point pos)
        {
            if (new Rectangle(returnButton.spritePos.ToPoint(), returnButton.spriteSize).Contains(pos))
                return KeysToInsanity.GameState.StartMenu;

            return KeysToInsanity.GameState.About;
        }
    }
}
