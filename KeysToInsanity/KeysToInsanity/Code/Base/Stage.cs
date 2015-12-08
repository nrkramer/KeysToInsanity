using KeysToInsanity.Code.Environment;
using KeysToInsanity.Code.Interactive_Objects;
using KeysToInsanity.Code.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Base
{
    class Stage
    {
        // stage information
        public ParallaxBackground background;
        public SpriteContainer statics = new SpriteContainer(); // all statics
        public SpriteContainer animatedStatics = new SpriteContainer(); // statics that have animations
        public SpriteContainer platforms = new SpriteContainer(); // platforms
        public SpriteContainer collidables = new SpriteContainer(); // collidable stuff
        public Key key; // 1 key and door per level, so these MAY be null
        public Door door;
        public SpriteContainer characters = new SpriteContainer(); // enemies
        public SpriteContainer lights = new SpriteContainer(); // lights
        public SpriteContainer fadeIns = new SpriteContainer(); // anything that you want to fade in visually
        public SpriteContainer gravitySprites = new SpriteContainer(); // any static that is also affected by gravity
        public SpriteContainer checkpoints = new SpriteContainer();
        public List<Sound> sounds = new List<Sound>(); // sound effects that are played in this stage

        public KeysToInsanity.Boundary start;
        public int startX = 0;
        public int startY = 0;

        public KeysToInsanity.Boundary end;
        public int endX = 0;
        public int endY = 0;

        public int stageX = 0;
        public int stageY = 0;
        public int stageWidth = 0;
        public int stageHeight = 0;

        public Stage(int x, int y, int w, int h)
        {
            stageX = x;
            stageY = y;
            stageWidth = w;
            stageHeight = h;
        }

        public void setStart(int x, int y, KeysToInsanity.Boundary b)
        {
            startX = x;
            startY = y;
            start = b;
        }

        public void setEnd(int x, int y, KeysToInsanity.Boundary b)
        {
            endX = x;
            endY = y;
            end = b;
        }

        public void addSound(Sound s)
        {
            sounds.Add(s);
        }

        public void addCharacter(AnimatedSprite c)
        {
            c.addTo(characters);
        }

        public void addStatic(BasicSprite s)
        {
            s.addTo(statics);
            s.addTo(collidables);
        }

        public void addGravityObject(BasicSprite s)
        {
            s.addTo(gravitySprites);
        }

        public void addFadeInObject(BasicSprite s)
        {
            s.addTo(fadeIns);
        }

        public void addAnimatedStatic(AnimatedSprite a)
        {
            a.addTo(animatedStatics);
            a.addTo(collidables);
        }

        public void addLight(LightEffect l)
        {
            l.addTo(lights);
        }

        public void addPlatform(Platform p)
        {
            p.addTo(platforms);
            p.addTo(collidables);
        }

        public void addHazard(Hazard h)
        {
            h.addTo(animatedStatics);
            h.addTo(collidables);
        }

        public void setKey(Key k)
        {
            k.addTo(statics);
            k.addTo(collidables);
            key = k;
        }

        public void setDoor(Door d)
        {
            d.addTo(statics);
            d.addTo(collidables);
            addLight(d.doorLight);
            door = d;
        }

        public void addCheckpoint(HatHanger h)
        {
            addStatic(h);
            h.addTo(checkpoints);
        }

        public void setBackground(ParallaxBackground b)
        {
            background = b;
        }

        public void playSounds()
        {
            for(int i = 0; i < sounds.Count; i++)
            {
                sounds[i].play(true);
            }
        }

        public void stopSounds()
        {
            for(int i = 0; i < sounds.Count; i++)
            {
                sounds[i].play(false);
            }
        }

        public void drawStage(SpriteBatch s)
        {
            background.draw(s);

            foreach (AnimatedSprite ast in animatedStatics)
            {
                ast.draw(s);
            }

            foreach (BasicSprite st in statics)
            {
                st.draw(s);
            }

            foreach (BasicSprite pl in platforms)
            {
                pl.draw(s);
            }

            if (key != null)
                key.draw(s);

            if (door != null)
                door.draw(s);

            foreach (AnimatedSprite sp in characters)
            {
                sp.draw(s);
            }
        }

        public void drawLights(SpriteBatch s)
        {
            foreach (LightEffect le in lights)
            {
                le.draw(s);
            }
        }
    }
}
