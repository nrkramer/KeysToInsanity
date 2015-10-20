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

        public void ParseLevel(XmlReader r)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch(r.Name)
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

        public void ParseStage(XmlReader r)
        {
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element)
                    switch(r.Name)
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
                        case "End":
                            break;
                        default:
                            break;
                    }
            }
        }
    }
}
