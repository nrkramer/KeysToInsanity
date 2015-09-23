using Microsoft.Xna.Framework;

namespace KeysToInsanity.Code.Entitys
{
    class TheDoctor
    {
       
        private BasicSprite doctor;

        public TheDoctor(Game game)
        {
            doctor = new BasicSprite(game, "TopHat.png");
        }
    }
}
