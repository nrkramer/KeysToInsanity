using Microsoft.Xna.Framework;

namespace KeysToInsanity.Code
{
    class Security : AnimatedSprite
    {

        private int center;
        public Security(Game game, int Xpos, int Ypos) : base(game, "TopHat",new Point (Xpos,Ypos),1,.25, false)
        {
            center = Xpos;
        }

        protected void Update()
        {
            if(center > getSpriteXPos() + 10)
            {

            }else if (center < getSpriteXPos() -10)
            {

            }
            else
            {

            }
        }

    }
}
