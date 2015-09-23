using Microsoft.Xna.Framework;

namespace KeysToInsanity
{
    class Nurse
    {
       private BasicSprite nurse;

        public Nurse(Game game)
        {
            nurse = new BasicSprite(game, "TopHat.png");
        }
    }
}
