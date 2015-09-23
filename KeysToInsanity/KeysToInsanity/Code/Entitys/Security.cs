using Microsoft.Xna.Framework;

namespace KeysToInsanity
{
    class Security
    {
        private BasicSprite security;
        

        public Security(Game game)
        {
            security = new BasicSprite(game, "TopHat.png");
        }
    }
}
