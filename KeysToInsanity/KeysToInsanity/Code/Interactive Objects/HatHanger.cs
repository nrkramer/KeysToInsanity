using Microsoft.Xna.Framework;
using System;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class HatHanger : AnimatedSprite
    {
        public bool playAnimation = false;
        private float animationTime = 0.0f;

        //the "checkpoints" of our game
        public HatHanger(Game game) : base(game, "hat hanger 2", new Point(192, 593), 2, 0.1, false)
        {
            spriteSize = new Point(50, 100);
        }

        public void Update(GameTime time)
        {
            if (playAnimation)
            {
                updateWithAnimation(time, 1);
                if ((time.TotalGameTime.TotalSeconds - animationTime) >= 0.64)
                    playAnimation = false;
            } else
            {
                updateWithAnimation(time, 0);
                animationTime = (float)time.TotalGameTime.TotalSeconds;
            }
        }

        protected override void loadAnimations()
        {
            Animation idle = new Animation();
            idle.AddFrame(new Rectangle(0, 0, 192, 593), TimeSpan.FromSeconds(1.0));

            Animation spin = new Animation();
            spin.AddUniformStrip(new Rectangle(0, 593, 1536, 593), new Point(192, 593), TimeSpan.FromSeconds(0.08));

            animations.Add(idle);
            animations.Add(spin);
        }

        public override void onCollide(BasicSprite s, Rectangle data, GameTime time)
        {
            base.onCollide(s, data, time);
        }
    }
}
