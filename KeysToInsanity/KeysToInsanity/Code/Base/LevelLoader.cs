using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using KeysToInsanity.Code.Interactive_Objects;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using KeysToInsanity.Code.Entitys;
using KeysToInsanity.Code.Interface;
using KeysToInsanity.Code.Environment;
using KeysToInsanity.Code.Objects;

namespace KeysToInsanity.Code.Base
{
    class LevelLoader
    {
        public int fullX = 0; // used when parsing expressions
        public int fullY = 0; // used when parsing expressions

        private Game game;
        private HUD gameHud;
        public Level level;

        public LevelLoader(Game game, string xmlFile, HUD hud)
        {
            this.game = game; // get a reference to the game
            fullX = game.GraphicsDevice.Viewport.Width;
            fullY = game.GraphicsDevice.Viewport.Height;
            gameHud = hud;

            using (XmlReader r = XmlReader.Create(xmlFile)) // make sure the load succeeds
            {
                while (r.Read())
                {
                    if (r.IsStartElement())
                    {
                        switch (r.Name)
                        {
                            case "Level":
                                level = new Level(r.GetAttribute("name"), int.Parse(r.GetAttribute("stages")));
                                ParseLevel(r);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        //reads through the XML file until the next "Stage" appears, generating everything it reads in as parts that make up the stage as a whole
        public void ParseLevel(XmlReader r)
        {
            int i = 0;
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch (r.Name)
                    {
                        case "Stage":
                            Stage s = new Stage(int.Parse(r.GetAttribute("x")), int.Parse(r.GetAttribute("y")), ParseExpression(r.GetAttribute("w"), fullX), ParseExpression(r.GetAttribute("h"), fullY));
                            ParseStage(r, s);
                            level.addStage(s, i);
                            i++;
                            // get stage info and parse it
                            break;
                        default:
                            break;
                    }
                if (r.NodeType == XmlNodeType.EndElement)
                    if (r.Name == "Level") // end of level
                        return;
            }
        }

        //Reads through the "Stage" part of the XML file to generate what is happening in each stage of the game, which is each individual room
        public void ParseStage(XmlReader r, Stage s)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch (r.Name)
                    {
                        case "Start":
                            s.setStart(ParseExpression(r.GetAttribute("x"), fullX), ParseExpression(r.GetAttribute("y"), fullY), ParseBoundary(r));
                            break;
                        case "Character":
                            s.addCharacter(ParseCharacter(r));
                            break;
                        case "Static":
                            BasicSprite sp = ParseStatic(r);
                            s.addStatic(sp);
                            try
                            {
                                if (bool.Parse(r.GetAttribute("fadein")))
                                    s.addFadeInObject(sp);
                            } catch (ArgumentNullException e) { }
                            break;
                        case "AnimatedStatic":
                            AnimatedSprite asp = ParseAnimatedStatic(r);
                            s.addAnimatedStatic(asp);
                            try {
                                if (bool.Parse(r.GetAttribute("fadein")))
                                    s.addFadeInObject(asp);
                            } catch (ArgumentNullException e) { }
                            break;
                        case "Background":
                            s.setBackground(ParseBackground(r));
                            break;
                        case "Checkpoint":
                            s.addStatic(ParseCheckpoint(r));
                            break;
                        case "Key":
                            s.setKey(ParseKey(r));
                            break;
                        case "Door":
                            s.setDoor(ParseDoor(r));
                            break;
                        case "Platform":
                            s.addPlatform(ParsePlatform(r));
                            break;
                        case "LightEffect":
                            LightEffect le = ParseLightEffect(r);
                            s.addLight(le);
                            try {
                                if (bool.Parse(r.GetAttribute("fadein")))
                                    s.addFadeInObject(le);
                            } catch (ArgumentNullException e ) { }
                            break;
                        case "Hazard":
                            s.addHazard(ParseHazard(r));
                            break;
                        case "Music":
                            level.setMusic(new Sound(game, r.GetAttribute("file")));
                            break;
                        case "End":
                            s.setEnd(ParseExpression(r.GetAttribute("x"), fullX), ParseExpression(r.GetAttribute("y"), fullY), ParseBoundary(r));
                            break;
                        default:
                            break;
                    }
                if (r.NodeType == XmlNodeType.EndElement)
                    if (r.Name == "Stage") // end of stage
                        return;
            }
        }

        public KeysToInsanity.Boundary ParseBoundary(XmlReader r)
        {
            switch (r.GetAttribute("boundary"))
            {
                case "none":
                    return KeysToInsanity.Boundary.None;
                case "left":
                    return KeysToInsanity.Boundary.Left;
                case "right":
                    return KeysToInsanity.Boundary.Right;
                case "top":
                    return KeysToInsanity.Boundary.Top;
                case "bottom":
                    return KeysToInsanity.Boundary.Bottom;
                default:
                    return KeysToInsanity.Boundary.None;
            }
        }

        public int ParseExpression(string exp, int value)
        {
            // To be replaced by NCalc
            if (exp != null)
            {
                exp = Regex.Replace(exp, @"\s+", ""); // remove whitespace
                if (char.IsNumber(exp[0]))
                    return int.Parse(exp);
                else
                {
                    if (exp[0] == 'c') // center
                        value /= 2;

                    if (((exp[0] == 'c') || (exp[0] == 'f')) && exp.Length == 1)
                        return value;

                    if (exp[1] == '+')
                        return value + int.Parse(exp.Substring(2));
                    else if (exp[1] == '-')
                        return value - int.Parse(exp.Substring(2));
                    else if (exp[1] == '*')
                        return value * int.Parse(exp.Substring(2));
                    else if (exp[1] == '/')
                        return value / int.Parse(exp.Substring(2));
                    else
                        return value;
                }
            } else
            {
                return 0;
            }
        }

        public BasicBackground ParseBackground(XmlReader r)
        {
            return new BasicBackground(game, r.GetAttribute("type"));
        }

        public Character ParseCharacter(XmlReader r)
        {
            switch(r.GetAttribute("type"))
            {
                case "Nurse":
                    int x = ParseExpression(r.GetAttribute("x"), fullX);
                    int y = ParseExpression(r.GetAttribute("y"), fullY);
                    int distance = ParseExpression(r.GetAttribute("distance"), 0);
                    int speed = ParseExpression(r.GetAttribute("speed"), 0);
                    Nurse nurse = new Nurse(game, speed,distance,x);
                    nurse.spritePos = new Vector2(x, y);
                    return nurse;
                case "AttackDog":
                    x = ParseExpression(r.GetAttribute("x"), fullX);
                    y = ParseExpression(r.GetAttribute("y"), fullY);
                    distance = ParseExpression(r.GetAttribute("distance"), 0);
                    speed = ParseExpression(r.GetAttribute("speed"), 0);
                    AttackDog dog = new AttackDog(game,speed,distance,x);
                    dog.spritePos = new Vector2(x, y);
                    return dog;
                case "Rats":
                    x = ParseExpression(r.GetAttribute("x"), fullX);
                    y = ParseExpression(r.GetAttribute("y"), fullY);
                    distance = ParseExpression(r.GetAttribute("distance"), 0);
                    speed = ParseExpression(r.GetAttribute("speed"), 0);
                    Rats rats = new Rats(game, speed,distance,x);
                    return rats;                
                case "Security":
                    x = ParseExpression(r.GetAttribute("x"), fullX);
                    y = ParseExpression(r.GetAttribute("y"), fullX);
                    distance = ParseExpression(r.GetAttribute("distance"), 0);
                    speed = ParseExpression(r.GetAttribute("speed"), 0);
                    Security guard = new Security(game, speed,distance,x);
                    return guard;
                case "Cars":
                    x = ParseExpression(r.GetAttribute("x"), fullX);
                    y = ParseExpression(r.GetAttribute("y"), fullX);
                    distance = ParseExpression(r.GetAttribute("distance"), 0);
                    speed = ParseExpression(r.GetAttribute("speed"), 0);
                    Cars car = new Cars(game, speed, distance, x);                        
                    return car;
                default:
                    return null;
            }
        }

        public Hazard ParseHazard(XmlReader r)
        {
            string asset = r.GetAttribute("type");
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);
            bool collidable = bool.Parse(r.GetAttribute("collide"));
            int width = 0;
            double speed = 0.0;
            try {
                width = int.Parse(r.GetAttribute("width"));
                speed = double.Parse(r.GetAttribute("speed"));
            } catch (ArgumentNullException e)
            {
                
            }
            float damage = float.Parse(r.GetAttribute("damage"));

            Hazard haz = new Hazard(game, asset, new Point(width, h), speed, collidable, damage);
            haz.spritePos = new Vector2(x, y);
            haz.spriteSize = new Point(w, h);

            return haz;
        }

        public AnimatedSprite ParseAnimatedStatic(XmlReader r)
        {
            string asset = r.GetAttribute("type");
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);
            bool collidable = bool.Parse(r.GetAttribute("collide"));
            int width = int.Parse(r.GetAttribute("width")); // width of one animation
            double speed = double.Parse(r.GetAttribute("speed"));

            AnimatedSprite s = new AnimatedSprite(game, asset, new Point(width, h), 1, speed, collidable);
            s.spritePos = new Vector2(x, y);
            s.spriteSize = new Point(w, h);

            return s;
        }

