using System;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace KeysToInsanity.Code.Base
{
    class LevelLoader
    {
        public LevelLoader(String xmlFile)
        {
            using (XmlReader r = XmlReader.Create(xmlFile)) // make sure the load succeeds
            {
                while (r.Read())
                {
                    if (r.IsStartElement())
                    {
                        switch (r.Name)
                        {
                            case "Level":
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        //reads through the XML file until the next "Level" appears, generating everything it reads in as parts that make up the level as a whole, which is a list of stages involved with that level
        public void ParseLevel(XmlReader r)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch (r.Name)
                    {
                        case "Stage":
                            break;
                        default:
                            break;
                    }
                if (r.NodeType == XmlNodeType.EndElement)
                    if (r.Name == "Level")
                        return;
            }
        }

        //Reads through the "Stage" part of the XML file to generate what is happening in each stage of the game, which is each individual room
        public void ParseStage(XmlReader r)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch (r.Name)
                    {
                        case "Start":
                            break;
                        case "Character":
                            break;
                        case "Static":
                            break;
                        case "Checkpoint":
                            break;
                        case "Key":
                            break;
                        case "Door":
                            break;
                        case "Platform":
                            break;
                        case "LightEffect":
                            break;
                        case "End":
                            break;
                        default:
                            break;
                    }
            }
        }
    }
}
