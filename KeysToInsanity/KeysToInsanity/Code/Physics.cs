using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace KeysToInsanity.Code
{
    class Physics 
        {
        public bool isHeJumping = false;
        public Velocity velocity;
        public int gravity;

        public void Update(GameTime gameTime)
        {

            if (isHeJumping == false)
            {
                velocity = Velocity.Zero;
            }
        }
        

    }
}
