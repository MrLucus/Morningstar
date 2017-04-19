using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using WindowsGame1.Views.Addons;

namespace Morningstar.Views.Addons
{
    class Button : Label
    {
        private MouseState presentMouse, pastMouse;


        private Texture2D textureHighlighted;


        public Button(ContentManager newContent, Vector2 windowScreen, String name, int number): base(newContent,windowScreen,name,number)
        {
            textureHighlighted = content.Load<Texture2D>("Menu/" + name + "Highlighted");

        }

        public bool isMouseOver
        {
            get
            {
                return isActive && isVisible && position.Contains(new Point(presentMouse.X, presentMouse.Y));
            }
        }

        public bool isLeftClicked
        {
            get { return isMouseOver && (presentMouse.LeftButton == ButtonState.Released) && pastMouse.LeftButton == ButtonState.Pressed; }
        }


        public void Update(MouseState mouse)
        {
            pastMouse = presentMouse;
            presentMouse = mouse;
            if (isMouseOver) textureCurrent = textureHighlighted;
            else textureCurrent = textureNormal;
        }

    }
}
