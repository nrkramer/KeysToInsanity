using KeysToInsanity.Code.Environment;
using KeysToInsanity.Code.Interactive_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code.Base
{
    class Stage
    {
        public enum Boundary
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 3,
            Bottom = 4
        }

        // stage information - gets emptied and refilled after each stage load
        private SpriteContainer characters;
        private SpriteContainer statics;
        private SpriteContainer lights;
        private SpriteContainer platforms;
        private BasicBackground background;
        private Key key;
        private Door door;

        private Boundary start;
        private int startX = 0;
        private int startY = 0;

        private Boundary end;

        private int stageX = 0;
        private int stageY = 0;
        private int stageWidth = 0;
        private int stageHeight = 0;

        public Stage(int x, int y, int w, int h)
        {
            stageX = x;
            stageY = y;
            stageWidth = w;
            stageHeight = h;
        }

        public void setStart(int x, int y, Boundary b)
        {
            startX = x;
            startY = y;
            start = b;
        }

        public void setEnd(Boundary b)
        {
            end = b;
        }

        public void addCharacter(BasicSprite c)
        {
            c.addTo(characters);
        }

        public void addStatic(BasicSprite s)
        {
            s.addTo(statics);
        }

        public void addLight(LightEffect l)
        {
            l.addTo(lights);
        }

        public void addPlatform(BasicSprite p)
        {
            p.addTo(platforms);
        }

        public void setKey(Key k)
        {
            key = k;
        }

        public void setDoor(Door d)
        {
            door = d;
        }

        public void setBackground(BasicBackground b)
        {
            background = b;
        }
    }
}
