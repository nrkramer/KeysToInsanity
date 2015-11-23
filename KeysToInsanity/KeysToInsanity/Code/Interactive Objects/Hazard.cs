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

        public Hazard(Game game, string file, Point animatedSpriteSize, double animationSpeed, bool collidable, float damage) : base(game, file, animatedSpriteSize, 1, animationSpeed, collidable)
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
                    tg.health -= damage;
                    tg.invincible = true;
                }
            }
        }
    }
}
