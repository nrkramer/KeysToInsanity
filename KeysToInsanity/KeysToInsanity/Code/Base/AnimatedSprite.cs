﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KeysToInsanity.Code
{
    public class AnimatedSprite : BasicSprite
    {
        protected List<Animation> animations = new List<Animation>();
        protected Animation currentAnimation = new Animation();

        private Point animatedSpriteSize = new Point(); // size of each individual sprite in the sprite sheet
        private int spriteAnimations = 1; // number of rows in sprite sheet
        private double animationSpeed = 0.25f;

        public AnimatedSprite(Game game, string file, Point animatedSpriteSize, int spriteAnimations, double animationSpeed, bool collidable) : base(game, file, collidable)
        {
            this.animatedSpriteSize = animatedSpriteSize;
            this.spriteAnimations = spriteAnimations;
            this.animationSpeed = animationSpeed;
            if (animatedSpriteSize.X > spriteTex.Width)
                this.animatedSpriteSize.X = spriteTex.Width;
            spriteSize = animatedSpriteSize;
            loadAnimations();
            updateWithAnimation(new GameTime(), 0);
        }

        public AnimatedSprite(Texture2D tex, Point animatedSpriteSize, int spriteAnimations, double animationSpeed, bool collidable) : base(tex, collidable)
        {
            this.animatedSpriteSize = animatedSpriteSize;
            this.spriteAnimations = spriteAnimations;
            this.animationSpeed = animationSpeed;
            if (spriteAnimations == 1)
                animatedSpriteSize = spriteSize;
            spriteSize = animatedSpriteSize;
            loadAnimations();
        }

        // In other classes, this should be overridden to provide custom animation loading for different entities
        // This will allow for idle animations, such as standing while facing a direction
        protected virtual void loadAnimations()
        {
            for (int i = 0; i < spriteAnimations; i++)
            {
                animations.Add(new Animation());
                for (int j = 0; j < spriteTex.Width / animatedSpriteSize.X; j++)
                {
                    animations[i].AddFrame(
                        new Rectangle(j * animatedSpriteSize.X, i * animatedSpriteSize.Y, animatedSpriteSize.X, animatedSpriteSize.Y),
                        TimeSpan.FromSeconds(animationSpeed));
                }
            }
        }

        public void updateWithAnimation(GameTime time, int index)
        {
            if (animations.Count > 0)
            {
                currentAnimation = animations[index];
                animations[index].Update(time);
            }
        }

        public override void draw(SpriteBatch s)
        {
            if (spriteTex != null)
            {
                Rectangle spriteBox = new Rectangle(spritePos.ToPoint(), spriteSize);
                s.Draw(spriteTex, new Rectangle(spritePos.ToPoint(), spriteSize), currentAnimation.CurrentRectangle, color * opacity);
                if (KeysToInsanity.DRAW_BOUNDING_BOXES)
                    drawBorder(s, spriteBox, 2, Color.Red);
            }
        }
    }
}
