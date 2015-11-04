using KeysToInsanity.Code.Interface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework.Graphics;

namespace KeysToInsanity.Code.Interactive_Objects
{
    public class Key : BasicSprite
    {
        private bool onHUD = false;
        private HUD hud; // hud to add key to
        private Timer timer = new Timer();
        private Game game; // game reference

        //the main objective of the game: collecting the keys 
        public Key(Game game, HUD hud) : base(game, "key", false)
        {
            this.game = game;
            spriteSize = new Point(45, 45);
            this.hud = hud;
            timer.Elapsed += new ElapsedEventHandler(move_to_interface_event);
            timer.Interval = 5;
        }

        //enables an event that will move the key up to the HUD space designated for it
        private void move_to_interface_event(object source, ElapsedEventArgs e)
        {
            updatePosition();
            if (spritePos.Y <= 10)
            {
                spritePos = new Vector2(game.GraphicsDevice.Viewport.Width - 52, 10);
                timer.Stop();
            }
        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            // The Gentleman picked up the key
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                container.Remove(this);
                hud.addKey(this);
                onHUD = true;
                double angle = Math.Atan2(10 - spritePos.Y, game.GraphicsDevice.Viewport.Width - 52 - spritePos.X) * 180.0 / Math.PI;
                velocity = Velocity.FromDirection((float)angle, 10.0f);
                timer.Start();
            }
        }

        public override void draw(SpriteBatch s)
        {
            base.draw(s);
        }
    }
}