        public BasicSprite ParseStatic(XmlReader r)
        {
            string asset = r.GetAttribute("type");
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);
            bool collidable = bool.Parse(r.GetAttribute("collide"));

            BasicSprite s = new BasicSprite(game, asset, collidable);
            s.spritePos = new Vector2(x, y);
            s.spriteSize = new Point(w, h);

            return s;
        }

        public HatHanger ParseCheckpoint(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);

            HatHanger s = new HatHanger(game);
            s.spritePos = new Vector2(x, y);

            return s;
        }

        public Key ParseKey(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);

            Key k = new Key(game, gameHud);
            k.spritePos = new Vector2(x, y);

            return k;
        }

        public Door ParseDoor(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);

            Door d = new Door(game);
            d.spritePos = new Vector2(x, y);
            d.spriteSize = new Point(w, h);

            return d;
        }

        public Platform ParsePlatform(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);
            int distance = ParseExpression(r.GetAttribute("distance"), 0);
            int speed = ParseExpression(r.GetAttribute("speed"), 0);
            bool direction = bool.Parse(r.GetAttribute("direction"));
            

            if (direction)
            {
                HorizontalPlatform platform = new HorizontalPlatform(game, speed, distance);
                platform.spritePos = new Vector2(x, y);
                platform.spriteSize = new Point(w, h);
                platform.center = x;

                return platform;
            } else
            {
                VerticalPlatform platform = new VerticalPlatform(game, speed, distance);
                platform.spritePos = new Vector2(x, y);
                platform.spriteSize = new Point(w, h);
                platform.center = y;

                return platform;
            }
        }

        public LightEffect ParseLightEffect(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);
            string colorString = r.GetAttribute("color");
            Color c = new Color(int.Parse(colorString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier), 
                int.Parse(colorString.Substring(2,2), System.Globalization.NumberStyles.AllowHexSpecifier),
                int.Parse(colorString.Substring(4,2), System.Globalization.NumberStyles.AllowHexSpecifier));

            LightEffect e = new LightEffect(game, "Lights\\" + r.GetAttribute("effect"), c);
            e.spritePos = new Vector2(x, y);
            e.spriteSize = new Point(w, h);

            return e;
        }
    }
}
