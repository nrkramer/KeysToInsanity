using Microsoft.Xna.Framework;
namespace KeysToInsanity.Code
{
    class TheGentleman : AnimatedSprite
    {

        private BasicInput input;
        public TheGentleman(Game game) : base(game,"tempGentleman",new Point(120,150),2,0.25)
        {
            input = new BasicInput(game, this);


        }

        public void handleInput(GameTime time)
        {
            input.defaultKeyboardHandler();
            this.updateWithAnimation(time, 0);

        }

        //use an update method to move ai in methods.
    }
}
