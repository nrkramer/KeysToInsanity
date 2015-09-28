using Microsoft.Xna.Framework;

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
