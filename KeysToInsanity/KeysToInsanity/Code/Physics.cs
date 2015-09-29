using Microsoft.Xna.Framework;

namespace KeysToInsanity.Code
{
    class Physics
    {
        public Velocity gravity = Velocity.FromDirection(270.0f, -9.8f);

        public void Update(GameTime gameTime, List<BasicSprite> spritesToPhysics)
        {
            foreach(BasicSprite i in spritesToPhysics)
        {
                i.velocity = i.velocity + gravity;
            }
        }
    }
}
