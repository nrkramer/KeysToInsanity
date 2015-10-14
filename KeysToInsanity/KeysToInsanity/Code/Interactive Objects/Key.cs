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
    class Key : BasicSprite
    {
        private bool onHUD = false;
        private HUD hud; // hud to add key to
        private Timer timer = new Timer();

        public Key(Game game, HUD hud) : base(game, "key", false)
        {
            spriteSize = new Point(50, 50);
            this.hud = hud;
            timer.Elapsed += new ElapsedEventHandler(move_to_interface_event);
            timer.Interval = 5;
        }

        private void move_to_interface_event(object source, ElapsedEventArgs e)
        {
            updatePosition();
            if (spritePos.Y <= 5)
            {
                spritePos = new Vector2(spritePos.X, 5);
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
                double angle = Math.Atan2(5 - spritePos.Y, 745 - spritePos.X) * 180.0 / Math.PI;
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
