using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KeysToInsanity.Code
{
    class Nurse : AnimatedSprite
    {
 
        private int center;

        public Nurse(Game game, int posX, int posY) : base(game, "TopHat", new Point(posX, posY), 1, .25, false)
        {
            center = posX;
        }

        protected  void Update()
        {
            //Deciding if we need to move to the right
            if(center > getSpriteXPos()+50)
            {
                //Ask for help
            }else if (center < getSpriteXPos()-50) //Or to the left
            {

            }else
            {

            }


        }
    }
}
