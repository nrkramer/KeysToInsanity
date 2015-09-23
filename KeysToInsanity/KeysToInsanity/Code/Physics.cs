using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public bool isHeJumping;
        public Vector2 position;
        public Vector2 velocity;

        public void Update(GameTime gameTime)
        {
            position += velocity;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 3f;
            }else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -3f;
            }
            else{
                velocity.X = 0f;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Space) && isHeJumping == false)
            {
                position.Y -= 10f;
                velocity.Y = -5f;
                isHeJumping = true;
            }
            if(isHeJumping == true)
            {
                int n = 1;
                velocity.Y = 0.15f + n;
            }

            if(position.Y >= 450)
            {
                isHeJumping = false;
            }

            if(isHeJumping == false)
            {
                velocity.Y = 0f;
            }

        }
        

    }
}
