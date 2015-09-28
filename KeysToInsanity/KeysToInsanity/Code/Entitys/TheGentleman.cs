using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code
{
    class TheGentleman : AnimatedSprite
    {

        private BasicInput input;
        public TheGentleman(Game game) : base(game,"tempGentleman",new Point(120,150),2,0.25,true)
        {
            input = new BasicInput(game, this);


        }

        public void handleInput(GameTime time)
        {
            input.defaultKeyboardHandler();
            this.updateWithAnimation(time, 0);

        }

        public override void onCollide(BasicSprite s)
        {
            Console.WriteLine("The Gentleman collided with " + s.GetType().Name);
        }

        //use an update method to move ai in methods.
    }
}
