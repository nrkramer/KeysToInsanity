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
        public bool isJumping;
        public Vector2 gravity = new Vector2(0, -9.8f);
        public Vector2 velocity;
        public Vector2 position;

        public void Update(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity += gravity * time;
            position += velocity * time;
        }
    }
}
