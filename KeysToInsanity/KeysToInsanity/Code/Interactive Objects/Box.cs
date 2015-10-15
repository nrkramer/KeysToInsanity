using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class Box : BasicSprite
    {


        public Box(Game game) : base(game, "box", false)
        {
            spriteSize = new Point(25, 25);

        }

        public override void onCollide(BasicSprite collided, Rectangle data, GameTime time)
        {
            base.onCollide(collided, data, time);
            // The Gentleman pushs into a box
            if (collided.ToString() == "KeysToInsanity.Code.TheGentleman")
            {
                if (data.Width <=0)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 0.5f);
                }else if (data.Width>=0)
                {
                    this.velocity = Velocity.FromDirection(0.0f, 0.5f);
                }
                
                 
                

             }
        }
    }
}
