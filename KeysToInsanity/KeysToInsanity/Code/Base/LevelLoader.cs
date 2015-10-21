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

namespace KeysToInsanity.Code.Base
{
    class LevelLoader
    {
        private int fullX = 0; // used when parsing expressions
        private int fullY = 0; // used when parsing expressions

        private Game game;
        private HUD gameHud;
        private Level level;

        public LevelLoader(Game game, string xmlFile, HUD hud)
        {
            this.game = game; // get a reference to the game
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
                            s.setStart(ParseExpression(r.GetAttribute("x"), fullX), ParseExpression(r.GetAttribute("Y"), fullY), ParseBoundary(r));
                            break;
                        case "Character":
                            s.addCharacter(ParseCharacter(r));
                            break;
                        case "Static":
                            s.addStatic(ParseStatic(r));
                            break;
                        case "Background":
                            s.setBackground(ParseBackground(r));
                            break;
                        case "Checkpoint":
                            s.addStatic(ParseCheckpoint(r));
                            break;
                        case "Key":
                            s.addStatic(ParseKey(r));
                            break;
                        case "Door":
                            s.setDoor(ParseDoor(r));
                            break;
                        case "Platform":
                            ParsePlatform(r);
                            break;
                        case "LightEffect":
                            s.addLight(ParseLightEffect(r));
                            break;
                        case "End":
                            s.setEnd(ParseBoundary(r));
                            break;
                        default:
                            break;
                    }
            }
        }

        public Stage.Boundary ParseBoundary(XmlReader r)
        {
            switch (r.GetAttribute("boundary"))
            {
                case "none":
                    return Stage.Boundary.None;
                case "left":
                    return Stage.Boundary.Left;
                case "right":
                    return Stage.Boundary.Right;
                case "top":
                    return Stage.Boundary.Top;
                case "bottom":
                    return Stage.Boundary.Bottom;
                default:
                    return Stage.Boundary.None;
            }
        }

        public int ParseExpression(string exp, int value)
        {
            // To be replaced by NCalc
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
        }

        public BasicBackground ParseBackground(XmlReader r)
        {
            return new BasicBackground(game, r.GetAttribute("type"));
        }

        public BasicSprite ParseCharacter(XmlReader r)
        {
            switch(r.GetAttribute("type"))
            {
                case "Nurse":
                    Nurse nurse = new Nurse(game, int.Parse(r.GetAttribute("patrol")));
                    return nurse;
                case "AttackDog":
                    AttackDog dog = new AttackDog(game, int.Parse(r.GetAttribute("patrol")));
                    return dog;
                case "Rats":
                    Rats rats = new Rats(game, int.Parse(r.GetAttribute("patrol")));
                    return rats;
                case "Birds":
                    Birds birds = new Birds(game);
                    return birds;
                case "Security":
                    Security guard = new Security(game, int.Parse(r.GetAttribute("patrol")));
                    return guard;
                default:
                    return null;
            }
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

        public BasicSprite ParseCheckpoint(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);

            BasicSprite s = new BasicSprite(game, "hat_hangar_2", false);
            s.spritePos = new Vector2(x, y);

            return s;
        }

        public Key ParseKey(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);

            Key k = new Key(game, gameHud);

            return k;
        }

        public Door ParseDoor(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);

            Door d = new Door(game);

            return d;
        }

        public void ParsePlatform(XmlReader r)
        {

        }

        public LightEffect ParseLightEffect(XmlReader r)
        {
            int x = ParseExpression(r.GetAttribute("x"), fullX);
            int y = ParseExpression(r.GetAttribute("y"), fullY);
            int w = ParseExpression(r.GetAttribute("w"), fullX);
            int h = ParseExpression(r.GetAttribute("h"), fullY);

            LightEffect e = new LightEffect(game, r.GetAttribute("effect"));
            e.spritePos = new Vector2(x, y);
            e.spriteSize = new Point(w, h);

            return e;
        }
    }
}
