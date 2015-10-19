using Microsoft.Xna.Framework;


namespace KeysToInsanity.Code.Interactive_Objects
{
    class HatHanger : BasicSprite
    {
        public HatHanger(Game game) : base(game, "hat hanger 2", false)
        {
            spriteSize = new Point(50, 100);
        }

        public override void onCollide(BasicSprite s, Rectangle data, GameTime time)
        {
            base.onCollide(s, data, time);
        }
    }
}
