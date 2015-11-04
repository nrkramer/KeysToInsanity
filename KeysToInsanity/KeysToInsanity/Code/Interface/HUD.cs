using KeysToInsanity.Code.Interactive_Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code.Interface
{
    public class HUD : BasicSprite
    {
        private new RenderTarget2D spriteTex; // This hides BasicSprite's Texture2D, which RenderTarget2D inherits
        private GraphicsDevice gd;
        private SpriteContainer hudSprites = new SpriteContainer();
        private Bar insanityBar;
        private Bar healthBar;

        // This class is kind of weird
        // First of all, all elements are drawn to external texture, why?
        // To ensure that the HUD is
        // a) drawn all at once
        // b) drawn over everything else
        // and c) not a part of collision detection (most important reason)
        // This also allows us to define custom behavior for drawing whatever sprites are added to
        // the HUD's list of sprites to draw. And adjust the color of the entire HUD itself.
        public HUD(Game game) : base(new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight), false)
        {
            spriteTex = new RenderTarget2D(game.GraphicsDevice,
                game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GraphicsDevice.PresentationParameters.BackBufferHeight);

            BasicSprite hud_key_frame = new BasicSprite(game, "hud_key_frame", false);
            hud_key_frame.spriteSize = new Point(60, 60);
            hud_key_frame.spritePos = new Vector2(740, 0);

            BasicSprite hud_health_frame = new BasicSprite(game, "health_bar_frame", false);
            hud_health_frame.spriteSize = new Point(200, 30);
            hud_health_frame.spritePos = new Vector2(1, 0);

            BasicSprite hud_insanity_frame = new BasicSprite(game, "insanity_bar_frame", false);
            hud_insanity_frame.spriteSize = new Point(200, 30);
            hud_insanity_frame.spritePos = new Vector2(211, 0);

            healthBar = new Bar(game, "health_bar_color");
            healthBar.spriteSize = new Point(202, 32);
            healthBar.spritePos = new Vector2(0, 0);
            healthBar.level = 100;

            insanityBar = new Bar(game, "insanity_bar_color");
            insanityBar.spriteSize = new Point(0, 32);
            insanityBar.spritePos = new Vector2(210, 0);
            insanityBar.level = 0;

            hud_key_frame.addTo(hudSprites);
            healthBar.addTo(hudSprites);
            hud_health_frame.addTo(hudSprites);
            insanityBar.addTo(hudSprites);
            hud_insanity_frame.addTo(hudSprites);

            gd = game.GraphicsDevice;
        }

        public void Update(GameTime time)
        {
            insanityBar.Update(time);
            healthBar.Update(time);
        }

        // This draws the HUD to it's texture
        // needs to be drawn before everything because
        // gd.SetRenderTarget(null) clears the back buffer
        public void drawHUD(SpriteBatch spriteBatch)
        {
            gd.SetRenderTarget(spriteTex);
            gd.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            foreach (BasicSprite s in hudSprites)
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

        // player picked up a key
        public void addKey(Key k)
        {
            k.addTo(hudSprites);
        }

        // player health update
        public void updateHealth(float health)
        {
            healthBar.level = health;
        }

        public void updateInsanity(float insanity)
        {
            insanityBar.level = insanity;
        }
    }
}
