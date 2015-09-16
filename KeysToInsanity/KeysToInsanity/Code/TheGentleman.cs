using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KeysToInsanity
{
    class TheGentleman
    {
        static Texture2D characterSheetTexture;
        Animation currentAnimation;
        Animation left;
        Animation right;


        public float X
        {
            get;
            set;
        }
        public float Y
        {
            get;
            set;
        }

        public TheGentleman(GraphicsDevice graphicsDevice)
        {
            if (characterSheetTexture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/charactersheet.png"))
                {
                    characterSheetTexture = Texture2D.FromStream(graphicsDevice, stream);

                }
            }

            left = new Animation();
            right = new Animation();

            left.AddFrame(new Rectangle(0, 300, 150, 300), TimeSpan.FromSeconds(.25));
            left.AddFrame(new Rectangle(150, 300, 150, 300), TimeSpan.FromSeconds(.25));
            left.AddFrame(new Rectangle(300, 300, 150, 300), TimeSpan.FromSeconds(.25));
            left.AddFrame(new Rectangle(450, 300, 150, 300), TimeSpan.FromSeconds(.25));

            right.AddFrame(new Rectangle(0,0,150,300), TimeSpan.FromSeconds(.25));
            right.AddFrame(new Rectangle(150,0,150,300), TimeSpan.FromSeconds(.25));
            right.AddFrame(new Rectangle(300,0,150,300), TimeSpan.FromSeconds(.25));
            right.AddFrame(new Rectangle(450,0,150,300), TimeSpan.FromSeconds(.25));

        }

        public void Update(GameTime gameTime)
        {
            currentAnimation = left;

            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.White;
            var sourceRectangle = currentAnimation.CurrentRectangle;

            spriteBatch.Draw(characterSheetTexture, topLeftOfSprite, tintColor);
        }
    }

        
    }

