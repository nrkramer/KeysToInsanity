using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using KeysToInsanity.Code.Environment;

namespace KeysToInsanity.Code.Interactive_Objects
{
    class Door : BasicSprite
    {
        private BasicSprite openSprite;
        public LightEffect doorLight { get; }
        private bool open = false;

        //doors that will open when the player has acquired the key
        public Door(Game game) : base(game, "closed_door_left_metal", true)
        {
            openSprite = new BasicSprite(game, "open_door_left_metal", false);
            doorLight = new LightEffect(game, "Lights\\open_door_left_light", Color.White);
            doorLight.hidden = true;
        }

        public void setOpen(bool open)
        {
            if (open) {
                // this calculates the size and position based on the
                // aspect ratio of the original asset
                // so the aspect ratio is given by: width/height
                // then multiply by the new width or height to get
                // a scaled version of the new height or width
                openSprite.spriteSize = new Point((int)((openSprite.spriteSize.X / (float)openSprite.spriteSize.Y) * spriteSize.Y), spriteSize.Y); // make sure this does what we want
                openSprite.spritePos = new Vector2(spritePos.X - openSprite.spriteSize.X + spriteSize.X, spritePos.Y);
                doorLight.spriteSize = new Point((int)((doorLight.spriteSize.X / (float)doorLight.spriteSize.Y) * spriteSize.Y), spriteSize.Y);
                doorLight.spritePos = new Vector2(spritePos.X - doorLight.spriteSize.X + spriteSize.X, spritePos.Y);
                collidable = false;
                doorLight.hidden = false;
            } else
            {
                collidable = true;
            }

            this.open = open;
        }

        public override void draw(SpriteBatch s)
        {
            if (!open)
            {
                base.draw(s);
            } else
            {
                openSprite.draw(s);
            }
        }
    }
}
