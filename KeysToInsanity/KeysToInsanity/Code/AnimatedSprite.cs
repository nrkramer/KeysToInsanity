using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    class AnimatedSprite : BasicSprite
    {
        private List<Animation> animations = new List<Animation>();
        private Animation currentAnimation = new Animation();

        private Rectangle animatedSpriteSize = new Rectangle(); // size of each individual sprite in the sprite sheet
        private int spriteAnimations = 1; // number of rows in sprite sheet
        private double animationSpeed = 0.25f;

        public AnimatedSprite(Game game, string file, Rectangle animatedSpriteSize, int spriteAnimations, double animationSpeed) : base(game, file)
        {
            this.animatedSpriteSize = animatedSpriteSize;
            this.spriteAnimations = spriteAnimations;
            this.animationSpeed = animationSpeed;
            loadAnimations();
        }

        public AnimatedSprite(Texture2D tex, Rectangle animatedSpriteSize, int spriteAnimations, double animationSpeed) : base(tex)
        {
            this.animatedSpriteSize = animatedSpriteSize;
            this.spriteAnimations = spriteAnimations;
            this.animationSpeed = animationSpeed;
            loadAnimations();
        }

        // In other classes, this should be overridden to provide custom animation loading for different entities
        // This will allow for idle animations, such as standing while facing a direction
        protected virtual void loadAnimations()
        {
            for (int i = 0; i < spriteAnimations; i++)
            {
                animations[i] = new Animation();
                for (int j = 0; j < spriteTex.Width / animatedSpriteSize.Width; j++)
                {
                    animations[i].AddFrame(
                        new Rectangle(j * animatedSpriteSize.Width, i * animatedSpriteSize.Height, animatedSpriteSize.Width, animatedSpriteSize.Height),
                        System.TimeSpan.FromSeconds(animationSpeed));
                }
            }
        }

        public void updateWithAnimation(GameTime time, int index)
        {
            animations[index].Update(time);
        }

        public override void draw(SpriteBatch s)
        {
            if (spriteTex != null)
            {
                Rectangle spriteBox = new Rectangle(spritePos, spriteSize);
                s.Draw(spriteTex, spriteBox, currentAnimation.CurrentRectangle, new Color(1.0f, 1.0f, 1.0f)); // add source rectangle
                if (KeysToInsanity.DRAW_BOUNDING_BOXES)
                    drawBorder(s, spriteBox, 2, Color.Red);
            }
        }
    }
}
