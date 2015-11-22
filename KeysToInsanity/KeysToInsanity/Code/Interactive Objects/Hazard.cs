using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class Hazard : AnimatedSprite
    {
        private float damage = 10;

        public Hazard(Game game, string file, bool collidable, float damage,float x, float y) : base(game, file, new Point((int)x, (int)y), 1, 0, collidable)
        {
            this.damage = damage;
        }

        // do damage to gentleman (maybe other entitys as well?)
        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);

            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                TheGentleman tg = (TheGentleman)collided;
                if (!tg.invincible)
                {
                    tg.invincible = true;
                    tg.health -= damage;
                }
            }
        }
    }
}
