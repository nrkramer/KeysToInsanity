using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeysToInsanity.Code
{
    class DeathScreen
    {
        //options for what will be on the death screen
        private BasicSprite restartCP;
        private BasicSprite restartL;
        private BasicSprite exitToMenu;

        //Used for position of the death screen        
        private Vector2 restartCPButtonPosition;
        private Vector2 restartLButtonPosition;
        private Vector2 exitToMenuPosition;

        //where the mouse is so you can click the button
        MouseState mouseState;
        MouseState previousMouseState;


    }
}
